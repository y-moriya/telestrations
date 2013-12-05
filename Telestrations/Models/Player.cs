using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Telestrations.Models
{
    public class Player
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SketchBook CurrentSketchBook { get; set; }
        [ForeignKey("SketchBook")]
        public int CurrentSketchBookId { get; set; }

        public bool HasSentPicture { get; set; }
        public string UserName { get; set; }

        public Game Game { get; set; }
        [ForeignKey("Game")]
        public int GameId { get; set; }

    }
}