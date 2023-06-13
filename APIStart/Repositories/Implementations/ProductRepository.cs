using APIStart.Contexts;
using APIStart.Models;
using APIStart.Repositories.Interfaces;

namespace APIStart.Repositories.Implementations;

public class ProductRepository : Repository<Product>, IProductRepository
{
	public ProductRepository( AppDbContext context ) : base( context )
	{
	}
}
