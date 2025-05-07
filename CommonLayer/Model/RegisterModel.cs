using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Model
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Name is required !!")]
        [RegularExpression(@"^[A-Za-z\s'-]+$", ErrorMessage = "Name must contain only letters !!")]
        public string FullName { get; set; }



        [Required(ErrorMessage = "Email is required !!")]
        [RegularExpression(@"^[a-zA-Z0-9]+([._-][0-9a-zA-Z]+)*@[a-zA-Z0-9]+([.-][0-9a-zA-Z]+)*\.[a-zA-Z]{2,}$",
                            ErrorMessage = "Email must be in proper format!!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password is required !!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
                            ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character !!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [Required(ErrorMessage = "Mobile number is required !!")]
        [RegularExpression(@"^(?:\+91)?[6-9]\d{9}$", ErrorMessage = "Enter a valid mobile number (10 digits or +91 followed by 10 digits).")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
    }
}
