using ChannelService.Contracts;
using ChannelService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ChannelService.Controllers
{
    [ApiController]
    [Route("channels")]
    public class ChannelController : ControllerBase
    {
        private readonly ChannelsRepository repository;

        public ChannelController(ChannelsRepository repository) 
        {
            this.repository = repository;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<Channel> GetChannel(string id)
        {
            return await repository.GetChannelById(id);
        }

        [HttpGet]
        [Route("/user/{userId}")]
        public Channel[] GetChannelsFromUser(string userId)
        {
            return new[] {  
                new Channel
                {
                    Id = Guid.NewGuid().ToString(),
                    ChannelType = ChannelType.PersonalChat,
                    Name = "Channel Name 1",
                    Image = "https://i.imgur.com/gxPewqw.jpg",
                    EncryptionKey = "1234 1234 1234 1234 1234 1234",
                    Members = new[]
                    {
                        new Member
                        {
                            Id = userId,
                            Nickname = "User 1",
                            Image = "",
                            Role = RoleType.Memeber
                        },
                        new Member
                        {
                            Id = Guid.NewGuid().ToString(),
                            Nickname = "User 2",
                            Image = "",
                            Role = RoleType.Memeber
                        }
                    }
                },
                new Channel
                {
                    Id = Guid.NewGuid().ToString(),
                    ChannelType = ChannelType.GroupChat,
                    Name = "Channel Name 2",
                    Image = "https://i.imgur.com/gxPewqw.jpg",
                    EncryptionKey = "1234 1234 1234 1234 1234 1234",
                    Members = new[]
                    {
                        new Member
                        {
                            Id = userId,
                            Nickname = "User 1",
                            Image = "",
                            Role = RoleType.Memeber
                        },
                        new Member
                        {
                            Id = Guid.NewGuid().ToString(),
                            Nickname = "User 2",
                            Image = "",
                            Role = RoleType.Moderator
                        },
                        new Member
                        {
                            Id = Guid.NewGuid().ToString(),
                            Nickname = "User 3",
                            Image = "",
                            Role = RoleType.Memeber
                        }
                    }
                }
            };
        }
    }
}
