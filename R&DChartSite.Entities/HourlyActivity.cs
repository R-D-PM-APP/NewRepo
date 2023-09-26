using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_DChartSite.Entities
{
    public class HourlyActivity
    {
        [Key]
        public int Id { get; set; }

        
        public int DailyActivityId { get; set; } 
        public DailyActivity DailyActivity { get; set; }


        public TimeSpan Time { get; set; } 
        public string Description { get; set; }
        public string ProjectCode { get; set; } 
    }
}
