namespace ChannelService.Contracts
{
    public class Channel
    {
        public string Id { get; set; }
        public ChannelType ChannelType { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string EncryptionKey { get; set; } // TODO move to dedicated endpoint
        public Member[] Members { get; set; }
        public Message[] Messages { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime DeletionDate { get; set; }
    }
}
