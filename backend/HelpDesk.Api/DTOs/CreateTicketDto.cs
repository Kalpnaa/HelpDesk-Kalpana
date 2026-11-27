
using System.ComponentModel.DataAnnotations;
using HelpDesk.Api.Enums;
using System.Text.Json.Serialization;

namespace HelpDesk.Api.DTOs;

public class CreateTicketDto
{
    [Required]
    [StringLength(150, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [EnumDataType(typeof(TicketPriority))]
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
}
