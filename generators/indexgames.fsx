#load "taglist.fsx"

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
  Taglist.generate' "Games" "game" ctx page
  |> Layout.render ctx
