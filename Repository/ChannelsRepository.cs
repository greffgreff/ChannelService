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

            const string channelSql = @"SELECT * FROM Channels WHERE Id = @Id AND DeletionDate IS NULL FETCH FIRST 1 ROWS ONLY";
            object channelParameters = new { Id = id };
            var channel = await connection.QueryFirstOrDefaultAsync<Channel>(channelSql, channelParameters);

            if (channel == null)
            {
                return null;
            }

            const string memebersSql = @"SELECT * FROM ChannelMemebers WHERE ChannelId = @ChannelId";
            object memebersParameters = new { ChannelId = id };
            var memebers = await connection.QueryAsync<Memeber>(memebersSql, memebersParameters);

            if (memebers.Any())
            {
                channel.Memebers = memebers.ToArray();
            }

            const string chatSql = @"
                SELECT *
                FROM ChatHistory h
                INNER JOIN ChannelMemebers m ON h.Author = m.Id
                INNER JOIN Channels c ON m.ChannelId = c.Id
                WHERE c.Id = @ChannelId";
            object chatParameters = new { ChannelId = id };
            var messages = await connection.QueryAsync<Message>(chatSql, chatParameters);

            if (messages.Any())
            {
                channel.Messages = messages.ToArray();
            }

            return channel;
        }

        public async Task<Channel[]> GetChannelsFromUser(string userId)
        {
            using var connection = factory.GetOpenConnection();

            object parameters = new { UserId = userId };
            const string sql = @"
                SELECT * FROM Channels c
                INNER JOIN ChannelMemebers m ON c.Id = m.ChannelId
                WHERE c.Id IN (
	                SELECT c.Id FROM Channels c
	                INNER JOIN ChannelMemebers m ON c.Id = m.ChannelId
	                INNER JOIN Users u ON m.UserId = u.Id
	                WHERE u.Id = @UserId)";

            var result = await connection.QueryAsync<Memeber, Channel, Channel>(
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
