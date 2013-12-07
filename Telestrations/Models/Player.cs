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

        public virtual SketchBook CurrentSketchBook { get; set; }
        public int NextSketchBookId { get; set; }

        public bool HasSentPicture { get; set; }
        public string UserName { get; set; }
        public virtual Game Game { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public Picture DrawPicture(string uri)
        {
            return new Picture()
            {
                Author = this.UserName,
                SketchBook = this.CurrentSketchBook,
                Uri = uri
            };
        }
    }
}