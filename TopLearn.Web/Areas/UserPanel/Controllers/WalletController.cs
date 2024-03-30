using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{


    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {

        IUserService _userService;
        public WalletController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("UserPanel/Wallet")]
        public IActionResult Index()
        {
            ViewBag.ListWallet = _userService.GetUserWallets(User.Identity.Name);
            return View();
        }

        [Route("UserPanel/Wallet")]
        [HttpPost]
        public IActionResult Index(ChargeWalletViewModel charge)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListWallet = _userService.GetUserWallets(User.Identity.Name);
                return View(charge);
            }
            int walletid = _userService.chargeWallet(User.Identity.Name, charge.Amount, "شارژ حساب ");

            #region Online Payment
            var Payment = new ZarinpalSandbox.Payment(charge.Amount);
            var respons = Payment.PaymentRequest("شارژ کیف پول ", "https://localhost:44349/OnlinePayment"+ walletid);
            if (respons.Result.Status==100)
            {  
              return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + respons.Result.Authority);
            }
            
            #endregion
            //TODO 

            return null;
        }


    }
}
