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
        public int RoomId { get; set; }

        public GamePhase Phase { get; set; }

        public List<Player> Players { get; set; }
        public List<SketchBook> SketchBooks { get; set; }

        public Game()
        {
            var now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.ToUniversalTime(), "Tokyo Standard Time");
            this.StartedAt = now;
            this.Phase = GamePhase.Waiting;
        }
    }

    public enum GamePhase
    {
        Waiting,
        Progress,
        Finished
    }
}