using System;
using System.Xml.Serialization;
namespace Scraper.Data.Models;
[Table(Name = "scrape_targets")]
public class ScrapeTarget : Model
{
    [Field(Name = "target_id", AutoIncrement = true)]
    [XmlIgnore]
    public long TargetId { get; set; } = 0;
    [Field(Name = "url")]
    [XmlText]
    public string? Url { get; set; }
    [Field(Name = "next_visit")]
    [XmlAttribute]
    public DateTime NextVisit { get; set; } = DateTime.MinValue;

    [Field(Name = "page_type_id")]
    [XmlIgnore]
    public long PageTypeId { get; set; } = 0;
    [XmlIgnore]
    public PageType? PageType { get; set; } = null;
    [Field(Name = "last_http_status_code")]
    [XmlAttribute]
    public int LastHttpStatusCode { get; set; } = -1;
}