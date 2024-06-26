using Application.Services.Interfaces;
using Application.Settings;
using Common.Constants;
using Common.Extensions;
using Common.Helpers;
using Domain.Constants;
using Domain.Models.VNPay;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using Utility.Helpers;

namespace Presentation.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IVNPayService _vnPayService;
        private readonly AppSettings _appSettings;
        public PaymentsController(IVNPayService vnPayService, IOptions<AppSettings> appSettings)
        {
            _vnPayService = vnPayService;
            _appSettings = appSettings.Value;
        }

        [HttpPost("request")]
        [Authorize(UserRoles.CUSTOMER)]
        public async Task<ActionResult<string>> CreatePayRequest(VnPayInputModel input)
        {
            var user = this.GetAuthenticatedUser();
            var now = DateTimeHelper.VnNow;
            var clientIp = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "";

            var requestModel = new VnPayRequestModel
            {
                TxnRef = Guid.NewGuid(),
                Command = VnPayConstant.Command,
                Locale = VnPayConstant.Locale,
                Version = VnPayConstant.Version,
                CurrencyCode = VnPayConstant.CurrencyCode,
                Amount = input.Amount,
                CreateDate = now,
                OrderType = "other",
                ExpireDate = now.AddMinutes(15),
                OrderInfo = "Payment for SuaMe88",
                IpAddress = clientIp,
                ReturnUrl = _appSettings.ReturnUrl,
                TmnCode = _appSettings.MerchantId
            };
            var result = await _vnPayService.AddRequest(user.Id, input.OrderId, requestModel);
            return result ? Ok(VnPayHelper.CreateRequestUrl(requestModel, _appSettings.VNPayUrl, _appSettings.MerchantPassword)) : NotFound();
        }

        [HttpGet("ipn")]
        public async Task<IActionResult> VnPayIpnEntry([FromQuery] Dictionary<string, string> queryParams)
        {
            if (!VnPayHelper.ValidateSignature(_appSettings.MerchantPassword, queryParams))
            {
                return BadRequest("Invalid Signature.");
            }
            var model = VnPayHelper.ParseToResponseModel(queryParams);
            var result = await _vnPayService.AddResponse(model);
            return result ? Ok() : BadRequest();
        }

        [HttpGet("result")]
        public ActionResult<VNPayViewModel> PaymentResult([FromQuery] Dictionary<string, string> queryParams)
        {
            if (!VnPayHelper.ValidateSignature(_appSettings.MerchantPassword, queryParams))
                return BadRequest("Invalid Signature.");
            var model = VnPayHelper.ParseToResponseModel(queryParams);

            DateTime? payDate = model.PayDate is null ? null : DateTime.ParseExact(model.PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            return Ok(new VNPayViewModel
            {
                TransactionStatus = model.TransactionStatus,
                Response = model.ResponseCode,
                OrderInfo = model.OrderInfo,
                BankCode = model.BankCode,
                Amount = model.Amount,
                CardType = model.CardType,
                PayDate = payDate,
                TransactionNo = model.TransactionNo
            });
        }

    }
}
