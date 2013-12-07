using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Telestrations.Models;

namespace Telestrations.Controllers
{
    public class RoomController : Controller
    {
        private IRoomService service;

        public RoomController()
        {
            this.service = new RoomService();
        }

        public RoomController(IRoomService service)
        {
            this.service = service;
        }

        // GET: /Room/
        public ActionResult Index()
        {
            var rooms = service.GetRooms();
            return View(rooms);
        }

        // GET: /Room/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomViewModel room = service.ShowRoom(id.Value);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: /Room/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Room/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Owner,Phase,Version")] Room room)
        {
            if (ModelState.IsValid)
            {
                //db.Rooms.Add(room);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(room);
        }

        // GET: /Room/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomViewModel room = service.ShowRoom(id.Value);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: /Room/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Owner,Phase,Version")] Room room)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(room).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        // GET: /Room/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomViewModel room = service.ShowRoom(id.Value);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: /Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomViewModel room = service.ShowRoom(id);
            //db.Rooms.Remove(room);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
