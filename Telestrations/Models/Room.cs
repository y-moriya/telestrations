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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasRequired(p => p.Game)
                                         .WithMany(g => g.Players)
                                         .Map(x => x.MapKey("PlayerId"))
                                         .WillCascadeOnDelete(true);

            modelBuilder.Entity<SketchBook>().HasRequired(sb => sb.Game)
                                             .WithMany(g => g.SketchBooks)
                                             .Map(x => x.MapKey("SketchBookId"))
                                             .WillCascadeOnDelete(true);

            modelBuilder.Entity<Picture>().HasRequired(pic => pic.SketchBook)
                                          .WithMany(sb => sb.Pictures)
                                          .Map(x => x.MapKey("PictureId"))
                                          .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
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