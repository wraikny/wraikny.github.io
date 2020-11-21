#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

let generate' title tag (ctx : SiteContents) (_: string) =
  let posts =
    ctx.TryGetValues<Postloader.Post> ()
    |> Option.defaultValue Seq.empty
    |> Seq.filter (fun p -> p.tags |> List.contains tag)

  Layout.layout ctx title [
    section [Class "hero is-info is-small is-bold"] [
      div [Class "hero-body"] [
        div [Class "container has-image-centered"] [
          h1 [Class "title"] [!!title]
        ]
      ]
    ]
    Layout.postcardsLayout posts
  ]
