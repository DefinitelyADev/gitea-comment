using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace IT.GiteaComment
{
    class Program
    {
        

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            var configuration = builder.Build();


            string GITEA_BASE_URL = Environment.GetEnvironmentVariable("PLUGIN_GITEA_BASE_URL");
            string GITEA_TOKEN = Environment.GetEnvironmentVariable("PLUGIN_GITEA_TOKEN");
            string DRONE_REPO_NAMESPACE = Environment.GetEnvironmentVariable("DRONE_REPO_NAMESPACE");
            string DRONE_REPO_NAME = Environment.GetEnvironmentVariable("DRONE_REPO_NAME");
            string DRONE_PULL_REQUEST = Environment.GetEnvironmentVariable("DRONE_PULL_REQUEST");
            string COMMENT_TITLE = Environment.GetEnvironmentVariable("PLUGIN_COMMENT_TITLE");
            string TEXT_COMMENT = Environment.GetEnvironmentVariable("PLUGIN_COMMENT");
            string COMMENT_FROM_FILE = Environment.GetEnvironmentVariable("PLUGIN_COMMENT_FROM_FILE");

            Console.WriteLine(COMMENT_FROM_FILE);
            Console.WriteLine(TEXT_COMMENT);
            if (string.IsNullOrEmpty(COMMENT_FROM_FILE) && string.IsNullOrWhiteSpace(COMMENT_FROM_FILE) && string.IsNullOrEmpty(TEXT_COMMENT) && string.IsNullOrWhiteSpace(TEXT_COMMENT))
            {
                throw new Exception("COMMENT_FROM_FILE and TEXT_COMMENT cannot both be empty!");
            }

            if (
                (string.IsNullOrEmpty(GITEA_TOKEN) && string.IsNullOrWhiteSpace(GITEA_TOKEN)) || 
                (string.IsNullOrEmpty(GITEA_BASE_URL) && string.IsNullOrWhiteSpace(GITEA_BASE_URL)) || 
                (string.IsNullOrEmpty(DRONE_REPO_NAMESPACE) && string.IsNullOrWhiteSpace(DRONE_REPO_NAMESPACE)) || 
                (string.IsNullOrEmpty(DRONE_REPO_NAME) && string.IsNullOrWhiteSpace(DRONE_REPO_NAME)) || 
                (string.IsNullOrEmpty(DRONE_PULL_REQUEST) && string.IsNullOrWhiteSpace(DRONE_PULL_REQUEST))
            )
            {
                throw new Exception("GITEA_TOKEN, GITEA_BASE_URL, DRONE_REPO_NAMESPACE, DRONE_REPO_NAME and DRONE_PULL_REQUEST cannot be empty!");
            }

            string comment = string.Empty;
            if (!string.IsNullOrEmpty(COMMENT_FROM_FILE) && !string.IsNullOrWhiteSpace(COMMENT_FROM_FILE))
            {
                string title = COMMENT_TITLE.ToString().Trim();
                title = !string.IsNullOrEmpty(title) ? title : "Gitea Comment";
                Console.WriteLine("Reading from file");
                comment = $"## {title}\\n```text\\n"+System.IO.File.ReadAllText(COMMENT_FROM_FILE.ToString()).Trim().Replace(Environment.NewLine, "\\n")+"\\n```";
            }
            else
            {
                Console.WriteLine("Reading from specified text");
                comment = TEXT_COMMENT.ToString().Trim();
            }

            string postBody = $"{{\"Body\":\"{comment}\"}}";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GITEA_BASE_URL.ToString().Trim());
            client.DefaultRequestHeaders.Add("Authorization", $"token {GITEA_TOKEN.ToString().Trim()}");
            HttpContent content = new StringContent(postBody, System.Text.Encoding.UTF8, "application/json");
            await client.PostAsync($"/api/v1/repos/{DRONE_REPO_NAMESPACE}/{DRONE_REPO_NAME}/issues/{DRONE_PULL_REQUEST}/comments", content);
            content.Dispose();
        }
    }
}
