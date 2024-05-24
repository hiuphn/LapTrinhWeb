using Microsoft.AspNetCore.Mvc;
using WebBanHang.Data;


namespace WebBanHang.ServicesPay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExcute(IQueryCollection collections);
    }
}
