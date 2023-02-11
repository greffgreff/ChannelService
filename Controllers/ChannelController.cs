using ChannelService.Contracts;
using ChannelService.Repository;
using Microsoft.AspNetCore.Mvc;
using Swan;

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
        [Route("{id}/members")]
        public IActionResult GetChannelMemebers(string id)
        {
            return Ok(repository.GetMembersFromChannel(id).Await());
        }

        [HttpGet]
        [Route("{id}/chat")]
        public IActionResult GetChatHistoryFromChannel(string id, [FromQuery] int count = 100, [FromQuery] int offset = 0)
        {
            if (count < 0 || offset < 0)
            {
                return BadRequest("The value of parameters `count` and `offset` cannot be negative");
            }

            var entries = repository.GetChatEntriesFromChannel(id, count, offset).Await();
            entries.ToList().ForEach(e => e.Content = e.DeletionDate != default ? e.Content = null : e.Content); // TODO handle this inside service layer

            string protocol = HttpContext.Request.IsHttps ? "https" : "http";
            string host = HttpContext.Request.Host.Value;
            string baseUrl = $@"{protocol}://{host}/channels/{id}/history";

            var history = new ChatHistory
            {
                CurrentPage = @$"{baseUrl}?count={count}&offset={offset}",
                NextPage = entries.Length < count ? null : @$"{baseUrl}?count={count}&offset={offset+count}",
                PreviousPage = offset - count < 0 ? null : @$"{baseUrl}?count={count}&offset={offset-count}",
                Entries = entries,
            };

            return Ok(history);
        }

        [HttpGet]
        [Route("/user/{userId}")]
        public Channel[] GetChannelsFromUser(string userId)
        {
            return Array.Empty<Channel>();
        }
    }
}
