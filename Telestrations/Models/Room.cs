using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Telestrations.Models
{
    public class RoomsContext : DbContext
    {
        public RoomsContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<SketchBook> SketchBooks { get; set; }
        public DbSet<Picture> Pictures { get; set; }
    }

    public class Room
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Owner { get; set; }
        public Phase Phase { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
    }

    public enum Phase
    {
        Interval,
        Progress
    }
}