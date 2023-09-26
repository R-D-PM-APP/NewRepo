using R_DChartSite.BussinessLayer;
using R_DChartSite.Entities;
using R_DChartSite.Entities.ValueObjects;
using R_DChartSite.Filters;
using RDChartSite.BussinessLayer;
using RDChartSite.BussinessLayer.Results;
using RDChartSite.Entities;
using RDChartSite.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RDChartSite.Controllers
{
    //[Auth]
    public class InSiteController : Controller
    {
        private ProjectsManager projectsManager = new ProjectsManager();
        private ActivitiesManager activitiesManager = new ActivitiesManager();
        private UserManager userManager = new UserManager();
        private FormsManager formManager = new FormsManager();

        // GET: InSite
        public ActionResult DailySchedule()
        {
            var viewModel = new DailyScheduleViewModel
            {
                Projects = projectsManager.List(),
                DailyActivity = new DailyActivity(),
                HourlyActivity = new HourlyActivity()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DailySchedule(DailyScheduleViewModel viewModel)
        {
            if (Session["Login"] != null)
            {
                var user = Session["Login"] as Users;
                var userId = user.Id;
                var userToUpdate = userManager.Find(x => x.Id == userId);
                string dateString = Request.Form["DailyActivities.Date"];
                DateTime date;
                if (DateTime.TryParse(dateString, out date))
                {
                    //günlük kayıt var mı kontrolü
                    if (activitiesManager.DailyExistControl(date))
                    {
                        List<HourlyActivity> hourlyActivities = new List<HourlyActivity>();
                        var existingdaily = activitiesManager.Find(x => x.Date == date);
                        existingdaily.HourlyActivities = new List<HourlyActivity>();
                        for (int i = 8; i < 24; i++)
                        {
                            TimeSpan time = TimeSpan.FromHours(i);
                            bool existingrecord = activitiesManager.HourlyControl(userId, date, time);
                            var hourlyDescription = Request.Form[$"HourlyActivities[{i - 8}].Description"];
                            if (!existingrecord)
                            {
                                if (!string.IsNullOrEmpty(hourlyDescription))
                                {
                                    var hourlyActivity = new HourlyActivity
                                    {
                                        DailyActivityId = existingdaily.Id,
                                        Time = time,
                                        Description = hourlyDescription,
                                        ProjectCode = Request.Form[$"HourlyActivities[{i - 8}].ProjectCode"]
                                    };

                                    if (hourlyActivity.ProjectCode != "Proje Kodu Seçiniz")
                                    {
                                        existingdaily.HourlyActivities.Add(hourlyActivity);
                                    }
                                    else
                                    {
                                        ModelState.AddModelError($"HourlyActivities[{i + 16}].ProjectCode", $"{i}:00 saatindeki girdinize ait proje kodunu girmediniz.");

                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError($"HourlyActivities[{i + 16}].Description", $"{i}:00 saatindeki girdinize ait kaydınız zaten mevcuttur.");

                            }
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            TimeSpan time = TimeSpan.FromHours(i);
                            bool existingrecord = activitiesManager.HourlyControl(userId, date, time);
                            var hourlyDescription = Request.Form[$"HourlyActivities[{i + 16}].Description"];
                            if (!existingrecord)
                            {
                                if (!string.IsNullOrEmpty(hourlyDescription))
                                {
                                    var hourlyActivity = new HourlyActivity
                                    {
                                        DailyActivityId = existingdaily.Id,
                                        Time = TimeSpan.FromHours(i),
                                        Description = hourlyDescription,
                                        ProjectCode = Request.Form[$"HourlyActivities[{i + 16}].ProjectCode"]
                                    };
                                    if (hourlyActivity.ProjectCode != "Proje Kodu Seçiniz")
                                    {
                                        existingdaily.HourlyActivities.Add(hourlyActivity);
                                    }
                                    else
                                    {
                                        ModelState.AddModelError($"HourlyActivities[{i + 16}].ProjectCode", $"{i}:00 saatindeki girdinize ait proje kodunu girmediniz.");

                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError($"HourlyActivities[{i + 16}].Description", $"{i}:00 saatindeki girdinize ait kaydınız zaten mevcuttur.");

                            }
                        }

                        activitiesManager.Update(existingdaily);
                        return View(viewModel);
                    }
                    else//günlük kayıt yoksa yenisini oluşturur
                    {
                        var dailyActivity = new DailyActivity
                        {
                            UserId = userToUpdate.Id,
                            Date = date,
                            HourlyActivities = new List<HourlyActivity>()
                        };
                        for (int i = 8; i < 24; i++)
                        {
                            TimeSpan time = TimeSpan.FromHours(i);
                            bool existingrecord = activitiesManager.HourlyControl(userId, date, time);
                            var dailyexits = activitiesManager.Find(x => x.Id == dailyActivity.Id);
                            var hourlyDescription = Request.Form[$"HourlyActivities[{i - 8}].Description"];
                            if (!existingrecord)
                            {
                                if (!string.IsNullOrEmpty(hourlyDescription))
                                {
                                    var hourlyActivity = new HourlyActivity
                                    {
                                        DailyActivityId = dailyActivity.Id,
                                        Time = time,
                                        Description = hourlyDescription,
                                        ProjectCode = Request.Form[$"HourlyActivities[{i - 8}].ProjectCode"]
                                    };

                                    if (hourlyActivity.ProjectCode != "Proje Kodu Seçiniz")
                                    {
                                        dailyActivity.HourlyActivities.Add(hourlyActivity);
                                    }
                                    else
                                    {
                                        ModelState.AddModelError($"HourlyActivities[{i + 16}].ProjectCode", $"{i}:00 saatindeki girdinize ait proje kodunu girmediniz.");

                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError($"HourlyActivities[{i + 16}].Description", $"{i}:00 saatindeki girdinize ait kaydınız zaten mevcuttur.");

                            }
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            TimeSpan time = TimeSpan.FromHours(i);
                            bool existingrecord = activitiesManager.HourlyControl(userId, date, time);
                            var hourlyDescription = Request.Form[$"HourlyActivities[{i + 16}].Description"];
                            if (!existingrecord)
                            {
                                if (!string.IsNullOrEmpty(hourlyDescription))
                                {
                                    var hourlyActivity = new HourlyActivity
                                    {
                                        DailyActivityId = dailyActivity.Id,
                                        Time = TimeSpan.FromHours(i),
                                        Description = hourlyDescription,
                                        ProjectCode = Request.Form[$"HourlyActivities[{i + 16}].ProjectCode"]
                                    };
                                    if (hourlyActivity.ProjectCode != "Proje Kodu Seçiniz")
                                    {
                                        dailyActivity.HourlyActivities.Add(hourlyActivity);
                                    }
                                    else
                                    {
                                        ModelState.AddModelError($"HourlyActivities[{i + 16}].ProjectCode", $"{i}:00 saatindeki girdinize ait proje kodunu girmediniz.");

                                    }
                                }

                            }
                            else
                            {
                                ModelState.AddModelError($"HourlyActivities[{i + 16}].Description", $"{i}:00 saatindeki girdinize ait kaydınız zaten mevcuttur.");

                            }
                        }

                        activitiesManager.Insert(dailyActivity);
                        activitiesManager.Save();
                        return View(viewModel);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Lütfen geçerli bir tarih seçin.");
                    return View(viewModel);
                }
            }


            return View();
        }



        public ActionResult MonthlySchedule()
        {

            return View();
        }


        [AuthAdmin]
        public ActionResult Dashboard()
        {

            return View();
        }

        [AuthAdmin]
        public ActionResult WeeklyActivities()
        {

            return View();
        }


        public ActionResult ProjectsDashboardList()
        {

            return View(projectsManager.List());
        }
        public ActionResult OffDayForm()
        {

            return View();
        }

        [HttpPost]
        public ActionResult OffDayForm(OffDayFormsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                BussinessLayerResult<OffDayForms> result = formManager.InsertIntoDb(model);
                if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach(error => ModelState.AddModelError("", error.Message));
                    return View(model);
                }
                else
                {
                    TempData["Notification"] = "İzin talebiniz başarıyla gönderildi.";
                    return RedirectToAction("OffDayForm"); 
                }
            }
        }

        public ActionResult FormApprovalPage(Guid FormGuid)
        {

            OffDayForms offDayForm = formManager.Find(x => x.FormGuid == FormGuid);

            return View(offDayForm);
        }

        [HttpPost]
        public ActionResult FormApprovalPage(Guid FormGuid, bool approval)
        {
            OffDayForms offDayForm = formManager.Find(x => x.FormGuid == FormGuid);
            if (approval)
            {
                if (offDayForm != null)
                {
                    offDayForm.IsApproved = true;
                    formManager.Update(offDayForm);
                }
                return RedirectToAction("Mainpage", "Home");
            }
            else
            {
                formManager.Delete(offDayForm);
                return RedirectToAction("Mainpage", "Home");
            }
        }
    }

}
