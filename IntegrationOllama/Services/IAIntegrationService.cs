using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using OllamaSharp;

namespace IntegrationOllama.Services
{
    public class IAIntegrationService : IIAIntegrationService
    {
        private readonly IChatCompletionService _chatCompletionService;
        private ChatHistory _chatMessageContents;
        private readonly Kernel _kernel;

        public IAIntegrationService(IChatCompletionService chatCompletion, ChatHistory chatHistory, Kernel kernel) {
            _chatCompletionService = chatCompletion;
            _chatMessageContents = chatHistory; 
            _kernel = kernel;
        }

        public async Task<string> GetIAAnswer(string prompt)
        {
            try
            {
                var url = new Uri("http://localhost:11434");
                var client = new OllamaApiClient(url);

                client.SelectedModel = "llama3.1:latest";

                var chat = new Chat(client);
                var returnAnswer = "";

                await foreach(var answer in chat.SendAsync(prompt))
                {
                    returnAnswer += answer;
                }

                return returnAnswer;
            }catch(Exception)
            {
                return "";
            }
        }

        public async Task<string?> GetIAAnserViaSemanticKernel(string prompt)
        {
            try
            {

                _chatMessageContents.AddUserMessage(prompt);

                OllamaPromptExecutionSettings settings = new OllamaPromptExecutionSettings()
                {
                    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                };
                     
                var answer = await _chatCompletionService.GetChatMessageContentAsync(
                    _chatMessageContents,
                    executionSettings: settings,
                    _kernel);

                _chatMessageContents.AddAssistantMessage(answer.Content ?? string.Empty);

                return answer.Content;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public async Task<List<Produtos>> GetAllDatabaseProducts()
        {
            
            return await Task.FromResult(new List<Produtos>
            {
                new Produtos { Nome = "Notebook Dell XPS", Quantidade = 15, Preco = 5499.99m },
                new Produtos { Nome = "Monitor Ultrawide LG", Quantidade = 30, Preco = 1899.90m },
                new Produtos { Nome = "Mouse Logitech MX", Quantidade = 50, Preco = 299.90m },
                new Produtos { Nome = "Teclado Mecânico Keychron", Quantidade = 25, Preco = 649.99m },
                new Produtos { Nome = "Headset Gamer HyperX", Quantidade = 40, Preco = 349.90m },
                new Produtos { Nome = "SSD Samsung 1TB", Quantidade = 65, Preco = 599.90m },
                new Produtos { Nome = "Webcam Logitech C920", Quantidade = 20, Preco = 399.90m },
                new Produtos { Nome = "Cadeira Gamer ThunderX3", Quantidade = 10, Preco = 1299.90m },
                new Produtos { Nome = "Impressora HP LaserJet", Quantidade = 8, Preco = 1899.90m },
                new Produtos { Nome = "Placa de Vídeo RTX 4060", Quantidade = 12, Preco = 2499.90m }
            });
        }
    }

    public class Produtos()
    {
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
