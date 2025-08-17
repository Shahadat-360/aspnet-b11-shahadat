using Amazon.S3;
using Amazon.S3.Model;
using DevSkill.Inventory.Application.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using System.Drawing.Imaging;

namespace DevSkill.Inventory.Worker
{
    public class ImageResizeWorker : BackgroundService
    {
        private readonly ILogger<ImageResizeWorker> _logger;
        private readonly ISqsService _sqs;
        private readonly IAmazonS3 _s3;
        private readonly string _bucket;
        private readonly IConfiguration _configuration;

        public ImageResizeWorker(
            ISqsService sqs,
            IAmazonS3 s3,
            IConfiguration configuration,
            ILogger<ImageResizeWorker> logger)
        {
            _sqs = sqs;
            _s3 = s3;
            _configuration = configuration;
            _bucket = configuration["AWS:BucketName"]!;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var messages = await _sqs.ReceiveMessagesAsync();
                if (messages == null || messages.Count == 0)
                {
                    await Task.Delay(5000, stoppingToken);
                    continue;
                }

                foreach (var message in messages)
                {
                    Guid guid;
                    string? ext;
                    try
                    {
                        guid = Guid.Parse(Path.GetFileNameWithoutExtension(message.Body));
                        ext = Path.GetExtension(message.Body);
                    }
                    catch (FormatException)
                    {
                        _logger.LogError("Invalid GUID in message {MessageId}: {Body}", message.MessageId, message.Body);
                        await _sqs.DeleteMessageAsync(message.ReceiptHandle);
                        continue;
                    }

                    var origKey = $"{_configuration["ImageUploadSettings:Product"]}/{guid}{ext}";

                    try
                    {
                        bool objectExists = await CheckObjectExistsAsync(_bucket, origKey);

                        if (!objectExists)
                        {
                            _logger.LogWarning("Image not found in S3: {OrigKey}. Skipping processing.", origKey);
                            await _sqs.DeleteMessageAsync(message.ReceiptHandle);
                            continue;
                        }

                        using var getResponse = await _s3.GetObjectAsync(_bucket, origKey);
                        using var originalImage = Image.FromStream(getResponse.ResponseStream);

                        using var resizedImage = new Bitmap(originalImage, new Size(400, 400));
                        using var memoryStream = new MemoryStream();

                        resizedImage.Save(memoryStream, ImageFormat.Jpeg);
                        memoryStream.Position = 0;
                        
                        await _s3.DeleteObjectAsync(_bucket, origKey);
                        _logger.LogInformation("Original image deleted: {OrigKey}", origKey);

                        await _s3.PutObjectAsync(new PutObjectRequest
                        {
                            BucketName = _bucket,
                            Key = origKey,
                            InputStream = memoryStream,
                            ContentType = "image/jpeg"
                        });

                        _logger.LogInformation("Resized image uploaded with same key: {OrigKey}", origKey);

                        await _sqs.DeleteMessageAsync(message.ReceiptHandle);
                    }
                    catch (AmazonS3Exception ex) when (ex.ErrorCode == "NoSuchKey")
                    {
                        _logger.LogWarning("Image not found in S3: {OrigKey}. Message will be removed from queue.", origKey);
                        await _sqs.DeleteMessageAsync(message.ReceiptHandle);
                    }
                    catch (AmazonS3Exception ex)
                    {
                        _logger.LogError(ex, "S3 error processing image {Guid} with key {OrigKey}", guid, origKey);
                        await _sqs.DeleteMessageAsync(message.ReceiptHandle);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error processing image {Guid} with key {OrigKey}", guid, origKey);
                        await _sqs.DeleteMessageAsync(message.ReceiptHandle);
                    }
                }

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task<bool> CheckObjectExistsAsync(string bucketName, string key)
        {
            try
            {
                await _s3.GetObjectMetadataAsync(bucketName, key);
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.ErrorCode == "NotFound" || ex.ErrorCode == "NoSuchKey")
            {
                return false;
            }
        }
    }
}