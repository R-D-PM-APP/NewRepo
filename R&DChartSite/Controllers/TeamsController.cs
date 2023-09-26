using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using R_DChartSite.Entities;
using RDChartSite.BussinessLayer;

namespace RDChartSite.Controllers
{
    public class TeamsController : Controller
    {
       private TeamsManager teamsManager = new TeamsManager();

        // GET: Teams
        public ActionResult Index()
        {
            return View(teamsManager.List());
        }

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teams teams = teamsManager.Find(x=>x.TeamId == id);
            if (teams == null)
            {
                return HttpNotFound();
            }
            return View(teams);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamId,TeamName,LeaderId")] Teams teams)
        {
            if (ModelState.IsValid)
            {
                teamsManager.Insert(teams);
                teamsManager.Save();
                return RedirectToAction("Index");
            }

            return View(teams);
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teams teams = teamsManager.Find(x=>x.TeamId == id);
            if (teams == null)
            {
                return HttpNotFound();
            }
            return View(teams);
        }

        // POST: Teams/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamId,TeamName,LeaderId")] Teams teams)
        {
            if (ModelState.IsValid)
            {

                teamsManager.Save();
                return RedirectToAction("Index");
            }
            return View(teams);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teams teams = teamsManager.Find(x => x.TeamId == id);
            if (teams == null)
            {
                return HttpNotFound();
            }
            return View(teams);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teams teams = teamsManager.Find(x => x.TeamId == id);
            teamsManager.Delete(teams);
            teamsManager.Save();
            return RedirectToAction("Index");
        }
    }
}
