using Aliyun.Api.LogService;

namespace H.Framework.Aliyun.Log
{
    public class LogCore
    {
        public LogCore(string projectName, string accessKeyId, string secretAccessKey, string endpoint)
        {
            Client = LogServiceClientBuilders.HttpBuilder
                           .Endpoint(endpoint, projectName)
                           .Credential(accessKeyId, secretAccessKey)
                           .Build();
            ProjectName = projectName;
        }

        public ILogServiceClient Client { get; }

        public string ProjectName { get; private set; }
    }
}