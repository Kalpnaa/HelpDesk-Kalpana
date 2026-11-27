
using System.ComponentModel.DataAnnotations;
using HelpDesk.Api.Enums;

namespace HelpDesk.Api.DTOs;

public class UpdateTicketStatusDto
{
    [EnumDataType(typeof(TicketStatus))]
    public TicketStatus Status { get; set; }
}
