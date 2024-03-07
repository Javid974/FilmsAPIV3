using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{

    public class TopMovie
    {
        public TopMovie()
        {
            Movies = new List<Movie>();
            Id = Guid.Empty;
            Title = string.Empty;
            CreatedOn = DateTime.Now;
        }

        public Guid Id { get; set; }
        public int Years { get; set; }
        public string Title { get; set; }
        public List<Movie> Movies { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
