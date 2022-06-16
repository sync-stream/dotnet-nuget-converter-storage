using System.Text.Json.Serialization;
using System.Xml.Serialization;

// Define our namespace
namespace SyncStream.Converter.Storage.Model;

/// <summary>
/// This class maintains the model structure of a conversion data source
/// </summary>
/// <typeparam name="TSource">The type the source records carry</typeparam>
[XmlRoot("dataSource")]
public class ConversionDocumentDataSourceModel<TSource>
{
    /// <summary>
    /// This property contains the name of the data source
    /// </summary>
    [JsonPropertyName("name")]
    [XmlAttribute("name")]
    public string Name { get; set; }

    /// <summary>
    /// This property contains the values of the data source
    /// </summary>
    [JsonPropertyName("source")]
    [XmlElement("value")]
    public List<TSource> Source { get; set; } = new();

    /// <summary>
    /// This method instantiates our data source
    /// </summary>
    public ConversionDocumentDataSourceModel() { }

    /// <summary>
    /// This method instantiates our data source with a name and values
    /// </summary>
    /// <param name="name">The unique name of the data source</param>
    /// <param name="values">The values for the data source</param>
    public ConversionDocumentDataSourceModel(string name, IEnumerable<TSource> values)
    {
        // Set the name into the instance
        Name = name;
        
        // Iterate over the values and add them to the instance
        foreach (TSource value in values) WithRecord(value);
    }

    /// <summary>
    /// This method fluidly adds a source record into the data source
    /// </summary>
    /// <param name="source">The new source record</param>
    /// <returns>The current instance of the data source</returns>
    public ConversionDocumentDataSourceModel<TSource> WithRecord(TSource source)
    {
        // Add the source record to the data source
        Source.Add(source);
        
        // We're done, return the instance
        return this;
    }
}
