using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO.Enumeration;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Scraper.Data.Models;
namespace Scraper;
public class Scraper
{
    ScraperCache? cache;
    Robots? robots = null;
    public void SetCacheFolder(string path)
    {
        cache = new();
        cache.SetCacheFolder(path);
    }
    public async Task Start(Website website)
    {
        await loadRobots(website);

        string content = await Scrape(website.GetUri(string.Empty));

        GetLinks(website, content);
    }

    async Task loadRobots(Website website)
    {
        //Get robots.txt
        Uri uri = website.GetUri("robots.txt");
        string? content = null;
        if (cache != null)
        {
            if (!cache.Check(uri)) 
            {
                content = await Robots.Lookup(website);
                if (content != null)
                {
                    await cache.Cache(uri, content);
                }
            }
            string robotText = await cache.Retrieve(uri);
            if (robotText != string.Empty) 
            {
                robots = new Robots(robotText);
            }
        }
    }

    public List<Uri> GetLinks(Website website, string html)
    {
        Uri websiteUri = new Uri(website.BaseUrl);
        List<Uri> list = new();
        HtmlDocument document = new HtmlDocument();
        document.LoadHtml(html);
        foreach (HtmlNode link in document.DocumentNode.SelectNodes("//a[@href]")) 
        {
            foreach (HtmlAttribute attrib in link.Attributes)
            {
                if (attrib.Name == "href") 
                {
                    Uri uri = new Uri(attrib.Value);

                    if (uri.Host != websiteUri.Host) 
                        continue;
                    if (robots != null && !robots.Allow(uri)) 
                        continue;

                    list.Add( uri );
                }
            }
        }
        return list;
    }

    public async Task<HttpResponseMessage?> Get(string url)
    {
        using (HttpClient client = new())
        {
            HttpRequestMessage message = new(HttpMethod.Get, url);
            HttpResponseMessage result = await client.SendAsync(message);

            if (result.IsSuccessStatusCode)
            {
                return result;
            }
        }
        return null;
    }

    public async Task<string> Scrape(Uri uri)
    {
        if (cache != null && cache.Check(uri))
        {
            return await cache.Retrieve(uri);
        }

        using (HttpClient client = new())
        {
            HttpRequestMessage message = new(HttpMethod.Get, uri);
            HttpResponseMessage result = await client.SendAsync(message);

            if (result.IsSuccessStatusCode)
            {
                string content = await result.Content.ReadAsStringAsync();
                await cacheIfSet(uri, content);
                return content;
            }
        }

        return string.Empty;
    }

    async Task cacheIfSet(Uri url, string content)
    {
        if (cache != null && !cache.Check(url))
        {
            await cache.Cache(url, content);
        }
    }
}