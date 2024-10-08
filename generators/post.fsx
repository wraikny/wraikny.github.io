#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"
#load "const.fsx"

open Html


let generate' (ctx : SiteContents) (page: string) =
    let post =
        ctx.TryGetValues<Postloader.Post>()
        |> Option.defaultValue Seq.empty
        |> Seq.find (fun n -> n.file = page)

    let siteInfo = ctx.TryGetValue<Globalloader.SiteInfo> ()
    let desc =
        siteInfo
        |> Option.map (fun si -> si.description)
        |> Option.defaultValue ""

    Layout.layout ctx post.title [
        section [Class "hero is-info is-small is-bold"] [
            div [Class "hero-body"] [
                div [Class "container has-image-centered"] [
                    h1 [Class "title"] [!!post.title]
                ]
            ]
        ]
        div [Class "container"] [
            section [Class "articles"] [
                div [Class <| sprintf "column is-%d is-offset-%d" Const.CardWidth Const.cardOffset] [
                    Layout.postLayout false post
                ]
            ]
        ]
    ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
    generate' ctx page
    |> Layout.render ctx