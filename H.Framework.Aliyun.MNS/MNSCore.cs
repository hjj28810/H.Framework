using Aliyun.MNS;
using Aliyun.MNS.Model;
using System;

namespace H.Framework.Aliyun.MNS
{
    public class MNSCore
    {
        private readonly IMNS _client;
        private AsyncCallback _receiveMessageCallback;

        public MNSCore(string queueName, string accessKeyId, string secretAccessKey, string endpoint)
        {
            _client = new MNSClient(accessKeyId, secretAccessKey, endpoint);
            QueueName = queueName;
        }

        public string QueueName { get; private set; }

        public Queue Queue => _client.GetNativeQueue(QueueName);

        public SendMessageResponse Send(string messageContent)
        {
            try
            {
                return Queue.SendMessage(new SendMessageRequest(messageContent));
            }
            catch (Exception ex)
            {
                Console.WriteLine("AsyncSend message failed, exception info: " + ex.Message);
                return null;
            }
        }

        public void SendAsync(string messageContent, AsyncCallback sendMessageCallback)
        {
            try
            {
                var sendMessageRequest = new SendMessageRequest(messageContent);
                Queue.BeginSendMessage(sendMessageRequest, sendMessageCallback, Queue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AsyncSend message failed, exception info: " + ex.Message);
            }
        }

        public void RegReceiveMessageCallback(AsyncCallback receiveMessageCallback)
        {
            _receiveMessageCallback += receiveMessageCallback;
        }

        public Message Receive()
        {
            try
            {
                return Queue.ReceiveMessage(30).Message;
            }
            catch (Exception ex)
            {
                Console.WriteLine("AsyncReceive message failed, exception info: " + ex.Message);
                return null;
            }
        }

        public void ReceiveAsync()
        {
            try
            {
                Queue.BeginReceiveMessage(new ReceiveMessageRequest(), _receiveMessageCallback, Queue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AsyncReceive message failed, exception info: " + ex.Message);
            }
        }

        public DeleteMessageResponse Delete(string receiptHandle)
        {
            try
            {
                var deleteMessageRequest = new DeleteMessageRequest(receiptHandle);
                return Queue.DeleteMessage(deleteMessageRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AsyncDeleteMessage failed, exception info: " + ex.Message);
                return null;
            }
        }

        public void DeleteAsync(string receiptHandle, AsyncCallback deleteMessageCallback)
        {
            try
            {
                var deleteMessageRequest = new DeleteMessageRequest(receiptHandle);
                Queue.BeginDeleteMessage(deleteMessageRequest, deleteMessageCallback, Queue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AsyncDeleteMessage failed, exception info: " + ex.Message);
            }
        }
    }
}