using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.DAL
{

    public class TreatmentServer
    {
        public static string conStr = AccommodateServer.conf.GetConnectionString("MyDatabase");
    }
}
