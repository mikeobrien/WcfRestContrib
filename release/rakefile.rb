require "albacore"
require "release/robocopy"
require "release/common"

reportsPath = "reports"
version = ENV["BUILD_NUMBER"]

desc "Inits the build"
task :initBuild do
	Common.EnsurePath(reportsPath)
end

desc "Generate assembly info."
assemblyinfo :assemblyInfo => :initBuild do |asm|
    asm.version = version
    asm.company_name = "Ultraviolet Catastrophe"
    asm.product_name = "Wcf Rest Contrib"
    asm.title = "Wcf Rest Contrib"
    asm.description = "Goodies for Wcf Rest."
    asm.copyright = "Copyright (c) 2011 Ultraviolet Catastrophe"
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
	nunit.options "/xml=#{reportsPath}/TestResult.xml"
end

nugetApiKey = ENV["NUGET_API_KEY"]
deployPath = "deploy"

packagePath = File.join(deployPath, "package")
nuspecName = "wcfrestcontrib.nuspec"
packageLibPath = File.join(packagePath, "lib")
binPath = "src/WcfRestContrib/bin/Release"

desc "Prep the package folder"
task :prepPackage => :unitTests do
	FileSystem.DeleteDirectory(deployPath)
	
	FileSystem.EnsurePath(packageLibPath)
	FileSystem.CopyFiles(File.join(binPath, "WcfRestContrib.dll"), packageLibPath)
	FileSystem.CopyFiles(File.join(binPath, "WcfRestContrib.pdb"), packageLibPath)
end

desc "Create the nuspec"
nuspec :createSpec => :prepPackage do |nuspec|
   nuspec.id = "wcfrestcontrib"
   nuspec.version = version
   nuspec.authors = "Mike O'Brien"
   nuspec.owners = "Mike O'Brien"
   nuspec.title = "WCF REST Contrib"
   nuspec.description = "The WCF REST Contrib library adds functionality to the current WCF REST implementation."
   nuspec.summary = "The WCF REST Contrib library adds functionality to the current WCF REST implementation."
   nuspec.language = "en-US"
   nuspec.licenseUrl = "https://github.com/mikeobrien/WcfRestContrib/blob/master/LICENSE"
   nuspec.projectUrl = "https://github.com/mikeobrien/WcfRestContrib"
   nuspec.iconUrl = "https://github.com/mikeobrien/HidLibrary/raw/master/misc/wcfrestcontrib.png"
   nuspec.working_directory = packagePath
   nuspec.output_file = nuspecName
   nuspec.tags = "wcf rest"
end

desc "Create the nuget package"
nugetpack :createPackage => :createSpec do |nugetpack|
   nugetpack.nuspec = File.join(packagePath, nuspecName)
   nugetpack.base_folder = packagePath
   nugetpack.output = deployPath
end

desc "Push the nuget package"
nugetpush :pushPackage => :createPackage do |nuget|
	nuget.apikey = nugetApiKey
	nuget.package = File.join(deployPath, "wcfrestcontrib.#{version}.nupkg")
end