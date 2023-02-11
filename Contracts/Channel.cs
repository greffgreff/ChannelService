namespace ChannelService.Contracts
{
    public class Channel
    {
        public string Id { get; set; }
        public ChannelType ChannelType { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string EncryptionKey { get; set; }
        public ChannelUser[] Memebers { get; set; }
        public DateTime CreatationDate { get; set; }
    }
}
