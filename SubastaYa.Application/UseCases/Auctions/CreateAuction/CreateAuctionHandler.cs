using SubastaYa.Api.Application.CQRS;
using SubastaYa.Api.Application.Interfaces.Persistence;
using SubastaYa.Api.Domain.Entities;
using SubastaYa.Api.Domain.Enums;
using SubastaYa.Api.Domain.Exceptions;

namespace SubastaYa.Api.Application.UseCases.Auctions.CreateAuction;

public class CreateAuctionHandler : ICommandHandler<CreateAuctionCommand, int>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAuctionHandler(IAuctionRepository auctionRepository, IUnitOfWork unitOfWork)
    {
        _auctionRepository = auctionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> HandleAsync(CreateAuctionCommand command, CancellationToken ct = default)
    {
        var req = command.Request;

        // Validaciones en servidor de integridad y reglas de negocio
        if (string.IsNullOrWhiteSpace(req.Title))
            throw new DomainException("El título de la subasta es obligatorio.");

        if (string.IsNullOrWhiteSpace(req.Description))
            throw new DomainException("La descripción de la subasta es obligatoria.");

        if (string.IsNullOrWhiteSpace(req.Category))
            throw new DomainException("La categoría de la subasta es obligatoria.");

        if (req.StartingPrice <= 0)
            throw new DomainException("El precio inicial debe ser mayor a cero.");

        if (req.StartTime == default || req.EndTime == default)
            throw new DomainException("Las fechas de inicio y finalización deben ser válidas.");

        if (req.StartTime >= req.EndTime)
            throw new DomainException("La fecha de inicio debe ser anterior a la fecha de finalización (StartTime < EndTime).");

        if (req.EndTime <= DateTime.UtcNow)
            throw new DomainException("La fecha de finalización debe encontrarse en el futuro.");

        var auction = new Auction
        {
            Title = req.Title.Trim(),
            Description = req.Description.Trim(),
            Category = req.Category.Trim(),
            StartingPrice = req.StartingPrice,
            CurrentPrice = req.StartingPrice,
            StartTime = req.StartTime,
            EndTime = req.EndTime,
            SellerId = command.SellerId,
            ImageUrl = req.ImageUrl,
            State = AuctionState.Active
        };

        await _auctionRepository.AddAsync(auction, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return auction.Id;
    }
}
