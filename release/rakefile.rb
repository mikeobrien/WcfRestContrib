require "albacore"
require "release/robocopy"

task :default => [:deployBinaries]

desc "Generate assembly info."
assemblyinfo :assemblyInfo do |asm|
  asm.version = ENV["GO_PIPELINE_LABEL"]
  asm.company_name = "Ultraviolet Catastrophe"
  asm.product_name = "WCF REST Contrib"
  asm.title = "WCF REST Contrib"
  asm.description = "Goodies for WCF REST."
  asm.copyright = "Copyright (c) 2010 Ultraviolet Catastrophe"
  asm.output_file = "src/WcfRestContrib/Properties/AssemblyInfo.cs"
end

desc "Builds the application."
msbuild :build => [:assemblyInfo] do |msb|
  msb.path_to_command = File.join(ENV['windir'], 'Microsoft.NET', 'Framework', 'v4.0.30319', 'MSBuild.exe')
  msb.properties :configuration => :Release
  msb.targets :Clean, :Build
  msb.solution = "src/WcfRestContrib.sln"
end

desc "Zips and eploys the application binaries."
zip :deployBinaries => [:build]  do |zip|
     zip.directories_to_zip "src/WcfRestContrib/bin/Release"
     zip.output_file = "WcfRestContrib_$(ENV['GO_PIPELINE_LABEL']).zip"
     zip.output_path = "D:/Websites/public.mikeobrien.net/wwwroot/Releases/WcfRestContrib/"
end
