using Scraper.Data.Models;
using HtmlAgilityPack;
using Scraper.Data;
namespace Scraper;

public class Analyzer
{
    public async Task Analyze(ScrapeTarget target, string html) 
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        /*List<TargetProperty> props = await Database.Instance.SelectById<TargetProperty>(target.PageTypeId);
        foreach (TargetProperty p in props)
        {
            string property = p.Property;
            string value = doc.QuerySelector(p.Selector).InnerText;
            TargetData data = new();
            data.PropertyId = p.PropertyId;
            data.Result = value;
            data.Scraped = DateTime.Now;
            Database.Instance.Insert(data);
        }*/
    }
}