using Domain.Dto;

namespace Application.Services;

public interface IReservationService
{
    Task ApplyExactReservationsAsync(Guid projectId, string userId, CancellationToken cancellationToken);
    Task UpdateReservationsAsync(List<BomItemDto> updatedBomItems, CancellationToken cancellationToken);
}