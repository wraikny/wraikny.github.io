#load "taglist.fsx"

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
  Taglist.generate' "Libraries" "library" ctx page
  |> Layout.render ctx
