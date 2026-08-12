using System;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;

public class GetTopExpensiveCatalogItemsResponse : BaseResponse
{
    public GetTopExpensiveCatalogItemsResponse(Guid correlationId) : base(correlationId)
    {
    }

    public GetTopExpensiveCatalogItemsResponse()
    {
    }

    public List<CatalogItemDto> CatalogItems { get; set; } = new();
}
