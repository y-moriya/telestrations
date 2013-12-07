using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telestrations.Models;

namespace Telestrations.Tests.Models
{
    public class TestRoomRepository : IRoomRepository
    {
        public List<Room> Rooms { get; set; }
        public List<Game> Games { get; set; }
        public List<Player> Players { get; set; }
        public List<SketchBook> SketchBooks { get; set; }
        public List<Picture> Pictures { get; set; }

        public TestRoomRepository()
        {
            this.Rooms = new List<Room>()
            {
                new Room() { Id = 1, Owner = "euro", Phase = Phase.Interval}
            };

            this.Games = new List<Game>()
            {
                new Game() 
                {
                    Id = 1,
                    RoomId = 1,
                    Phase = GamePhase.Waiting
                }
            };

            this.Players = new List<Player>();
            this.SketchBooks = new List<SketchBook>();
            this.Pictures = new List<Picture>();
        }

        public Room FindRoom(int id)
        {
            return this.Rooms.Find(r => r.Id == id);
        }

        public Game GetCurrentGame(int roomId)
        {
            return this.Games.First(g => g.RoomId == roomId && g.Phase < GamePhase.Finished);
        }

        public List<Game> GetGames(int roomId)
        {
            return this.Games;
        }

        public List<Player> GetPlayers(int gameId)
        {
            return this.Players.Where(p => p.Game.Id == gameId).ToList();
        }

        public void AddPlayer(Player player)
        {
            if (this.Players.Count == 0)
            {
                player.Id = 1;
            }
            else
            {
                player.Id = this.Players.Max(p => p.Id) + 1;
            }
            this.Players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            this.Players.Remove(player);
        }

        public SketchBook GetSketchBook(int id)
        {
            return this.SketchBooks.Find(sb => sb.Id == id);
        }

        public List<SketchBook> GetSketchBooks(int gameId)
        {
            return this.SketchBooks.Where(sb => sb.Game.Id == gameId).ToList();
        }

        public void AddPicture(Picture pic)
        {
            if (this.Pictures.Count == 0)
            {
                pic.Id = 1;
            }
            else
            {
                pic.Id = this.Pictures.Max(sb => sb.Id) + 1;
            }
            this.Pictures.Add(pic);
        }

        public List<Picture> GetPictures(int sbookId)
        {
            return this.Pictures.Where(pic => pic.SketchBook.Id == sbookId).ToList();
        }

        public void AddSketchBook(SketchBook sbook)
        {
            if (this.SketchBooks.Count == 0)
            {
                sbook.Id = 1;
            }
            else
            {
                sbook.Id = this.SketchBooks.Max(sb => sb.Id) + 1;
            }
            this.SketchBooks.Add(sbook);
        }

        public void Save()
        {

        }

        public List<Room> GetRooms()
        {
            return Rooms;
        }
    }
}
