using rtil.api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "rtil is work");

app.MapPost("/regex_match", RegexApi.RegexMatch);
app.MapPost("/regex_capture", RegexApi.RegexCapture);
app.MapPost("/regex_replace", RegexApi.RegexReplace);

app.MapPost("/review_contents_parse", ReviewApi.ReviewContentsParse);
app.MapPost("/review_teamsmessageid_update", ReviewApi.ReviewTeamsMessageIdUpdate);

app.Run();
