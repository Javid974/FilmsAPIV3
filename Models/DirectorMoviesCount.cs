using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DirectorMoviesCount
    {
        public DirectorMoviesCount()
        {
            DirectorName = string.Empty; // Initialized to avoid null
        }
        public int DirectorId { get; set; }
        public string DirectorName { get; set; }
        public int Count { get; set; }
    }

}
