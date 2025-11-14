using WinesoftPlatform.API.Inventory.Domain.Model.Aggregates;
using WinesoftPlatform.API.Inventory.Domain.Model.Commands;
using WinesoftPlatform.API.Inventory.Domain.Repositories;
using WinesoftPlatform.API.Inventory.Domain.Services;
using WinesoftPlatform.API.Shared.Domain.Repositories;

namespace WinesoftPlatform.API.Inventory.Application.Internal.CommandServices;

public class SupplyCommandService(
    ISupplyRepository supplyRepository,
    IUnitOfWork unitOfWork
) : ISupplyCommandService
{
    public async Task<Supply?> Handle(CreateSupplyCommand command)
    {
        var supply = new Supply(command);

        try
        {
            await supplyRepository.AddAsync(supply);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[CreateSupply] Error: {e.Message}");
            return null;
        }

        return supply;
    }

    public async Task<Supply?> Handle(UpdateSupplyCommand command)
    {
        var existing = await supplyRepository.FindByIdAsync(command.Id);
        if (existing is null)
            return null;

        existing.UpdateDetails(
            command.SupplyName,
            command.Quantity,
            command.Unit,
            command.Supplier,
            command.Price,
            command.Date
        );

        try
        {
            supplyRepository.Update(existing);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[UpdateSupply] Error: {e.Message}");
            return null;
        }

        return existing;
    }

    public async Task<bool> Handle(DeleteSupplyCommand command)
    {
        var existingSupply = await supplyRepository.FindByIdAsync(command.Id);
        if (existingSupply is null)
            return false;

        try
        {
            supplyRepository.Remove(existingSupply);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Error - DeleteSupply] {e.Message}");
            return false;
        }

        return true;
    }
}
