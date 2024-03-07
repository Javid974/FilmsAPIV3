using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class MightWatchMovieDAO : MovieDAO
    {
        public MightWatchMovieDAO()
        {
        }

        public double? SenscritiqueNote { get; set; }

        public double? ImdbNote { get; set; }

    }
}
