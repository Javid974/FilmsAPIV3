namespace Models
{
    public class Genre
    {
        public Genre()
        {
            Name = string.Empty;  // or some other default value
        }
        public int Id { get; set; }
        public string Name { get; set; }
       
    }
}