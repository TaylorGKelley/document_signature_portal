using System.ComponentModel.DataAnnotations;

namespace DocSign.Infrastructure.Configuration;

public class StorageOptions {
    public const string SectionName = "Storage";

    #region Options
    [Required, Url] public string Endpoint { get; init; } = default!;
    [Required] public string AccessKey { get; init; } = default!;
    [Required] public string SecretKey { get; init; } = default!;
    [Required] public string DocumentsBucket { get; init; } = "documents";
    [Required] public string QuarantineBucket { get; init; } = "quarantine";
    [Range(1, 1440)] public int PresignedUrlExpiryMinutes { get; init; } = 15;
    #endregion
}
