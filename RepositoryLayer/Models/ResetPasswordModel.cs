using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text;

namespace RepositoryLayer.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "New Password is required !!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
                            ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character !!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required !!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
                            ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character !!")]
        [DataType(DataType.Password)] 
        public string ConfirmPassword { get; set; }
    }
}
