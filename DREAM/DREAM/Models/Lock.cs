using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class Lock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        public DateTime ExpireTime { get; set; }

        [Required]
        [ConcurrencyCheck]
        public Guid UserID { get; set; }
    }
}