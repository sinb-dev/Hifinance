using System;
using System.Xml.Serialization;
namespace Scraper.Data.Models;
[Table(Name = "websites")]
public class Website : Model
{
    [Field(Name = "website")]
    [XmlAttribute("Name")]
    public string Name { get; set; } = string.Empty;
    [Field(Name = "base_url")]
    [XmlAttribute]
    public string BaseUrl { get; set; } = string.Empty;

    [Field(Name = "created")]
    [XmlAttribute]
    public DateTime Created { get; set; } = DateTime.Now;
    [XmlAttribute]
    public DateTime LastScrape { get; set; } = DateTime.MinValue;
    public int Latency { get; set; }= 120;

    [XmlIgnore]
    public string Filename => $"{new Uri(BaseUrl).Host}.xml";

    [XmlElement("Target")]
    public List<ScrapeTarget> Targets { get; set; } = new();
    public Uri GetUri(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri)) 
        {
            return new Uri($"{BaseUrl}");
        }
        if (uri.Substring(0, 1) != "/")
            uri = "/" + uri;
        return new Uri($"{BaseUrl}{uri}");
    }
}