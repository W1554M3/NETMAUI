
namespace FilmFinder;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(FilmsDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}