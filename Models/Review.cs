using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNKRS.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }

        public int Rating { get; set; }

        public DateTime DateTime { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}