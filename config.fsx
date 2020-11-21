#r "_lib/Fornax.Core.dll"

open Config
open System.IO

let aboutPredicate (_, page) = page = "contents/about.md"

let postPredicate (projectRoot: string, page: string) =
    let fileName = Path.Combine(projectRoot,page)
    let ext = Path.GetExtension page
    if ext = ".md" then
        let ctn = File.ReadAllText fileName
        ctn.Contains("layout: post")
    else
        false

let staticPredicate (projectRoot: string, page: string) =
    let ext = Path.GetExtension page

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
        "README.md"
    |]

    if ext = ".fsx" || excludes |> Seq.exists page.Contains
    then
        false
    else
        true

let config = {
    Generators = [
        {Script = "less.fsx"; Trigger = OnFileExt ".less"; OutputFile = ChangeExtension "css" }
        {Script = "sass.fsx"; Trigger = OnFileExt ".scss"; OutputFile = ChangeExtension "css" }
        {Script = "post.fsx"; Trigger = OnFilePredicate postPredicate; OutputFile = ChangeExtension "html" }
        {Script = "staticfile.fsx"; Trigger = OnFilePredicate staticPredicate; OutputFile = SameFileName }
        {Script = "index.fsx"; Trigger = Once; OutputFile = NewFileName "index.html" }
        {Script = "about.fsx"; Trigger = Once; OutputFile = NewFileName "about.html" }
        {Script = "gamesindex.fsx"; Trigger = Once; OutputFile = NewFileName "contents/games/index.html" }
        {Script = "worksindex.fsx"; Trigger = Once; OutputFile = NewFileName "contents/works/index.html" }
        {Script = "articlesindex.fsx"; Trigger = Once; OutputFile = NewFileName "contents/articles/index.html" }
    ]
}
