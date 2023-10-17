using System;
namespace Scraper.Data.Models;
[Table(Name = "target_data")]
public class TargetData : Model
{
    [Field(Name = "property_id")]
    public int PropertyId { get; set;} = 0;
    [Field(Name = "result")]
    public string Result {get;set;} = string.Empty;
    [Field(Name = "scraped")]
    public DateTime Scraped {get;set;} = DateTime.MinValue;
}