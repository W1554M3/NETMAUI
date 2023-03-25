using System.Net.Http.Json;

namespace FilmFinder.Services
{
    public class FilmsService
    {
        HttpClient httpClient;
        public FilmsService()
        {
            this.httpClient = new HttpClient();
        }

        List<Film> filmList;
        public async Task<List<Film>> GetFilms()
        {
            if (filmList?.Count > 0)
                return filmList;

            // Online
            var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");
            if (response.IsSuccessStatusCode)
            {
                filmList = await response.Content.ReadFromJsonAsync<List<Film>>();
            }
            // Offline

            using var stream = await FileSystem.OpenAppPackageFileAsync("filmdata.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            filmList = JsonSerializer.Deserialize<List<Film>>(contents);

            return filmList;
        }
    }
}
