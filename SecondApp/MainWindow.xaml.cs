using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstApp;

namespace SecondApp
{
    public partial class MainWindow : Window
    {
        private List<Product> productList;
        private StackPanel userChoice, editingButtons, removeOrBackButton;
        private TextBlock label;
        private ListBox productListBox;
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            // Window options
            Title = "Administration";
            Width = 750;
            Height = 550;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Scrolling
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Content = root;

            // Main grid
            Grid grid = new Grid();
            root.Content = grid;
            grid.Margin = new Thickness(5);
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //0
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(350) }); //1
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //2
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //3

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });

            productList = new List<Product>();

            label = new TextBlock
            {
                FontSize = 20,
                Margin = new Thickness(2),
                TextAlignment = TextAlignment.Center,
                Width = 200,
                Text = "Vad vill du göra?"
            };
            grid.Children.Add(label);
            Grid.SetColumn(label, 1);
            Grid.SetRow(label, 0);

            //Button stackpanel
            userChoice = new StackPanel { Orientation = Orientation.Vertical };
            grid.Children.Add(userChoice);
            Grid.SetColumn(userChoice, 1);
            Grid.SetRow(userChoice, 2);

            Button productEditing = new Button
            {
                Content = "Produktändringar",
                Width = 150,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            userChoice.Children.Add(productEditing);
            productEditing.Click += ClickedProductEditing;

            Button discountEditing = new Button
            {
                Content = "Rabattändringar",
                Width = 150,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            userChoice.Children.Add(discountEditing);

            Button exitProgram = new Button
            {
                Content = "Avsluta",
                Width = 150,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            userChoice.Children.Add(exitProgram);
            exitProgram.Click += ClickedExit;
            //Button stack panel above

            //After clicking on "produktändringar"
            productListBox = new ListBox
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 200,
                Visibility = Visibility.Collapsed
            };
            grid.Children.Add(productListBox);
            Grid.SetColumn(productListBox, 1);
            Grid.SetRow(productListBox, 1);

            //Editing buttons after user clicked on productediting
            editingButtons = new StackPanel { Orientation = Orientation.Horizontal };
            grid.Children.Add(editingButtons);
            Grid.SetColumn(editingButtons, 1);
            Grid.SetRow(editingButtons, 2);
            editingButtons.Visibility = Visibility.Collapsed;

            Button AddNewProduct = new Button
            {
                Content = "Lägg till",
                Width = 100,
                Margin = new Thickness(50, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            editingButtons.Children.Add(AddNewProduct);

            Button editProductButton = new Button
            {
                Content = "Ändra",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            editingButtons.Children.Add(editProductButton);

            //Remove or back buttons after user clicked on "Produktändringar"
            removeOrBackButton = new StackPanel { Orientation = Orientation.Horizontal };
            grid.Children.Add(removeOrBackButton);
            Grid.SetColumn(removeOrBackButton, 1);
            Grid.SetRow(removeOrBackButton, 3);
            removeOrBackButton.Visibility = Visibility.Collapsed;

            Button RemoveProductButton = new Button
            {
                Content = "Ta bort",
                Width = 100,
                Margin = new Thickness(50, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            removeOrBackButton.Children.Add(RemoveProductButton);

            Button backFromProductEditing = new Button
            {
                Content = "Tillbaka",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            removeOrBackButton.Children.Add(backFromProductEditing);
            backFromProductEditing.Click += ClickedBackFromEditing;
        }

        private void ClickedBackFromEditing(object sender, RoutedEventArgs e)
        {
            //Hiding all the editing GUI
            productListBox.Visibility = Visibility.Collapsed;
            editingButtons.Visibility = Visibility.Collapsed;
            removeOrBackButton.Visibility = Visibility.Collapsed;

            //Showing the GUI from the "startwindow".
            userChoice.Visibility = Visibility.Visible;
            //Changing the label back to start label.
            label.Text = "Vad vill du göra?";

            //Rensar dessa för att återställa alla värden
            productList.Clear();
            productListBox.Items.Clear();
        }

        private void ClickedProductEditing(object sender, RoutedEventArgs e)
        {
            //Läser in produktlistan från csv. Anropar metoden.
            ReadProductList();

            //Showing these after user clicked on "Produktändringar"
            productListBox.Visibility = Visibility.Visible;
            editingButtons.Visibility = Visibility.Visible;
            removeOrBackButton.Visibility = Visibility.Visible;
            //Hiding the buttons from "startwindow".
            userChoice.Visibility = Visibility.Collapsed;
            //Changing the label
            label.Text = "Sortiment";
        }
        private void ClickedExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ReadProductList()
        { 
            //Läser in produktlistan ur csv från huvudprogrammet (FirstApp).
            string[] productArray = File.ReadAllLines("produktLista.csv");

            //Separerar alla ',' och lägger in de i diverse titel.
            foreach (string line in productArray)
            {
                string[] columns = line.Split(',');
                string productName = columns[0];
                string productDescription = columns[1];
                decimal productPrice = decimal.Parse(columns[2].Replace('.', ','));
                string productImage = columns[3];

                //För varje rad i csv filen skapar vi ett nytt objekt av klassen.
                Product products = new Product(productName, productDescription, productPrice, productImage);
                //Lägger till objektet i en lista (titelnamn, beskrivning, bild och pris)
                productList.Add(products);
            }
            foreach (Product p in productList)
            {
                productListBox.Items.Add(p.Title + " | " + p.Price.ToString("C"));
            }
        }
    }
}
