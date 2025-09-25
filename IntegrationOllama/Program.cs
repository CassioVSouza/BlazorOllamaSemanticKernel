using IntegrationOllama.Components;
using IntegrationOllama.Plugins;
using IntegrationOllama.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

var builder = WebApplication.CreateBuilder(args);
var builderKernel = Kernel.CreateBuilder()
    .AddOllamaChatCompletion(
    modelId: "llama3.1:latest",
    endpoint: new Uri("http://localhost:11434")
    );

var appKernel = builderKernel.Build();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<IIAIntegrationService, IAIntegrationService>();
builder.Services.AddSingleton<IChatCompletionService>(appKernel.GetRequiredService<IChatCompletionService>());
builder.Services.AddScoped<ProductsPlugin>();
builder.Services.AddScoped<ChatHistory>();
builder.Services.AddSingleton(appKernel);

var app = builder.Build();


using(var scope = app.Services.CreateScope())
{
    var pluginInstance = scope.ServiceProvider.GetRequiredService<ProductsPlugin>();
    appKernel.Plugins.AddFromObject(pluginInstance, "ProductsPlugin");
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
