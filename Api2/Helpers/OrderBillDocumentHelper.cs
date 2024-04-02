using Api2.Helpers.Interfaces;
using Core.Templates;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using Api2.ApiModels;
using static Core.Constants.Constants;
using Core.Constants;

namespace Api2.Helpers
{
    public class OrderBillDocumentHelper (IDocumentHelper documentHelper) : IOrderBillDocumentHelper
    {
        private readonly IDocumentHelper _documentHelper = documentHelper ?? throw new ArgumentNullException(nameof(documentHelper));

        public StreamResultDTO GenerateOrderBillDocument(BillsDTO billsDTO)
        {
            try
            {
                var sourceFilePath = TemplateHelper.GetBillTemplateFilePath();

                MemoryStream newDocStream = new();

                using (WordprocessingDocument sourceDoc = WordprocessingDocument.Open(sourceFilePath, false))
                {
                    using WordprocessingDocument newDoc = WordprocessingDocument.Create(newDocStream, WordprocessingDocumentType.Document);
                    if (sourceDoc.MainDocumentPart == null) throw new ArgumentNullException(ErrorMessages.PdfGenerationFailed);

                    foreach (var part in sourceDoc.Parts)
                    {
                        newDoc.AddPart(part.OpenXmlPart, part.RelationshipId);
                    }

                    ReplaceOrderBillPlaceholders(newDoc, billsDTO);
                    newDocStream.Position = 0;
                }
                
                return new StreamResultDTO() { MemoryStream = newDocStream, ErrorMessage = null };
            }
            catch (Exception exception)
            {
                throw new ArgumentNullException(ErrorMessages.PdfGenerationFailed);
            }
        }

        private void ReplaceOrderBillPlaceholders(WordprocessingDocument doc, BillsDTO billsDTO)
        {
            _documentHelper.ReplacePlaceholder(doc.MainDocumentPart!, OrderBillInformation.TOTAL_PRICE, billsDTO.TotalPrice.ToString());
            _documentHelper.ReplacePlaceholder(doc.MainDocumentPart!, OrderBillInformation.ORDER_NO, billsDTO.OrderNo!);
            _documentHelper.ReplacePlaceholder(doc.MainDocumentPart!, OrderBillInformation.COMPANY, billsDTO.CompanyName!);
            _documentHelper.ReplacePlaceholder(doc.MainDocumentPart!, OrderBillInformation.WORKPOINT, billsDTO.WorkpointName!);
            _documentHelper.ReplacePlaceholder(doc.MainDocumentPart!, OrderBillInformation.STATUS, billsDTO.Status!);
            _documentHelper.ReplacePlaceholder(doc.MainDocumentPart!, OrderBillInformation.CREATED_BY, billsDTO.CreatedBy!);
            _documentHelper.ReplacePlaceholder(doc.MainDocumentPart!, OrderBillInformation.DATE, billsDTO.Date!.ToString());
        }
    }
}
