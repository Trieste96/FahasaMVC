using FAHASA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAHASA
{
    public static class FahasaInit
    {
        public static FahasaContext context;
        public static void Initialize()
        {
            FahasaInit.context = new FahasaContext();
            FahasaInit.context.Database.Initialize(false);
        }
    }
}