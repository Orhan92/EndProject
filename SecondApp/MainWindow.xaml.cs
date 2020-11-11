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
        private Image image;
        private Grid productGrid, discountGrid, grid, startGrid;
        private List<Product> productList;
        private List<Discount> discountList;
        private StackPanel userChoice, addNewProduct, showProductListInEdit, discountPanel, newDiscountPanel;
        private WrapPanel imageWrapPanel;
        private TextBlock label;
        private ListBox productListBox, discountListBox;
        private TextBox newPrice, addDiscountCode, addDiscountPercentage;
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
            grid = new Grid();
            root.Content = grid;
            grid.Margin = new Thickness(5);
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //0
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //1
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //2
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //3

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            grid.Visibility = Visibility.Visible;

            //Start window Grid.
            startGrid = new Grid();
            grid.Children.Add(startGrid);
            Grid.SetColumn(startGrid, 0);
            Grid.SetRow(startGrid, 0);
            Grid.SetColumnSpan(startGrid, 3);
            Grid.SetRowSpan(startGrid, 4);

            startGrid.Margin = new Thickness(5);
            startGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //0
            startGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //1
            startGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //2
            startGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //3

            startGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            startGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            startGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            startGrid.Visibility = Visibility.Visible;

            //Grid for product editing
            productGrid = new Grid();
            grid.Children.Add(productGrid);
            Grid.SetColumn(productGrid, 0);
            Grid.SetRow(productGrid, 0);
            Grid.SetColumnSpan(productGrid, 3);
            Grid.SetRowSpan(productGrid, 4);

            productGrid.Margin = new Thickness(5);
            productGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //0
            productGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //1
            productGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //2
            productGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //3

            productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            productGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            productGrid.Visibility = Visibility.Collapsed;

            //Grid for discount editing
            discountGrid = new Grid();
            grid.Children.Add(discountGrid);
            Grid.SetColumn(discountGrid, 0);
            Grid.SetRow(discountGrid, 0);
            Grid.SetColumnSpan(discountGrid, 3);
            Grid.SetRowSpan(discountGrid, 4);

            discountGrid.Margin = new Thickness(5);
            discountGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //0
            discountGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //1
            discountGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //2
            discountGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); //3

            discountGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            discountGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            discountGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            discountGrid.Visibility = Visibility.Collapsed;


            discountList = new List<Discount>();
            productList = new List<Product>();

            //Start Window
            label = new TextBlock
            {
                FontSize = 20,
                Margin = new Thickness(2),
                TextAlignment = TextAlignment.Center,
                Width = 200,
                Text = "Vad vill du göra?"
            };
            startGrid.Children.Add(label);
            Grid.SetColumn(label, 1);
            Grid.SetRow(label, 0);

            //Button stackpanel
            userChoice = new StackPanel { Orientation = Orientation.Vertical };
            startGrid.Children.Add(userChoice);
            Grid.SetColumn(userChoice, 1);
            Grid.SetRow(userChoice, 2);

            Button productEditing = new Button
            {
                Content = "Produktändringar",
                Width = 150,
                Height = 25,
                Margin = new Thickness(0, 125, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            userChoice.Children.Add(productEditing);
            productEditing.Click += ClickedProductEditing;

            Button discountEditing = new Button
            {
                Content = "Rabattändringar",
                Width = 150,
                Height = 25,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            userChoice.Children.Add(discountEditing);
            discountEditing.Click += ClickedDiscountEditing;

            Button exitProgram = new Button
            {
                Content = "Avsluta",
                Width = 150,
                Height = 25,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            userChoice.Children.Add(exitProgram);
            exitProgram.Click += ClickedExit;

            //RABATTÄNDRINGAR
            //KOLUMN 0 står tom.
            //KOLUMN 1 RABATT
            newDiscountPanel = new StackPanel { Orientation = Orientation.Vertical };
            discountGrid.Children.Add(newDiscountPanel);
            Grid.SetColumn(newDiscountPanel, 1);
            Grid.SetRow(newDiscountPanel, 1);

            image = AddImage(@"\Pictures\happysmiley.jpg");
            image.Width = 100;
            image.Margin = new Thickness(0, 50, 0, 0);
            newDiscountPanel.Children.Add(image);

            TextBlock addNewDiscountLabel = new TextBlock
            {
                FontSize = 25,
                Margin = new Thickness(0, 20, 0, 0),
                TextAlignment = TextAlignment.Center,
                Text = "Skapa ny rabattkod",
            };
            newDiscountPanel.Children.Add(addNewDiscountLabel);

            TextBlock addNewDiscountCode = new TextBlock
            {
                FontSize = 20,
                Margin = new Thickness(0, 20, 0, 0),
                TextAlignment = TextAlignment.Center,
                Text = "Rabattkod: ",
            };
            newDiscountPanel.Children.Add(addNewDiscountCode);

            addDiscountCode = new TextBox
            {
                Background = Brushes.LightGoldenrodYellow,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 5, 0),
                Width = 150,
            };
            newDiscountPanel.Children.Add(addDiscountCode);

            TextBlock discountPercentageLabel = new TextBlock
            {
                FontSize = 20,
                Margin = new Thickness(0, 20, 0, 0),
                TextAlignment = TextAlignment.Center,
                Text = "Procent %: ",
            };
            newDiscountPanel.Children.Add(discountPercentageLabel);

            addDiscountPercentage = new TextBox
            {
                Background = Brushes.LightGoldenrodYellow,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 5, 0),
                Width = 150,
            };
            newDiscountPanel.Children.Add(addDiscountPercentage);

            Button addDiscount = new Button
            {
                Content = "Lägg till",
                Width = 150,
                Margin = new Thickness(0, 36, 5, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            newDiscountPanel.Children.Add(addDiscount);
            addDiscount.Click += AddNewDiscount;

            //KOLUMN 2 RABATT
            discountPanel = new StackPanel { Orientation = Orientation.Vertical };
            discountGrid.Children.Add(discountPanel);
            Grid.SetColumn(discountPanel, 2);
            Grid.SetRow(discountPanel, 1);

            TextBlock discountLabel = new TextBlock
            {
                FontSize = 15,
                Margin = new Thickness(0, 20, 0, 0),
                TextAlignment = TextAlignment.Center,
                Text = "Aktiva rabattkoder: ",
            };
            discountPanel.Children.Add(discountLabel);

            discountListBox = new ListBox
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 180,
                Height = 325,
            };
            discountPanel.Children.Add(discountListBox);

            Button editDiscountButton = new Button
            {
                Content = "Ändra",
                Width = 180,
                Margin = new Thickness(10, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            discountPanel.Children.Add(editDiscountButton);

            Button saveDiscount = new Button
            {
                Content = "Spara",
                Width = 180,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            discountPanel.Children.Add(saveDiscount);
            saveDiscount.Click += ClickedSaveDiscount;

            Button removeDiscountButton = new Button
            {
                Content = "Ta bort",
                Width = 180,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            discountPanel.Children.Add(removeDiscountButton);
            removeDiscountButton.Click += ClickedRemoveDiscount;

            Button backFromDiscountEditing = new Button
            {
                Content = "Tillbaka",
                Width = 180,
                Margin = new Thickness(0, 40, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            discountPanel.Children.Add(backFromDiscountEditing);
            backFromDiscountEditing.Click += ClickedBackFromDiscount;


            //PRODUKTÄNDRINGAR NEDAN.
            //KOLUMN 0 PRODUKT
            imageWrapPanel = new WrapPanel { Orientation = Orientation.Vertical };
            productGrid.Children.Add(imageWrapPanel);
            Grid.SetColumn(imageWrapPanel, 0);
            Grid.SetRow(imageWrapPanel, 1);

            TextBlock imageLabel = new TextBlock
            {
                FontSize = 15,
                Margin = new Thickness(50, 20, 0, 0),
                TextAlignment = TextAlignment.Center,
                //Width = 200,
                Text = "Bilder: ",
            };
            imageWrapPanel.Children.Add(imageLabel);
            image = AddImage(@"Pictures\tummenupp.jpg");
            imageWrapPanel.Children.Add(image);


            //KOLUMN 1 PRODUKT
            addNewProduct = new StackPanel { Orientation = Orientation.Vertical };
            productGrid.Children.Add(addNewProduct);
            Grid.SetColumn(addNewProduct, 1);
            Grid.SetRow(addNewProduct, 1);

            TextBlock titleLabel = new TextBlock
            {
                FontSize = 15,
                Margin = new Thickness(0, 20, 67, 0),
                TextAlignment = TextAlignment.Center,
                Text = "Produkttitel: ",
            };
            addNewProduct.Children.Add(titleLabel);

            TextBox newTitle = new TextBox
            {
                Background = Brushes.LightGoldenrodYellow,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 0, 0),
                Width = 150,
            };
            addNewProduct.Children.Add(newTitle);

            TextBlock descriptionLabel = new TextBlock
            {
                FontSize = 15,
                Margin = new Thickness(0, 20, 70, 0),
                TextAlignment = TextAlignment.Center,
                Text = "Beskrivning: ",
            };
            addNewProduct.Children.Add(descriptionLabel);

            TextBox newDescription = new TextBox
            {
                Background = Brushes.LightGoldenrodYellow,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 0, 0),
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Width = 150,
                Height = 150,
            };
            addNewProduct.Children.Add(newDescription);

            TextBlock priceLabel = new TextBlock
            {
                FontSize = 15,
                Margin = new Thickness(0, 25, 95, 0),
                TextAlignment = TextAlignment.Center,
                Text = "Pris (kr): ",
            };
            addNewProduct.Children.Add(priceLabel);

            newPrice = new TextBox
            {
                Background = Brushes.LightGoldenrodYellow,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 50, 2),
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Width = 100,
            };
            addNewProduct.Children.Add(newPrice);

            TextBlock imageTextBlock = new TextBlock
            {
                FontSize = 15,
                Margin = new Thickness(12, 5, 95, 0),
                TextAlignment = TextAlignment.Center,
                Text = "Bildnamn: ",
            };
            addNewProduct.Children.Add(imageTextBlock);

            TextBox imageBox = new TextBox
            {
                Background = Brushes.LightGoldenrodYellow,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 50, 2),
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Width = 100,
            };
            addNewProduct.Children.Add(imageBox);

            Button addNewProductButton = new Button
            {
                Content = "Lägg till",
                Width = 100,
                Margin = new Thickness(0, 2, 50, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            addNewProduct.Children.Add(addNewProductButton);

            //KOLUMN 2 PRODUKT
            showProductListInEdit = new StackPanel { Orientation = Orientation.Vertical };
            productGrid.Children.Add(showProductListInEdit);
            Grid.SetColumn(showProductListInEdit, 2);
            Grid.SetRow(showProductListInEdit, 1);

            TextBlock currentProductList = new TextBlock
            {
                FontSize = 15,
                Margin = new Thickness(0, 20, 0, 0),
                TextAlignment = TextAlignment.Center,
                Text = "Produktlista: ",
            };
            showProductListInEdit.Children.Add(currentProductList);

            productListBox = new ListBox
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 180,
                Height = 325,
            };
            showProductListInEdit.Children.Add(productListBox);

            Button editProductButton = new Button
            {
                Content = "Ändra",
                Width = 180,
                Margin = new Thickness(10, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            showProductListInEdit.Children.Add(editProductButton);

            Button saveProduct = new Button
            {
                Content = "Spara",
                Width = 180,
                Margin = new Thickness(10, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            showProductListInEdit.Children.Add(saveProduct);
            saveProduct.Click += ClickedSaveProductList;

            Button RemoveProductButton = new Button
            {
                Content = "Ta bort",
                Width = 180,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            showProductListInEdit.Children.Add(RemoveProductButton);
            RemoveProductButton.Click += ClickedRemoveProduct;

            Button backFromProductEditing = new Button
            {
                Content = "Tillbaka",
                Width = 180,
                Margin = new Thickness(0, 40, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            showProductListInEdit.Children.Add(backFromProductEditing);
            backFromProductEditing.Click += ClickedBackFromProduct;
        }

        private void AddNewDiscount(object sender, RoutedEventArgs e)
        {
            //Kollar om användaren inte har knappat in en kod eller om längden på koden är mindre än 3 eller större än 20.
            if (addDiscountCode.Text == string.Empty || addDiscountCode.Text.Length < 3 || addDiscountCode.Text.Length > 20)
            {
                MessageBox.Show("Din rabattkod uppfyller inte kriterierna (3-20 tecken).");
                return;
            }
            //Om användarens rabattkod är giltigt inmatad så gör vi om de till stora bokstäver.
            else
            {
                addDiscountCode.Text = addDiscountCode.Text.ToUpper();
            }
            //Kollar om användaren knappar in siffror. Om inte så visa en MessageBox.
            decimal parsedValue;
            if (!decimal.TryParse(addDiscountPercentage.Text, out parsedValue))
            {
                MessageBox.Show("Procentmängden behöver matas in i form av siffror.");
                return;
            }

            //else
            //{
            //    //Tanken är att lägga in Texten som användaren matat in i discountList så att det läggs in på detta vis (ex): "HEJHEJ88,22.00"  
            //}
        }

        private void ClickedSaveDiscount(object sender, RoutedEventArgs e)
        {
            var csv = new StringBuilder();
            foreach (Discount discount in discountList)
            {
                var code = discount.Code;
                var percentage = discount.DiscountPercentage;

                var newLine = string.Format("{0},{1}", code, percentage.ToString().Replace(',', '.'));
                csv.AppendLine(newLine);
            }
            File.WriteAllText(@"C:\Windows\Temp\savedDiscountList.csv", csv.ToString());

            MessageBoxResult info = MessageBox.Show("Tack, ändringar för dina rabattkoder har nu sparats..", "Sparade ändringar", MessageBoxButton.OK, MessageBoxImage.Information);
            switch (info)
            {
                case MessageBoxResult.OK:
                    break;
            }
        }

        private void ClickedSaveProductList(object sender, RoutedEventArgs e)
        {
            var csv = new StringBuilder();
            foreach (Product product in productList)
            {
                var title = product.Title;
                var description = product.Description;
                var price = product.Price;
                var image = product.Image;

                var newLine = string.Format("{0},{1},{2},{3}", title, description, price.ToString().Replace(',', '.'), image);
                csv.AppendLine(newLine);
            }
            File.WriteAllText(@"C:\Windows\Temp\savedEditedProducts.csv", csv.ToString());

            MessageBoxResult info = MessageBox.Show("Tack, ändringar i ditt produktutbud har nu sparats..", "Sparade ändringar", MessageBoxButton.OK, MessageBoxImage.Information);
            switch (info)
            {
                case MessageBoxResult.OK:
                    break;
            }
        }

        private void ClickedRemoveDiscount(object sender, RoutedEventArgs e)
        {
            int selectedIndex = discountListBox.SelectedIndex;
            discountListBox.Items.RemoveAt(selectedIndex);

            discountList.RemoveAt(selectedIndex);
        }

        private void ClickedRemoveProduct(object sender, RoutedEventArgs e)
        {
            int selectedIndex = productListBox.SelectedIndex;
            productListBox.Items.RemoveAt(selectedIndex);

            productList.RemoveAt(selectedIndex);
        }

        private void ClickedBackFromDiscount(object sender, RoutedEventArgs e)
        {
            startGrid.Visibility = Visibility.Visible;
            discountGrid.Visibility = Visibility.Collapsed;
        }

        private void ClickedDiscountEditing(object sender, RoutedEventArgs e)
        {
            startGrid.Visibility = Visibility.Collapsed;
            discountGrid.Visibility = Visibility.Visible;

            ReadDiscountList();
        }

        private void ClickedBackFromProduct(object sender, RoutedEventArgs e)
        {
            startGrid.Visibility = Visibility.Visible;
            productGrid.Visibility = Visibility.Collapsed;
        }

        private void ClickedProductEditing(object sender, RoutedEventArgs e)
        {
            startGrid.Visibility = Visibility.Collapsed;
            productGrid.Visibility = Visibility.Visible;

            ReadProductList();
        }

        private void ClickedExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Calling for ReadProductList when user clicks on ClickedProductEditing method.
        private void ReadProductList()
        {
            productList.Clear();
            productListBox.Items.Clear();
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

        //Calling for ReadDiscountList when user clicks on ClickedDiscountEditing method.
        private void ReadDiscountList()
        {
            discountList.Clear();
            discountListBox.Items.Clear();

            //Läser in produktlistan ur csv från huvudprogrammet (FirstApp).
            string[] discountArray = File.ReadAllLines("rabattKoder.csv");

            //Separerar alla ',' och lägger in de i diverse titel.
            foreach (string line in discountArray)
            {
                string[] columns = line.Split(',');
                string discountCode = columns[0];
                decimal discountPercentage = decimal.Parse(columns[1].Replace('.', ','));

                //För varje rad i csv filen skapar vi ett nytt objekt av klassen.
                Discount discount = new Discount(discountCode, discountPercentage);
                //Lägger till objektet i en lista (titelnamn, beskrivning, bild och pris)
                discountList.Add(discount);
            }
            foreach (Discount d in discountList)
            {
                discountListBox.Items.Add(d.Code + " | " + (d.DiscountPercentage * 100) + "%");
            }
        }

        private Image AddImage(string filePath)
        {
            ImageSource source = new BitmapImage(new Uri(filePath, UriKind.Relative));
            image = new Image
            {
                Source = source,
                Width = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };
            // A small rendering tweak to ensure maximum visual appeal.
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            return image;
        }
    }
}
