using WinesoftPlatform.API.Orders.Domain.Model.Aggregates;
using WinesoftPlatform.API.Orders.Domain.Repositories;
using WinesoftPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using WinesoftPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace WinesoftPlatform.API.Orders.Infrastructure.Persistence.EFC.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }
}