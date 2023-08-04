namespace ALEXforums.Data.Entity
{
    public class Message
    {
        public Guid      UserId           { get; set; }
        public Guid      ThemeId          { get; set; }
        public string    ContentMessage   { get; set; } = null!;
        public DateTime  CreatedAtMessage { get; set; }

    }
}
