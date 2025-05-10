using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class CartEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [ForeignKey("Books")]
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }  // Discount price 
        public DateTime AddedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual UserEntity Users { get; set; }
        public virtual Books Books { get; set; }

    }

    }
