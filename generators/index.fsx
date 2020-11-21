#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

let generate' (ctx : SiteContents) (_: string) =
  let posts = ctx.TryGetValues<Postloader.Post> () |> Option.defaultValue Seq.empty
  let siteInfo = ctx.TryGetValue<Globalloader.SiteInfo> ()
  let desc =
    siteInfo
    |> Option.map (fun si -> si.description)
    |> Option.defaultValue ""

  Layout.layout ctx "Home" [
    section [Class "hero is-info is-small is-bold"] [
      div [Class "hero-body"] [
        div [Class "container has-image-centered"] [
          h1 [Class "title"] [!!desc]
        ]
      ]
    ]
    Layout.postcardsLayout posts
  ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
  generate' ctx page
  |> Layout.render ctx