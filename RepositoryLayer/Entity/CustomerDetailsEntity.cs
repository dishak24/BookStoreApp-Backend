using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class CustomerDetailsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Type { get; set; }

        //navigation property
        public virtual UserEntity Users { get; set; }
    }
}
