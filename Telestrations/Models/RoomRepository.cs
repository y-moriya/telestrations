using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telestrations.Models
{
    public interface IRoomRepository
    {
        List<Room> GetRooms();
        Room FindRoom(int id);
        Game GetCurrentGame(int roomId);
        List<Game> GetGames(int roomId);
        //void AddGame(Game game);
        //void RemoveGame(int roomId, Game game);
        List<Player> GetPlayers(int gameId);
        void AddPlayer(Player player);
        void RemovePlayer(Player player);
        SketchBook GetSketchBook(int id);
        List<SketchBook> GetSketchBooks(int gameId);
        void AddPicture(Picture pic);
        List<Picture> GetPictures(int sbookId);
        void AddSketchBook(SketchBook sbook);

        void Save();
    }

    public class RoomRepository : IRoomRepository
    {
        private RoomsContext db;

        public RoomRepository()
        {
            this.db = new RoomsContext();
        }

        public List<Room> GetRooms()
        {
            return this.db.Rooms.ToList();
        }

        public Room FindRoom(int id)
        {
            return this.db.Rooms.Find(id);
        }

        public Game GetCurrentGame(int roomId)
        {
            var games = this.GetGames(roomId);
            return games.FirstOrDefault(g => g.Phase != GamePhase.Closed);
        }

        public List<Game> GetGames(int roomId)
        {
            return this.db.Games.Where(g => g.RoomId == roomId).ToList();
        }

        public List<Player> GetPlayers(int gameId)
        {
            return this.db.Players.Where(p => p.Game.Id == gameId).ToList();
        }

        public void AddPlayer(Player player)
        {
            this.db.Players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            this.db.Players.Remove(player);
        }

        public SketchBook GetSketchBook(int id)
        {
            return this.db.SketchBooks.Find(id);
        }

        public List<SketchBook> GetSketchBooks(int gameId)
        {
            return this.db.SketchBooks.Where(sb => sb.Game.Id == gameId).ToList();
        }

        public void AddPicture(Picture pic)
        {
            this.db.Pictures.Add(pic);
        }

        public List<Picture> GetPictures(int sbookId)
        {
            return this.db.Pictures.Where(pic => pic.SketchBook.Id == sbookId).ToList();
        }

        public void AddSketchBook(SketchBook sbook)
        {
            this.db.SketchBooks.Add(sbook);
        }

        public void Save()
        {
            this.db.SaveChanges();
        }
    }
}