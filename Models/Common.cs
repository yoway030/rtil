namespace rtil.Models
{
    class RegexMatchRequest
    {
        public string? text { get; set; }
        public string? pattern { get; set; }
    }

    class RegexMatchResponse
    {
        public string? value { get; set; }
        public Int32? index { get; set; }
        public List<RegexGroup>? groups { get; set; }
        public Dictionary<string, RegexGroup>? names { get; set; }
    }

    class RegexGroup
    {
        public Int32? index { get; set; }
        public Int32? length { get; set; }
        public string? value { get; set; }
        public List<RegexCapture>? capures { get; set; }
    }

    class RegexCapture
    {
        public Int32? index { get; set; }
        public Int32? length { get; set; }
        public string? value { get; set; }
    }

    class RegexCaptureRequest
    {
        public string? text { get; set; }
        public string? pattern { get; set; }
    }

    class RegexCaptureResponse
    {
        public Dictionary<string, RegexGroup>? names { get; set; }
    }

    class RegexReplaceRequest
    {
        public string? text { get; set; }
        public string? pattern { get; set; }
        public string? to { get; set; }
    }

    class RegexReplaceResponse
    {
        public string? value { get; set; }
    }
}