using System.Diagnostics.Tracing;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Scraper.Data.Models;

namespace Scraper.Data;
[XmlRoot("Config")]
public class Config
{
    public readonly static Config None = new();
    static string folder = "config";
    string filename = "";
    [XmlElement("Website")]
    public List<Website> Websites = new();
    public static Config Instance = None;
    public Config() : this("")
    {
    }
    public Config(string filename)
    {
        this.filename = filename;
    }

    public static Config Load(string filename)
    {
        string xml = File.ReadAllText(folder + Path.DirectorySeparatorChar + filename);
        Config instance = LoadXml(xml);
        instance.filename = filename;
        return instance;
    }
    public static Config LoadXml(string xml)
    {
        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        XmlSerializer serializer = new XmlSerializer(typeof(Config));

        XmlReader reader = XmlReader.Create(stream);

        Config instance = new();

        Config? deserialized = (Config?)serializer.Deserialize(reader);
        if (deserialized != null)
        {
            instance = deserialized;

        }

        Instance = instance;
        return instance;
    }
    public static void Save()
    {
        if (Instance == null) return;
        Save(Instance);
    }
    public static void Save(Config instance)
    {
        if (string.IsNullOrEmpty(instance.filename))
        {
            throw new FileNotFoundException("Missing config filename");
        }
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true
        };
        try
        {
            using (XmlWriter writer = XmlWriter.Create(folder + Path.DirectorySeparatorChar + instance.filename, settings))
            {
                XmlSerializer ser = new XmlSerializer(typeof(Config));
                ser.Serialize(writer, instance);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Unable to save config.xml: " + e.Message);
        }
    }
}