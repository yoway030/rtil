using rtil.api;
using rtil.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ReviewDatabaseSettings>(builder.Configuration.GetSection("ReviewDatabase"));

builder.Services.AddSingleton<ReviewDatabaseService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "rtil is work");

//regex api 
app.MapPost("/regex_match", RegexApi.RegexMatch);
app.MapPost("/regex_capture", RegexApi.RegexCapture);
app.MapPost("/regex_replace", RegexApi.RegexReplace);

app.MapControllers();
app.Run();
