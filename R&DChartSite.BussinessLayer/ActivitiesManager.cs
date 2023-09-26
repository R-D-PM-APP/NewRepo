using RDChartSite.BussinessLayer;
using R_DChartSite.Entities;
using RDChartSite.BussinessLayer.Abstract;
using RDChartSite.DataAccessLayer.EntitiyFramework;
using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.Entity;
using R_DChartSite.Entities.ValueObjects;

namespace RDChartSite.BussinessLayer
{
    public class ActivitiesManager : ManagerBase<DailyActivity>
    {
        private static readonly Random random = new Random();

        public string GenerateRandomString()
        {
            const int minLength = 60;
            int randomStringLength = random.Next(minLength, 150); 
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string result = "";

            for (int i = 0; i < randomStringLength; i++)
            {
                int randomIndex = random.Next(chars.Length);
                result += chars[randomIndex];
            }

            return result;
        }



        Repository<DailyActivity> repositoryDaily = new Repository<DailyActivity>();
        Repository<HourlyActivity> repositoryHourly = new Repository<HourlyActivity>();
        public List<HourlyActivity> GetHourlyActivities()
        {

            return repositoryHourly.List();
        }

        public int GetHourlyActivityCount(int userId)
        {
            using (var context = new RdDbContext())
            {
                var count = context.DailyActivities
                    .Where(d => d.UserId == userId)
                    .SelectMany(d => d.HourlyActivities)
                    .Count();

                return count;
            }
        }
        public int ProjectHoursCount(string ProjectCode)
        {
            using (var context = new RdDbContext())
            {
                var count = context.HourlyActivity
                    .Where(d =>d.ProjectCode == ProjectCode)
                    .Count();

                return count;
            }
        }
        public int ProjectUsersCount(string ProjectCode)
        {
            using (var context = new RdDbContext())
            {
                var count = context.HourlyActivity
                    .Where(d => d.ProjectCode == ProjectCode)
                    .GroupBy(d=>d.DailyActivity.UserId)
                    .Count();

                return count;
            }
        }
        private static Tuple<DateTime, DateTime> GetWeekBounds(DateTime date)
        {
            int currentWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int currentYear = date.Year;

            DateTime weekStart = new DateTime(currentYear, 1, 1).AddDays((currentWeekNumber - 3) * 7); 
            DayOfWeek startDayOfWeek = CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(weekStart);

            if (startDayOfWeek != DayOfWeek.Monday)
                weekStart = weekStart.AddDays(7 - (int)startDayOfWeek + 1);

            DateTime weekEnd = weekStart.AddDays(6);

            return Tuple.Create(weekStart, weekEnd);
        }

        public int GetHourlyActivityCountInWeek()
        {
            DateTime today = DateTime.Today;

            Tuple<DateTime, DateTime> weekBounds = GetWeekBounds(today);
            DateTime weekStart = weekBounds.Item1;
            DateTime weekEnd = weekBounds.Item2;

            using (var context = new RdDbContext())
            {
                var weeklyData = context.DailyActivities
                    .Where(d => d.Date >= weekStart && d.Date <= weekEnd)
                    .ToList();

                return weeklyData.Count;
            }
        }
        public List<DailyActivityData> GetDailyActivitiesInWeek()
        {
            DateTime today = DateTime.Today;

            Tuple<DateTime, DateTime> weekBounds = GetWeekBounds(today);
            DateTime weekStart = weekBounds.Item1;
            DateTime weekEnd = weekBounds.Item2;

            using (var context = new RdDbContext())
            {
                var query = from dailyActivity in context.DailyActivities
                            join user in context.Users on dailyActivity.UserId equals user.Id
                            join hourlyActivity in context.HourlyActivity on dailyActivity.Id equals hourlyActivity.DailyActivityId
                            where dailyActivity.Date >= weekStart && dailyActivity.Date <= weekEnd
                            select new DailyActivityData
                            {
                                DailyActivity = dailyActivity,
                                User = user,
                                HourlyActivity = hourlyActivity
                            };

                var result = query.ToList();
                return result;
            }
        }

        public int GetHourlyActivityCount(int userId, DateTime date)
        {
            using (var context = new RdDbContext())
            {
                var count = context.DailyActivities
                    .Where(d => d.UserId == userId && d.Date == date)
                    .SelectMany(d => d.HourlyActivities)
                    .Count();

                return count;
            }
        }
        public bool HourlyControl(int userId, DateTime date, TimeSpan time)
        {
            using (var context = new RdDbContext())
            {
                bool existingRecord = context.HourlyActivity
                    .Any(a => a.DailyActivity.UserId == userId && a.DailyActivity.Date == date && a.Time == time);

                return existingRecord;
            }
        }
        public bool DailyExistControl(DateTime date)
        {
            bool existingdaily;
            using (var context = new RdDbContext())
            {

                existingdaily = context.DailyActivities.Any(x => x.Date == date);
                return existingdaily;
            }
        }

        public Dictionary<string, int> GetTimeCountByProjectCode()
        {
            Dictionary<string, int> timeCountByProjectCode = new Dictionary<string, int>();
            using (RdDbContext db = new RdDbContext())
            {
                var hourlyActivities = db.HourlyActivity.GroupBy(x => x.ProjectCode);
                foreach (var group in hourlyActivities)
                {
                    string projectCode = group.Key;
                    int timeCount = group.Count();
                    timeCountByProjectCode.Add(projectCode, timeCount);
                }
            }
            return timeCountByProjectCode;
        }
    }
}
