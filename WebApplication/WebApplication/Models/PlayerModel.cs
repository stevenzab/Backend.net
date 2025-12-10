namespace WebApplication.Models
{
    public class PlayerModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ShortName { get; set; }

        public string Sex { get; set; }

        public CountryModel Country { get; set; }

        public string Picture { get; set; }

        public DataModel Data { get; set; }
    }
}
