namespace Api2.Services.Interfaces
{
    public interface IDocumentConverterService
    {
        Task<(byte[], string)> ConvertAsync(IFormFile file);
        Task<(byte[], string)> ConvertAsync(byte[] fileBytes);
        Task<(MemoryStream, string)> ConvertAsync(MemoryStream fileStream);
    }

}
