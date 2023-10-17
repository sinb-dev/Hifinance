using Scraper;
using Scraper.Data;
using Scraper.Data.Models;

Scraper.Scraper scraper = new();

try 
{
    scraper.SetCacheFolder("cache");
    Uri test = new Uri("https://www.proshop.dk/Bundkort/ASUS-ROG-MAXIMUS-Z690-HERO-Bundkort-Intel-Z690-Intel-LGA1700-socket-DDR5-RAM-ATX/3000492");
    string html = await scraper.Scrape( test );
    
    File.Delete("scraper.db");
    Database.Create("scraper.db");
    await Database.Instance.Insert(new Website() {
        Name = "Proshop",
        BaseUrl = new("https://proshop.dk")
    });
    PageType productPage = new PageType() {
        Website = "Proshop",
        Name = "productPage",
        IdentifyBySelector = "article.site-new-product-page-design"
    }; 
    await Database.Instance.Insert(productPage);
    await Database.Instance.Insert(new PageType() {
        Website = "Proshop",
        Name = "productList",
        IdentifyBySelector = "ul#products"
    });

    await Database.Instance.Insert(new TargetProperty() {
        Property = "price",
        Selector = "div.site-currency-attention",
        PageTypeId = 1
    });

    ScrapeTarget scraperTarget = new ScrapeTarget() {
        Url = test,
        NextVisit = DateTime.Now + TimeSpan.FromHours(productPage.ScrapeIntervalHours),
        PageTypeId = 1,
        LastHttpStatusCode = 200
    };
    await Database.Instance.Insert(scraperTarget);
    Analyzer a = new();
    await a.Analyze(scraperTarget, html);    

} 
catch (IOException e) 
{
    Console.WriteLine(e.Message);
}