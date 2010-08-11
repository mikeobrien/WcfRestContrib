require 'rubygems'
require 'rake/gempackagetask'

task :default => [:init, :package, :clean]

task :clean do
	if FileTest.exists?("lib") then FileUtils.rm_rf("lib") end
end

task :init do
	if FileTest.exists?("lib") then FileUtils.rm_rf("lib") end
	if FileTest.exists?("pkg") then FileUtils.rm_rf("pkg") end
	
	FileUtils.mkdir_p "lib"
	
	Dir["../src/WcfRestContrib/bin/release/*"].each do | file |
		FileUtils.copy(file, "lib");
	end

	spec = Gem::Specification.new do |spec|
		spec.platform = Gem::Platform::RUBY
		spec.summary = "Goodies for .NET WCF Rest"
		spec.name = "wcfrestcontrib"
		spec.version = "1.0.5.0"
		spec.files = Dir["lib/**/*"]
		spec.authors = ["Mike O'Brien"]
		spec.homepage = "http://github.com/mikeobrien/WcfRestContrib"
		spec.description = "The WCF REST Contrib library adds functionality to the current .NET WCF REST implementation."
	end

	Rake::GemPackageTask.new(spec) do |package|
	end
end