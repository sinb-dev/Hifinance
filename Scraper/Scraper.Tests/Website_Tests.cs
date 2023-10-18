using Scraper.Data;
using Scraper.Data.Models;
namespace Scraper.Tests;

public class Website_Tests
{
    [Fact]
    public void Get()
    {
        Website website = new()
        {
            Name = "Dr.dk",
            BaseUrl = "https://dr.dk"
        };

        Assert.Equal(new Uri("https://dr.dk/nyheder"), website.GetUri("nyheder"));
        Assert.Equal(new Uri("https://dr.dk/nyheder"), website.GetUri("/nyheder"));
        Assert.Equal(new Uri("https://dr.dk/robots.txt"), website.GetUri("robots.txt"));
    }
}