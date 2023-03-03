#r "../_lib/Fornax.Core.dll"
#r "../_lib/Markdig.dll"

#load "../utils/FileContent.fsx"

open System
open System.IO
open Markdig

type Post = {
    file: string
    link : string
    title: string
    // author: string option
    published: DateTime option
    tags: string list
    content: string
    summary: string
    thumbnail: string option
    externalLink: string option
}

let contentDir = ""

let private markdownPipeline =
    MarkdownPipelineBuilder()
        .UsePipeTables()
        .UseGridTables()
        .UseMediaLinks()
        .UseCitations()
        .Build()

///`fileContent` - content of page to parse. Usually whole content of `.md` file
///returns HTML version of content of the page
let private getContent (fileContent : string) link =
    let summary, content = FileContent.getContent fileContent link

    Markdown.ToHtml(summary, markdownPipeline),
    Markdown.ToHtml(content, markdownPipeline)

let private trimString (str : string) =
    str.Trim().TrimEnd('"').TrimStart('"')

let private loadFile directories n =
    let text = File.ReadAllText n

    if text.Contains("layout: post") then
        let file = Path.Combine(directories, (n |> Path.GetFileNameWithoutExtension) + ".md").Replace("\\", "/")
        let link =
            [|  "/"
                yield!
                    directories.Split(Path.DirectorySeparatorChar) |> Seq.skip 1
                Path.GetFileNameWithoutExtension(n) + ".html"
            |]
            |> Path.Combine
            |> fun s -> s.Replace("\\", "/")
        
        let config = FileContent.getConfig text

        let externalLink = config |> Map.tryFind "externallink"

        let summary, content = getContent text (externalLink |> Option.defaultValue link)

        let title = config |> Map.find "title" |> trimString
        // let author = config |> Map.tryFind "author" |> Option.map trimString
        let published = config |> Map.tryFind "published" |> Option.map (trimString >> System.DateTime.Parse)

        let tags =
            let tagsOpt =
                config
                |> Map.tryFind "tags"
                |> Option.map (trimString >> fun n -> n.Split ',' |> Array.toList)
            defaultArg tagsOpt []

        let thumbnail = config |> Map.tryFind "thumbnail"
        

        { file = file
          link = link
          title = title
        //   author = author
          published = published
          tags = tags
          content = content
          summary = summary
          thumbnail = thumbnail
          externalLink = externalLink }
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

    siteContent

