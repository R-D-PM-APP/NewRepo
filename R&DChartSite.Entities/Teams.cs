using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_DChartSite.Entities
{
    public class Teams
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; } 

        public string TeamName { get; set; }
        public int? LeaderId { get; set; }
        public virtual ICollection<Users> Users { get; set; } = new List<Users>();

    }
}
