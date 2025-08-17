using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface ISqsService
    {
        Task<List<Message>> ReceiveMessagesAsync(int maxMessages = 5);
        Task DeleteMessageAsync(string receiptHandle);
        Task SendKeyAsync(string key);
    }
}
