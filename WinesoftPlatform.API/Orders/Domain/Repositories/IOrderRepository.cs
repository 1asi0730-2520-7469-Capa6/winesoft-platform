using WinesoftPlatform.API.Orders.Domain.Model.Aggregates;
using WinesoftPlatform.API.Shared.Domain.Repositories;

namespace WinesoftPlatform.API.Orders.Domain.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{

}