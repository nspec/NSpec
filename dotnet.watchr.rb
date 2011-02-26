require './watcher_dot_net.rb'

@dw = WatcherDotNet.new ".", { :builder => :MSBuilder, :test_runner => :LambSpecRunner}

LambSpecRunner.lamb_spec_path = 
  '.\ConsoleApplication1\bin\Debug\NSpecRunner.exe'

MSTestRunner.ms_test_path = 
  'C:\program files (x86)\microsoft visual studio 10.0\common7\ide\mstest.exe'

NUnitRunner.nunit_path = 
  'C:\program files (x86)\nunit 2.5.9\bin\net-2.0\nunit-console-x86.exe'

MSBuilder.ms_build_path =
  'C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe'

RakeBuilder.rake_command = 'rake'

#set to empty string if you dont have growl installed
GrowlNotifier.growl_path = 
  'C:\program files (x86)\Growl for Windows\growlnotify.exe'

#use/uncomment the following to override test dll finding behavior
#@dw.test_runner.test_dlls = ['.\SampleSpecs\bin\Debug\SampleSpecs.dll']

@dw.test_runner.dll = '.\SampleSpecs\bin\Debug\SampleSpecs.dll'

runner = @dw.test_runner
def runner.find file
  file.split('/').last.split('.cs').first  
end                                        

def handle filename
  @dw.consider filename
end

def reload
  puts "Reloading SpecWatchr because a project/sln file changed."
  `touch dotnet.watchr.rb`
end

def tutorial
  puts "======================== SpecWatcher has started ==========================\n\n"
  puts "TEST RUNNER: #{@dw.test_runner.class}\n\n"
  puts "(you can change your test runner in dotnet.watchr.rb...)\n\n"

  if(@dw.test_runner.test_dlls.count == 0)
    puts "WARNING WARNING WARNING"
    puts "I didn't find any test projects.  Test projects MUST end in the word Test or Tests.  For example: UnitTests.csproj"
    puts "If you have these projects, try building your solution and re-running SpecWatchr\n\n"
  else
    puts "I have found the following test dll's in your solution:"
    @dw.test_runner.test_dlls.each { |dll| puts dll }
  end

  puts "\n\n"

  if(GrowlNotifier.growl_path)
    puts "GROWL PATH: #{GrowlNotifier.growl_path}"
    puts "If you do not have Growl For Windows installed, open up dotnet.watchr.rb and set GrowlNotifier.growl_path to \"\" (empty string)"
  end

  puts "\n\n"

  puts "USAGE INSTRUCTIONS FOR #{@dw.test_runner.class}"
  puts @dw.test_runner.usage
end

tutorial

watch ('.*.cs$') { |md| handle md[0] }
watch ('.*.csproj$') { |md| reload }
watch ('.*.sln$') { |md| reload }
