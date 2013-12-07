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

        public List<Picture> Pictures { get; set; }
        public virtual Game Game { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
    }
}