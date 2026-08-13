using Microsoft.eShopWeb.ApplicationCore.Entities;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Specifications;

public class TopExpensiveCatalogItemsSpecification
{
    [Fact]
    public void ReturnsItemsOrderedByPriceDescending()
    {
        var spec = new eShopWeb.ApplicationCore.Specifications.TopExpensiveCatalogItemsSpecification(5);

        var result = spec.Evaluate(GetTestCollection()).ToList();

        Assert.NotNull(result);
        Assert.Equal(3.00m, result[0].Price);
        Assert.Equal(2.00m, result[1].Price);
        Assert.Equal(1.50m, result[2].Price);
    }

    [Fact]
    public void ReturnsOnlyRequestedCount()
    {
        var spec = new eShopWeb.ApplicationCore.Specifications.TopExpensiveCatalogItemsSpecification(2);

        var result = spec.Evaluate(GetTestCollection()).ToList();

        Assert.Equal(2, result.Count);
    }

    private List<CatalogItem> GetTestCollection()
    {
        var catalogItemList = new List<CatalogItem>();

        catalogItemList.Add(new CatalogItem(1, 1, "Item 1", "Item 1", 1.00m, "TestUri1"));
        catalogItemList.Add(new CatalogItem(1, 1, "Item 1.5", "Item 1.5", 1.50m, "TestUri1"));
        catalogItemList.Add(new CatalogItem(2, 2, "Item 2", "Item 2", 2.00m, "TestUri2"));
        catalogItemList.Add(new CatalogItem(3, 3, "Item 3", "Item 3", 3.00m, "TestUri3"));

        return catalogItemList;
    }
}
