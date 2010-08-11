#light

open System.IO
open System.Xml
open System.Text.RegularExpressions
open System.Reflection

// Get the project path
let getCommandLine = 
    let args = fsi.CommandLineArgs
    let startingPoint = Array.findIndex (fun i -> i = "...") args
    System.String.Join(" ", args, startingPoint + 1, args.Length - startingPoint - 1)

let projectPath = getCommandLine

// Get the assembly identity
let getAssemblyIdentity path = 
    let assemblyPath = path
    let assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath)
    assembly.FullName
    
let identity = getAssemblyIdentity (Path.Combine(projectPath, "bin\WcfRestcontrib.dll"))

// Create a distrib project with the proper assembly reference
let projectFile = Path.Combine(projectPath, "NielsBohrLibrary.csproj")
let distribProjectFile = Path.Combine(projectPath, "NielsBohrLibrary.Distrib.csproj")

if File.Exists(distribProjectFile) then File.Delete(distribProjectFile)

let project = new XmlDocument()
project.Load(projectFile)

let msBuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003"
let nsManager = new XmlNamespaceManager(project.NameTable);
nsManager.AddNamespace("msb", msBuildNamespace);

// Delete the project ref
let projectRefNode = 
    project.SelectNodes("/msb:Project/msb:ItemGroup", nsManager)
    |> Seq.cast<XmlNode>
    |> Seq.find (fun n -> n.FirstChild.Name = "ProjectReference")
    
projectRefNode.RemoveChild(projectRefNode.FirstChild)

// Add the file ref
let referenceItemGroup = 
    project.SelectNodes("/msb:Project/msb:ItemGroup", nsManager)
    |> Seq.cast<XmlNode>
    |> Seq.find (fun n -> n.FirstChild.Name = "Reference")

let referenceNode = project.CreateElement("Reference", msBuildNamespace)

referenceNode.SetAttribute("Include", identity)
referenceItemGroup.AppendChild(referenceNode)

// Clear the post build event
let postBuildEvent = 
    project.SelectNodes("/msb:Project/msb:PropertyGroup", nsManager)
    |> Seq.cast<XmlNode>
    |> Seq.find (fun n -> n.FirstChild.Name = "PostBuildEvent")
    
postBuildEvent.RemoveChild(postBuildEvent.FirstChild)
    
project.Save(distribProjectFile)

// Update the web.confg to have the right assembly version

let webConfig = Path.Combine(projectPath, "web.config")
let webConfigSource = File.ReadAllText(webConfig)

let updatedWebConfigSource = Regex.Replace(webConfigSource, "WcfRestContrib, Version=([0-9]|[1-9][0-9]|[1-9][0-9][0-9]).([0-9]|[1-9][0-9]|[1-9][0-9][0-9]).([0-9]|[1-9][0-9]|[1-9][0-9][0-9]).([0-9]|[1-9][0-9]|[1-9][0-9][0-9]), Culture=neutral, PublicKeyToken=89183999a8dc93b5", identity)

File.WriteAllText(webConfig, updatedWebConfigSource)