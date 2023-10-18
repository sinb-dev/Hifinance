using System;
namespace Scraper.Data.Models;
[Table(Name = "page_types")]
public class PageType : Model
{
    [Field(Name = "page_type_id", AutoIncrement = true)]
    public long PageTypeId { get; set;} = 0;
    [Field(Name = "name")]
    public string Name {get;set;} = string.Empty;
    [Field(Name = "website")]
    public string Website {get;set;} = string.Empty;
    [Field(Name = "scrape_interval_hours")]
    public int ScrapeIntervalHours {get;set;} = 24;
    
    [Field(Name = "identify_by_selector")]
    public string IdentifyBySelector {get;set;} = string.Empty;
}