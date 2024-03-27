using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearn.Core.DTOs
{
    public class ChargeWalletViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید ")]
        [Display(Name = "مبلغ")]
        public int Amount { get; set; }
    }
    public class WalletViewModel
    {
        public int Amount { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }

}
