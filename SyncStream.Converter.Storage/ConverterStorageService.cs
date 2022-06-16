using Amazon.S3;
using SyncStream.Aws.S3.Client;
using SyncStream.Converter.Storage.Config;
using SyncStream.Converter.Storage.Model;

// Define our namespace
namespace SyncStream.Converter.Storage;

/// <summary>
/// This class maintains the structure of our storage
/// </summary>
public static class ConverterStorageService
{
    /// <summary>
    /// This property contains the bucket to store documents in
    /// </summary>
    public static readonly string BucketName = Environment.GetEnvironmentVariable("SS_CONVERTER_BUCKET");

    /// <summary>
    /// This property contains the prefix to attach to object names in S3
    /// </summary>
    public static string BucketPrefix =
        Environment.GetEnvironmentVariable("SS_CONVERTER_BUCKET_PREFIX");

    /// <summary>
    /// This method constructs our static class with a configured client library
    /// </summary>
    static ConverterStorageService()
    {
        // Set the configuration details into our client library
        S3Client.WithConfiguration(new AwsSimpleStorageServiceConfig());
    }

    /// <summary>
    /// This method asynchronously lists the conversions that have been executed
    /// </summary>
    /// <returns>An awaitable task containing a list of object documents</returns>
    public static Task<List<ConversionDocumentModel>> History() =>
        S3Client.ListObjectsAsync<ConversionDocumentModel>($"{BucketName}/{BucketPrefix}");

    /// <summary>
    /// This method asynchronously reads a document model from S3
    /// </summary>
    /// <param name="documentId">The unique ID of the document to download</param>
    /// <param name="objectNameSuffix">Optional suffix to add to the object name <paramref name="documentId" />-<paramref name="objectNameSuffix" />.json</param>
    /// <returns></returns>
    public static Task<ConversionDocumentModel> ReadAsync(Guid documentId, string objectNameSuffix = null) =>
        S3Client.DownloadObjectAsync<ConversionDocumentModel>(
            string.IsNullOrEmpty(objectNameSuffix) || string.IsNullOrWhiteSpace(objectNameSuffix)
                ? $"{BucketName}/{BucketPrefix}/{documentId}.json"
                : $"{BucketName}/{BucketPrefix}/{documentId}-{objectNameSuffix}.json");

    /// <summary>
    /// This method resets the bucket prefix for the storage
    /// </summary>
    /// <param name="bucketPrefix"></param>
    public static void WithBucketPrefix(string bucketPrefix) =>
        BucketPrefix = bucketPrefix;

    /// <summary>
    /// This method asynchronously writes a document model to S3
    /// </summary>
    /// <param name="document">The document payload to save</param>
    /// <param name="objectNameSuffix">Optional suffix to add to the object name {document.Id}-<paramref name="objectNameSuffix" />.json</param>
    /// <returns>An awaitable task containing the document</returns>
    public static async Task<ConversionDocumentModel> WriteAsync(ConversionDocumentModel document,
        string objectNameSuffix = null)
    {
        // Check the document for an ID
        if (!document.Id.HasValue)
        {
            // Generate a new document ID
            document.Id = Guid.NewGuid();
            // Reset the creation timestamp
            document.Created = DateTimeOffset.UtcNow.DateTime;
        }
        
        // Reset the updated timestamp on the document
        document.Updated = DateTimeOffset.UtcNow.DateTime;

        // Define our object name
        string objectName = string.IsNullOrEmpty(objectNameSuffix) || string.IsNullOrWhiteSpace(objectNameSuffix)
            ? $"{BucketName}/{BucketPrefix}/{document.Id}.json"
            : $"{BucketName}/{BucketPrefix}/{document.Id}-${objectNameSuffix}.json";

        // Upload the document to S3
        await S3Client.UploadAsync<ConversionDocumentModel>(objectName, document, acl: S3CannedACL.Private);

        // We're done, return the unique ID of the document
        return document;
    }
}
