using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Data.SqlTypes;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using Scraper.Data.Models;
namespace Scraper.Data;
public enum Query {
    Insert, SelectByField
}
static class Table { 
    public static string Website = "websites"; 
    public static string PageType = "page_types"; 
    public static string ScrapeTarget = "scrape_target"; 
    public static string TargetProperty = "target_properties"; 
    public static string TargetData = "target_data"; 
}

public class Database 
{
    public static Database Instance;
    readonly string DATABASE_FILENAME;
    bool Exists => File.Exists(DATABASE_FILENAME);

    Dictionary<Query, string> queries = new() {
        {Query.Insert, "INSERT INTO {0} ({1}) VALUES ({2})"},
        {Query.SelectByField, "SELECT {0} FROM {1} WHERE {2} = '{3}'"},
    };

    public Database(string databaseFilename)
    {
        DATABASE_FILENAME = databaseFilename;
    }
    public static void Create(string databaseFilename) 
    {
        Instance = new Database(databaseFilename);
        if (Instance.Exists == false) 
        {
            string sql = File.ReadAllText("Data/database.sql");
            Instance.execute(sql);
        }
    }

    async Task execute(string sql, Dictionary<string, object>? parameters = null) 
    {
        using (SQLiteConnection conn = new SQLiteConnection("Data Source="+DATABASE_FILENAME))
        {
            await conn.OpenAsync();
            SQLiteCommand command = conn.CreateCommand();
            command.CommandText = sql;
            if (parameters != null) 
            {
                foreach (var param in parameters) 
                {
                    command.Parameters.AddWithValue($"@{param.Key}", param.Value);
                }
            }
            await command.ExecuteNonQueryAsync();
        }
    }
    async Task<List<T>> query<T>(string sql, Dictionary<string, object>? parameters = null) where T : new()
    {
        List<T> result = new();
        using (SQLiteConnection conn = new SQLiteConnection("Data Source="+DATABASE_FILENAME))
        {
            await conn.OpenAsync();
            SQLiteCommand command = conn.CreateCommand();
            command.CommandText = sql;
            if (parameters != null) 
            {
                foreach (var param in parameters) 
                {
                    //command.Parameters.AddWithValue($"@{param.Key}", param.Value);
                }
            }
            var reader = await command.ExecuteReaderAsync();
            var props = typeof(T).GetProperties();
            while (reader.Read()) {
                T record = new();
                for (int i = 0; i < reader.FieldCount; i++) {
                    foreach (var p in props) 
                    {
                        object[] attribs = p.GetCustomAttributes(typeof(FieldAttribute), false);
                        if (attribs.Length == 0) continue;
                        if ((attribs[0] as FieldAttribute).Name == reader.GetName(i)) {
                            p.SetValue(record, reader.GetValue(i));
                        }
                    }
                }
                result.Add(record);
            }
        }
        return result;
    }

    public async Task Insert(Model record) 
    {
        string tableName = string.Empty;
        List<string> fields = new();
        List<string> paramKeys = new();
        var typeInfo = record.GetType();
        object[] tableAttributes = typeInfo.GetCustomAttributes(typeof(TableAttribute), false);
        if (tableAttributes.Length > 0) 
        {
            tableName = (tableAttributes[0] as TableAttribute).Name;
        }
        
        Dictionary<string, object> paramValues = new();
        var properties = typeInfo.GetProperties();
        foreach (var prop in properties) 
        {
            object[] attributes = prop.GetCustomAttributes(typeof(FieldAttribute), false);
            if (attributes.Length > 0) 
            {
                if ((attributes[0] as FieldAttribute).AutoIncrement) continue;
                string fieldName = (attributes[0] as FieldAttribute).Name;
                paramValues.Add(fieldName, prop.GetValue(record));
                fields.Add(fieldName);
                paramKeys.Add("@"+fieldName);
            }
        }
        
        string sql = string.Format(queries[Query.Insert], tableName, string.Join(",",fields), string.Join(",",paramKeys));
        await execute(sql, paramValues);
    }

    public async Task<List<T>> SelectById<T>(long id) where T : new()
    {
        List<T> result = new();
        string fields = "*";
        string table = "target_properties";
        string key = "page_type_id";
        string sql = string.Format(queries[Query.SelectByField], fields, table, key, id);
        result = await query<T>(sql, new Dictionary<string, object> {
            {"@value", id}
        });

        return result;
    }
}