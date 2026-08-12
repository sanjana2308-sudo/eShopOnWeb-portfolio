using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

/// <summary>
/// Get the top 5 most expensive catalog items
/// </summary>
public class GetTopExpensiveCatalogItemsEndpoint(IRepository<CatalogItem> itemRepository, IUriComposer uriComposer)
    : EndpointWithoutRequest<GetTopExpensiveCatalogItemsResponse>
{
    public override void Configure()
    {
        Get("api/catalog-items-top-expensive");
        AllowAnonymous();
        Description(d =>
            d.Produces<GetTopExpensiveCatalogItemsResponse>()
            .WithTags("CatalogItemEndpoints"));
    }

    public override async Task<GetTopExpensiveCatalogItemsResponse> ExecuteAsync(CancellationToken ct)
    {
        var response = new GetTopExpensiveCatalogItemsResponse();

        var spec = new TopExpensiveCatalogItemsSpecification(5);
        var items = await itemRepository.ListAsync(spec, ct);

        response.CatalogItems = items.Select(item => new CatalogItemDto
        {
            Id = item.Id,
            CatalogBrandId = item.CatalogBrandId,
            CatalogTypeId = item.CatalogTypeId,
            Description = item.Description,
            Name = item.Name,
            PictureUri = uriComposer.ComposePicUri(item.PictureUri),
            Price = item.Price
        }).ToList();

        return response;
    }
}
