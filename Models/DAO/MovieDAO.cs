using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class MovieDAO
    {
        public MovieDAO()
        {
            DirectorsIds = new List<int> { };
            Title = string.Empty;
            OriginalTitle = string.Empty;
            Overview = string.Empty;
            Genres = new List<Genre> { };
            Countries = new List<Country> { };
            CreatedOn = DateTime.Now;
        }
        private int _viewingYear;
        public Guid Uuid { get; set; }
        public int Id { get; set; }
        public List<int> DirectorsIds { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Overview { get; set; }
        public string? AdditionalInfos { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ViewingDate { get; set; }

        public int ViewingYear
        {
            get
            {
                if (_viewingYear == 0)
                {
                    return ViewingDate.Year;
                }
                else
                {
                    return _viewingYear;
                }
            }
            set => _viewingYear = value;
        }
        public DateTime CreatedOn { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
        public Support? Support { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Country> Countries { get; set; }
        public bool? MightWatch { get; set; }
    }
}
