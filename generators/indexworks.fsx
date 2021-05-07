#load "taglist.fsx"

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
  Taglist.generate' "Works" "work" ctx page
  |> Layout.render ctx
