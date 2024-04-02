using Api2.ApiModels;

namespace Api2.Helpers.Interfaces
{
    public interface IOrderBillDocumentHelper
    {
        StreamResultDTO GenerateOrderBillDocument(BillsDTO billsDTO);
    }
}
