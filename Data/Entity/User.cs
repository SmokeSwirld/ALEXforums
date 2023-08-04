namespace ALEXforums.Data.Entity
{
    public class User
    {
        public Guid      Id           { get; set; }
        public String    Login        { get; set; } = null!;
        public String    PasswordHash { get; set; } = null!;       
        public String?   Avatar       { get; set; }
        public DateTime  RegisteredDt { get; set; }

        // Навігаційна властивість для зв'язку з класом Theme
      //  public ICollection<Theme> Themes { get; set; }

        // Навігаційна властивість для зв'язку з класом Message
      //  public ICollection<Message> Messages { get; set; }
    }
}
