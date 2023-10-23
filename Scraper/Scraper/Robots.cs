using Scraper.Data.Models;

namespace Scraper;

public class Robots
{
    Dictionary<string, List<string>> settings = new();
    public Robots(string robotsText)
    {
        var lines = robotsText.Split("\n");

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || !line.Contains(":")) continue;
            string[] keyValue = line.Split(":");
            string key = keyValue[0].Trim();
            string value = keyValue[1].Trim();
            if (!settings.ContainsKey(key))
            {
                settings[key] = new();
            }
            settings[key].Add(value);
        }
    }
    
    public bool Allow(Uri uri)
    {
        if (settings.ContainsKey("Disallow")) 
        {
            foreach (string disallowed in settings["Disallow"]) 
            {
                string local = uri.LocalPath;
                if (local.EndsWith("/") == false) local += "/";
                if (local.StartsWith(disallowed))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public async static Task<string?> Lookup(Website website) 
    {
        string? content = null;
        using (HttpClient client = new())
        {
            HttpRequestMessage message = new(HttpMethod.Get, website.GetUri("robots.txt"));
            HttpResponseMessage result = await client.SendAsync(message);

            if (result.IsSuccessStatusCode)
            {
                content = await result.Content.ReadAsStringAsync();
            }
        }
        return content;
    }
}