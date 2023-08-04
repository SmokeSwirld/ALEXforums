namespace ALEXforums.Data.Entity
{
    public class Theme
    {
        public Guid      Id             { get; set; }
        public Guid      UserId         { get; set; }
        public string    TitleTheme     { get; set; } = null!;
        public String    ImageUrlTheme  { get; set; } = null!;
        public string    ContentTheme   { get; set; } = null!;
        public DateTime  CreatedAtTheme { get; set; }


        // Навігаційна властивість для зв'язку з класом Message
       // public ICollection<Message> Messages { get; set; }
    }
}
