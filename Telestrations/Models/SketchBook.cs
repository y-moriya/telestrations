using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Telestrations.Models
{
    public class SketchBook
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Author { get; set; }
        public string QuestionWord { get; set; }

        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string Answer5 { get; set; }
        public string Answer6 { get; set; }
        public string Answer7 { get; set; }
        public string Answer8 { get; set; }
        public string Answer9 { get; set; }
        public string Answer10 { get; set; }

        public Game Game { get; set; }
        [ForeignKey("Game")]
        public int GameId { get; set; }

    }
}