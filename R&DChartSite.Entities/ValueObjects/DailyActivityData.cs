using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_DChartSite.Entities.ValueObjects
{
    public class DailyActivityData
    {
        public DailyActivity DailyActivity { get; set; }
        public Users User { get; set; }
        public HourlyActivity HourlyActivity { get; set; }
    }
}
