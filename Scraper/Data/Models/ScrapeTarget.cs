using System;
namespace Scraper.Data.Models;
[Table(Name = "scrape_targets")]
public class ScrapeTarget : Model
{
    [Field(Name = "target_id", AutoIncrement = true)]
    public long TargetId { get; set;} = 0;
    [Field(Name = "url")]
    public Uri? Url {get;set;}
    [Field(Name = "next_visit")]
    public DateTime NextVisit {get;set;} = DateTime.MinValue;
    
    [Field(Name = "page_type_id")]
    public long PageTypeId {get;set;} = 0;

    public PageType? PageType {get;set;} = null;
    [Field(Name = "last_http_status_code")]
    public int LastHttpStatusCode {get;set;} = -1;
}