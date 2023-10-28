using System.Runtime.ExceptionServices;
using Scraper.Data;
using Scraper.Data.Models;
using Xunit.Sdk;
namespace Scraper.Tests;

public class Scraper_GetLinksTests
{
    const string html1 = @"<!DOCTYPE html>
<html>
<body>
<h1>This is a title</h1>
<a href=""https://example.com"">
</body>
</html>
";
    const string twoExternalLinksThreeLocal = @"<!DOCTYPE html>
<html>
<body>
<h1>This is a title</h1><ul>
<li><a href=""https://external.com""></li>
<li><a href=""/user/profile""></li>
<li><a href=""status""></li>
<li><a href=""https://example.com/news""></li>
<li><a href=""https://nonexisting.com""></li>
</body>
</html>
";

    [Fact]
    public async void GetLinks_GetFullyQualifiedAnchorLink()
    {
        Website website = new()
        {
            Name = "Example.com",
            BaseUrl = "https://example.com"
        };
        Scraper scraper = new Scraper();
        var list = scraper.GetLinks(website, html1);
        Assert.Single(list);

        Assert.Equal(new Uri("https://example.com"), list[0]);
    }
    [Fact]
    public async void GetLinks_GetOnlyLocalLinks()
    {
        Website website = new()
        {
            Name = "Example.com",
            BaseUrl = "https://example.com"
        };
        Scraper scraper = new Scraper();
        var list = scraper.GetLinks(website, twoExternalLinksThreeLocal);
        
        //Should only contain three links (local to the domain)
        Assert.Equal(3, list.Count);

        //First link should not contain a hostname
        Assert.Equal(string.Empty, list[0].Host);

        //Last link should contain a domain (the others are relative)
        Assert.Equal("example.com", list[2].Host);

        
    }
}