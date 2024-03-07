using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Statistics
    {
        public Statistics()
        {
            CountryCount = new Dictionary<string, Tuple<Country, int>>();
            SupportCount = new Dictionary<string, int>();
            MovieCountByRegion = new Dictionary<string, int>();
            CountriesByRegion = new Dictionary<string, List<Country>>();
        }
        public int Year { get; set; }
        public Dictionary<string, Tuple<Country, int>> CountryCount { get; set; }
        public Dictionary<string, int> SupportCount { get; set; }
        public Dictionary<string, int> MovieCountByRegion { get; set; }
        public Dictionary<string, List<Country>> CountriesByRegion { get; set; }

        public void CalculateStatistics(IList<Movie> movies, IList<Country> countries)
        {
            CountryCount = GetMoviesCountPerCountry(movies, countries);
            CalculateCountryCountsByRegion();
            SupportCount = CountSupports(movies);
        }

        public Dictionary<string, int> CountSupports(IList<Movie> movies)
        {
#pragma warning disable CS8629 // Nullable value type may be null.
            return movies
                .Where(movie => movie.Support != null && movie.Support.HasValue) // Exclure les films sans support défini
                .GroupBy(movie => movie.Support.Value.ToString()) // Grouper par le support
                .ToDictionary(group => group.Key, group => group.Count()); // Convertir en dictionnaire
#pragma warning restore CS8629 // Nullable value type may be null.
        }



        private void CalculateCountryCountsByRegion()
        {
            foreach (var countryCount in CountryCount)
            {
                var country = countryCount.Value.Item1;
                var count = countryCount.Value.Item2;
                var region = country.Region;
                if (!MovieCountByRegion.ContainsKey(region))
                {
                    MovieCountByRegion[region] = 0;
                    CountriesByRegion[region] = new List<Country>();
                }
                MovieCountByRegion[region] += count;
                CountriesByRegion[region].Add(country);
            }
        }

        private Dictionary<string, Tuple<Country, int>> GetMoviesCountPerCountry(IList<Movie> movies, IList<Country> countries)
        {
            var countryCountMap = new Dictionary<string, int>();
            foreach (var movie in movies)
            {
                foreach (var country in movie.Countries)
                {
                    if (countryCountMap.ContainsKey(country.Iso_3166_1))
                    {
                        countryCountMap[country.Iso_3166_1]++;
                    }
                    else
                    {
                        countryCountMap[country.Iso_3166_1] = 1;
                    }
                }
            }

            var moviesCountPerCountry = new Dictionary<string, Tuple<Country, int>>();
            foreach (var country in countries)
            {
                int count = countryCountMap.ContainsKey(country.Iso_3166_1) ? countryCountMap[country.Iso_3166_1] : 0;
                moviesCountPerCountry[country.Iso_3166_1] = new Tuple<Country, int>(country, count);
            }

            return moviesCountPerCountry.OrderByDescending(kvp => kvp.Value.Item2).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

    }

}
