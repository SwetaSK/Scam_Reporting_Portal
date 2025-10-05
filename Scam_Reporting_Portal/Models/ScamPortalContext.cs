using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Scam_Reporting_Portal.Models;

namespace Scam_Reporting_Portal.Models
{
    public class ScamPortalContext : DbContext
    {
        //public ScamPortalContext() : base("name=Scam_Reporting_PortalEntities") { }

        public ScamPortalContext() : base("name=Scam_Reporting_PortalEntities1") { }

        public DbSet<User> Users { get; set; } 
        public DbSet<ScamReport> ScamReports { get; set; }
    }
}