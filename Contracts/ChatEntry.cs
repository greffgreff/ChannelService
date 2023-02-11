namespace ChannelService.Contracts
{
    public class ChatEntry
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string EntryType { get; set; }
        public object Content { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime DeletionDate { get; set; }
    }
}