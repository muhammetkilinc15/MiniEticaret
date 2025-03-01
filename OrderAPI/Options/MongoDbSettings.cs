namespace OrderAPI.Options
{
    public sealed record MongoDbSettings
    {
        public string DatabaseName { get; init; }
        public string ConnectionString { get; init; }
    };
}
