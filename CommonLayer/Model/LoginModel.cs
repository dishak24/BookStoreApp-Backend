using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required !!")]
        [RegularExpression(@"^[a-zA-Z0-9]+([._-][0-9a-zA-Z]+)*@[a-zA-Z0-9]+([.-][0-9a-zA-Z]+)*\.[a-zA-Z]{2,}$",
                            ErrorMessage = "Email must be in proper format!!")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password is Required !!")]
        public string Password { get; set; }
    }
}
