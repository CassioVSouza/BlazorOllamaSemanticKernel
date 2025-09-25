using IntegrationOllama.Services;
using Microsoft.SemanticKernel;

namespace IntegrationOllama.Plugins
{
    public class ProductsPlugin
    {

        private readonly IIAIntegrationService _integrationService;

        public ProductsPlugin(IIAIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        [KernelFunction("get_all_products")]
        public async Task<List<Produtos>> GetAllProducts()
        {
            return await _integrationService.GetAllDatabaseProducts();
        }
    }
}
