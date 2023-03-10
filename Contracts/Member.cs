namespace ChannelService.Contracts
{
    public class Member
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Nickname { get; set; }
        public string Image { get; set; }
        public RoleType Role { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime LeaveDate { get; set; }
    }
}