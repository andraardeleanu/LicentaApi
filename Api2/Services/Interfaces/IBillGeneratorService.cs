using Api2.ApiModels;
using Api2.Requests;
using Core.Common;

namespace Api2.Services.Interfaces
{
    public interface IBillGeneratorService
    {
        Task<GenericFileDTO> GenerateOrderBillDocument(BillsDTO billsDTO);
    }
}
