using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.Metrics;

namespace Models.ViewModel
{

    public class Movie
    {
        public Movie()
        {
            Directors = new List<Director>();
            Genres = new List<Genre>();
            Countries = new List<Country>();
            Title = string.Empty;
            OriginalTitle = string.Empty;
        }

        public Guid Uuid { get; set; }
        public int Id { get; set; }
        public List<Director> Directors { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string? AdditionalInfos { get; set; }
        public string? Overview { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ViewingDate { get; set; }
        public int ViewingYear { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
        public Support? Support { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Country> Countries { get; set; }
        public bool? MightWatch { get; set; }
        public double? ImdbNote { get; set; }
        public double? SenscritiqueNote { get; set; }
        public double? AverageNote
        {
            get
            {
                if (ImdbNote == null && SenscritiqueNote != null)
                {
                    return SenscritiqueNote;
                }
                else if (SenscritiqueNote == null && ImdbNote != null)
                {
                    return ImdbNote;
                }
                else if (SenscritiqueNote != null && ImdbNote != null)
                {
                    return (SenscritiqueNote.Value + ImdbNote.Value) / 2.0;
                }
                else
                {
                    return null;
                }
            }
        }

    }

}