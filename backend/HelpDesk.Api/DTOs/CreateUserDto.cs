using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;
using HelpDesk.Api.Constants;

namespace HelpDesk.Api.DTOs;

public class CreateUserDto
{
    [Required]
    [StringLength(100,MinimumLength =2)]
    public string FullName {get;set;} = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email {get;set;} = string.Empty;

    [Required]
    [MinLength(8)]
    public string password {get;set;} = string.Empty;

    [Required]
    [RegularExpression("^(Customer|Agent|Manager|Admin)$",
       ErrorMessage = " only allowed four roles :customer,agent,amanger,admin"
    )]

    public string Role { get; set; } = UserRoles.Agent;

}