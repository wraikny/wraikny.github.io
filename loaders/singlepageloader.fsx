#r "../_lib/Fornax.Core.dll"
#r "../_lib/Markdig.dll"

#load "../utils/SinglePageTarget.fsx"

open System
open System.IO
open Markdig
open SinglePageTarget

type SinglePage = {
    file: string
    title: string
    content: string
    showNavBar: bool
    noIndex: bool }

let private markdownPipeline =
    MarkdownPipelineBuilder()
        .UseMediaLinks()
        .Build()

let loader (projectRoot: string) (siteContent: SiteContents) =
    for x in targets do
        let content = File.ReadAllText x.filename
        siteContent.Add
            ({  file = x.filename
                title = x.title
                content = Markdown.ToHtml(content, markdownPipeline)
                showNavBar = x.showNavBar
                noIndex = x.noIndex })
    
    siteContent
