require "fileutils"

module Common

	def Common.EnsurePath(path)
		if !Dir.exists?(path) then 
			FileUtils.mkdir_p(path)
		end
	end

	def Common.DeleteDirectory(path)
		if Dir.exists?(path) then 
			 FileUtils.rm_rf path
		end
	end

	def Common.CopyFiles(source, target) 
		Dir.glob(source) do |name|
			FileUtils.cp(name, target)
		end	
	end

	def Common.ReadAllFileText(path)
	  data = ""
	  file = File.open(path, "r") 
	  file.each_line do |line|
		data += line
	  end
	  return data
	end

	def Common.WriteAllFileText(path, text) 
		File.open(path, 'w') do |file|  
		  file.puts text
		end 
	end

end