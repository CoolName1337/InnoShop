using System.Net.Http.Json;
namespace Infrastructure
{
    public class MyHttpClient(HttpClient httpClient)
    {
        public async Task<List<int>> SoftDelete(int ownerId, bool isDeleted)
        {
            var response = await httpClient.PostAsync(
                $"api/Product/admin/soft-delete/{ownerId}&{isDeleted}", null);
            
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<int>>() ?? [];      
        }
    }
}
