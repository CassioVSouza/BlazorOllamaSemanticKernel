namespace IntegrationOllama.Services
{
    public interface IIAIntegrationService
    {
        Task<string> GetIAAnswer(string prompt);
        Task<string?> GetIAAnserViaSemanticKernel(string prompt);
        Task<List<Produtos>> GetAllDatabaseProducts();
    }
}
