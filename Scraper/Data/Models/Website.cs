using System;
namespace Scraper.Data.Models;
[Table(Name = "websites")]
public class Website : Model
{
    [Field(Name = "website")]
    public string Name { get; set;} = string.Empty;
    [Field(Name = "base_url")]
    public string BaseUrl {get;set;} = string.Empty;
    
    [Field(Name = "created")]
    public DateTime Created {get;set;} = DateTime.Now;
}