#r "../_lib/Fornax.Core.dll"
#r "../_lib/Markdig.dll"

open System
open System.IO
open Markdig

type About = { content: string }

let private filename = "contents/about.md"

let private markdownPipeline =
    MarkdownPipelineBuilder()
        .UseMediaLinks()
        .Build()

let loader (projectRoot: string) (siteContent: SiteContents) =
    let content = File.ReadAllText filename
    siteContent.Add( { content = Markdown.ToHtml(content, markdownPipeline)})
    
    siteContent
