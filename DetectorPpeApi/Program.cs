using DetectorPpeApi.Authentication;
using DetectorPpeApi.Models;
using DetectorPpeApi.Services;
using Microsoft.OpenApi.Models;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

string vaultUri = Environment.GetEnvironmentVariable("VaultUri")
    ?? throw new InvalidOperationException("Vault URI not found.");

Uri keyVaultEndpoint = new(vaultUri);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

string authApikey = builder.Configuration["Authentication-ApiKey"]
    ?? throw new InvalidOperationException("API Key not found.");
string whatsAppApiAccessToken = builder.Configuration["WhatsAppApi-AccessToken"]
    ?? throw new InvalidOperationException("Access Token not found.");
string whatsAppApiPhoneNumberId = builder.Configuration["WhatsAppApi-PhoneNumberId"]
    ?? throw new InvalidOperationException("Phone Number ID not found.");

// Load configuration.
builder.Services.Configure<WhatsAppApiSettings>(builder.Configuration.GetSection("WhatsAppApi"));

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IWhatsAppService, WhatsAppService>();
builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
        Reference = new OpenApiReference
        {
            Id = "ApiKey",
            Type = ReferenceType.SecurityScheme
        },
        In = ParameterLocation.Header
    };
    OpenApiSecurityRequirement securityRequirements = new()
    {
        { securityScheme, Array.Empty<string>() }
    };
    opt.AddSecurityRequirement(securityRequirements);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
