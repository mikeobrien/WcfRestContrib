require "albacore"
require "release/robocopy"
require "release/common"

ReleasePath = "D:/Websites/public.mikeobrien.net/wwwroot/Releases/WcfRestContrib/"

task :default => [:deploySample]

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

desc "Zips and eploys the application binaries."
zip :deployBinaries => :build do |zip|
     zip.directories_to_zip "src/WcfRestContrib/bin/Release"
     zip.output_file = "WcfRestContrib_#{ENV['GO_PIPELINE_LABEL']}.zip"
     zip.output_path = ReleasePath
end

desc "Zips and eploys the application binaries."
zip :deploySample => :deployBinaries do |zip|
     zip.directories_to_zip "src/NielsBohrLibrary"
     zip.output_file = "WcfRestContribSample_#{ENV['GO_PIPELINE_LABEL']}.zip"
     zip.output_path = ReleasePath
end

