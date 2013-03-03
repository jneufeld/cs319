using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class Keyword
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string keyword { get; set; }
        public bool enabled { get; set; }
    }

    public class KeywordContext : DbContext
    {
        public DbSet<Keyword> Keyword { get; set; }
    }
}