using APIStart.Contexts;
using APIStart.Models;
using APIStart.Repositories.Interfaces;

namespace APIStart.Repositories.Implementations;

public class ServiceRepository : Repository<Service>, IServiceRepository
{
    public ServiceRepository(AppDbContext context) : base(context)
    {
    }
}