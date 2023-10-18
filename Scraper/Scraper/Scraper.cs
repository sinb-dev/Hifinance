using System.Diagnostics;
using System.IO.Enumeration;
using System.Net;
using System.Runtime.InteropServices;
using Scraper.Data.Models;
namespace Scraper;
public class Scraper
{
    ScraperCache? cache;
    string robots = "";
    public void SetCacheFolder(string path)
    {
        cache = new();
        cache.SetCacheFolder(path);
    }
    public async Task Start(Website website)
    {
        //Get robots.txt
        Uri uri = website.GetUri("robots.txt");

        HttpResponseMessage? response = await get(uri.ToString());
        if (response != null)
        {
            robots = response?.Content?.ToString();
        }
        Console.WriteLine(robots);
    }

    async Task<HttpResponseMessage?> get(string url)
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