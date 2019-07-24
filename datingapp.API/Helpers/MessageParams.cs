namespace datingapp.API.Helpers
{
    public class MessageParams
    {
        public int UserId { get; set; }
        public string MessageContainer { get; set; } = "Unread";
    }
}