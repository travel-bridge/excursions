using System.ComponentModel.DataAnnotations;

namespace Excursions.Infrastructure.Events;

public class EventsOptions
{
    public const string SectionKey = "Events";

    [Required]
    public string BootstrapServers { get; set; } = null!;
}