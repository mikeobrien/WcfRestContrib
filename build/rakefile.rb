require "albacore"
require_relative "filesystem"
require_relative "gallio-task"

reportsPath = "reports"
version = ENV["BUILD_NUMBER"]

task :build => [:createPackage]
task :deploy => [:pushPackage]

task :initBuild do
	FileSystem.EnsurePath(reportsPath)
end

assemblyinfo :assemblyInfo => :initBuild do |asm|
    asm.version = version
    asm.company_name = "Ultraviolet Catastrophe"
    asm.product_name = "Wcf Rest Contrib"
    asm.title = "Wcf Rest Contrib"
    asm.description = "Goodies for Wcf Rest."
    asm.copyright = "Copyright (c) 2011 Ultraviolet Catastrophe"
    asm.output_file = "src/WcfRestContrib/Properties/AssemblyInfo.cs"
end

msbuild :buildLibrary => :assemblyInfo do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/WcfRestContrib/WcfRestContrib.csproj"
end

msbuild :buildTestProject => :buildLibrary do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/WcfRestContrib.Tests/WcfRestContrib.Tests.csproj"
end

msbuild :buildSampleApp => :buildTestProject do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/NielsBohrLibrary/NielsBohrLibrary.csproj"
end

task :addSampleAssemblyReference => :buildSampleApp do
    path = "src/NielsBohrLibrary/NielsBohrLibrary.csproj"
	replace = /<ProjectReference.*<\/ProjectReference>/m
	reference = "<Reference Include=\"WcfRestContrib\"><HintPath>bin\WcfRestContrib.dll</HintPath></Reference>"
    project = FileSystem.ReadAllFileText(path)
    project = project.gsub(replace, reference)
    FileSystem.WriteAllFileText(path, project) 
end

msbuild :buildInstaller => :addSampleAssemblyReference do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/Installer/Installer.wixproj"
end

gallio :unitTests => :buildInstaller do |runner|
	runner.echo_command_line = true
	runner.add_test_assembly("src/WcfRestContrib.Tests/bin/Release/WcfRestContrib.Tests.dll")
	runner.verbosity = 'Normal'
	runner.report_directory = reportsPath
	runner.report_name_format = 'tests'
	runner.add_report_type('Html')
end

nugetApiKey = ENV["NUGET_API_KEY"]
deployPath = "deploy"

packagePath = File.join(deployPath, "package")
nuspecName = "wcfrestcontrib.nuspec"
packageLibPath = File.join(packagePath, "lib")
binPath = "src/WcfRestContrib/bin/Release"

task :prepPackage => :unitTests do
	FileSystem.DeleteDirectory(deployPath)
	
	FileSystem.EnsurePath(packageLibPath)
	FileSystem.CopyFiles(File.join(binPath, "WcfRestContrib.dll"), packageLibPath)
	FileSystem.CopyFiles(File.join(binPath, "WcfRestContrib.pdb"), packageLibPath)
end

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
   nuspec.iconUrl = "https://github.com/mikeobrien/WcfRestContrib/raw/master/misc/wcfrestcontrib.png"
   nuspec.working_directory = packagePath
   nuspec.output_file = nuspecName
   nuspec.tags = "wcf rest"
end

nugetpack :createPackage => :createSpec do |nugetpack|
   nugetpack.nuspec = File.join(packagePath, nuspecName)
   nugetpack.base_folder = packagePath
   nugetpack.output = deployPath
end

nugetpush :pushPackage => :createPackage do |nuget|
	nuget.apikey = nugetApiKey
	nuget.package = File.join(deployPath, "wcfrestcontrib.#{version}.nupkg").gsub('/', '\\')
end