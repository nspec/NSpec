namespace SampleSpecs
{
    public class User
    {
        public User()
        {
            Id = 1;
        }

        public int Id { get; set; }

        public bool Terminated { get; set; }

        public bool Admin { get; set; }
    }
}