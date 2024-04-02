using Api2.ApiModels;

namespace Api2.Responses
{
    public class GenericFileResponse
    {
        public GenericFileDTO? File { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
