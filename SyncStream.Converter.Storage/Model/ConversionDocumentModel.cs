using System.Text.Json.Serialization;
using System.Xml.Serialization;

// Define our namespace
namespace SyncStream.Converter.Storage.Model;

/// <summary>
/// This class maintains the model structure of our conversion document
/// </summary>
[XmlInclude(typeof(ConversionDocumentDataSourceModel<>))]
[XmlInclude(typeof(List<ConversionDocumentDataSourceModel<dynamic>>))]
[XmlRoot("conversionDocumet")]
public class ConversionDocumentModel
{
    /// <summary>
    /// This property contains the timestamp at which the document was created
    /// </summary>
    [JsonPropertyName("created")]
    [XmlAttribute("created")]
    public DateTime Created { get; set; } = DateTimeOffset.UtcNow.DateTime;

    /// <summary>
    /// This property contains the public data sources
    /// </summary>
    [JsonPropertyName("dataSources")]
    [XmlElement("dataSource")]
    public List<ConversionDocumentDataSourceModel<dynamic>> DataSources { get; set; } = new();
    
    /// <summary>
    /// This property contains the timestamp at which the conversion finished
    /// </summary>
    [JsonPropertyName("finished")]
    [XmlAttribute("finished")]
    public DateTime? Finished { get; set; }
    
    /// <summary>
    /// This property contains the unique ID of the document
    /// </summary>
    [JsonPropertyName("id")]
    [XmlAttribute("id")]
    public Guid? Id { get; set; }
    
    /// <summary>
    /// This property contains the timestamp at which the conversion was started
    /// </summary>
    [JsonPropertyName("started'")]
    [XmlAttribute("started")]
    public DateTime? Started { get; set; }

    /// <summary>
    /// This property contains the timestamp at which the document was last updated
    /// </summary>
    [JsonPropertyName("updated")]
    [XmlAttribute("updated")]
    public DateTime Updated { get; set; } = DateTimeOffset.UtcNow.DateTime;

    /// <summary>
    /// This method fluidly adds a data source to the instance
    /// </summary>
    /// <param name="name">The unique name of the data source</param>
    /// <param name="values">The values of the data source</param>
    /// <typeparam name="TSource">The data source values type</typeparam>
    /// <returns>The current instance</returns>
    public ConversionDocumentModel AddDataSource<TSource>(string name, IEnumerable<TSource> values)
    {
        // Remove any existing data sources
        DataSources.RemoveAll(d => d.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

        // Instantiate our new data source
        DataSources.Add(
            new ConversionDocumentDataSourceModel<TSource>(name, values) as ConversionDocumentDataSourceModel<dynamic>);

        // We're done, return the instance
        return this;
    }

    /// <summary>
    /// This method adds a record to a data source in the instance
    /// </summary>
    /// <param name="dataSourceName">The name of the data source to modify</param>
    /// <param name="value">The value to add to the data source</param>
    /// <typeparam name="TSource">The data source value type</typeparam>
    /// <returns>The current instance</returns>
    public ConversionDocumentModel AddDataSourceRecord<TSource>(string dataSourceName, TSource value)
    {
        // Add the record to the data source
        (DataSources.First(d => d.Name.Equals(dataSourceName, StringComparison.CurrentCultureIgnoreCase)) as
            ConversionDocumentDataSourceModel<TSource>)?.WithRecord(value);

        // We're done, return the instance
        return this;
    }

    /// <summary>
    /// This method retrieves a typed data source from the instance
    /// </summary>
    /// <param name="dataSourceName">The name of the data source to retrieve</param>
    /// <typeparam name="TSource">The data source value type</typeparam>
    /// <returns>The data source named <paramref name="dataSourceName" /></returns>
    public ConversionDocumentDataSourceModel<TSource> GetDataSource<TSource>(string dataSourceName) =>
        DataSources.First(d => d.Name.Equals(dataSourceName, StringComparison.CurrentCultureIgnoreCase)) as
            ConversionDocumentDataSourceModel<TSource>;
}
