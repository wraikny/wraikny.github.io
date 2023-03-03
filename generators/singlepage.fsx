#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"
#load "const.fsx"

open Markdig
open Html
open SinglePageTarget
open Singlepageloader

let generate' (ctx : SiteContents) (file: string) =
  ctx.TryGetValues<SinglePage>()
  |> Option.map (fun xs -> xs |> Seq.find (fun x -> x.file = file))
  |> function
  | None -> Layout.layout ctx "" []
  | Some sp ->
    Layout.layoutWith sp.showNavBar sp.noIndex ctx sp.title [
      section [Class "hero is-info is-small is-bold"] [
        div [Class "hero-body"] [
          div [Class "container has-image-centered"] [
            h1 [Class "title"] [!!sp.title]
          ]
        ]
      ]
      div [Class "container"] [
        section [Class "articles"] [
          div [Class <| sprintf "column is-%d is-offset-%d" Const.CardWidth Const.cardOffset] [
              div [Class "card article"] [
                  div [Class "card-content"] [
                      div [Class "content article-body"] [
                          !! sp.content
                      ]
                  ]
              ]
          ]
        ]
      ]]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
  generate' ctx page
  |> Layout.render ctx
