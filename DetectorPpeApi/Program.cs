using DetectorPpeApi.Authentication;
using DetectorPpeApi.Models;
using DetectorPpeApi.Services;
using Microsoft.OpenApi.Models;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Configure Azure Key Vault
string? vaultUri = Environment.GetEnvironmentVariable("VaultUri");

if (string.IsNullOrEmpty(vaultUri))
{
    throw new InvalidOperationException("Vault URI not found.");
}

builder.Configuration.AddAzureKeyVault(new(vaultUri), new DefaultAzureCredential());

// Get WhatsApp API configuration
builder.Services.Configure<WhatsAppApiSettings>(builder.Configuration.GetSection("WhatsAppApi"));

// Add services to the container
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IWhatsAppService, WhatsAppService>();
builder.Services.AddScoped<ApiKeyAuthFilter>();

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger with API Key authentication
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key is required",
        In = ParameterLocation.Header,
        Name = "x-api-key",
        Scheme = "ApiKeyScheme",
        Type = SecuritySchemeType.ApiKey
    });
    OpenApiSecurityScheme securityScheme = new()
    {
        Reference = new OpenApiReference { Id = "ApiKey", Type = ReferenceType.SecurityScheme },
        In = ParameterLocation.Header
    };
    OpenApiSecurityRequirement securityRequirements = new() { { securityScheme, Array.Empty<string>() } };
    opt.AddSecurityRequirement(securityRequirements);
});

var app = builder.Build();

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
