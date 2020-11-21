#r "../_lib/Fornax.Core.dll"
#r "../_lib/Markdig.dll"

open System
open System.IO
open Markdig

type PostConfig = {
    disableLiveRefresh: bool
}

type Post = {
    file: string
    link : string
    title: string
    // author: string option
    published: DateTime option
    tags: string list
    content: string
    summary: string
}

let contentDir = "contents"

let private markdownPipeline =
    MarkdownPipelineBuilder()
        .UsePipeTables()
        .UseGridTables()
        .UseMediaLinks()
        .UseCitations()
        .Build()

let private isSeparator (input : string) =
    input.StartsWith "---"

let private isSummarySeparator (input: string) =
    input.Contains "<!--more-->"


///`fileContent` - content of page to parse. Usually whole content of `.md` file
///returns content of config that should be used for the page
let private getConfig (fileContent : string) =
    let fileContent = fileContent.Split '\n'
    let fileContent = fileContent |> Array.skip 1 //First line must be ---
    let indexOfSeperator = fileContent |> Array.findIndex isSeparator
    let splitKey (line: string) = 
        let seperatorIndex = line.IndexOf(':')
        if seperatorIndex > 0 then
            let key = line.[.. seperatorIndex - 1].Trim().ToLower()
            let value = line.[seperatorIndex + 1 ..].Trim() 
            Some(key, value)
        else 
            None
    fileContent
    |> Array.splitAt indexOfSeperator
    |> fst
    |> Seq.choose splitKey
    |> Map.ofSeq

///`fileContent` - content of page to parse. Usually whole content of `.md` file
///returns HTML version of content of the page
let private getContent (fileContent : string) link =
    let fileContent = fileContent.Split '\n'
    let fileContent = fileContent |> Array.skip 1 //First line must be ---
    let indexOfSeperator = fileContent |> Array.findIndex isSeparator
    let _, content = fileContent |> Array.splitAt indexOfSeperator

    let summary, content =
        match content |> Array.tryFindIndex isSummarySeparator with
        | Some indexOfSummary ->
            let summary, _ = content |> Array.splitAt indexOfSummary
            [| yield! summary; sprintf """<a href="%s" class="button">More</a>""" link |], content
        | None ->
            content, content

    let summary = summary |> Array.skip 1 |> String.concat "\n"
    let content = content |> Array.skip 1 |> String.concat "\n"

    Markdown.ToHtml(summary, markdownPipeline),
    Markdown.ToHtml(content, markdownPipeline)

let private trimString (str : string) =
    str.Trim().TrimEnd('"').TrimStart('"')

let private loadFile directories n =
    let text = File.ReadAllText n

    if text.Contains("layout: post") then

        let file = Path.Combine(directories, (n |> Path.GetFileNameWithoutExtension) + ".md").Replace("\\", "/")
        let link = "/" + Path.Combine(directories, (n |> Path.GetFileNameWithoutExtension) + ".html").Replace("\\", "/")
        
        let config = getConfig text
        let summary, content = getContent text link

        let title = config |> Map.find "title" |> trimString
        // let author = config |> Map.tryFind "author" |> Option.map trimString
        let published = config |> Map.tryFind "published" |> Option.map (trimString >> System.DateTime.Parse)

        let tags =
            let tagsOpt =
                config
                |> Map.tryFind "tags"
                |> Option.map (trimString >> fun n -> n.Split ',' |> Array.toList)
            defaultArg tagsOpt []

        { file = file
          link = link
          title = title
        //   author = author
          published = published
          tags = tags
          content = content
          summary = summary }
        |> Some
    else
        None


let loader (projectRoot: string) (siteContent: SiteContents) =
    let rec getFilesRecursively (path: string) directories = seq {
        for file in Directory.GetFiles path do
            if file.EndsWith ".md" then
                match loadFile directories file with
                | Some f -> yield f
                | _ -> ()

        for dir in Directory.GetDirectories path do
            
            let directories = Path.Combine(directories, Path.GetFileName(dir))
            yield! getFilesRecursively dir directories
    }

    let postsPath = Path.Combine(projectRoot, contentDir)

    getFilesRecursively postsPath contentDir
    |> Seq.iter siteContent.Add

    siteContent.Add({disableLiveRefresh = false})
    siteContent

