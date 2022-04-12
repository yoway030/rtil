namespace rtil.Models
{
    class ReviewContentsParseRequest
    {
        public string? reviewContents { get; set; }
        public string? teamsMemberInfo { get; set; }
    }

    class ReviewContentsParseResponse
    {
        public string? reviewId { get; set; }
        public string? reviewAuthor { get; set; }
        public string? reviewUpdate { get; set; }
        public string? reviewTitle { get; set; }
        public string? reviewUrl { get; set; }
        public string? teamsMessageId { get; set; }
        public string[]? teamsMentionId { get; set; }
    }

    class ReviewTeamsMessageIdUpdateRequest
    {
        public string? reviewId { get; set; }
        public string? teamsMessageId { get; set; }
    }

    class ReviewTeamsMessageIdUpdateResponse
    {
        public string? value { get; set; }
    }
}