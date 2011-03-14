require "albacore"
require "release/robocopy"
require "release/common"
require "release/nuget"

ReleasePath = "D:/Websites/public.mikeobrien.net/wwwroot/Releases/WcfRestContrib/#{ENV['GO_PIPELINE_LABEL']}"

task :default => [:deploySample]

desc "Inits the build"
task :initBuild do
	Common.EnsurePath("reports")
end

desc "Generate assembly info."
assemblyinfo :assemblyInfo => :initBuild do |asm|
    asm.version = ENV["GO_PIPELINE_LABEL"] + ".0"
    asm.company_name = "Ultraviolet Catastrophe"
    asm.product_name = "Wcf Rest Contrib"
    asm.title = "Wcf Rest Contrib"
    asm.description = "Goodies for Wcf Rest."
    asm.copyright = "Copyright (c) 2010 Ultraviolet Catastrophe"
    asm.output_file = "src/WcfRestContrib/Properties/AssemblyInfo.cs"
end

desc "Builds the library."
msbuild :buildLibrary => :assemblyInfo do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/WcfRestContrib/WcfRestContrib.csproj"
end

desc "Builds the test project."
msbuild :buildTestProject => :buildLibrary do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/WcfRestContrib.Tests/WcfRestContrib.Tests.csproj"
end

desc "Builds the sample app."
msbuild :buildSampleApp => :buildTestProject do |msb|
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
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/Installer/Installer.wixproj"
end

desc "NUnit Test Runner"
nunit :unitTests => :buildInstaller do |nunit|
	nunit.command = "lib/nunit/net-2.0/nunit-console.exe"
	nunit.assemblies "src/WcfRestContrib.Tests/bin/Release/WcfRestContrib.Tests.dll"
	nunit.options "/xml=reports/TestResult.xml"
end

desc "Inits the deploy"
task :initDeploy => :unitTests do
    Common.EnsurePath(ReleasePath)
end

desc "Deploys the installer"
task :deployInstaller => :initDeploy do
	path = "src/Installer/bin/Release/"
	File.rename("#{path}WcfRestContrib.msi", "#{ReleasePath}/WcfRestContrib_#{ENV['GO_PIPELINE_LABEL']}.msi")
end

desc "Zips and eploys the application binaries."
zip :deployBinaries => :deployInstaller do |zip|
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

desc "Prep the package folder"
task :prepPackage => :deploySample do
	Common.DeleteDirectory("deploy")
	Common.EnsurePath("deploy/package/lib")
	Common.CopyFiles("src/WcfRestContrib/bin/Release/WcfRestContrib.dll", "deploy/package/lib")
	Common.CopyFiles("src/WcfRestContrib/bin/Release/WcfRestContrib.pdb", "deploy/package/lib")
end

desc "Create the nuspec"
nuspec :createSpec => :prepPackage do |nuspec|
   nuspec.id = "wcfrestcontrib"
   nuspec.version = ENV["GO_PIPELINE_LABEL"]
   nuspec.authors = "Mike O'Brien"
   nuspec.owners = "Mike O'Brien"
   nuspec.description = "The WCF REST Contrib library adds functionality to the current WCF REST implementation."
   nuspec.summary = "The WCF REST Contrib library adds functionality to the current WCF REST implementation."
   nuspec.language = "en-US"
   nuspec.licenseUrl = "https://github.com/mikeobrien/WcfRestContrib/blob/master/LICENSE"
   nuspec.projectUrl = "https://github.com/mikeobrien/WcfRestContrib"
   nuspec.working_directory = "deploy/package"
   nuspec.output_file = "wcfrestcontrib.nuspec"
   nuspec.tags = "wcf rest"
end

desc "Create the nuget package"
nugetpack :createPackage => :createSpec do |nugetpack|
   nugetpack.nuspec = "deploy/package/wcfrestcontrib.nuspec"
   nugetpack.base_folder = "deploy/package"
   nugetpack.output = "deploy"
end

desc "Push the nuget package"
nugetpush :pushPackage => :createPackage do |nugetpush|
   nugetpush.package = "deploy/wcfrestcontrib.#{ENV['GO_PIPELINE_LABEL']}.nupkg"
end

desc "Tag the current release"
task :tagRelease do
	result = system("git", "tag", "-a", "v#{ENV['GO_PIPELINE_LABEL']}", "-m", "release-v#{ENV['GO_PIPELINE_LABEL']}")
	result = system("git", "push", "--tags")
end

