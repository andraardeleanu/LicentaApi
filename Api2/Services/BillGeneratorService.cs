using Api2.ApiModels;
using Api2.Helpers.Interfaces;
using Api2.Services.Interfaces;
using static Core.Constants.Constants;


namespace Api2.Services
{
    public class BillGeneratorService(IOrderBillDocumentHelper orderBillDocumentHelper, IDocumentConverterService documentConverterService) : IBillGeneratorService
    {
        IOrderBillDocumentHelper _orderBillDocumentHelper = orderBillDocumentHelper ?? throw new ArgumentNullException(nameof(orderBillDocumentHelper));
        private readonly IDocumentConverterService _documentConverterService = documentConverterService ?? throw new ArgumentNullException(nameof(documentConverterService));

        public async Task<GenericFileDTO> GenerateOrderBillDocument(BillsDTO billsDTO)
        {
            try
            {
                var docxStream = _orderBillDocumentHelper.GenerateOrderBillDocument(billsDTO);

                var pdfStream = await _documentConverterService.ConvertToPdfAsync(docxStream.MemoryStream!);

                var file = new GenericFileDTO()
                {
                    Blob = Convert.ToBase64String(pdfStream!.ToArray()),
                    FileName = OrderBillInformation.BILL_FILE_NAME                    
                };
                
                return file;
        }
            catch (Exception exception)
            {
                return null;
            }
}

    }
}
