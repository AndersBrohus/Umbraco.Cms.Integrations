﻿using System.Text;
using System.Text.Json;

using Umbraco.Cms.Integrations.PIM.Inriver.Models;

namespace Umbraco.Cms.Integrations.PIM.Inriver.Services
{
    public class InriverService : IInriverService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public InriverService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //public async Task<ServiceResponse<Entity>> GetEntitySummary(int id)
        //{
        //    var client = _httpClientFactory.CreateClient(Constants.InriverClient);

        //    var response = await client.GetAsync($"entities/{id}/summary");

        //    var content = await response.Content.ReadAsStringAsync();

        //    return response.IsSuccessStatusCode
        //        ? ServiceResponse<Entity>.Ok(JsonSerializer.Deserialize<Entity>(content))
        //        : ServiceResponse<Entity>.Fail(content);
        //}

        public async Task<ServiceResponse<IEnumerable<EntityType>>> GetEntityTypes()
        {
            var client = _httpClientFactory.CreateClient(Constants.InriverClient);

            var response = await client.GetAsync("model/entitytypes");

            var content = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode
                ? ServiceResponse<IEnumerable<EntityType>>.Ok(JsonSerializer.Deserialize<IEnumerable<EntityType>>(content))
                : ServiceResponse<IEnumerable<EntityType>>.Fail(content);
        }

        public async Task<ServiceResponse<IEnumerable<EntityData>>> FetchData(FetchDataRequest request)
        {
            var client = _httpClientFactory.CreateClient(Constants.InriverFetchClient);

            var response = await client.PostAsync("entities:fetchdata",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode
                ? ServiceResponse<IEnumerable<EntityData>>.Ok(JsonSerializer.Deserialize<IEnumerable<EntityData>>(content))
                : ServiceResponse<IEnumerable<EntityData>>.Fail(content);
        }

        public async Task<ServiceResponse<QueryResponse>> Query(QueryRequest request)
        {
            var client = _httpClientFactory.CreateClient(Constants.InriverClient);

            var response = await client.PostAsync("query",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode
                ? ServiceResponse<QueryResponse>.Ok(JsonSerializer.Deserialize<QueryResponse>(content))
                : ServiceResponse<QueryResponse>.Fail(content);
        }
    }
}
