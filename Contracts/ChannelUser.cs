using System.Text.Json.Serialization;

namespace ChannelService.Contracts
{
    public class ChannelUser
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Image { get; set; }
        public RoleType Role { get; set; }
        public DateTime JoinDate { get; set; }
    }
}