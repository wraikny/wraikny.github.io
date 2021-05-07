#r "../_lib/Fornax.Core.dll"

type Page = {
    title: string
    link: string
}

let loader (projectRoot: string) (siteContent: SiteContents) =
    siteContent.Add({title = "Home"; link = "/"})
    siteContent.Add({title = "About"; link = "/about.html"})
    siteContent.Add({title = "Games"; link = "/contents/games/index.html"})
    siteContent.Add({title = "Works"; link = "/contents/works/index.html"})
    siteContent.Add({title = "Libraries"; link = "/contents/libraries/index.html"})
    // siteContent.Add({title = "Articles"; link = "/contents/articles/index.html"})

    siteContent
