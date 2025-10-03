#r "_lib/Fornax.Core.dll"

#load "utils/FileContent.fsx"
#load "utils/SinglePageTarget.fsx"

open Config
open System
open System.IO

let singlePagePredicate (_, page) =
    SinglePageTarget.targets |> List.exists (fun x -> x.filename = page)

let postPredicate (projectRoot: string, page: string) =
    let fileName = Path.Combine(projectRoot, page)
    Path.GetExtension page = ".md"
    && (
        let content = File.ReadAllText fileName
        content.Contains("layout: post")
        && (
            let config = FileContent.getConfig content
            config |> Map.containsKey "externallink" |> not
        )
    )

let staticPredicate (projectRoot: string, page: string) =
    let includedTopDirectories = [|
        "images"
        "js"
    |]

    let excludedDirectories = [|
        "_public"
        "_lib"
        "_bin"
        "_obj"
        "_data"
        "_settings"
        ".git"
        ".ionide"
        ".config"
        ".vscode"
    |]

    let excludedFilenames = [|
        "_config.yml"
    |]

    let excludedFileExtentions = [|
        ".sass-cache"
        ".sass-cache"
        ".md"
        ".fsx"
    |]

    let pathItems = page.Split '/'

    includedTopDirectories |> Seq.exists (fun d -> Some d = Array.tryHead pathItems)
    && not (
        pathItems |> Seq.exists (fun d -> Array.contains d excludedDirectories)
        || excludedFilenames |> Seq.exists (Path.GetFileName page |> (=))
        || excludedFileExtentions |> Seq.exists (Path.GetExtension page |> (=))
    )

let postOutputFile (s: string) =
    [|

        for dir in
            Path.GetDirectoryName(s).Split(Path.DirectorySeparatorChar)
            |> Seq.skip 1
        -> dir
        yield Path.ChangeExtension(Path.GetFileName(s), ".html")
    |] |> Path.Combine

let config = {
    Generators = [
        {Script = "less.fsx"; Trigger = OnFileExt ".less"; OutputFile = ChangeExtension "css" }
        {Script = "sass.fsx"; Trigger = OnFileExt ".scss"; OutputFile = ChangeExtension "css" }
        {Script = "staticfile.fsx"; Trigger = OnFilePredicate staticPredicate; OutputFile = SameFileName }
        {Script = "singlepage.fsx"; Trigger = OnFilePredicate singlePagePredicate; OutputFile = Custom postOutputFile }
        {Script = "post.fsx"; Trigger = OnFilePredicate postPredicate; OutputFile = Custom postOutputFile }
        {Script = "index.fsx"; Trigger = Once; OutputFile = NewFileName "index.html" }
        {Script = "indexgames.fsx"; Trigger = Once; OutputFile = NewFileName "games.html" }
        {Script = "indexworks.fsx"; Trigger = Once; OutputFile = NewFileName "works.html" }
        {Script = "indexlibraries.fsx"; Trigger = Once; OutputFile = NewFileName "libraries.html" }
        {Script = "indexarticles.fsx"; Trigger = Once; OutputFile = NewFileName "articles.html" }
    ]
}
