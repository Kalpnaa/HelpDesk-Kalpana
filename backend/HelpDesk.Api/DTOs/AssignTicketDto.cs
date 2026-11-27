
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Api.DTOs;

public class AssignTicketDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int AssignedToId { get; set; }
}
