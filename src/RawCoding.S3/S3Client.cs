using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace RawCoding.S3
{
    public class S3Client
    {
        private readonly S3StorageSettings _settings;
        private readonly AmazonS3Client _client;

        public S3Client(S3StorageSettings settings)
        {
            _settings = settings;

            var config = new AmazonS3Config
            {
                ServiceURL = $"https://{_settings.Server}",
            };

            _client = new AmazonS3Client(
                settings.AccessKey,
                settings.SecretKey,
                config);
        }

        public async Task<string> SavePublicFile(string fileName, Stream fileStream)
        {
            var extension = Path.GetExtension(fileName);
            await _client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _settings.Bucket,
                Key = $"{_settings.RootPath}/{fileName}",
                ContentType = ContentTypeFactory.ResolveContentType(extension),
                InputStream = fileStream,
                CannedACL = S3CannedACL.PublicRead,
            });

            return ImagePath(fileName);
        }

        public async Task<string> SavePublicFile(string fileName, ContentType contentType, Stream fileStream)
        {
            await _client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _settings.Bucket,
                Key = $"{_settings.RootPath}/{fileName}",
                ContentType = ContentTypeFactory.ResolveContentType(contentType),
                InputStream = fileStream,
                CannedACL = S3CannedACL.PublicRead,
            });

            return ImagePath(fileName);
        }

        private string ImagePath(string fileName) => $"https://{_settings.Bucket}.{_settings.Server}/{_settings.RootPath}/{fileName}";
    }
}