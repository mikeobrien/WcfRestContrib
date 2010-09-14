require "albacore"
require "release/robocopy"
require "release/common"
require "rubygems"
require "rake/gempackagetask"

ReleasePath = "D:/Websites/public.mikeobrien.net/wwwroot/Releases/WcfRestContrib/#{ENV['GO_PIPELINE_LABEL']}"

task :default => [:deploySample]

desc "Inits the build"
task :initBuild do
	Common.EnsurePath("reports")
end

desc "Generate assembly info."
assemblyinfo :assemblyInfo => :initBuild do |asm|
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

desc "Builds the library."
msbuild :buildLibrary => :setAssemblyVersion do |msb|
    msb.path_to_command = File.join(ENV['windir'], 'Microsoft.NET', 'Framework', 'v4.0.30319', 'MSBuild.exe')
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/WcfRestContrib/WcfRestContrib.csproj"
end

desc "Builds the test project."
msbuild :buildTestProject => :buildLibrary do |msb|
    msb.path_to_command = File.join(ENV['windir'], 'Microsoft.NET', 'Framework', 'v4.0.30319', 'MSBuild.exe')
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/WcfRestContrib.Tests/WcfRestContrib.Tests.csproj"
end

desc "Builds the sample app."
msbuild :buildSampleApp => :buildTestProject do |msb|
    msb.path_to_command = File.join(ENV['windir'], 'Microsoft.NET', 'Framework', 'v4.0.30319', 'MSBuild.exe')
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/NielsBohrLibrary/NielsBohrLibrary.csproj"
end

desc "Set assembly reference in the sample project."
task :addSampleAssemblyReference => :buildSampleApp do
    path = "src/NielsBohrLibrary/NielsBohrLibrary.csproj"
	replace = /<ProjectReference.*<\/ProjectReference>/m
	reference = "<Reference Include=\"WcfRestContrib\"><HintPath>bin\WcfRestContrib.dll</HintPath></Reference>"
    project = Common.ReadAllFileText(path)
    project = project.gsub(replace, reference)
    Common.WriteAllFileText(path, project) 
end

desc "Builds the installer."
msbuild :buildInstaller => :addSampleAssemblyReference do |msb|
    msb.path_to_command = File.join(ENV['windir'], 'Microsoft.NET', 'Framework', 'v4.0.30319', 'MSBuild.exe')
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/Installer/Installer.wixproj"
end

desc "NUnit Test Runner"
nunit :unitTests => :buildInstaller do |nunit|
	nunit.path_to_command = "lib/nunit/net-2.0/nunit-console.exe"
	nunit.assemblies "src/WcfRestContrib.Tests/bin/Release/WcfRestContrib.Tests.dll"
	nunit.options "/xml=reports/TestResult.xml"
end

desc "Inits the deploy"
task :initDeploy => :unitTests do
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
    docs = "#{gem}/files/docs"
    pkg = "#{gem}/pkg"
    
	Common.DeleteDirectory(gem)
	
    Common.EnsurePath(lib)
    Common.EnsurePath(pkg)
    Common.EnsurePath(docs)
    
	Common.CopyFiles("src/WcfRestContrib/bin/Release/*", lib) 
	Common.CopyFiles("src/docs/**/*", docs) 

end

desc "Creates gem"
task :createGem => :prepareGemFiles do

    FileUtils.cd("gem/files") do
    
        spec = Gem::Specification.new do |spec|
            spec.platform = Gem::Platform::RUBY
            spec.summary = "Goodies for .NET WCF Rest"
            spec.name = "wcfrestcontrib"
            spec.version = "#{ENV['GO_PIPELINE_LABEL']}"
            spec.files = Dir["lib/**/*"] + Dir["docs/**/*"]
            spec.authors = ["Mike O'Brien"]
            spec.homepage = "http://github.com/mikeobrien/WcfRestContrib"
            spec.description = "The WCF REST Contrib library adds functionality to the current .NET WCF REST implementation."
        end

        Rake::GemPackageTask.new(spec) do |package|
            package.package_dir = "../pkg"
        end
        
        Rake::Task["package"].invoke
    end
end

desc "Push the gem to ruby gems"
task :pushGem => :createGem do
	result = system("gem", "push", "gem/pkg/wcfrestcontrib-#{ENV['GO_PIPELINE_LABEL']}.gem")
end

