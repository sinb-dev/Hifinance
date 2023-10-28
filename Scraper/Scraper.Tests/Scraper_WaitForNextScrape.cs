using Scraper.Data;
using Scraper.Data.Models;
using Xunit.Sdk;
namespace Scraper.Tests;

public class Scraper_WaitForNextScrape
{
    [Fact]
    public async void WaitForNextScrape_ShouldDelay()
    {
        int waitSeconds = 2;
        DateTime start = DateTime.Now;
        Website website = new()
        {
            Name = "Example.com",
            BaseUrl = "https://example.com"
        };
        //Set last scrape to be almost a scrape latency ago
        website.LastScrape = start - TimeSpan.FromSeconds(website.Latency - waitSeconds);

        Scraper scraper = new();
        //Should scrape the website in 2 seconds
        await scraper.Start(website);
        var span = DateTime.Now - start;
        Assert.Equal(waitSeconds, span.Seconds);
    }

    [Fact]
    public async void WaitForNextScrape_ShouldRunImmediately()
    {
        DateTime start = DateTime.Now;

        //Website latency should default to 120 seconds.
        //Last scrape was never (DateTime.MinValue)
        Website website = new()
        {
            Name = "Example.com",
            BaseUrl = "https://example.com"
        };

        Scraper scraper = new();
        //Should scrape the website in 0 seconds
        await scraper.Start(website);
        var span = DateTime.Now - start;
        Assert.Equal(0, span.Seconds);
    }
}