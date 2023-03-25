namespace FilmFinder.View;

public partial class MainPage : ContentPage
{
	public MainPage(FilmsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}

