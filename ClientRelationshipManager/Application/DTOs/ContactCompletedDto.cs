using System.ComponentModel.DataAnnotations;
using BusinessLogic.Models;

namespace Application.DTOs;

public class ContactCompletedDto
{
    public string Description { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
}