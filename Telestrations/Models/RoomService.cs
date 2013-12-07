using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telestrations.Models
{
    public interface IRoomService
    {
        List<RoomViewModel> GetRooms();
        RoomViewModel ShowRoom(int roomId);

        void Load(int roomId);
        void Reload();
        bool CanJoinGame(string userName);
        void JoinCurrentGame(Player player);
        void LeaveCurrentGame(Player player);
        bool CanStartGame();
        void StartCurrentGame();
        Player GetPlayerByUserName(string userName);
        Player GetNextPlayer(Player p);
        void SendPicture(Picture pic);
        bool CanUpdateRound();
        void UpdateRound();
        bool CanFinishGame();
        void FinishCurrentGame();
    }

    public class RoomService : IRoomService
    {
        #region Properties and Members
        public Room Room { get; set; }
        public Game CurrentGame { get; set; }
        private IRoomRepository rep;
        private IWordRepository words;
        public const int MAX_PLAYER_NUMBER = 8;
        public const int MIN_PLAYER_NUMBER = 4;
        #endregion

        #region Constructors
        public RoomService()
        {
            this.rep = new RoomRepository();
            this.words = new WordRepository();
        }

        public RoomService(IRoomRepository rep, IWordRepository words)
        {
            this.rep = rep;
            this.words = words;
        }
        #endregion

        #region Methods
        public void Load(int roomId)
        {
            this.Room = rep.FindRoom(roomId);
            if (this.Room == null)
            {
                return;
            }

            this.CurrentGame = rep.GetCurrentGame(roomId);
            if (this.CurrentGame == null)
            {
                return;
            }

            Reload();
        }

        public void Reload()
        {
            this.CurrentGame.Players = rep.GetPlayers(this.CurrentGame.Id);

            if (CurrentGame.Phase != GamePhase.Waiting)
            {
                this.CurrentGame.SketchBooks = rep.GetSketchBooks(this.CurrentGame.Id);
                foreach (var sbook in this.CurrentGame.SketchBooks)
                {
                    sbook.Pictures = rep.GetPictures(sbook.Id);
                }
                foreach (var player in this.CurrentGame.Players)
                {
                    player.CurrentSketchBook = this.CurrentGame.SketchBooks.Find(sb => sb.Id == player.CurrentSketchBook.Id);
                }
            }
        }

        public bool CanJoinGame(string userName)
        {
            if (this.CurrentGame.Phase != GamePhase.Waiting)
            {
                return false;
            }

            if (this.CurrentGame.Players.Count >= MAX_PLAYER_NUMBER)
            {
                return false;
            }

            if (this.CurrentGame.Players.Any(p => p.UserName == userName))
            {
                return false;
            }

            return true;
        }

        public void JoinCurrentGame(Player player)
        {
            if (!CanJoinGame(player.UserName))
            {
                return;
            }

            rep.AddPlayer(player);
            rep.Save();
        }

        public void LeaveCurrentGame(Player player)
        {
            if (!(this.CurrentGame.Players.Any(p => p.UserName == player.UserName)))
            {
                return;
            }
            rep.RemovePlayer(player);
        }

        public bool CanStartGame()
        {
            if (this.CurrentGame.Phase != GamePhase.Waiting)
            {
                return false;
            }

            if (this.CurrentGame.Players.Count < MIN_PLAYER_NUMBER || MAX_PLAYER_NUMBER < this.CurrentGame.Players.Count)
            {
                return false;
            }

            return true;
        }

        public void StartCurrentGame()
        {
            if (!CanStartGame())
            {
                return;
            }

            this.CurrentGame.Phase = GamePhase.Progress;
            var count = this.CurrentGame.Players.Count;
            var players = this.CurrentGame.Players;
            var qwords = words.GetQuestionWords(count);

            // 人数分のスケッチブックを初期化
            for (int i = 0; i < count; i++)
            {
                var sbook = new SketchBook()
                {
                    Game = this.CurrentGame,
                    QuestionWord = qwords[i],
                    Author = players[i].UserName
                };

                this.rep.AddSketchBook(sbook);
                //this.CurrentGame.SketchBooks.Add(sbook);
            }

            this.rep.Save();
            this.CurrentGame.SketchBooks = this.rep.GetSketchBooks(this.CurrentGame.Id);

            // 各プレイヤーにスケッチブックを割り当て
            foreach (var p in this.CurrentGame.Players)
            {
                var mySketchBook = this.CurrentGame.SketchBooks.Find(sb => sb.Author == p.UserName);
                p.CurrentSketchBook = mySketchBook;
            }
            this.rep.Save();
        }

        public Player GetPlayerByUserName(string userName)
        {
            return this.CurrentGame.Players.Find(p => p.UserName == userName);
        }

        public Player GetNextPlayer(Player p)
        {
            var index = this.CurrentGame.Players.IndexOf(p);
            if (this.CurrentGame.Players.Count == index + 1)
            {
                return this.CurrentGame.Players.First();
            }
            else
            {
                return this.CurrentGame.Players[index + 1];
            }
        }

        public void SendPicture(Picture pic)
        {
            // TODO: 要検討
            var author = GetPlayerByUserName(pic.Author);
            pic.SketchBook = author.CurrentSketchBook;
            rep.AddPicture(pic);
            author.HasSentPicture = true;

            var nextUser = GetNextPlayer(author);
            nextUser.NextSketchBookId = pic.SketchBook.Id;

            rep.Save();
        }

        public bool CanUpdateRound()
        {
            if (this.CurrentGame.Phase != GamePhase.Progress)
            {
                return false;
            }

            if (this.CurrentGame.Players.Any(p => !p.HasSentPicture))
            {
                return false;
            }

            return true;
        }

        public void UpdateRound()
        {
            if (!CanUpdateRound())
            {
                return;
            }

            foreach (var p in this.CurrentGame.Players)
            {
                p.HasSentPicture = false;
                p.CurrentSketchBook = rep.GetSketchBook(p.NextSketchBookId);
                // 念のためリセット
                p.NextSketchBookId = -1;
            }

            rep.Save();
        }

        public bool CanFinishGame()
        {
            if (this.CurrentGame.Phase != GamePhase.Progress)
            {
                return false;
            }

            if (this.CurrentGame.Players.Any(p => !p.HasSentPicture))
            {
                return false;
            }

            foreach (var p in this.CurrentGame.Players)
            {
                var sb = rep.GetSketchBook(p.NextSketchBookId);
                if (sb.Author != p.UserName)
                {
                    return false;
                }
            }

            return true;
        }

        public void FinishCurrentGame()
        {
            if (!CanFinishGame())
            {
                return;
            }

            this.UpdateRound(); // SketchBook の位置を戻す
            this.CurrentGame.Phase = GamePhase.Finished;
        }

        #endregion

        public List<RoomViewModel> GetRooms()
        {
            return this.rep.GetRooms().Select(r => new RoomViewModel(r)).ToList();
        }

        public RoomViewModel ShowRoom(int roomId)
        {
            this.Load(roomId);
            return new RoomViewModel(this.Room);
        }
    }
}