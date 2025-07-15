using Microsoft.Graph;
using OpenAI;
using OpenAI.Chat;
using System.Reflection;
using System.Threading.Tasks;
namespace Gepeteco
{
    public class ChatGepeteco
    {
        string apiKey = "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";
        var openAI = new OpenAIClient(apiKey);
        var chatRequest = new ChatRequest()
        {
            Model = Models.Gpt_3_5_Turbo,
            Messages =
            {
                new ChatMessage(ChatRole.User, "Olá, me explique como usar o OpenAI no C#.")
            }
        };

    }
}
