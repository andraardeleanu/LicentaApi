using DocumentFormat.OpenXml.Packaging;

namespace Api2.Helpers.Interfaces
{
    public interface IDocumentHelper
    {
        void ReplacePlaceholder(MainDocumentPart mainPart, string placeholder, string replacement);
    }
}
