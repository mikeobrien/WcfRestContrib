require "albacore"
require "release/robocopy"
require "release/common"
require 'rubygems'
require 'rake/gempackagetask'

ReleasePath = "D:/Websites/public.mikeobrien.net/wwwroot/Releases/WcfRestContrib/#{ENV['GO_PIPELINE_LABEL']}/"

task :default => [:build]

desc "Generate assembly info."
assemblyinfo :assemblyInfo do |asm|
  asm.version = ENV["GO_PIPELINE_LABEL"]
  asm.company_name = "Ultraviolet Catastrophe"
  asm.product_name = "Wcf Rest Contrib"
  asm.title = "Wcf Rest Contrib"
  asm.description = "Goodies for Wcf Rest."
  asm.copyright = "Copyright (c) 2010 Ultraviolet Catastrophe"
  asm.output_file = "src/WcfRestContrib/Properties/AssemblyInfo.cs"
end

desc "Set assembly version in web.config"
task :setAssemblyVersion => :assemblyInfo do
	path = "src/NielsBohrLibrary/Web.config"
	project = Common.ReadAllFileText(path)
	project = project.gsub("WcfRestContrib, Version=1.0.0.0,", "WcfRestContrib, Version=#{ENV['GO_PIPELINE_LABEL']},")
	Common.WriteAllFileText(path, project) 
end

desc "Builds the application."
msbuild :build => :setAssemblyVersion do |msb|
  msb.path_to_command = File.join(ENV['windir'], 'Microsoft.NET', 'Framework', 'v4.0.30319', 'MSBuild.exe')
  msb.properties :configuration => :Release
  msb.targets :Clean, :Build
  msb.solution = "src/WcfRestContrib.sln"
end
