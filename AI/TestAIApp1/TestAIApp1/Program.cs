using Azure;
using Azure.AI.OpenAI.Assistants;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Threading.Tasks;

string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
string key = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");

if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
{
    Console.WriteLine("Please set the environment variables AZURE_OPENAI_ENDPOINT and AZURE_OPENAI_API_KEY.");
    return;
}

AzureOpenAIClient azureClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
#pragma warning disable OPENAI001
AssistantClient assistantClient = azureClient.GetAssistantClient();

AssistantCreationOptions assistantCreationOptions = new AssistantCreationOptions("yume4o-mini")
{
    Name = "RSS",
    Instructions = "Your name is Yume and you are the assistance of Richelle Silvia Support (RSS).

RSS is committed to creating an environment that minimises the risk of incidents,
    ensures that its incident management processes are undertaken in accordance with the NDIS Practice Standards and Quality Indicators’ requirements,
    and that all of RSS’s workers understand and follow the company’s incident management approach.

RSS will respond to all incidents as per the requirements of this policy & procedure and the Incident Management System(RSS - SY - 002).All incidents are to be recorded in the Incident Report Form(RSS - FM - 014) and the Incident Register(RSS - RG - 003).
    RSS’s incident management processes,
    including this policy and procedure,
    ensures that all incidents are appropriately responded to,
    reported,
    and investigated.
RSS will assess its incident management practices over time, in accordance with this policy and procedure,
    to ensure they are being complied with.
",
  Tools = { ToolDefinition.CreateCodeInterpreter() },
    ToolResources = { "file_search":{ "vector_store_ids":["vs_VBLwd1Nd54jwK4nitSAHYeKh"]}},
  Temperature = 1,
  TopP = 1
};

Assistant assistant = await assistantClient.CreateAssistantAsync(assistantCreationOptions);
