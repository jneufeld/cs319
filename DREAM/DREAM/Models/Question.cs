using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    // based on following http://www.asp.net/mvc/tutorials/mvc-4/getting-started-with-aspnet-mvc4/adding-a-model

     public class Question
    {
         [Key]
         [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
         public int id { get; set; }
         public string question { get; set; }
         public int timeTaken { get; set; }
         public string response { get; set; }
         public int probability { get; set; }
         public int severity { get; set; }
         public string specialNotes { get; set; }
         public TumourGroup tumourGroup { get; set; }
         public int requestID { get; set; }
         public List<string> keywords { get; set; }
    }

     public class QuestionContext : DbContext
     {
         public DbSet<Question> Question { get; set; }
     }
}