using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Configuration;

namespace helpme.Helpers
{
    public class FileService
    {
        private readonly string _storageAccount = "helpmestorage";
        //private readonly string _key = Configuration.GetConnectionString("ApplicationDbContext");
        private readonly BlobContainerClient _filesContainer;
        private readonly IConfiguration _config;
        public FileService(IConfiguration configuration)
        {
            _config = configuration;
            var credencial = new StorageSharedKeyCredential(_storageAccount, _config.GetConnectionString("AzureStorage"));
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credencial);
            _filesContainer = blobServiceClient.GetBlobContainerClient("testing-helpme");
            
        }

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
        {
            BlobResponseDto response = new();
            BlobClient client =  _filesContainer.GetBlobClient(blob.FileName);

            await using (Stream? data = blob.OpenReadStream()) 
            {
                await client.UploadAsync(data, new BlobHttpHeaders { ContentType = blob.ContentType });
            }

            response.Status = $"File {blob.FileName} successfully updated.";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;

            return response;
        }

        

        
    }

    public class BlobDto
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }

    }

    public class BlobResponseDto
    {
        public BlobResponseDto()
        {
            Blob = new BlobDto();
        }

        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobDto Blob { get; set; }

    }
}
