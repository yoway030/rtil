namespace rtil.Models
{
    class RegexCaptureRequest
    {
        public string? text { get; set; }
        public string? pattern { get; set; }
    }

    class RegexCaptureResponse
    {
        public string? name { get; set; }
        public string? value { get; set; }
    }

    class RegexReplaceRequest
    {
        public string? text { get; set; }
        public string? pattern { get; set; }
        public string? to { get; set; }
    }
}