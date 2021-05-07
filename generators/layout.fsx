#r "../_lib/Fornax.Core.dll"
#if !FORNAX
#load "../loaders/aboutloader.fsx"
#load "../loaders/postloader.fsx"
#load "../loaders/pageloader.fsx"
#load "../loaders/globalloader.fsx"
#endif

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
    webpage.Insert ( (index + head.Length + 1),websocketScript)

let tweetButton =
    div [] [
        a [ Href @"https://twitter.com/share?ref_src=twsrc%5Etfw"
            Class "twitter-share-button"
            Custom ("data-show-count", "false")
        ] [ !! "Tweet" ]
        script [ Async true; Src "https://platform.twitter.com/widgets.js"; CharSet "utf-8" ] []
    ]

let layout (ctx : SiteContents) active bodyCnt =
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
        head [] [
            meta [CharSet "utf-8"]
            meta [Name "viewport"; Content "width=device-width, initial-scale=1"]
            title [] [!! ttl]
            // link [Rel "icon"; Type "image/png"; Sizes "32x32"; Href "/images/favicon.png"]
            // link [Rel "stylesheet"; Href "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css"]
            link [Rel "stylesheet"; Href "https://fonts.googleapis.com/css2?family=Noto+Sans+JP:wght@300&display=swap"]
            link [Rel "stylesheet"; Href "https://unpkg.com/bulma@0.9.1/css/bulma.min.css"]
            link [Rel "stylesheet"; Type "text/css"; Href "/style/style.css"]
        ]
        body [] [
          nav [Class "navbar"] [
            div [Class "container"] [
              div [Class "navbar-brand"] [
                // a [Class "navbar-item"; Href "/"] [
                //   img [Src "/images/bulma.png"; Alt "Logo"]
                // ]
                span [Class "navbar-burger burger"; Custom ("data-target", "navbarMenu")] [
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
        script [ Type "text/javascript"; Src "/js/navbar_burger.js" ] []
        script [ Type "text/javascript"; Src "/js/external_a.js" ] []
    ]

let render (ctx : SiteContents) cnt =
    let disableLiveRefresh =
        ctx.TryGetValue<Postloader.PostConfig> ()
        |> Option.map (fun n -> n.disableLiveRefresh)
        |> Option.defaultValue false

    cnt
    |> HtmlElement.ToString
    |> fun n -> if disableLiveRefresh then n else injectWebsocketCode n


let published (post: Postloader.Post) =
    post.published
    |> Option.defaultValue System.DateTime.Now
    |> fun n -> n.ToString("yyyy-MM-dd")


let postLayout (useSummary: bool) (post: Postloader.Post) =
    div [Class "card article"] [
        match post.thumbnail with
        | Some path ->
            div [Class "card-image"] [
                let thumbnail = image [Src path] []

                if useSummary then a [Href post.link] [thumbnail] else thumbnail
            ]
        | None -> ()

        div [Class "card-content"] [
            div [Class "media-content has-image-centered"] [
                p [Class "title article-title"; ] [ a [Href post.link] [!! post.title]]
                p [Class "subtitle is-6 article-subtitle"] [
                    // a [Href "#"] [!! (defaultArg post.author "")]
                    !! (sprintf "on %s" (published post))
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
            div [Class "column is-8 is-offset-2"] [
                div [Class "tile is-parent is-vertical"] (
                    psts
                    |> List.map(fun e -> div [Class "tile is-child"] [e])
                )
            ]
        ]
    ]
