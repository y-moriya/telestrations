using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Telestrations.Models
{
    public class Picture
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Uri { get; set; }
        public string Author { get; set; }

        public SketchBook SketchBook { get; set; }
        [ForeignKey("SketchBook")]
        public int SketchBookId { get; set; }
    }
}