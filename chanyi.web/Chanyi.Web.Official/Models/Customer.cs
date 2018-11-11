using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chanyi.Web.Official.Models
{
    /// <summary>
    /// 客户资料
    /// </summary>
    public class Customer
    {
        [Required(ErrorMessage = "请填写单位名称", AllowEmptyStrings = false)]
        [Display(Name = "单位")]
        public string Department { get; set; }

        [Required(ErrorMessage = "请填写牧场规模", AllowEmptyStrings = false)]
        [RegularExpression(@"^\d+$", ErrorMessage = "请正确填写牧场规模（存栏量-数字）")]
        [Display(Name = "牧场规模")]
        public string Scale { get; set; }

        [Required(ErrorMessage = "请填写联系人", AllowEmptyStrings = false)]
        [Display(Name = "联系人")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "请填写联系电话", AllowEmptyStrings = false)]
        [RegularExpression(@"^(0|86|17951)?(13[0-9]|15[012356789]|17[0678]|18[0-9]|14[57])[0-9]{8}$", ErrorMessage = "请正确填写联系电话")]
        [Display(Name = "联系电话")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "请填写联系地址", AllowEmptyStrings = false)]
        [Display(Name = "联系地址")]
        public string Address { get; set; }


    }
}