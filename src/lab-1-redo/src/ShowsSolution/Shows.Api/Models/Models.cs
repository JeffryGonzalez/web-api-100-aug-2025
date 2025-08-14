namespace Shows.Api.Models
{
    public class Models
    {
        public record ShowCreateRequest
        {
            public string Name { get; init; } = string.Empty;
            public string Description { get; init; } = string.Empty;
            public string StreamingService { get; init; } = string.Empty;
        }
        public record ShowDetailsResponse
        {
            public Guid Id { get; init; } = Guid.Empty;
            public string Name { get; init; } = string.Empty;
            public string Description { get; init; } = string.Empty;
            public string StreamingService { get; init; } = string.Empty;

            public DateTimeOffset CreatedAt { get; init; }
        }

        public record ShowSummaryResponse
        {
            public Guid Id { get; init; }
            public string Name { get; init; } = string.Empty;
        }

        public class ShowEntity
        {
            public Guid Id { get; init; } = Guid.Empty;
            public string Name { get; init; } = string.Empty;
            public string Description { get; init; } = string.Empty;
            public string StreamingService { get; init; } = string.Empty;

            public DateTimeOffset CreatedAt { get; init; }
        }
    }
}
