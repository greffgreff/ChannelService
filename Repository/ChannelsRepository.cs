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

            const string channelSql = @"
                SELECT * 
                FROM Channels
                WHERE Id = @Id AND DeletionDate IS NULL 
                FETCH FIRST 1 ROWS ONLY";

            var channel = await connection.QueryFirstOrDefaultAsync<Channel>(channelSql, new { Id = id });

            if (channel == null)
            {
                return null;
            }

            const string membersSql = @"
                SELECT * 
                FROM ChannelMembers 
                WHERE ChannelId = @ChannelId AND LeaveDate IS NULL";

            var members = await connection.QueryAsync<Member>(membersSql, new { ChannelId = id });

            if (members.Any())
            {
                channel.Members = members.ToArray();
            }

            const string chatSql = @"
                SELECT *
                FROM ChatHistory h
                INNER JOIN ChannelMembers m ON h.Author = m.Id
                INNER JOIN Channels c ON m.ChannelId = c.Id
                WHERE c.Id = @ChannelId AND h.DeletionDate IS NULL";

            var messages = await connection.QueryAsync<Message>(chatSql, new { ChannelId = id });

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

            var result = await connection.QueryAsync<Member, Channel, Channel>(
                sql, 
                (user, channel) => { // TODO not working
                    channel.Members = new[] { user };
                    return channel;
                }, 
                parameters);

            return result.ToArray();
        }
    }
}
