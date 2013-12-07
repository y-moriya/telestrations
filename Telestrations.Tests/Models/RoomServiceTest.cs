using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telestrations.Models;

namespace Telestrations.Tests.Models
{
    [TestClass]
    public class RoomServiceTest
    {
        private RoomService GetTestService()
        {
            IRoomRepository rep = new TestRoomRepository();
            IWordRepository words = new TestWordRepository();
            return new RoomService(rep, words);
        }

        [TestMethod]
        public void LoadTest()
        {
            RoomService service = GetTestService();
            service.Load(1);

            Assert.IsNotNull(service.Room);
            Assert.AreEqual(1, service.Room.Id);

            Assert.IsNotNull(service.CurrentGame);
            Assert.AreEqual(1, service.CurrentGame.Id);
        }

        [TestMethod]
        public void CanJoinGameTest()
        {
            RoomService service = GetTestService();
            service.Load(1);

            Assert.IsTrue(service.CanJoinGame("euro"));

            var p = new Player() { UserName = "euro", GameId = 1 };
            service.JoinCurrentGame(p);
            service.Reload();
            Assert.IsFalse(service.CanJoinGame("euro"));
            Assert.IsTrue(service.CanJoinGame("euro2"));

            service.JoinCurrentGame(new Player() { UserName = "euro2", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro3", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro4", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro5", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro6", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro7", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro8", GameId = 1 });
            service.Reload();
            Assert.IsFalse(service.CanJoinGame("euro9"));
            service.LeaveCurrentGame(p);
            service.Reload();

            Assert.IsTrue(service.CanJoinGame("euro9"));

            service.StartCurrentGame();
            Assert.IsFalse(service.CanJoinGame("euro9"));
        }

        [TestMethod]
        public void JoinCurrentGameTest()
        {
            RoomService service = GetTestService();
            service.Load(1);

            Assert.AreEqual(0, service.CurrentGame.Players.Count);

            var p = new Player() { UserName = "euro", GameId = 1 };
            service.JoinCurrentGame(p);
            service.Reload();

            Assert.AreEqual(1, service.CurrentGame.Players.Count);
        }

        [TestMethod]
        public void LeaveCurrentGameTest()
        {
            RoomService service = GetTestService();
            service.Load(1);

            Assert.AreEqual(0, service.CurrentGame.Players.Count);

            var p = new Player() { UserName = "euro", GameId = 1 };
            service.JoinCurrentGame(p);
            service.Reload();

            Assert.AreEqual(1, service.CurrentGame.Players.Count);

            service.LeaveCurrentGame(p);
            service.Reload();

            Assert.AreEqual(0, service.CurrentGame.Players.Count);
        }

        [TestMethod]
        public void CanStartGameTest()
        {
            RoomService service = GetTestService();
            service.Load(1);

            Assert.IsFalse(service.CanStartGame());

            service.JoinCurrentGame(new Player() { UserName = "euro1", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro2", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro3", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro4", GameId = 1 });
            service.Reload();
            Assert.IsTrue(service.CanStartGame());

            service.StartCurrentGame();
            Assert.IsFalse(service.CanStartGame());
        }

        [TestMethod]
        public void StartCurrentGameTest()
        {
            RoomService service = GetTestService();
            service.Load(1);

            service.JoinCurrentGame(new Player() { UserName = "euro1", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro2", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro3", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro4", GameId = 1 });
            service.Reload();

            service.StartCurrentGame();
            service.Reload();

            Assert.AreEqual(GamePhase.Progress, service.CurrentGame.Phase);
            for (int i = 0; i < service.CurrentGame.Players.Count; i++)
            {
                Assert.IsNotNull(service.CurrentGame.Players[i].CurrentSketchBook);
                Assert.AreEqual("hoge" + i.ToString(), service.CurrentGame.Players[i].CurrentSketchBook.QuestionWord);
            }
        }

        [TestMethod]
        public void GetPlayerByUserNameTest()
        {
            RoomService service = GetTestService();
            service.Load(1);

            service.JoinCurrentGame(new Player() { UserName = "euro1", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro2", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro3", GameId = 1 });
            service.JoinCurrentGame(new Player() { UserName = "euro4", GameId = 1 });
            service.Reload();

            var p = service.GetPlayerByUserName("euro1");
            Assert.AreEqual("euro1", p.UserName);
        }

        [TestMethod]
        public void SendPictureTest()
        {
            RoomService service = GetTestService();
            service.Load(1);

            var pl1 = new Player() { UserName = "euro1", GameId = 1 };
            var pl2 = new Player() { UserName = "euro2", GameId = 1 };
            var pl3 = new Player() { UserName = "euro3", GameId = 1 };
            var pl4 = new Player() { UserName = "euro4", GameId = 1 };

            service.JoinCurrentGame(pl1);
            service.JoinCurrentGame(pl2);
            service.JoinCurrentGame(pl3);
            service.JoinCurrentGame(pl4);
            service.Reload();

            service.StartCurrentGame();
            service.Reload();

            Assert.IsFalse(pl1.HasSentPicture);

            var pic1 = pl1.DrawPicture("uri1");
            service.SendPicture(pic1);
            service.Reload();

            Assert.IsTrue(pl1.HasSentPicture);
        }

        [TestMethod]
        public void CanUpdateRoundTest()
        {
            RoomService service = GetTestService();
            service.Load(1);

            Assert.IsFalse(service.CanUpdateRound());

            var pl1 = new Player() { UserName = "euro1", GameId = 1 };
            var pl2 = new Player() { UserName = "euro2", GameId = 1 };
            var pl3 = new Player() { UserName = "euro3", GameId = 1 };
            var pl4 = new Player() { UserName = "euro4", GameId = 1 };

            service.JoinCurrentGame(pl1);
            service.JoinCurrentGame(pl2);
            service.JoinCurrentGame(pl3);
            service.JoinCurrentGame(pl4);
            service.Reload();

            service.StartCurrentGame();
            service.Reload();

            var pic1 = pl1.DrawPicture("uri1");
            var pic2 = pl2.DrawPicture("uri2");
            var pic3 = pl3.DrawPicture("uri3");
            var pic4 = pl4.DrawPicture("uri4");

            service.SendPicture(pic1);
            service.SendPicture(pic2);
            service.SendPicture(pic3);
            service.Reload();

            Assert.IsFalse(service.CanUpdateRound());

            service.SendPicture(pic4);
            service.Reload();
            Assert.IsTrue(service.CanUpdateRound());
        }

        [TestMethod]
        public void GetNextPlayerTest()
        {
            RoomService service = GetTestService();
            service.Load(1);
            var pl1 = new Player() { UserName = "euro1", GameId = 1 };
            var pl2 = new Player() { UserName = "euro2", GameId = 1 };
            var pl3 = new Player() { UserName = "euro3", GameId = 1 };
            var pl4 = new Player() { UserName = "euro4", GameId = 1 };

            service.JoinCurrentGame(pl1);
            service.JoinCurrentGame(pl2);
            service.JoinCurrentGame(pl3);
            service.JoinCurrentGame(pl4);
            service.Reload();

            service.StartCurrentGame();
            service.Reload();

            Assert.AreEqual(pl2, service.GetNextPlayer(pl1));
            Assert.AreEqual(pl3, service.GetNextPlayer(pl2));
            Assert.AreEqual(pl4, service.GetNextPlayer(pl3));
            Assert.AreEqual(pl1, service.GetNextPlayer(pl4));
        }

        [TestMethod]
        public void UpdateRoundTest()
        {
            RoomService service = GetTestService();
            service.Load(1);
            var pl1 = new Player() { UserName = "euro1", GameId = 1 };
            var pl2 = new Player() { UserName = "euro2", GameId = 1 };
            var pl3 = new Player() { UserName = "euro3", GameId = 1 };
            var pl4 = new Player() { UserName = "euro4", GameId = 1 };

            service.JoinCurrentGame(pl1);
            service.JoinCurrentGame(pl2);
            service.JoinCurrentGame(pl3);
            service.JoinCurrentGame(pl4);
            service.Reload();

            service.StartCurrentGame();
            service.Reload();

            var pic1 = pl1.DrawPicture("uri1");
            var pic2 = pl2.DrawPicture("uri2");
            var pic3 = pl3.DrawPicture("uri3");
            var pic4 = pl4.DrawPicture("uri4");

            service.SendPicture(pic1);
            service.SendPicture(pic2);
            service.SendPicture(pic3);
            service.SendPicture(pic4);
            service.Reload();

            service.UpdateRound();
            service.Reload();

            Assert.AreEqual("euro4", pl1.CurrentSketchBook.Author);
            Assert.AreEqual("euro1", pl2.CurrentSketchBook.Author);
            Assert.AreEqual("euro2", pl3.CurrentSketchBook.Author);
            Assert.AreEqual("euro3", pl4.CurrentSketchBook.Author);
        }

        [TestMethod]
        public void CanFinishGameTest()
        {
            RoomService service = GetTestService();
            service.Load(1);

            Assert.IsFalse(service.CanFinishGame());

            var pl1 = new Player() { UserName = "euro1", GameId = 1 };
            var pl2 = new Player() { UserName = "euro2", GameId = 1 };
            var pl3 = new Player() { UserName = "euro3", GameId = 1 };
            var pl4 = new Player() { UserName = "euro4", GameId = 1 };

            service.JoinCurrentGame(pl1);
            service.JoinCurrentGame(pl2);
            service.JoinCurrentGame(pl3);
            service.JoinCurrentGame(pl4);
            service.Reload();

            Assert.IsFalse(service.CanFinishGame());

            service.StartCurrentGame();
            service.Reload();

            Assert.IsFalse(service.CanFinishGame());

            var pic1 = pl1.DrawPicture("uri1");
            var pic2 = pl2.DrawPicture("uri2");
            var pic3 = pl3.DrawPicture("uri3");
            var pic4 = pl4.DrawPicture("uri4");

            service.SendPicture(pic1);
            service.SendPicture(pic2);
            service.SendPicture(pic3);
            service.SendPicture(pic4);
            service.Reload();

            service.UpdateRound();
            service.Reload();

            Assert.IsFalse(service.CanFinishGame());

            var pic5 = pl1.DrawPicture("uri5");
            var pic6 = pl2.DrawPicture("uri6");
            var pic7 = pl3.DrawPicture("uri7");
            var pic8 = pl4.DrawPicture("uri8");

            service.SendPicture(pic5);
            service.SendPicture(pic6);
            service.SendPicture(pic7);
            service.SendPicture(pic8);
            service.Reload();

            service.UpdateRound();
            service.Reload();

            Assert.IsFalse(service.CanFinishGame());

            var pic9 = pl1.DrawPicture("uri9");
            var pic10 = pl2.DrawPicture("uri10");
            var pic11 = pl3.DrawPicture("uri11");
            var pic12 = pl4.DrawPicture("uri12");

            service.SendPicture(pic9);
            service.SendPicture(pic10);
            service.SendPicture(pic11);
            service.SendPicture(pic12);
            service.Reload();

            service.UpdateRound();
            service.Reload();

            Assert.IsFalse(service.CanFinishGame());

            var pic13 = pl1.DrawPicture("uri13");
            var pic14 = pl2.DrawPicture("uri14");
            var pic15 = pl3.DrawPicture("uri15");
            var pic16 = pl4.DrawPicture("uri16");

            service.SendPicture(pic13);
            service.SendPicture(pic14);
            service.SendPicture(pic15);
            service.SendPicture(pic16);
            service.Reload();

            Assert.IsTrue(service.CanFinishGame());
        }
        
        //[TestMethod]
        //public void CanJoinGameTest()
        //{
        //    RoomService service = GetTestService();
        //}

        //[TestMethod]
        //public void CanJoinGameTest()
        //{
        //    RoomService service = GetTestService();
        //}

        //[TestMethod]
        //public void CanJoinGameTest()
        //{
        //    RoomService service = GetTestService();
        //}

        //[TestMethod]
        //public void CanJoinGameTest()
        //{
        //    RoomService service = GetTestService();
        //}

        //[TestMethod]
        //public void CanJoinGameTest()
        //{
        //    RoomService service = GetTestService();
        //}

        //[TestMethod]
        //public void CanJoinGameTest()
        //{
        //    RoomService service = GetTestService();
        //}

        //[TestMethod]
        //public void CanJoinGameTest()
        //{
        //    RoomService service = GetTestService();
        //}
    }
}
