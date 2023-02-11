using ChannelService.Contracts;
using ChannelService.Repository.Connection;
using Dapper;

namespace ChannelService.Repository
{
    public class ChannelsRepository
    {
        private readonly IConnectionFactory factory;

        public ChannelsRepository(IConnectionFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<Channel> GetChannelById(string id)
        {
            using var connection = factory.GetOpenConnection();

            var channel = await connection.QueryFirstOrDefaultAsync<Channel>("SELECT * FROM Channels WHERE Id = @Id FETCH FIRST 1 ROWS ONLY", new { Id = id });

            if (channel == null)
            {
                return null;
            }

            var channelUsers = await connection.QueryAsync<ChannelUser>("SELECT *, UserId AS Id FROM ChannelUsers WHERE ChannelId = @ChannelId", new { ChannelId = id });
            channel.Memebers = channelUsers.ToArray();

            return channel;
        }

        public async Task<Channel[]> GetChannelsFromUser(string userId)
        {
            using var connection = factory.GetOpenConnection();

            object parameters = new { UserId = userId };
            const string sql = @"
                SELECT * FROM Channels c
                INNER JOIN ChannelUsers u ON c.Id = u.ChannelId
                WHERE c.Id IN (
	                SELECT Id FROM Channels c
	                INNER JOIN ChannelUsers u ON c.Id = u.ChannelId
	                WHERE u.UserId = @UserId)";

            var result = await connection.QueryAsync<ChannelUser, Channel, Channel>(
                sql, 
                (user, channel) => { // TODO not working
                    channel.Memebers = new[] { user };
                    return channel;
                }, 
                parameters);

            return result.ToArray();
        }
    }
}
