using Amazon.SQS;
using Amazon.SQS.Model;
using DevSkill.Inventory.Application.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Services
{
    public class SqsService: ISqsService
    {
        private readonly IAmazonSQS _sqs;
        private readonly string _url;

        public SqsService(IAmazonSQS sqs, IConfiguration cfg)
        {
            _sqs = sqs;
            _url = cfg["AWS:SqsUrl"]!;
        }

        public async Task<List<Message>> ReceiveMessagesAsync(int maxMessages = 5)
        {
            var resp = await _sqs.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = _url,
                MaxNumberOfMessages = maxMessages,
                WaitTimeSeconds = 10
            });
            return resp.Messages;
        }

        public Task DeleteMessageAsync(string receiptHandle)
            => _sqs.DeleteMessageAsync(_url, receiptHandle);

        public async Task SendKeyAsync(string key)
        {
            await _sqs.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _url,
                MessageBody = key
            });
        }
    }
}
