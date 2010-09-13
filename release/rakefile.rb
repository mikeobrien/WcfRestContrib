require "albacore"
require "release/robocopy"
require "release/common"
require 'rubygems'
require 'rake/gempackagetask'

ReleasePath = "D:/Websites/public.mikeobrien.net/wwwroot/Releases/WcfRestContrib/#{ENV['GO_PIPELINE_LABEL']}/"

task :default => [:deployGem]

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

desc "Inits the deploy"
task :initDeploy => :build do
	Common.EnsurePath(ReleasePath)
end

desc "Zips and eploys the application binaries."
zip :deployBinaries => :initDeploy do |zip|
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

task :deployGem => :deploySample do
	spec = Gem::Specification.new do |spec|
		spec.platform = Gem::Platform::RUBY
		spec.summary = "Goodies for .NET WCF Rest"
		spec.name = "wcfrestcontrib"
		spec.version = "#{ENV['GO_PIPELINE_LABEL']}"
		spec.files = Dir["src/WcfRestContrib/bin/Release/*"]
		spec.authors = ["Mike O'Brien"]
		spec.homepage = "http://github.com/mikeobrien/WcfRestContrib"
		spec.description = "The WCF REST Contrib library adds functionality to the current .NET WCF REST implementation."
	end

	Rake::GemPackageTask.new(spec) do |package|
		package.package_dir_path = "release/pkg"
	end
	
	Common.CopyFiles("release/pkg/*.gem", ReleasePath) 
end
