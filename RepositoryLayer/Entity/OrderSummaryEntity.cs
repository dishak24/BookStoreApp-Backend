using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class OrderSummaryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        public int Quantity { get; set; }
        public int TotalAmount { get; set; }
        public DateTime OrderedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual UserEntity User { get; set; }
        public virtual Books Book { get; set; }
    }

}
