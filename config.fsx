#r "_lib/Fornax.Core.dll"

open Config
open System.IO

let aboutPredicate (_, page) = page = "contents/about.md"

let postPredicate (projectRoot: string, page: string) =
    let fileName = Path.Combine(projectRoot,page)
    let ext = Path.GetExtension page
    if ext = ".md" then
        let ctn = File.ReadAllText fileName
        let result = ctn.Contains("layout: post")
        if result then stdout.WriteLine(fileName)
        result
    else
        false

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
