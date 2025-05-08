using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class JwtModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
