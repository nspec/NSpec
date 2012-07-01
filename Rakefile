begin
  require 'nokogiri'
  require 'cgi'
  require 'date'
  require 'zip/zipfilesystem'
rescue LoadError
  puts "looks like you don't have all the ruby libraries needed to build NSpec, make sure you have latest on https://github.com/mattflo/NSpec and you have run the command 'bundle install' (the bundler gem is required to run this command)"
  puts ""
  puts "If the 'bundle install' command fails, you have to install bundler first by running the command 'gem install bundler'"
  puts ""
  puts "If the 'gem install bundler' or 'bundle install' commands fails, it's probably because you are behind a firewall, setting the HTTP_PROXY system environment variable in the OS with your firewall proxy url will allow 'gem install bundler' and 'bundle install' to work"
  exit
end

#############################################################################
#
# Standard tasks
#
#############################################################################
desc 'default rake task builds and runs unit tests'
task :default => [:build, :spec]

desc 'build'
task :build do
  sh 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /verbosity:q NSpec.sln'
end

desc 'run specs'
task :spec => :build do
  sh '"libs\NUnit.Runners.2.6.0.12051\tools\nunit-console-x86.exe" /nologo NSpecSpecs/bin/Debug/NSpecSpecs.dll'
end

desc 'run SampleSpecs with NSpecRunner. you can supply a single spec like so -> rake samples[spec_name]'
task :samples, :spec do |t,args| 
  spec = args[:spec] || ''

  sh "NSpecRunner/bin/debug/NSpecRunner.exe SampleSpecs/bin/debug/SampleSpecs.dll #{spec}"
end

desc 'supply commit message as parameter - rake all m="commit message" - version bump, nuget, zip and everything shall be done for you'
task :all => [:pull,:version,:nuget,:commit,:website,:zip,:upload]

desc 'run the sample describe_before'
task :before do
  sh "NSpecRunner/bin/debug/NSpecRunner.exe SampleSpecs/bin/debug/SampleSpecs.dll describe_before"
end

desc 'run the sample describe_specfications'
task :specifies do
  sh "nspecrunner/bin/debug/nspecrunner.exe samplespecs/bin/debug/samplespecs.dll describe_specifications"
end

desc 'test failure exit code'
task :failure => :specifies do
  puts "YOU SHOULD NOT SEE THIS LINE"
end
#############################################################################
#
# GIT tasks
#
#############################################################################
task :pull do
   result = `git pull`

   raise 'Merge Required' if result.include? 'Aborting'
end

task :commit => :pull do
  `git add -A .`
  `git commit -m "#{ENV['m']}`
  `git push`
  `git tag -a v#{get_version_node.text} -m "#{get_version_node.text}" HEAD`
  `git push --tags`
end

#############################################################################
#
# Packaging tasks
#
#############################################################################

desc 'zip the upload'
task :zip do
  fileName = create_zip_filename 
  File.delete(fileName) if File.exists? fileName
  Zip::ZipFile.open(fileName, Zip::ZipFile::CREATE) { 
  |zipfile|
    zipfile.add 'NSpecRunner.exe', 'NSpecRunner\bin\Debug\NSpecRunner.exe'
    zipfile.add 'NSpec.dll', 'NSpec\bin\Debug\NSpec.dll'
  }
end

desc 'merge nunit dll into nspec'
task :ilmerge do
  File.rename 'NSpecRunner\bin\Debug\NSpec.dll','NSpecRunner\bin\Debug\NSpec-partial.dll'
  sh 'ilmerge NSpecRunner\bin\Debug\NSpec-partial.dll NSpecRunner\bin\Debug\nunit.framework.dll /out:NSpecRunner\bin\Debug\NSpec.dll /internalize' 
  File.delete'NSpecRunner\bin\Debug\NSpec-partial.dll'
  File.delete'NSpecRunner\bin\Debug\nunit.framework.dll'

  File.rename 'NSpec\bin\Debug\NSpec.dll','NSpec\bin\Debug\NSpec-partial.dll'
  sh 'ilmerge NSpec\bin\Debug\NSpec-partial.dll NSpec\bin\Debug\nunit.framework.dll /out:NSpec\bin\Debug\NSpec.dll /internalize' 
  File.delete'NSpec\bin\Debug\NSpec-partial.dll'
end

def create_zip_filename
  "NSpec-#{get_version_node.text}.zip"
end

desc 'upload the zip'
task :upload do
  sh "upload.bat #{create_zip_filename}"
end

desc 'Increments version number.'
task :bump_version do
  node = get_version_node

  nextVer = "0.9." + (node.text.split('.').last.to_i + 1).to_s

  xml = Nokogiri::XML(File.read 'nspec.nuspec')
  xml.root.default_namespace = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"
  node =  xml.root.xpath('//xmlns:version')[0]

  node.inner_html = nextVer

  File.open("nspec.nuspec", 'w') {|f| f.write(xml.to_xml) }
end

desc 'create and upload a nuget package. requires deploy.bat with secure hash to be in your path. autoincrements version number.'
task :nuget => [:spec, :ilmerge] do
  Dir['nspec*{nupkg}'].each {|f| File.delete(f)}
  create_nuget_package
  sh "deployNuget.bat #{Dir['*{nupkg}'][0]}"
end

desc 'creates the nuget package without incrementing the version number'
task :nuget_pack do
  create_nuget_package
end

def create_nuget_package
  sh 'nuget.exe pack nspec.nuspec'
end

#############################################################################
#
# Website tasks
#
#############################################################################
def get_version_node
  xml = Nokogiri::XML(File.read 'nspec.nuspec')
  xml.root.default_namespace = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"
  xml.root.xpath('//xmlns:version')[0]
end

desc 'supply the current tutorial markup in index.html and generate a new index.html containing current source code and output'
task :website => :spec do
  if(!File.exists?("_includes")) 
    `mkdir _includes`
  end

  files_to_comment = Array.new

  Dir['SampleSpecs/WebSite/**/*.*'].each do |f| 
    file_name = generate_html f

    files_to_comment << file_name

    sh "git add #{file_name}"

    FileUtils.copy(file_name, "../gh-pages/_includes")
  end

  sh "git commit -m \"updated website on master\""

  cd "../gh-pages"

  files_to_comment.each do |f| 
    sh "git add #{f}"
  end

  sh "git commit -m \"updated website\""

  sh "git push origin gh-pages"

  cd "../src"
end

def generate_html file
  
  file_name = file.split('/').last.split('.').first.gsub /describe_/, ""

  anchor_name = file_name

  title = file_name.gsub /_/, " "

  file_name = "_includes/" + file_name + ".html"
  file_output = code_markup(file) + "\r\n" + output_markup(file) 

  file_output = "<p><a name=\"#{anchor_name}\"></a></p>\r\n<div class=\"zone zone-sub-page-title\">\r\n<h1>#{title}</h1>\r\n</div>\r\n<div id=\"layout-content\" class=\"group\" style=\"padding-top: 10px;\">" + 
    "\r\n" + 
    file_output + 
    "\r\n" + 
    "</div>"  

  File.open(file_name, 'w') { |f| f.write(file_output) }

  return file_name

=begin
  node = @doc.at("\##{class_for(file)}_code")

  if node.nil?
    puts "can't find pre with id = #{class_for(file)}_code"
  else
    node.add_next_sibling  

    node.remove

    puts "pre with id = #{class_for(file)}_code replaced successfully"
  end

  node = nil

  node = @doc.at("\##{class_for(file)}_output")

  if node.nil?
    puts "can't find pre with id = #{class_for(file)}_output"
  else
    node.add_next_sibling output_markup file 

    node.remove

    puts "pre with id = #{class_for(file)}_outputreplaced successfully"
  end
=end
end

def class_for file
  file.split('/').last.split('.').first
end

def code_markup file
  out = "<pre id=\"#{class_for(file)}_code\" data-timestamp=\"#{Time.new.inspect}\" class=\"brush: csharp;\">"
  out += CGI::escapeHTML((File.read file).gsub('', "").gsub(/\n^public.*/m,''))
  out += '</pre>'
end

def output_markup file
  out = "<pre id=\"#{class_for(file)}_output\" data-timestamp=\"#{Time.new.inspect}\" style=\"font-size: 1.1em !important; color: #5ce632; background-color: #1b2426; padding: 10px;\">"
  output =  `nspecrunner/bin/debug/nspecrunner.exe samplespecs/bin/debug/samplespecs.dll #{class_for(file)}`.strip#.gsub("\ï\»\¿","")
  output.each_line do |line|
    cleanLine = line.rstrip
    if cleanLine.length <= 94
      out += CGI::escapeHTML(line.rstrip) + "\n"
    else
      out += CGI::escapeHTML(line.rstrip[0..94-line.length] + "...\n")
    end
  end
  out += '</pre>'
end

#############################################################################
#
# Custom tasks (add your own tasks here)
#
#############################################################################
task :version => [:bump_version, :version_gallio_adapter] do
  lines = ["[assembly: AssemblyVersion(\"#{get_version_node.text}\")]",
           "[assembly: AssemblyFileVersion(\"#{get_version_node.text}\")]"]
  update_version 'SharedAssemblyInfo.cs', lines
end

desc 'update GallioAdapter plugin version'
task :version_gallio_adapter do
  file = 'NSpec.GallioAdapter/NSpec.GallioAdapter.plugin'

  version = get_version_node.text
  if version.count('.') == 2
    version = version + '.0'
  end
  
  xml = Nokogiri::XML(File.read file)
  
  xml.root.xpath('//xmlns:assembly[@codeBase="NSpec.dll"]')[0].set_attribute('fullName', "NSpec, Version=#{version}, Culture=neutral, PublicKeyToken=null")
  xml.root.xpath('//xmlns:assembly[@codeBase="NSpec.GallioAdapter.dll"]')[0].set_attribute('fullName', "NSpec.GallioAdapter, Version=#{version}, Culture=neutral, PublicKeyToken=null")

  xml.root.xpath('//xmlns:version').each {|n| n.inner_html = version}
  xml.root.xpath('//xmlns:component[@componentId="NSpec.TestFramework"]/xmlns:traits/xmlns:frameworkAssemblies')[0].inner_html = "NSpec, Version=#{version}"
  File.open(file, 'w') {|f| f.write(xml.to_xml) }
end

def update_version file, version_lines
  newlines =  (File.read file).split("\n")[0..-3] + version_lines

  File.open(file, 'w') {|f| f.write(newlines.join("\n"))}
end
