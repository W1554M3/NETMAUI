namespace FilmFinder.ViewModel;

[QueryProperty(nameof(Film), "Film")]
public partial class FilmsDetailsViewModel : BaseViewModel
{
    IMap map;
    public FilmsDetailsViewModel(IMap map)
    {
        this.map = map;
    }

    [ObservableProperty]
    Film film;

    [RelayCommand]
    async Task OpenMap()
    {
        try
        {
            await map.OpenAsync(Film.Latitude, Film.Longitude, new MapLaunchOptions
            {
                Name = Film.Title,
                NavigationMode = NavigationMode.None
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Shell.Current.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }
}
