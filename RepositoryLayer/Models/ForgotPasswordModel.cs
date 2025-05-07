using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Models
{
    public class ForgotPasswordModel
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
