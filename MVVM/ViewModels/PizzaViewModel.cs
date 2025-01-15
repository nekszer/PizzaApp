using Light.Maui.MVVM.ViewModels;
using System.IO.IsolatedStorage;

namespace PizzaApp.MVVM.ViewModels
{
	public class PizzaViewModel : ViewModelBase
	{

		#region Notified Property PizzaSizes
		/// <summary>
		/// PizzaSizes
		/// </summary>
		private List<PizzaSize> pizzaSizes;
		public List<PizzaSize> PizzaSizes
		{
			get { return pizzaSizes; }
			set { pizzaSizes = value; OnPropertyChanged(); }
		}
		#endregion

		#region Notified Property SelectPizzaSizeCommand
		/// <summary>
		/// SelectPizzaSizeCommand
		/// </summary>
		private ICommand selectPizzaSizeCommand;
		public ICommand SelectPizzaSizeCommand
		{
			get { return selectPizzaSizeCommand; }
			set { selectPizzaSizeCommand = value; OnPropertyChanged(); }
		}
		#endregion

		#region Notified Property CurrentPizaSelected
		/// <summary>
		/// CurrentPizaSelected
		/// </summary>
		private PizzaSize currentPizzaSelected;
		public PizzaSize CurrentPizaSelected
		{
			get { return currentPizzaSelected; }
			set { currentPizzaSelected = value; OnPropertyChanged(); }
		}
		#endregion

		#region Notified Property Toppings
		/// <summary>
		/// Toppings
		/// </summary>
		private List<Topping> toppings;
		public List<Topping> Toppings
		{
			get { return toppings; }
			set { toppings = value; OnPropertyChanged(); }
		}
		#endregion

		#region Notified Property ToppingSelectionCommand
		/// <summary>
		/// ToppingSelectionCommand
		/// </summary>
		private ICommand toppingSelectionCommand;
		public ICommand ToppingSelectionCommand
		{
			get { return toppingSelectionCommand; }
			set { toppingSelectionCommand = value; OnPropertyChanged(); }
		}
		#endregion

		#region Notified Property TotalPrice
		/// <summary>
		/// TotalPrice
		/// </summary>
		private double totalPrice;
		public double TotalPrice
		{
			get { return totalPrice; }
			set { totalPrice = value; OnPropertyChanged(); }
		}
		#endregion

		#region Notified Property BtnAddPizzaToBasket
		/// <summary>
		/// BtnAddPizzaToBasket
		/// </summary>
		private ICommand btnAddPizzaToBasket;
		public ICommand BtnAddPizzaToBasket
		{
			get { return btnAddPizzaToBasket; }
			set { btnAddPizzaToBasket = value; OnPropertyChanged(); }
		}
		#endregion

		public override void Appearing(string route)
		{
			base.Appearing(route);

			PizzaSizes = new List<PizzaSize>
			{
				new PizzaSize("S", 240, 169, 30),
				new PizzaSize("M", 260, 199, 40),
				new PizzaSize("L", 300, 249, 45)
			};

			SelectPizzaSizeCommand = new LightCommand<PizzaSize>(SelectPizzaSizeCommand_Execute);
			SelectPizzaSizeCommand_Execute(PizzaSizes.First());

			Toppings = new List<Topping>
			{
				new Topping("Jalapeños", "jalapenos.png", "jalapenospizza.png"),
				new Topping("Hongos", "hongos.png", "hongospizza.png"),
				new Topping("Aceitunas", "aceitunas.png", "aceitunas.png")
			};

			ToppingSelectionCommand = new AsyncCommand<Topping>(ToppingSelectionCommand_Execute);

			BtnAddPizzaToBasket = new LightCommand(BtnAddPizzaToBasket_Execute);
		}

		private void BtnAddPizzaToBasket_Execute(object obj)
		{
			(View as Page).DisplayAlert("Info", "Se ha agregado tu pizza al carrito", "Aceptar");
		}

		private const double ToppingPrice = 10;

		private async Task ToppingSelectionCommand_Execute(Topping topping)
		{
			topping.IsSelected = !topping.IsSelected;
			CalculateTotalPrice();
			await AnimateToppings(topping);
		}

		private void CalculateTotalPrice()
		{
			var totalToppingsSelected = Toppings?.Count(t => t.IsSelected) ?? 0;
			TotalPrice = CurrentPizaSelected.Price + (totalToppingsSelected * ToppingPrice);
		}

		private async Task AnimateToppings(Topping topping)
		{
			var gridImages = FindViewByName<Grid>("GridImages");
			if (topping.IsSelected)
			{
				await ShowTopping(topping.View);
				gridImages.Add(topping.View);
			}
			else
			{
				await HideTopping(topping.View);
				gridImages.Remove(topping.View);
			}
		}

		private async Task HideTopping(Image view)
		{
			await view.FadeTo(0);
		}

		private async Task ShowTopping(Image view)
		{
			await view.FadeTo(1);
			await view.ScaleTo(CurrentPizaSelected.ScaleTo * 1.1);
		}

		private void SelectPizzaSizeCommand_Execute(PizzaSize pizzaSize)
		{
			var previousSelection = PizzaSizes.FirstOrDefault(p => p.IsSelected);
			if (previousSelection != null && previousSelection.Initial == pizzaSize.Initial)
				return;

			PizzaSizes.ForEach(p => p.IsSelected = false);
			CurrentPizaSelected = pizzaSize;
			pizzaSize.IsSelected = true;

			CalculateTotalPrice();
			AnimatePizza(pizzaSize);
		}

		private void AnimatePizza(PizzaSize pizzaSize)
		{
			var pizzaImage = FindViewByName<Image>("PizzaImage");
			var tableImage = FindViewByName<Image>("TableImage");

			var baseElements = new List<Image> { pizzaImage, tableImage };
			foreach (var item in baseElements)
				AnimageBaseImage(item, pizzaSize);

			var gridImages = FindViewByName<Grid>("GridImages");
			foreach (var image in gridImages.Children.Cast<Image>())
			{
				if(baseElements.Contains(image)) 
					continue;
				
				ShowTopping(image);
			}
		}

		public void AnimageBaseImage (Image image, PizzaSize pizzaSize)
		{
			image.ScaleTo(pizzaSize.ScaleTo);
			image.RotateTo(image.Rotation + 10);
		}
	}

	public class Topping : BindableObject
	{
		public string Name { get; set; }
		public string PickerImage { get; set; }
		public string PizzaImage { get; set; }

		#region Notified Property IsSelected
		/// <summary>
		/// IsSelected
		/// </summary>
		private bool isSelected;
		public bool IsSelected
		{
			get { return isSelected; }
			set { isSelected = value; OnPropertyChanged(); }
		}

		public Image View { get; set; }
		#endregion

		public Topping(string name, string pickerImage, string overPizzaImage)
		{
			Name = name;
			PickerImage = pickerImage;
			PizzaImage = overPizzaImage;
			var image = new Image { Source = overPizzaImage, HeightRequest = 240, WidthRequest = 240 };
			View = image;
		}
	}

	public class PizzaSize : BindableObject
	{
		public string Name { get; set; }
		public string Initial { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public double Price { get; set; }
		public int Centimeters { get; set; }
		public double ScaleTo { get; set; }

		#region Notified Property IsSelected
		/// <summary>
		/// IsSelected
		/// </summary>
		private bool isSelected;
		public bool IsSelected
		{
			get { return isSelected; }
			set { isSelected = value; OnPropertyChanged(); }
		}
		#endregion

		public PizzaSize(string name, int size, double price, int centimeters)
		{
			Name = name;
			Initial = name?.FirstOrDefault().ToString() ?? string.Empty;
			ScaleTo = size / 300d;
			Width = size;
			Height = size;
			Price = price;
			Centimeters = centimeters;
		}
	}
}