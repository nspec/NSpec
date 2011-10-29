class GrowlNotifier
  def self.growl_path
    @@growl_path
  end
  
  def self.growl_path= value
    @@growl_path = value
  end

  def execute title, text, color
    return unless GrowlNotifier.growl_path

    text.gsub!('"', "'")

    text = text + "\n\n---"

    opts = ["\"#{GrowlNotifier.growl_path}\"", "\"#{text}\"", "/t:\"#{title}\""]

    opts << "/i:\"#{File.expand_path("#{color}.png")}\"" 

    `#{opts.join ' '}`
  end
end

class MSBuilder
  attr_accessor :failed

  def initialize folder
    @folder = folder
    @sh = CommandShell.new
    @failed = false
    @@ms_build_path = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\MSBuild.exe"
  end

  def execute
    output = @sh.execute "#{build_cmd sln_file}"
    @failed = output.match(/error/)
    output
  end
  
  def sln_file
    Find.find(@folder) { |f| return f if /\.sln$/.match(f) != nil && /\/.*\//.match(f) == nil }
  end

  def build_cmd file
    "\"#{MSBuilder.ms_build_path}\" \"#{file}\" /verbosity:quiet /nologo"
  end

  def self.ms_build_path
    @@ms_build_path
  end
  
  def self.ms_build_path= value
    @@ms_build_path = value
  end
end

class RakeBuilder
  attr_accessor :failed

  def initialize folder
    @sh = CommandShell.new
    @failed = false
    @folder = folder
    @@rake_command = "rake"
  end

  def self.rake_command
    @@rake_command
  end

  def self.rake_command= command
    @@rake_command = command
  end

  def execute
    output = @sh.execute @@rake_command 
    @failed = output.match(/rake aborted/)
    filtered_output = ""

    output.split("\n").each do |line|
      if(false == 
          [/msbuild.exe/, 
          /rake aborted/, 
          /See full trace/,
          /^\(in/,
          /Command failed with/].any? { |pattern| line.match(pattern) })
        filtered_output << line + "\n" 
      end
    end

    filtered_output
  end
end

class TestRunner
  attr_accessor :first_failed_test, :executed_context

  def initialize folder
    @folder = folder
    @test_dlls_override = nil
    @executed_context = Hash.new
  end
  
  def test_dlls= override
    @test_dlls_override = override 
  end

  def test_dlls
    return @test_dlls_override if(@test_dlls_override)

    dlls = Array.new

    Find.find(@folder) do |f| 
      if(true == [/test.dll$/, /tests.dll$/, /spec.dll$/, /specs.dll$/].any? { |pattern| f.downcase.match(pattern) && f.downcase.match(/bin\/debug/) })
        dlls << f if(!f.downcase.match(/\/nspec.dll/))
      end
    end

    dlls
  end

  def get_test_dll_for(file_name)
    rootDirectory = /\.\/.*\//.match(file_name)

    test_dll = nil

    test_dlls.each { |dll| test_dll = dll if /#{rootDirectory}/.match dll }

    test_dll
  end

  def find file
    return nil if [/\.sln$/, /\.csproj$/].any? { |pattern| file.match(pattern) }

    return nil if !file.match(/\./)

    just_file_name = File.basename(file, ".cs")
    
    if(file.match(/describe_.*\//))
      return file.match(/describe_.*\//).to_s.gsub "/", ""
    elsif(just_file_name.match(/^describe_/))
      return just_file_name
    else
      return "describe_" + just_file_name
    end
  end

  def usage
    "no usage defined"
  end
end

class NSpecRunner < TestRunner
  attr_accessor :failed
  def initialize folder
    super folder
    @sh = CommandShell.new
    @test_statuses = Hash.new
    @@nspec_path_override = nil
  end

  def test_dlls
    Dir['**/*NSpec.dll'].select {|f| !f.downcase.include?('packages')}.map {|f| f.gsub(/([^\/]*)\/bin\/Debug\/.*/, '\1/bin/Debug/\1.dll')}
  end

  def find file
    return nil if super(file) == nil

    just_file_name = File.basename(file, ".cs")
    
    if(contained_in_test_project(file))
      return file.gsub("./", "")
                 .gsub(root_folder(file), "")
                 .gsub(/^\//, "")
                 .gsub(".cs", "")
                 .gsub("/", "\\.")
    else
      return "describe_" + just_file_name
    end
  end

  def root_folder file
    return file.gsub("./", "").gsub(/\/.*/, "")
  end

  def contained_in_test_project file
    test_dlls.each do |dll|
      return root_folder(dll) == root_folder(file)
    end

    false
  end

  def self.nspec_path
    return @@nspec_path_override if @@nspec_path_override != nil

    exes = Array.new

    Find.find(".") do |f|
      exes << f if(/tools\/nspecrunner.exe$/.match(f.downcase))
    end

    exes.sort do |a, b|  
      version_a = a.gsub("./packages/nspec.", "").gsub("/tools/nspecrunner.exe", "")
      version_b = b.gsub("./packages/nspec.", "").gsub("/tools/nspecrunner.exe", "")

      tokens_a = version_a.split(/\./)
      tokens_b = version_b.split(/\./)

      if((tokens_a[0].to_i <=> tokens_b[0].to_i) != 0)
        tokens_a[0].to_i <=> tokens_b[0].to_i 
      elsif((tokens_a[1].to_i <=> tokens_b[1].to_i) != 0)
        tokens_a[1].to_i <=> tokens_b[1].to_i
      else
        tokens_a[2].to_i <=> tokens_b[2].to_i
      end
    end.last
  end

  def self.nspec_path= value
    @@nspec_path_override = value
  end

  def execute test_name
    @test_results = ""
    @failed = false
    @test_statuses.clear
    @first_failed_test = nil

    test_dlls.each do |dll| 
      output = @sh.execute(test_cmd(dll, test_name))

      test_result = Hash.new
      in_failure = false
      first_new_line = false

      output.each_line do |line|
        if(in_failure == true)
          @first_failed_test += line
          if(line.strip == "" && first_new_line)
            first_new_line = false
          elsif(line.strip == "")
            in_failure = false
          end
        end

        if(/\*\*\*\* FAILURES \*\*\*\*/.match(line))
          test_result[:failed] = true
          if(@first_failed_test == nil)
            @first_failed_test = "Failed Tests:"
            in_failure = true
            first_new_line = true
          end
        end
      end

      @test_statuses[dll] = test_result

      @test_results += output
    end

    @test_statuses.each_value do |value|
      @failed = @failed || value[:failed]
    end

    @test_results
  end

  def test_results
    @test_results
  end

  def test_cmd dll, name
    return "\"#{NSpecRunner.nspec_path}\" \"#{dll}\" \"#{name}\""
  end
  
  def inconclusive
    false
  end
  
  def usage
    puts "Discovered and using: #{@dll}"
  end
end


class NUnitRunner < TestRunner
  def initialize folder
    super folder
    @sh = CommandShell.new
    @failed_tests = Array.new
    @failed_tests = Array.new
    @status_by_dll = Hash.new
    @@nunit_path = "C:\\program files (x86)\\nunit 2.5.9\\bin\\net-2.0\\nunit-console-x86.exe"
  end

  def usage
    <<-OUTPUT
NUnitRunner runner will use the following exe to run your tests: 
#{NUnitRunner.nunit_path}

NUnitRunner for SpecWatchr uses category attributes for running unit tests.  Let's say you have a class called Person (located in file Person.cs).  You'll need to create a test class called describe_Person.cs (all tests associated with Person.cs should go under describe_Person.cs).  Once the test class is created all tests defined should be decorated with the Category attribute.  For example:

//here is the person class (located in Person.cs)
public class Person 
{
    public string FirstName { get; set; }
}

//here is the test class (located in describe_Person.cs)...notice the category attribute
namespace YourUnitTests
{
    [TestFixture]
    [Category("Person")]
    public class when_initializing_person
    {
        [Test]
        public void should_set_first_name_to_empty_string()
        {
            Person person = new Person();
            Assert.AreEqual(string.Empty, person.FirstName);
        }
    }

    [TestFixture]
    [Category("Person")]
    public class some_other_test_associated_with_person
    {
        [Test]
        public void should_run_test_if_Person_class_changes()
        {
            Assert.Fail();
        }
    }
}

Whenever you save Person.cs, all tests with the category "Person" will get executed.
OUTPUT
  end

  def self.nunit_path
    @@nunit_path
  end

  def self.nunit_path= value
    @@nunit_path = value
  end

  def execute test_name
    dll_test_results = Hash.new
    @inconclusive = true
    @failed = false
    @test_results = ""
    @tests = Hash.new
    @first_failed_test = nil

    test_dlls.each do |test_dll| 
      
      console_output = @sh.execute(test_cmd(test_dll, test_name))
      puts console_output
      test_result = Hash.new
      test_result[:inconclusive] = false
      test_result[:failed] = false
      dll_test_results[test_dll] = test_result

      console_output.each_line do |line|
        if(/Tests run: 0/.match(line))
          test_result[:inconclusive] = true
        end

        if(/Errors and Failures:/.match(line))
          test_result[:failed] = true
        end
      end
      
      in_failures = false
      last_test = nil
      failure_patterns = 
        [
          /Test Failure : /, 
          /Test Error : /, 
          /TearDown Error : /,
          /SetUp Error : /
        ]

      console_output.each_line do |line|
        if(/^\*\*\*\*\*/.match(line))
          test = Hash.new
          @tests[line.gsub("***** ", "").strip] = test
          
          tokens = line.gsub("***** ", "").strip.split(".")

          test[:name] = tokens[-1].gsub("_", " ")
          test[:spec] = tokens[-2].gsub("_", " ")
          test[:failed] = false
          test[:error] = ""
          test[:dll] = test_dll
        elsif(failure_patterns.any? { |pattern| pattern.match(line) })
          selected_pattern = failure_patterns.select { |pattern| pattern.match(line) != nil }.first
          full = line.gsub!(selected_pattern, "").strip
          full = full[3..-1].strip #the strip call needs to be put under test
          @tests[full][:failed] = true
          last_test = @tests[full]
          in_failures = true
        elsif(in_failures && line.strip != "")
          last_test[:error] += "        " + line.strip + "\n"
        end
      end
    end

    @test_results = ""

    dll_test_results.each_value do |value|
      @inconclusive = @inconclusive && value[:inconclusive]
      @failed = @failed || value[:failed]
    end
    
    tests_to_display = Hash.new

    test_dlls.each do |test_dll|
      test_output = ""
      failed = dll_test_results[test_dll][:failed]
      inconclusive = dll_test_results[test_dll][:inconclusive]

      if(failed)
        test_output = "Failed Tests:\n"
        tests_to_display = @tests.select { |k, v| v[:dll] == test_dll && v[:failed] == true }
      elsif(!failed && !inconclusive)
        if(!@failed)
          test_output = "All Passed:\n"
          tests_to_display = @tests.select { |k, v| v[:dll] == test_dll && v[:failed] == false }
        else
          test_output = ""  #need a test for this line
          tests_to_display = Hash.new #need a test for this line
        end
      else
        if(@inconclusive)
          test_output = "Test Inconclusive:\nNo tests found under #{ test_name }\n\n"
        else
          test_output = ""  #need a test for this line
        end

        tests_to_display = Hash.new
      end  
      
      current_spec = ""
      tests_to_display.each do |k, v|
        if(current_spec != v[:spec])
          test_output += v[:spec] + "\n"
          current_spec = v[:spec]
        end
      
        test_output += "    " + v[:name] + "\n"
        test_output += v[:error] if(v[:error])
        test_output += "\n"

        if(@failed && @first_failed_test == nil && v[:failed])
          @first_failed_test = test_output
        end
      end

      @test_results += test_output
    end

    if(!@inconclusive && !@failed)
      @test_results += "#{@tests.count} tests ran and passed\n"
    end

    puts @test_results
  end

  def test_results
    @test_results
  end

  def tests
    @tests
  end

  def test_cmd test_dll, test_name
     "\"#{@@nunit_path}\" \"#{test_dll}\" /nologo /labels"
  end

  def inconclusive
    @inconclusive
  end

  def failed
    @failed
  end
end

class MSTestRunner < TestRunner
  attr_accessor :failed, :inconclusive, :test_results
  
  def initialize folder
    super folder
    @sh = CommandShell.new
    @failed_tests = Array.new
    @status_by_dll = Hash.new
    @@ms_test_path = "C:\\program files (x86)\\microsoft visual studio 10.0\\common7\\ide\\mstest.exe"
  end

  def self.ms_test_path
     @@ms_test_path
  end

  def self.ms_test_path= value
    @@ms_test_path = value
  end
  
  def usage
    <<-OUTPUT
MSTestRunner will use the following exe to run your tests: 
#{MSTestRunner.ms_test_path}

MSTestRunner for SpecWatchr uses a convension based approach for running unit tests.  Let's say you have a class called Person (located in file Person.cs).  You'll need to create a test class called describe_Person.cs (all tests associated with Person.cs should go under describe_Person.cs).  Once the test class is created, change the namespace of the class to include PersonSpec.  For example:

//here is the person class (located in Person.cs)
public class Person 
{
    public string FirstName { get; set; }
}

//here is the test class (located in describe_Person.cs)...notice the namespace
namespace YourUnitTests.describe_Person
{
    [TestClass]
    public class when_initializing_person
    {
        [TestMethod]
        public void should_set_first_name_to_empty_string()
        {
            Person person = new Person();
            Assert.AreEqual(string.Empty, person.FirstName);
        }
    }

    [TestClass]
    public class some_other_test_associated_with_person
    {
        [TestMethod]
        public void should_run_test_if_Person_class_changes()
        {
            Assert.Fail();
        }
    }
}

Whenever you save Person.cs, all tests under the namespace describe_Person will get executed.
OUTPUT
  end

  def failed_tests
    @failed_tests
  end

  def status_by_dll
    @status_by_dll
  end

  def passed_tests
    @passed_tests
  end

  def test_config
    Find.find(@folder) { |f| return f.gsub("./", "") if /Local.testsettings$/.match(f) != nil || /LocalTestRun.testrunconfig$/.match(f) != nil }
  end

  def set_test_status test_dll, test_output
    results = Hash.new
    results[:failed] = false
    results[:inconclusive] = false

    test_output.each_line do |output| 
      results[:failed] = true if [/^Failed/, /errormessage/].any? { |pattern| output.match(pattern) }
      results[:inconclusive] = true if output.match(/No tests to execute/)
    end

    @status_by_dll[test_dll] = results
  end

  def itemize_test_results test_dll, test_output, test_name
    last_test_item = nil
    in_error = false

    test_output.split("\n").each do |line|
      if(line.strip.match(/^Failed/))
        in_error = false
        tokens = line.split(".")
        if(tokens.length > 1)
          test_name = tokens[-1].strip
          test_spec = tokens[-2].strip
          last_test_item = { :spec => test_spec.gsub("_", " "), :name => test_name.gsub("_", " "), :dll => test_dll }
          @failed_tests << last_test_item
        end
      elsif(line.strip.match(/errormessage/))
        in_error = true 
        tokens = line.split(".")
        if(last_test_item)
          last_test_item[:errormessage] = line.gsub("[errormessage] ", "")
        end
      elsif(line.strip.match(/^Passed/))
        in_error = false
        tokens = line.split(".")
        if(tokens.length > 1)
          test_name = tokens[-1].strip
          test_spec = tokens[-2].strip
          last_test_item = { :spec => test_spec.gsub("_", " "), :name => test_name.gsub("_", " "), :dll => test_dll }
          @passed_tests << last_test_item
        end
      elsif(in_error)
         last_test_item[:errormessage] += "\n    " + line
      end
    end
    
    @failed_tests.sort! { |a, b| a[:spec] <=> b[:spec] }
    @passed_tests.sort! { |a, b| a[:spec] <=> b[:spec] }
  end

  def set_test_result_output test_dll, test_name
    tests = Array.new
    failed = @status_by_dll[test_dll][:failed]
    inconclusive = @status_by_dll[test_dll][:inconclusive]

    if(failed)
      test_output = "Failed Tests:\n"
      tests = @failed_tests.select { |kvp| kvp[:dll] == test_dll }
    elsif(!failed && !inconclusive)
      if(!@failed)
        test_output = "All Passed:\n"
        tests = @passed_tests.select { |kvp| kvp[:dll] == test_dll }
      else
        test_output = ""
      end
    else
      test_output = "Test Inconclusive:\nNo tests found under #{ test_name }\n\n"
    end
    
    current_spec = ""
    tests.each do |line|
      if(current_spec != line[:spec])
        test_output += line[:spec] + "\n"
        current_spec = line[:spec]
      end
    
      test_output += "    " + line[:name] + "\n"
      test_output += "    " + line[:errormessage] + "\n" if(line[:errormessage])
      test_output += "\n"

      if(failed && @first_failed_test == nil && line[:errormessage])
        @first_failed_test = test_output
      end
    end

    @test_results += test_output

    puts @test_results
  end

  def execute test_name
    @test_results = ""
    @failed_tests = Array.new
    @passed_tests = Array.new
    @status_by_dll.clear
    @first_failed_test = nil

    test_dlls.each do |test_dll|
      test_output = @sh.execute "#{test_cmd(test_dll, test_name)}"
  
      set_test_status test_dll, test_output
      itemize_test_results test_dll, test_output, test_name
      
    end

    @inconclusive = true
    @failed = false

    @status_by_dll.each_value do |value|
      @inconclusive = @inconclusive && value[:inconclusive]
      @failed = @failed || value[:failed]
    end

    test_dlls.each do |test_dll|
      set_test_result_output test_dll, test_name
    end

    if(!@inconclusive && !@failed)
      @test_results += "#{@passed_tests.count} tests ran and passed\n"
    end

    @test_results
  end
  
  def test_cmd test_dll, test_name
    if test_name
      return "\"#{MSTestRunner.ms_test_path}\" /testcontainer:#{test_dll} /runconfig:#{test_config} /nologo /test:#{test_name} /detail:errormessage"
    else
      return "\"#{MSTestRunner.ms_test_path}\" /testcontainer:#{test_dll} /runconfig:#{test_config} /nologo /detail:errormessage"
    end
  end 
  
end

class CommandShell
  def execute cmd
    puts cmd + "\n\n"

    str=""
    STDOUT.sync = true # That's all it takes...
    IO.popen(cmd+" 2>&1") do |pipe| # Redirection is performed using operators
      pipe.sync = true
      while s = pipe.gets
        str+=s # This is synchronous!
      end
    end
    
    puts str + "\n\n"

    str
  end
end

class WatcherDotNet
  attr_accessor :notifier, :test_runner, :builder, :sh, :first_run
  require 'find'

  EXCLUDES = [/\.dll$/, /debug/i, /TestResult.xml/, /testresults/i, /\.rb$/, /\.suo$/]
	
  def initialize folder, config 
    @folder = folder
    @sh = CommandShell.new
    @notifier = GrowlNotifier.new
    @builder = Kernel.const_get(config[:builder].to_s).new folder
    @test_runner = Kernel.const_get(config[:test_runner].to_s).new folder
    @first_run = true
  end

  def require_build file
    false == EXCLUDES.any? { |pattern| file.match(pattern) }
  end
    
  def consider file
    if(@first_run)
      @notifier.execute "specwatchr", "builder: #{@builder.class}\ntest runner: #{@test_runner.class}\nconfig file: dotnet.watchr.rb", "green"
      @first_run = false
    end

    puts "====================== changed: #{file} ===================="
    puts "====================== excluded ============================" if false == require_build(file)		

    if false == require_build(file)
      puts "===================== done consider ========================"
      return
    end

    build_output = @builder.execute
    puts build_output
    
    @notifier.execute "build failed", build_output, 'red' if @builder.failed

    if @builder.failed
      puts "===================== done consider ========================"
      return
    end

    if @test_runner.test_dlls.count == 0
      @notifier.execute "discovery", "specwatchr didn't find any test dll's. specwatchr looks for a .csproj that ends in Test, Tests, Spec, or Specs.  If you do have that, stop specwatchr, rebuild your solution and start specwatchr back up. If you want to explicitly specify the test dll's, you can do so via dotnet.watchr.rb.", "red"

      puts "===================== done consider ========================"
      return
    end

    test_results = ""

    spec = @test_runner.find file

    if(!spec)
      puts "===================== done consider ========================"
      return
    end
   
    puts "=========== running spec: #{spec} ====="
    
    test_output = @test_runner.execute spec

    if @test_runner.inconclusive
      @notifier.execute "no spec found", "create spec #{spec}", 'red'
      puts @test_runner.usage
    end

    @notifier.execute "tests failed", @test_runner.first_failed_test, 'red' if @test_runner.failed
    
    @notifier.execute @test_runner.test_results.split("\n").last, '', 'green' if !@test_runner.failed and !@test_runner.inconclusive

    puts "===================== done consider ========================"

  end
end


