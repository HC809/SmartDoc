using SmartDoc.BL;
using SmartDoc.BL.Services.InvoiceAnalyze;
using SmartDoc.BL.Services.SentimentAnalysis;
using SmartDoc.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DocumentAnalysisSettings>(builder.Configuration.GetSection("AzureCognitiveServices"));
builder.Services.Configure<SentimentAnalysisSettings>(builder.Configuration.GetSection("SentimentAnalysis"));

builder.Services.AddBusinessLogic();
builder.Services.AddDataAccess(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
