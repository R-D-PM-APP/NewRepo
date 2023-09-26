using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDChartSite.Entities;

namespace R_DChartSite.Entities
{
    public class OffDayForms
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Guid FormGuid { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string PType { get; set; }
        public TimeSpan StartTime { get; set; }

        public TimeSpan ReturnTime { get; set; }

        [Display(Name = "Proje Kodu")]
        public string ProjectCode { get; set; }

        [StringLength(150)]
        public string Description { get; set; }
        
        public string Reason { get; set; }

        public DateTime PostTime { get; set; }

        public int UserId { get; set; } 
        public Users User { get; set; }

        public bool IsApproved { get; set; }

    }
}
