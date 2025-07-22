using Amazon.S3;
using Amazon.S3.Transfer;
using DevSkill.Inventory.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure
{
    public class S3ImageService:IImageService
    {
        private readonly IAmazonS3 _s3;
        private readonly string _bucket;
        private readonly TimeSpan _expiry;
        public S3ImageService(IAmazonS3 s3, IConfiguration config)
        {
            _s3 = s3;
            _bucket = config["AWS:BucketName"]!;
            var mins = config.GetValue<double?>("AWS:UrlExpiryMinutes") ?? 15;
            _expiry = TimeSpan.FromMinutes(mins);
        }
        public async Task<string> SaveImageAsync(IFormFile imageFile, string folder)
        {
            var ext = Path.GetExtension(imageFile.FileName);
            var guid = Guid.NewGuid().ToString();
            var key = $"{folder.TrimEnd('/')}/{guid}{ext}";
            using var st = imageFile.OpenReadStream();
            var req = new TransferUtilityUploadRequest
            {
                BucketName = _bucket,
                Key = key,
                InputStream = st,
                ContentType = imageFile.ContentType,
                CannedACL = S3CannedACL.Private
            };
            await new TransferUtility(_s3).UploadAsync(req);
            return key;
        }

        public string GetPreSignedURL(string key)
        {
            var req = new Amazon.S3.Model.GetPreSignedUrlRequest
            {
                BucketName = _bucket,
                Key = key,
                Expires = DateTime.UtcNow.Add(_expiry),
                Verb = HttpVerb.GET
            };
            return _s3.GetPreSignedURL(req);
        }

        public async Task<bool> DeleteImageAsync(string key)
        {
            try
            {
                await _s3.DeleteObjectAsync(_bucket, key);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}