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

            channel.Members = await GetMembersFromChannel(channelSql);

            return channel;
        }

        public async Task<Member[]> GetMembersFromChannel(string channelId)
        {
            using var connection = factory.GetOpenConnection();

            const string membersSql = @"
                SELECT * 
                FROM ChannelMembers 
                WHERE ChannelId = @ChannelId AND LeaveDate IS NULL";

            var members = await connection.QueryAsync<Member>(membersSql, new { ChannelId = channelId });

            return members.ToArray();
        }

        public async Task<ChatEntry[]> GetChatEntriesFromChannel(string channelId, int count = 100, int offset = 0)
        {
            using var connection = factory.GetOpenConnection();

            const string sql = @"
                SELECT *
                FROM ChatEntries e
                INNER JOIN ChannelMembers m ON e.Author = m.Id
                INNER JOIN Channels c ON m.ChannelId = c.Id
                WHERE c.Id = @ChannelId
                ORDER BY SendDate DESC
                LIMIT @Count OFFSET @Offset";

            var param = new 
            {
                ChannelId = channelId,
                Count = count,
                Offset = offset
            };

            var entries = await connection.QueryAsync<ChatEntry>(sql, param);

            return entries.ToArray();
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
