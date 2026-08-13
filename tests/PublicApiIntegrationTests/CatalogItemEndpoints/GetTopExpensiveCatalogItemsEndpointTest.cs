using Microsoft.eShopWeb;
using Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace PublicApiIntegrationTests.CatalogItemEndpoints;

[TestClass]
public class GetTopExpensiveCatalogItemsEndpointTest
{
    [TestMethod]
    public async Task ReturnsFiveItemsOrderedByPriceDescending()
    {
        var response = await ProgramTest.NewClient.GetAsync("api/catalog-items-top-expensive");
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var model = stringResponse.FromJson<GetTopExpensiveCatalogItemsResponse>();

        Assert.AreEqual(5, model!.CatalogItems.Count);

        for (int i = 0; i < model.CatalogItems.Count - 1; i++)
        {
            Assert.IsTrue(model.CatalogItems[i].Price >= model.CatalogItems[i + 1].Price);
        }
    }
}
