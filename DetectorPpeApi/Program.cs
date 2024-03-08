using DetectorPpeApi.Models;
using DetectorPpeApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Load configuration.
builder.Services.Configure<WhatsAppApiSettings>(builder.Configuration.GetSection("WhatsAppApi"));

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IWhatsAppService, WhatsAppService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
