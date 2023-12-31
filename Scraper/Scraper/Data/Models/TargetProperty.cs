using System;
namespace Scraper.Data.Models;
[Table(Name = "target_properties")]
public class TargetProperty : Model
{
    [Field(Name = "property_id", AutoIncrement = true)]
    public long PropertyId { get; set;} = 0;
    [Field(Name = "property")]
    public string Property {get;set;} = string.Empty;
    [Field(Name = "data_type")]
    public long DataType {get;set;} = 0;
    [Field(Name = "xpath")]
    public string? Xpath {get;set;} = string.Empty;

    [Field(Name = "selector")]
    public string? Selector {get;set;} = string.Empty;

    [Field(Name = "page_type_id")]
    public long PageTypeId {get;set;} = 0;
}