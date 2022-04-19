namespace rtil.Models
{
    public class TeamsMember
    {
        public string? displayName { get; set; }
        public string? mail { get; set; }
        public string? id { get; set; }
    }

    public class ReviewContentsParseRequest
    {
        public string? reviewContents { get; set; }
        public TeamsMember[]? teamsMembers { get; set; }
    }

    public class ReviewContentsParseResponse
    {
        public string? reviewId { get; set; }
        public string? reviewAuthor { get; set; }
        public string? reviewUpdate { get; set; }
        public string? reviewTitle { get; set; }
        public string? reviewUrl { get; set; }
        public string? teamsMessageId { get; set; }
        public string[]? teamsMentionId { get; set; }
    }

    public class ReviewTeamsMessageIdUpdateRequest
    {
        public string? reviewId { get; set; }
        public string? teamsMessageId { get; set; }
    }

    public class ReviewTeamsMessageIdUpdateResponse
    {
        public string? value { get; set; }
    }
}