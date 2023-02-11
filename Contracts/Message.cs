namespace ChannelService.Contracts
{
    public class Message
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}