using System.ComponentModel.DataAnnotations;

namespace DocSign.Infrastructure.Configuration;

public class RabbitMqOptions {
    public const string SectionName = "RabbitMq";

    #region Options
    [Required] public string Host { get; init; } = default!;
    [Range(1, 65535)] public int Port { get; init; } = 5672; // default port
    [Required] public string Username { get; init; } = default!;
    [Required] public string Password { get; init; } = default!;
    #endregion
}
