using R_DChartSite.Entities;
using R_DChartSite.Entities.ValueObjects;
using RDChartSite.BussinessLayer.Abstract;
using RDChartSite.BussinessLayer.Results;
using RDChartSite.DataAccessLayer.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.BussinessLayer
{
    public class TeamsManager : ManagerBase<Teams>
    {
        public List<Teams> GetTeams() { return new List<Teams>(); }

        public string IdtoName(int? Id)
        {
            if (Id == null)
            {
                return "system";
            }
            else
            {

                Teams team = Find(x => x.TeamId == Id);
                return team.TeamName;
            }
        }

        public List<int> GetLeadersList()
        {
            using (var dbContext = new RdDbContext()) 
            {
                var leaderIds = dbContext.Teams
                    .Where(team => team.LeaderId.HasValue) 
                    .Select(team => team.LeaderId.Value)
                    .ToList();

                return leaderIds;   
            }
        }



    }
}
