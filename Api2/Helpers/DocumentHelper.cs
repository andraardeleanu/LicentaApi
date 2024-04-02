using Api2.Helpers.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Api2.Helpers
{
    public class DocumentHelper : IDocumentHelper
    {
        public void ReplacePlaceholder(MainDocumentPart mainPart, string placeholder, string replacement)
        {
            IEnumerable<Text> textElements = mainPart.Document.Descendants<Text>();

            foreach (Text text in textElements)
            {
                if (text.Text.Contains(placeholder))
                {
                    text.Text = text.Text.Replace(placeholder, replacement);
                }
            }
        }
    }
}
