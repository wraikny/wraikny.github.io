#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

let generate' title tag (ctx : SiteContents) (_: string) =
  let posts =
    ctx.TryGetValues<Postloader.Post> ()
    |> Option.defaultValue Seq.empty
    |> Seq.filter (fun p -> p.tags |> List.contains tag)

  let psts =
    posts
    |> Seq.sortByDescending Layout.published
    |> Seq.toList
    |> List.map (Layout.postLayout true)

  Layout.layout ctx title [
    section [Class "hero is-info is-medium is-bold"] [
      div [Class "hero-body"] [
        div [Class "container has-image-centered"] [
          h1 [Class "title"] [!!title]
        ]
      ]
    ]
    div [Class "container"] [
      section [Class "articles"] [
        div [Class "column is-8 is-offset-2"] psts
      ]
    ]]
