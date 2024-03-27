using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Controllers
{
    public class HomeController : Controller
    {
        IUserService _UserService;
        public HomeController(IUserService UserService)
        {
            _UserService = UserService;
        }
        public IActionResult Index() => View();


        [Route("OnlinePayment/{id}")]
        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];
                var wallet = _UserService.GetWalletByWalletId(id);
                var payment = new ZarinpalSandbox.Payment(wallet.Amount);
                var res = payment.Verification(authority).Result;
                if (res.Status ==100)
                {
                    ViewBag.code = res.RefId;
                    ViewBag.IsSuccess = true;
                    wallet.IsPay = true;
                    _UserService.UpdateWallet(wallet);
                }
            }  
            return View();
        }

    }
}