using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Telestrations.Models
{

    public class Game
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Nullable<DateTime> StartedAt { get; set; }

        public Room Room { get; set; }
        [ForeignKey("Room")]
        public int RommId { get; set; }

        public Game()
        {
            var now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time");
            this.StartedAt = now;
        }
    }
}