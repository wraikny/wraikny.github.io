#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Markdig
open Html

let generate' (ctx : SiteContents) (_: string) =
  let content =
      ctx.TryGetValue<Aboutloader.About>()
      |> Option.map (fun x -> x.content)
      |> Option.defaultValue ""

  Layout.layout ctx "About" [
    section [Class "hero is-info is-small is-bold"] [
      div [Class "hero-body"] [
        div [Class "container has-image-centered"] [
          h1 [Class "title"] [!!"About"]
        ]
      ]
    ]
    div [Class "container"] [
      section [Class "articles"] [
        div [Class "column is-8 is-offset-2"] [
            div [Class "card article"] [
                div [Class "card-content"] [
                    div [Class "content article-body"] [
                        !! content
                    ]
                ]
            ]
        ]
      ]
    ]]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
  generate' ctx page
  |> Layout.render ctx