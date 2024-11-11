using Azure;
using Azure.AI.OpenAI;
//using Azure.AI.OpenAI.Assistants;
using OpenAI.Assistants;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#pragma warning disable OPENAI001
internal class Program
{
    private static void Main(string[] args)
    {
        AssistantClient assistantClient = Program.SetupAssistantClient();
        AssistantCreationOptions assistantCreationOptions = Program.SetupAssistantCreationOptions();
        
        Program.RunAssistant(assistantClient, assistantCreationOptions).GetAwaiter().GetResult();
    }

    private static async Task RunAssistant(AssistantClient assistantClient, AssistantCreationOptions assistantCreationOptions)
    {
        Assistant assistant = await assistantClient.CreateAssistantAsync(Env.GetModelName(), assistantCreationOptions);

        ThreadInitializationMessage initialMessage = new ThreadInitializationMessage(MessageRole.User, [Console.ReadLine()]);

        AssistantThread thread = await assistantClient.CreateThreadAsync(new ThreadCreationOptions()
        {
            InitialMessages = { initialMessage },
        });

        RunCreationOptions runOptions = new RunCreationOptions()
        {
            AdditionalInstructions = "When possible, talk like a pirate."
        };

        await foreach (StreamingUpdate streamingUpdate in assistantClient.CreateRunStreamingAsync(thread.Id, assistant.Id, runOptions))
        {
            if (streamingUpdate.UpdateKind == StreamingUpdateReason.RunCreated)
            {
                Console.WriteLine("--- Run started! ---");
            }
            else if (streamingUpdate is MessageContentUpdate contentUpdate)
            {
                Console.Write(contentUpdate.Text);
                if (contentUpdate.ImageFileId is not null)
                {
                    Console.WriteLine($"[Image content file ID: {contentUpdate.ImageFileId}]");
                }
            }
        }

        await Task.Delay(1000);
        Console.WriteLine("Method completed.");
    }

    private static AssistantClient SetupAssistantClient()
    {
        Env.Init();

        string endpoint = Environment.GetEnvironmentVariable(Env.GetEndpointName());
        string key = Environment.GetEnvironmentVariable(Env.GetKeyName());

        if (string.IsNullOrEmpty(endpoint))
        {
            Console.WriteLine("Please set the environment variable AZURE_OPENAI_ENDPOINT:");
            endpoint = Console.ReadLine();
        }

        if (string.IsNullOrEmpty(key))
        {
            Console.WriteLine("Please set the environment variable AZURE_OPENAI_API_KEY:");
            key = Console.ReadLine();
        }

        return new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key)).GetAssistantClient();
    }

    private static AssistantCreationOptions SetupAssistantCreationOptions()
    {
        return new AssistantCreationOptions()
        {
            Name = Env.GetName()
            , Instructions = Env.GetInstruction()
            , Tools = { Env.GetTools() }
            , ToolResources = new ToolResources()
            //, Temperature = 1
            //, TopP = 1
        };
    }
}

internal class Env
{
    public static void Init()
    {
        Environment.SetEnvironmentVariable(Env.GetEndpointName(), Env.GetEndpoint());
        Environment.SetEnvironmentVariable(Env.GetKeyName(), Env.GetKey());
    }

    public static string GetEndpointName()
    {
        return "AZURE_OPENAI_ENDPOINT";
    }

    public static string GetEndpoint() 
    { 
        return "https://yume1.openai.azure.com/openai/deployments/yume4o-mini/chat/completions?api-version=2024-08-01-preview"; 
    }

    public static string GetKeyName()
    {
        return "AZURE_OPENAI_API_KEY";
    }

    public static string GetKey() 
    { 
        return "cf9566d1f8734c218d672df47cdb938e"; 
    }

    public static string GetModelName()
    {
        return "yume4o-mini";
    }

    public static string GetName()
    {
        return "RSS";
    }

    public static string GetInstruction()
    {
        return "Your name is Yume and you are the assistance of Richelle Silvia Support (RSS). RSS is committed to creating an environment that minimises the risk of incidents, ensures that its incident management processes are undertaken in accordance with the NDIS Practice Standards and Quality Indicators’ requirements, and that all of RSS’s workers understand and follow the company’s incident management approach. RSS will respond to all incidents as per the requirements of this policy & procedure and the Incident Management System (RSS-SY-002). All incidents are to be recorded in the Incident Report Form (RSS-FM-014) and the Incident Register (RSS-RG-003). RSS’s incident management processes, including this policy and procedure, ensures that all incidents are appropriately responded to, reported, and investigated.RSS will assess its incident management practices over time, in accordance with this policy and procedure, to ensure they are being complied with.";
    }

    public static CodeInterpreterToolDefinition GetTools()
    {
        return new CodeInterpreterToolDefinition();
    }
}
