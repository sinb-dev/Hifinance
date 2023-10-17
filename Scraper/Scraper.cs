using System.IO.Enumeration;
using System.Net;
using System.Runtime.InteropServices;
namespace Scraper;
public class Scraper
{
    ScraperCache? cache;
    public void SetCacheFolder(string path) 
    {
        cache = new();
        cache.SetCacheFolder(path);
    }

    public async Task<string> Scrape(Uri uri) 
    {
        if (cache != null && cache.Check(uri)) 
        {
            return await cache.Retrieve(uri);
        }
        
        HttpClient client = new();

        HttpRequestMessage message = new(HttpMethod.Get, uri);

        HttpResponseMessage result = await client.SendAsync(message);
        if (result.IsSuccessStatusCode) 
        {
            string content = await result.Content.ReadAsStringAsync();
            await cacheIfSet(uri, content);
            return content;
        }
        return string.Empty;
    }
    
    

    async Task cacheIfSet(Uri url, string content)
    {
        if (cache != null && !cache.Check(url)) {
            await cache.Cache(url, content);
        }
    }
}