using System.Text.RegularExpressions;
using rtil.Models;
using Microsoft.EntityFrameworkCore;

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

app.MapPost("/regex_capture", (RegexCaptureRequest request) =>
{
    if (request.text == null ||
        request.pattern == null)
    {
        return Results.BadRequest();
    }
    
    List<RegexCaptureResponse> matchedList = new List<RegexCaptureResponse>();

    Regex rgx = new Regex(request.pattern);
    Match match = rgx.Match(request.text);
    if (match.Success == false)
    {
        Results.NotFound();
    }

    string[] names = rgx.GetGroupNames();
    foreach (var name in names)
    {
        Group grp = match.Groups[name];
        matchedList.Add(new RegexCaptureResponse{name = name, value = grp.Value});
    }

    return Results.Ok(matchedList);
});

app.MapPost("/regex_replace", (RegexReplaceRequest request) =>
{
    if (request.text == null ||
        request.pattern == null ||
        request.to == null)
    {
        return Results.BadRequest();
    }

    string result = Regex.Replace(request.text, request.pattern, request.to);

    return Results.Ok(result);
});

app.MapGet("/messageid_from_reviewtitle", () => 
{
});

app.Run();
