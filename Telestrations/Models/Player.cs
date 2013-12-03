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

        public string UserName { get; set; }

        public Room Room { get; set; }
        [ForeignKey("Room")]
        public int RommId { get; set; }

    }
}