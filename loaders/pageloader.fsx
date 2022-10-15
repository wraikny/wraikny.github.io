#r "../_lib/Fornax.Core.dll"

type Page = {
    title: string
    link: string
}

let loader (projectRoot: string) (siteContent: SiteContents) =
    siteContent.Add({title = "Home"; link = "/"})
    siteContent.Add({title = "About"; link = "/about.html"})
    siteContent.Add({title = "Articles"; link = "/articles.html"})
    siteContent.Add({title = "Games"; link = "/games.html"})
    siteContent.Add({title = "Works"; link = "/works.html"})
    siteContent.Add({title = "Libraries"; link = "/libraries.html"})

    siteContent
