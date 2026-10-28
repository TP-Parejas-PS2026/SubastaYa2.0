using System.ComponentModel.DataAnnotations;

namespace SubastaYa.Api.Application.DTOs.Auctions;

public class CreateAuctionRequestDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public string Category { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio inicial debe ser mayor a 0.")]
    public decimal StartingPrice { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es requerida.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "La fecha de fin es requerida.")]
    public DateTime EndTime { get; set; }

    public string? ImageUrl { get; set; }
}
