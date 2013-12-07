using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telestrations.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }
        //public GameViewModel CurrentGame { get; set; }

        public RoomViewModel(Room room)
        {
            this.Id = room.Id;
            this.Owner = room.Owner;
            this.Status = room.Phase.ToString();
        }
    }

    //public class GameViewModel
    //{
    //    public int Id { get; set; }
    //    public List<Player> Players { get; set; }
    //}

    //public class PlayerViewModel
    //{
    //    public int Id { get; set; }
    //    public SketchBookViewModel CurrentSketchBook { get; set; }
    //    public bool HasSentPicture { get; set; }
    //    public string UserName { get; set; }
    //}

    //public class SketchBookViewModel
    //{
    //    public int Id { get; set; }
    //    public string Author { get; set; }
    //    public string QuestionWord { get; set; }
    //    public List<PictureViewModel> Pictures { get; set; }
    //}

    //public class PictureViewModel
    //{
    //    public int Id { get; set; }
    //    public string Uri { get; set; }
    //    public string Author { get; set; }
    //}
}