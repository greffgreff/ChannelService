namespace Observer.Repository
{
    public static class Queries
    {
        public const string CreateOrUpdateChannel = @"CreateOrUpdateChannel @Id, @Name, @YoutubeUrl, @Description, @VideoCount, @SubscriberCount, @ViewCount, @Keywords, @Country, @ImageUrl, @PublishedAt";
        public const string CreateOrUpdateVideo = @"CreateOrUpdateVideo @Id, @Title, @Description, @Tags, @CategoryId, @Duration, @LikeCount, @ViewCount, @CommentCount, @YoutubeUrl, @ThumbnailUrl, @LiveStreamStart, @LiveStreamEnd, @PublishedAt, @ChannelId";
        public const string CreateOrUpdateComment = @"CreateOrUpdateComment @Id, @VideoId, @Content, @LikeCount, @ReplyCount, @YoutubeUrl, @UpdatedAt, @PublishedAt, @ChannelId";
        public const string CreateOrUpdateChapter = @"CreateOrUpdateChapter @Id, @VideoId, @Time, @Description, @YoutubeUrl, @TrackId";
        public const string CreateOrUpdateTrack = @"CreateOrUpdateTrack @Id, @Title, @DurationMs, @SpotifyUrl, @Popularity, @ImageUrl, @PreviewUrl, @PublishedAt";
        public const string CreateOrUpdateArtist = @"CreateOrUpdateArtist @Id, @Name, @SpotifyUrl";
        public const string CreateOrUpdateAudio = @"CreateOrUpdateAudio @Id, @TrackId, @TimeSignature, @Key, @Mode, @Acousticness, @Danceability, @Energy, @Instrumentalness, @Liveness, @Loudness, @Speechiness, @Tempo, @Valence";
        public const string CreateOrUpdateYoutubeNotification = @"CreateOrUpdateYoutubeNotification @Id, @VideoTitle, @VideoId, @ChannelId, @PublishedAt, @UpdatedAt, @IsNewVideo";
        public const string CreateOrUpdateYoutubeSubscription = @"CreateOrUpdateYoutubeSubscription @Id, @ChannelId, @Subscribed, @FeedCallback, @FeedTopic";
        public const string CreateOrUpdateYoutubeUpload = @"CreateOrUpdateYoutubeUpload @Id, @VideoId, @ChannelId, @IsRecorded";
        public const string CreateOrUpdateArtistTracks = @"CreateOrUpdateArtistTracks @ArtistId, @TrackId";
    }
}