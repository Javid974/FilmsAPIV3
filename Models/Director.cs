namespace Models
{
    public class Director
    {
        public Director()
        {
            Name = string.Empty;  // or some other default value
            CreatedOn = DateTime.Now;
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreatedOn { get; private set; }
    }
}