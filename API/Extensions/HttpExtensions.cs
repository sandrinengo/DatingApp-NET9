using System;
using System.Text.Json;
using API.Helpers;

namespace API.Extensions;

public static class HttpExtensions
{
	public static void AddPaginationHeader<T>(this HttpResponse response, PagedListHelper<T> data)
	{
		var paginationHeader = new PaginationHeaderHelper(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);

		var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		// We cannot rely on the API to return the camel case, we have to instruct it to return camel case data
		response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationHeader, jsonOptions));
		// By default the client cannot access to the header pagination above, and we need to return a header
		// to allow the client to access the header.
		response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
	}
}
