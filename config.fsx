#r "_lib/Fornax.Core.dll"

#load "loaders/FileContent.fsx"

open Config
open System
open System.IO

let aboutPredicate (_, page) = page = "contents/about.md"

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
    let excludes = [|
        "_public"
        "_bin"
        "_lib"
        "_data"
        "_settings"
        "_config.yml"
        ".sass-cache"
        ".sass-cache"
        ".git"
        ".ionide"
        ".config"
        ".vscode"
        ".md"
        ".fsx"
    |]

    if excludes |> Seq.exists page.Contains
    then
        false
    else
        true

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
        {Script = "about.fsx"; Trigger = OnFilePredicate aboutPredicate; OutputFile = NewFileName "about.html" }
        {Script = "post.fsx"; Trigger = OnFilePredicate postPredicate; OutputFile = Custom postOutputFile }
        {Script = "index.fsx"; Trigger = Once; OutputFile = NewFileName "index.html" }
        {Script = "indexgames.fsx"; Trigger = Once; OutputFile = NewFileName "games.html" }
        {Script = "indexworks.fsx"; Trigger = Once; OutputFile = NewFileName "works.html" }
        {Script = "indexlibraries.fsx"; Trigger = Once; OutputFile = NewFileName "libraries.html" }
        {Script = "indexarticles.fsx"; Trigger = Once; OutputFile = NewFileName "articles.html" }
    ]
}
