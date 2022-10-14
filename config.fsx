#r "_lib/Fornax.Core.dll"

#load "loaders/FileContent.fsx"

open Config
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

let config = {
    Generators = [
        {Script = "less.fsx"; Trigger = OnFileExt ".less"; OutputFile = ChangeExtension "css" }
        {Script = "sass.fsx"; Trigger = OnFileExt ".scss"; OutputFile = ChangeExtension "css" }
        {Script = "staticfile.fsx"; Trigger = OnFilePredicate staticPredicate; OutputFile = SameFileName }
        {Script = "about.fsx"; Trigger = OnFilePredicate aboutPredicate; OutputFile = NewFileName "about.html" }
        {Script = "post.fsx"; Trigger = OnFilePredicate postPredicate; OutputFile = ChangeExtension "html" }
        {Script = "index.fsx"; Trigger = Once; OutputFile = NewFileName "index.html" }
        {Script = "indexgames.fsx"; Trigger = Once; OutputFile = NewFileName "contents/games/index.html" }
        {Script = "indexworks.fsx"; Trigger = Once; OutputFile = NewFileName "contents/works/index.html" }
        {Script = "indexlibraries.fsx"; Trigger = Once; OutputFile = NewFileName "contents/libraries/index.html" }
        {Script = "indexarticles.fsx"; Trigger = Once; OutputFile = NewFileName "contents/articles/index.html" }
    ]
}
