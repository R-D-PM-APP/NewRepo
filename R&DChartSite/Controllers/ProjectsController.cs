using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using RDChartSite.BussinessLayer;
using RDChartSite.BussinessLayer.Results;
using RDChartSite.Entities;

using System.Security.Cryptography.X509Certificates;
using System.Web.Configuration;
using R_DChartSite.Filters;

namespace RDChartSite.Controllers
{
    public class ProjectsController : Controller
    {
        
        BussinessLayerResult<Users> result = new BussinessLayerResult<Users>();
        private ProjectsManager projectsManager = new ProjectsManager();
        private UserManager usersManager = new UserManager();
       

        public ActionResult Index()
        {
            return View(projectsManager.List());
        }

        [HttpPost]
        public ActionResult Index(List<int> selectedProjects)
        {
            if (selectedProjects == null || !selectedProjects.Any())
            {
                return View(projectsManager.List());
            }
            else if (Session["Login"] != null)
            {
                var user = Session["Login"] as Users;
                var userId = user.Id;
                var userToUpdate = usersManager.Find(x => x.Id == userId);      
                if (userToUpdate != null)
                {
                    foreach (int projectId in selectedProjects)
                    {
                        var projectToAdd = projectsManager.Find(x=>x.Id==projectId);
                        bool projectexistinguser = projectsManager.ProjectHasSameUserControl(projectId,userId);
                        bool userexistingproject = usersManager.UserHasSameProjectControl(projectId, userId);
                        if (projectToAdd != null && !userexistingproject && !projectexistinguser)
                        {
                            userToUpdate.Projects.Add(projectToAdd);
                        }
                    }
                    usersManager.Save();
                }
                return View(projectsManager.List());
            }
            return View(projectsManager.List());
        }

        [AuthAdmin]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = projectsManager.Find(x=>x.Id==id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        [Auth]
        [AuthAdmin]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Projects projects)
        {
            if (ModelState.IsValid)
            {
               projectsManager.Insert(projects);
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        [AuthAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = projectsManager.Find(x => x.Id == id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Projects projects)
        {
            if (ModelState.IsValid)
            {
                Projects existingProject = projectsManager.Find(x => x.Id == projects.Id);

                if (existingProject != null)
                {
                    
                    existingProject.ProjectName = projects.ProjectName;
                    existingProject.ProjectCode = projects.ProjectCode;
                    existingProject.IsProjectActive = projects.IsProjectActive;

                    projectsManager.Update(existingProject);
                }

                return RedirectToAction("Index");
            }
            return View(projects);
        }

        [AuthAdmin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = projectsManager.Find(x => x.Id == id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = projectsManager.Find(x => x.Id == id);
          projectsManager.Delete(projects);
            return RedirectToAction("Index");
        }

    }
}
