using System.Text.RegularExpressions;
using rtil.Models;

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

app.MapPost("/regex_match", (RegexMatchRequest request) =>
{
    if (request.text == null ||
        request.pattern == null)
    {
        return Results.BadRequest();
    }
    
    Regex rgx = new Regex(request.pattern);
    Match match = rgx.Match(request.text);

    List<RegexMatchResponse> matchedList = new List<RegexMatchResponse>();

    while (match.Success == true)
    {
        RegexMatchResponse regexMatch = new RegexMatchResponse{value = match.Value, index = match.Index};
        regexMatch.groups = new List<RegexGroup>();
        regexMatch.names = new Dictionary<string, RegexGroup>();

        int groupCtr = 0;
        foreach (Group group in match.Groups)
        {
            RegexGroup regexGroup = new RegexGroup{ index=group.Index, length=group.Length, value=group.Value };
            regexGroup.capures = new List<RegexCapture>();

            int captureCtr = 0;
            foreach (Capture capture in group.Captures)
            {
                regexGroup.capures.Add(new RegexCapture{ index=capture.Index, length=capture.Length, value=capture.Value });
                captureCtr++;
            }
            groupCtr++;
            regexMatch.groups.Add(regexGroup);

            if (group.Name != null)
            {
                regexMatch.names.Add(group.Name, regexGroup);
            }
        }

        matchedList.Add(regexMatch);
        match = match.NextMatch();
    }

    return Results.Ok(matchedList);
});

app.MapPost("/regex_capture", (RegexCaptureRequest request) =>
{
    if (request.text == null ||
        request.pattern == null)
    {
        return Results.BadRequest();
    }
    
    Regex rgx = new Regex(request.pattern);
    Match match = rgx.Match(request.text);

    string[] names = rgx.GetGroupNames();
    List<RegexCaptureResponse> matchedList = new List<RegexCaptureResponse>();

    while (match.Success == true)
    {
        RegexCaptureResponse regexCaptrue = new RegexCaptureResponse();
        regexCaptrue.names = new Dictionary<string, RegexGroup>();

        foreach (var name in names)
        {
            Group group = match.Groups[name];
            if (group != null)
            {
                RegexGroup regexGroup = new RegexGroup{ index=group.Index, length=group.Length, value=group.Value };
                regexCaptrue.names.Add(name, regexGroup);
            }
        }

        matchedList.Add(regexCaptrue);
        match = match.NextMatch();
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

    return Results.Ok(new RegexReplaceResponse{ value = result});
});

app.Run();
