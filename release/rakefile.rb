require "albacore"
require "release/robocopy"
require "release/common"
require 'rubygems'
require 'rake/gempackagetask'

ReleasePath = "D:/Websites/public.mikeobrien.net/wwwroot/Releases/WcfRestContrib/#{ENV['GO_PIPELINE_LABEL']}/"

task :default => [:createGem]

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

desc "Prepares the gem files to be packaged."
task :prepareGemFiles => :build do
	
	gem = "gem"
	lib = "#{gem}/files/lib"
	pkg = "#{gem}/pkg"
	
	if Dir.exists?(gem) then 
		 FileUtils.rm_rf gem
	end

	FileUtils.mkdir_p(lib)
	FileUtils.mkdir_p(pkg)
	
    Dir.glob("src/WcfRestContrib/bin/Release/*") do |name|
		FileUtils.cp(name, lib)
	end	
	
end

	spec = Gem::Specification.new do |spec|
		spec.platform = Gem::Platform::RUBY
		spec.summary = "Goodies for .NET WCF Rest"
		spec.name = "wcfrestcontrib"
		spec.version = "#{ENV['GO_PIPELINE_LABEL']}"
		spec.files = Dir["lib/**/*"]
		spec.authors = ["Mike O'Brien"]
		spec.homepage = "http://github.com/mikeobrien/WcfRestContrib"
		spec.description = "The WCF REST Contrib library adds functionality to the current .NET WCF REST implementation."
	end

	# Create the Gem package task
	Rake::GemPackageTask.new(spec) do |package|
	end
desc "Creates gem"
task :createGem do

	
	Rake::Task["package"].execute

end

desc "Push the gem to ruby gems"
task :pushGem => :package do
	result = system("gem", "push", "release/pkg/wcfrestcontrib-#{ENV['GO_PIPELINE_LABEL']}.gem")
end

