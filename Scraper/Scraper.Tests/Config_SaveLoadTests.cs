using System.IO.Enumeration;
using Scraper.Data;
using Scraper.Data.Models;
namespace Scraper.Tests;

public class Config_SaveLoadTests
{
    const string xml1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Config xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Website Name=""Google"" BaseUrl=""https://google.com"" Created=""2023-10-18T14:16:03.1220285+02:00"">
    <Target NextVisit=""2023-10-18T14:16:03.136351+02:00"" LastHttpStatusCode=""-1"">/</Target>
  </Website>
  <Website Name=""DR"" BaseUrl=""https://dr.dk"" Created=""2023-10-18T14:16:03.1364591+02:00"">
    <Target NextVisit=""2023-10-18T14:16:03.1364604+02:00"" LastHttpStatusCode=""-1"">/</Target>
    <Target NextVisit=""2023-10-18T14:16:03.1364607+02:00"" LastHttpStatusCode=""-1"">/nyheder</Target>
  </Website>
</Config>";
const string xml2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Website xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" Name=""Example"" BaseUrl=""https://example.com"" Created=""2023-10-18T21:14:54.9416101+02:00"" LastScrape=""2023-10-24T22:42:47.2650521+02:00"">
  <Latency>120</Latency>
</Website>";
    void seed(Config config)
    {
        if (!Directory.Exists("config")) Directory.CreateDirectory("config");
        config.Websites.Add(
            new Website()
            {
                Name = "Google",
                Targets = new()
                {
                    new ScrapeTarget
                    {
                        Url = "/",
                        NextVisit = DateTime.Now,
                        LastHttpStatusCode = -1
                    },
                    new ScrapeTarget
                    {
                        Url = "/wait",
                        NextVisit = DateTime.Now,
                        LastHttpStatusCode = -1
                    },
                }
            });
        config.Websites.Add(
            new Website()
            {
                Name = "DR",
                Targets = new()
                {
                    new ScrapeTarget
                    {
                        Url = "/",
                        NextVisit = DateTime.Now,
                        LastHttpStatusCode = -1
                    },
                    new ScrapeTarget
                    {
                        Url = "/nyheder",
                        NextVisit = DateTime.Now,
                        LastHttpStatusCode = -1
                    },
                }
            }
        );
    }
    void cleanup()
    {
        if (Directory.Exists("config"))
            Directory.Delete("config");
        
    }
    [Fact]
    public void SaveConfig_ShouldSave()
    {
        string filename = Path.GetRandomFileName() + ".xml";
        if (!Directory.Exists("config")) Directory.CreateDirectory("config");
        Config config = new(filename);
        seed(config);

        Config.Save(config);
        string path = "config" + Path.DirectorySeparatorChar + filename;
        Assert.True(File.Exists(path));
        File.Delete(path);
    }

    [Fact]
    public void SaveConfig_NofilenameShouldThrowException()
    {
        Config config = new();
        seed(config);
        try
        {
            Config.Save(config);
            Assert.Fail("");
        }
        catch (FileNotFoundException)
        {
            //Expected behaviour
        }
        catch (Xunit.Sdk.FailException)
        {
            Assert.Fail("Should have thrown FileNotFoundException");
        }
        catch (Exception e)
        {
            Assert.Fail("Caught wrong exception type. Expecting FileNotFoundException: " + e.GetType());
        }
    }
    [Fact]
    public void LoadConfig_ShouldSucceed()
    {
        Config instance = Config.LoadXml(xml1);
        Assert.Equal(2, instance.Websites.Count);
        Assert.Equal(2, instance.Websites[1].Targets.Count);

        Assert.Equal("DR", instance.Websites[1].Name);
        Assert.Equal("https://dr.dk", instance.Websites[1].BaseUrl);
        Assert.Equal(DateTime.Parse("2023-10-18T14:16:03.1364591+02:00"), instance.Websites[1].Created);
    }

    [Fact]
    public void LoadConfigWebsite_ShouldSucceed()
    {
        seed(new Config());
        string filename = Path.GetRandomFileName()+".xml";
        File.WriteAllText(filename, xml2);

        Website? website = Config.LoadWebsite(filename);
        File.Delete(filename); //Clean up
        
        //Cannot load a website where filename and BaseUrl does not match
        Assert.Null(website);
        
        filename = "example.com.xml";
        File.WriteAllText(filename, xml2);
        website = Config.LoadWebsite(filename);
        //Filename matches XML, should therefore not be null
        Assert.NotNull(website);
        

        
        cleanup();
    }
}