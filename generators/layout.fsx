#r "../_lib/Fornax.Core.dll"
#if !FORNAX
#load "../loaders/singlepageloader.fsx"
#load "../loaders/postloader.fsx"
#load "../loaders/pageloader.fsx"
#load "../loaders/globalloader.fsx"
#endif

#load "./const.fsx"

open Html

let injectWebsocketCode (webpage:string) =
    let websocketScript =
        """
    <script type="text/javascript">
        var wsUri = "ws://localhost:8080/websocket";
        function init()
        {
          websocket = new WebSocket(wsUri);
          websocket.onclose = function(evt) { onClose(evt) };
        }
        function onClose(evt)
        {
          console.log('closing');
          websocket.close();
          document.location.reload();
        }
        window.addEventListener("load", init, false);
    </script>
        """
    let head = "<head>"
    let index = webpage.IndexOf head
    webpage.Insert ( (index + head.Length + 1), websocketScript)

let injectGoogleAnalytics (webpage: string) =
    let googleAnalyticsScript = """
<!-- Google tag (gtag.js) -->
<script async src="https://www.googletagmanager.com/gtag/js?id=G-5BDCQ8TPS1"></script>
<script>
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());

  gtag('config', 'G-5BDCQ8TPS1');
</script>"""

    let head = "<head>"
    let index = webpage.IndexOf head
    webpage.Insert ( (index + head.Length + 1), googleAnalyticsScript)

let tweetButton =
    div [] [
        a [ Href @"https://twitter.com/share?ref_src=twsrc%5Etfw"
            Class "twitter-share-button"
            HtmlProperties.Custom ("data-show-count", "false")
        ] [ !! "Tweet" ]
        script [ Async true; Src "https://platform.twitter.com/widgets.js"; CharSet "utf-8" ] []
    ]

let layoutWith (showNavBar: bool) (noIndex: bool) (ctx : SiteContents) active bodyCnt =
    let pages = ctx.TryGetValues<Pageloader.Page> () |> Option.defaultValue Seq.empty
    let siteInfo = ctx.TryGetValue<Globalloader.SiteInfo> ()
    let ttl =
        siteInfo
        |> Option.map (fun si ->
            if active <> null && active <> "" && active <> "Home"
            then sprintf "%s - %s" active si.title
            else si.title
        )
        |> Option.defaultValue ""

    let menuEntries =
        pages
        |> Seq.map (fun p ->
            let cls = if p.title = active then "navbar-item is-active" else "navbar-item"
            a [Class cls; Href p.link] [!! p.title ])
        |> Seq.toList

    html [] [
        let highlightjs = "10.7.2"

        head [] [
            meta [CharSet "utf-8"]
            meta [Name "viewport"; Content "width=device-width, initial-scale=1"]

            if not noIndex then
                meta [Name "robots"; Content "noindex"]

            title [] [!! ttl]
            // link [Rel "icon"; Type "image/png"; Sizes "32x32"; Href "/images/favicon.png"]
            link [Rel "stylesheet"; Href "https://use.fontawesome.com/releases/v5.5.0/css/all.css"]

            link [ Rel "preconnect"; Href "https://fonts.googleapis.com" ]
            link [ Rel "preconnect"; Href "https://fonts.gstatic.com"; CrossOrigin "" ]
            link [Rel "stylesheet"; Href "https://fonts.googleapis.com/css2?family=Noto+Sans+JP:wght@400&display=swap"]
            link [Rel "stylesheet"; Href "https://fonts.googleapis.com/css2?family=Noto+Sans+Mono:wght@400&display=swap"]
            link [Rel "stylesheet"; Href "https://unpkg.com/bulma@0.9.1/css/bulma.min.css"]
            link [Rel "stylesheet"; Type "text/css"; Href "/style/style.css"]

            link [Rel "stylesheet"; Href <| sprintf "//cdnjs.cloudflare.com/ajax/libs/highlight.js/%s/styles/vs.min.css" highlightjs]
        ]
        body [] [
            if showNavBar then
                nav [Class "navbar"] [
                    div [Class "container"] [
                    div [Class "navbar-brand"] [
                        // a [Class "navbar-item"; Href "/"] [
                        //   img [Src "/images/bulma.png"; Alt "Logo"]
                        // ]
                        span [Class "navbar-burger burger"; HtmlProperties.Custom ("data-target", "navbarMenu")] [
                        span [] []
                        span [] []
                        span [] []
                        ]
                    ]
                    div [Id "navbarMenu"; Class "navbar-menu"] menuEntries
                    ]
                ]
            yield! bodyCnt
        ]

        footer [] [
            script [ Src <| sprintf "//cdnjs.cloudflare.com/ajax/libs/highlight.js/%s/highlight.min.js" highlightjs] []
            for lang in [ "fsharp"; "csharp" ] do
                script [Src <| sprintf "//cdnjs.cloudflare.com/ajax/libs/highlight.js/%s/languages/%s.min.js" highlightjs lang] []

            script [] [
                !! "hljs.highlightAll()"
            ]

            script [ Type "text/javascript"; Src "/js/navbar_burger.js" ] []
            script [ Type "text/javascript"; Src "/js/external_a.js" ] []
        ]
    ]

let layout (ctx : SiteContents) active bodyCnt = layoutWith true false ctx active bodyCnt

let render (ctx : SiteContents) cnt =
    cnt
    |> HtmlElement.ToString
#if WATCH
    |> injectWebsocketCode
#else
    |> injectGoogleAnalytics
#endif

let published (post: Postloader.Post) =
    post.published
    |> Option.defaultValue System.DateTime.Now
    |> fun n -> n.ToString("yyyy-MM-dd")

let postLink (post: Postloader.Post) children =
    post.externalLink
    |> function
    | None ->
        a [ Href post.link ] children
    | Some el ->
        a [ Href el; Target "_blank"; Rel "noopener noreferrer" ] [
            yield! children
            span [] [ !! " " ]
            i [ Class "fas fa-external-link-alt" ] []
        ]


let postLayout (useSummary: bool) (post: Postloader.Post) =
    let pageLink =
        post.externalLink |> Option.defaultValue post.link

    div [Class "card article"] [
        match post.thumbnail with
        | Some path ->
            div [Class "card-image"] [
                let thumbnail =
                    figure [ Class "image" ] [ image [Src path] [] ]

                if useSummary then a [Href pageLink; Class "no-icon"] [thumbnail] else thumbnail
            ]
        | None -> ()

        div [Class "card-content"] [
            div [Class "media"] [
                div [Class "media-content has-image-centered"] [
                    p [Class "title article-title"; ] [ postLink post [!! post.title]]
                    p [Class "subtitle is-6 article-subtitle"] [
                        // a [Href "#"] [!! (defaultArg post.author "")]
                        !! (sprintf "on %s" (published post))
                    ]
                    p [Class "tags"] (
                        post.tags
                        |> List.filter (fun s -> s <> null && s <> "")
                        |> List.map (fun tag ->
                            span [ Class "tag is-primary" ] [ !! tag ]
                        )
                    )
                ]
            ]

            div [Class "content article-body"] [
                !! (if useSummary then post.summary else post.content)

            ]

            if not useSummary then
                div [Class "content article-footer"] [
                    tweetButton
                ]
        ]
    ]

let postcardsLayout (posts: #seq<Postloader.Post>) =
    let psts =
        posts
        |> Seq.sortByDescending published
        |> Seq.toList
        |> List.map (postLayout true)
    
    div [Class "container"] [
        section [Class "articles"] [
            let width = 10 // 1 ~ 12
            let offset = (12 - width) / 2
            div [Class <| sprintf "column is-%d is-offset-%d" Const.CardWidth Const.cardOffset] [
                div [Class "tile is-parent is-vertical"] (
                    psts
                    |> List.map(fun e -> div [Class "tile is-child"] [e])
                )
            ]
        ]
    ]
