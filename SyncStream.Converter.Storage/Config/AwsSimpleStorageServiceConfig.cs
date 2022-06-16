using System.Text.Json.Serialization;
using System.Xml.Serialization;
using SyncStream.Aws.S3.Client.Config;

// Define our namespace
namespace SyncStream.Converter.Storage.Config;

/// <summary>
/// This class maintains the structure of our AWS S3 configuration values
/// </summary>
[XmlRoot("awsSimpleStorageServiceConfiguration")]
public class AwsSimpleStorageServiceConfig : IS3ClientConfig
{
    /// <summary>
    /// This property contains our AWS Access Key
    /// </summary>
    [JsonPropertyName("accessKeyId")]
    [XmlAttribute("accessKeyId")]
    public string AccessKeyId { get; set; } = Environment.GetEnvironmentVariable("SS_AWS_ACCESS_KEY_ID");

    /// <summary>
    /// This property contains the AWS KMS key ID to use for encryption
    /// </summary>
    [JsonPropertyName("keyManagementServiceKeyId")]
    [XmlAttribute("keyManagementServiceKeyId")]
    public string KeyManagementServiceKeyId { get; set; } = Environment.GetEnvironmentVariable("SS_AWS_KEY_MANAGEMENT_SERVICE_KEY_ID");
    
    /// <summary>
    /// This property contains the region in which assets reside
    /// </summary>
    [JsonPropertyName("region")]
    [XmlAttribute("region")]
    public string Region { get; set; } = Environment.GetEnvironmentVariable("SS_AWS_REGION");

    /// <summary>
    /// This property contains our AWS Secret Key
    /// </summary>
    [JsonPropertyName("secretAccessKey")]
    [XmlAttribute("secretAccessKey")]
    public string SecretAccessKey { get; set; } = Environment.GetEnvironmentVariable("SS_AWS_SECRET_ACCESS_KEY");
}
