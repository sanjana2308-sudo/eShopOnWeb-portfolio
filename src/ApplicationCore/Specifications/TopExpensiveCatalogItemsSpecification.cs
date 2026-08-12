using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public class TopExpensiveCatalogItemsSpecification : Specification<CatalogItem>
{
    public TopExpensiveCatalogItemsSpecification(int count)
        : base()
    {
        Query
            .OrderByDescending(i => i.Price)
            .Take(count);
    }
}
