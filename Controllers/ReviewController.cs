using rtil.Models;
using rtil.Database;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace rtil.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ReviewDatabaseService _reviewService;

    public ReviewController(ReviewDatabaseService reviewService) => _reviewService = reviewService;

    [HttpGet("[action]")]
    public async Task<ActionResult<List<ReviewDatabaseCollection>>> List()
    {
        var reviewCollection= await _reviewService.GetAsync();

        if (reviewCollection is null)
        {
            return NotFound();
        }

        return reviewCollection;
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<ResponseReviewContentsParse>> ContentsParse(ReviewContentsParseRequest request)
    {
        if (request.reviewContents == null)
        {
            return BadRequest("requset param error");
        }

        // review parse
        request.reviewContents = request.reviewContents.Replace("\r\n", "<br>");

        string reviewPattern = "(\r\n)?(?<ReviewUpdate>.*|)\r\n\\-\\-\\-\r\n\r\nID: (?<ReviewId>[a-zA-Z0-9\\-]+|) (?<ReviewUrl>[a-zA-Z0-9\\-\\/\\.:]+|).*Title: (?<ReviewTitle>.*|)\r\nStatement of Objectives:.*Author: .* \\[(?<ReviewAuthor>[a-zA-Z0-9\\-]+|)\\]\r\nReviewers:";
        reviewPattern = reviewPattern.Replace("\r\n", "<br>");

        Regex reviewRgx = new Regex(reviewPattern);
        Match reviewMatch = reviewRgx.Match(request.reviewContents);

        if (reviewMatch.Success == false)
        {
            return BadRequest("parse error");
        }

        string? matchReviewId = reviewMatch.Groups["ReviewId"].Success ? reviewMatch.Groups["ReviewId"].Value : null;
        string? matchReviewUpdate = reviewMatch.Groups["ReviewUpdate"].Success ? reviewMatch.Groups["ReviewUpdate"].Value : null;
        string? matchReviewUrl = reviewMatch.Groups["ReviewUrl"].Success ? reviewMatch.Groups["ReviewUrl"].Value : null;
        string? matchReviewTitle = reviewMatch.Groups["ReviewTitle"].Success ?  reviewMatch.Groups["ReviewTitle"].Value : null;
        string? matchReviewAuthor = reviewMatch.Groups["ReviewAuthor"].Success ?  reviewMatch.Groups["ReviewAuthor"].Value : null;

        if (matchReviewId == null || matchReviewUpdate == null || 
            matchReviewUrl == null || matchReviewTitle == null || matchReviewAuthor == null)
        {
            return BadRequest("parse capture failed");
        }

        // mention id parse

        var memberIds = new SortedSet<string>();
        memberIds.Add(matchReviewAuthor);

        string mentionPattern = " \\[(?<MentionId>[a-zA-Z0-9_]+)\\]( |\r\n)+";
        mentionPattern = mentionPattern.Replace("\r\n", "<br>");

        string teamsMentionAt = "";
        string teamsMentionBody = "[";

        Regex mentionRgx = new Regex(mentionPattern);
        Match mentionMatch = mentionRgx.Match(matchReviewUpdate);

        while (mentionMatch.Success == true)
        {
            string? memberId = mentionMatch.Groups["MentionId"].Success ? mentionMatch.Groups["MentionId"].Value : null;
            if (memberId != null)
            {
                memberIds.Add(memberId);
            }

            mentionMatch = mentionMatch.NextMatch();
        }

        if (request.teamsMembers != null && memberIds.Count() != 0)
        {
            int mentionIndex = 1;
            foreach (var memberId in memberIds)
            {
                foreach (var teamsMember in request.teamsMembers)
                {
                    if (teamsMember.id == null || teamsMember.mail == null || teamsMember.displayName == null)
                    {
                        continue;
                    }

                    if (teamsMember.mail != $"{memberId}@nexon.co.kr")
                    {
                        continue;
                    }

                    teamsMentionBody += $"{{ " +
                        $"\"id\": {mentionIndex}, " +
                        $"\"mentionText\": \"{teamsMember.displayName}\", " +
                        $"\"mentioned\": {{ " +
                            $"\"user\": {{ " +
                                $"\"id\": \"{teamsMember.id}\", " +
                                $"\"displayName\": \"{teamsMember.displayName}\", " +
                                $"\"userIdentityType\": \"aadUser\" " +
                            $"}} }} }}";
                    teamsMentionAt += $"<at id=\"{mentionIndex}\">{teamsMember.displayName}</at>,";
                    mentionIndex++;
                }
            }
        }

        teamsMentionBody += "]";

        // etc string clear
        string urlPattern = "(https?:\\/\\/[a-zA-Z0-9\\/\\-\\.#@_%]+)";
        Regex urlRgx = new Regex(urlPattern);
        matchReviewUpdate = urlRgx.Replace(matchReviewUpdate, "<a href='$0'>$0</a>");
        matchReviewUpdate = matchReviewUpdate.Replace("  ", "&nbsp;&nbsp;");


        var reviewCollection = await _reviewService.GetAsync(matchReviewId);
        if (reviewCollection is null)
        {
            ReviewDatabaseCollection newCollection = new ReviewDatabaseCollection();
            newCollection.ReviewId = matchReviewId;
            newCollection.ReviewAuthor = matchReviewAuthor;
            newCollection.ReviewTitle = matchReviewTitle;
            newCollection.ReviewUrl = matchReviewUrl;
            
            await _reviewService.CreateAsync(newCollection);
            reviewCollection = newCollection;
        }

        return new ResponseReviewContentsParse {
            ReviewId = reviewCollection.ReviewId,
            ReviewAuthor = reviewCollection.ReviewAuthor,
            ReviewTitle = reviewCollection.ReviewTitle,
            ReviewUrl = reviewCollection.ReviewUrl,
            ReviewUpdate = matchReviewUpdate,
            TeamsMessageId = reviewCollection.TeamsMessageId,
            TeamsMentionBody = teamsMentionBody,
            TeamsMentionAt = teamsMentionAt
        };
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<ReviewDatabaseCollection>> TeamsMessageIdUpdate(ReviewTeamsMessageIdUpdateRequest request)
    {
        if (request.reviewId == null ||
            request.teamsMessageId == null)
        {
            return BadRequest();
        }

        var reviewCollection = await _reviewService.GetAsync(request.reviewId);

        if (reviewCollection is null)
        {
            return NotFound();
        }

        reviewCollection.TeamsMessageId = request.teamsMessageId;
        await _reviewService.UpdateAsync(reviewCollection.ReviewId, reviewCollection);

        return reviewCollection;
    }
}