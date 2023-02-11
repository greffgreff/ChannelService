namespace ChannelService.Contracts
{
    public partial class ChatHistory
    {
        public string CurrentPage { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }
        public ChatEntry[] Entries { get; set; }
    }
}