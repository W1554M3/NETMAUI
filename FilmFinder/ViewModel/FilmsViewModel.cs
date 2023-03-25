using FilmFinder.Services;

namespace FilmFinder.ViewModel;

public partial class FilmsViewModel : BaseViewModel
{
    public ObservableCollection<Film> Films { get; } = new();
    FilmsService Service;
    IConnectivity connectivity;
    IGeolocation geolocation;
    public FilmsViewModel(FilmsService filmService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Film Finder";
        this.Service = filmService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }
    
    [RelayCommand]
    async Task GoToDetails(Film film)
    {
        if (film == null)
        return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
        {
            {"Film", film }
        });
    }

    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    async Task GetFilmsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No connectivity!",
                    $"Please check internet and try again.", "OK");
                return;
            }

            IsBusy = true;
            var films = await Service.GetFilms();

            if(Films.Count != 0)
                Films.Clear();

            foreach(var film in films)
                Films.Add(film);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get films: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }

    }

    [RelayCommand]
    async Task GetClosestFilms()
    {
        if (IsBusy || Films.Count == 0)
            return;

        try
        {
            // Get cached location, else get real location.
            var location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }

            // Find closest monkey to us
            var first = Films.OrderBy(m => location.CalculateDistance(
                new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                .FirstOrDefault();

            await Shell.Current.DisplayAlert("", first.Title + " " +
                first.Country, "OK");

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
}
