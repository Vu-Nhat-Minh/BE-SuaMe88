using Domain.Models.VNPay;

namespace Application.Services.Interfaces
{
    public interface IVNPayService
    {
        Task<bool> AddRequest(Guid userId, Guid orderId, VnPayRequestModel model);
        Task<bool> AddResponse(VnPayResponseModel model);
        Task CreateAdmin();
    }
}
