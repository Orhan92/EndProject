using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace FirstApp
{
    public class Product
    {
        public string Title;
        public string Description;
        public decimal Price;
        public string Image;
        public Product(string title, string description, decimal price, string pictures)
        {
            Title = title;
            Description = description;
            Price = price;
            Image = pictures;
        }
    }
    public class Discount
    {
        public string Code;
        public decimal DiscountPercentage;
        public Discount(string code, decimal discountPercentage)
        {
            Code = code;
            DiscountPercentage = discountPercentage;
        }
    }
    public partial class MainWindow : Window
    {
        private Image image;
        private Grid grid;
        private ListBox productListBox, chartListBox, orderedProductListBox;
        private TextBox discountBox;
        private TextBlock chart, productDescritpion, productListLabel, productDescription, orderSum, chartSumLabel;
        private Button addDiscount, order, emptyChart, saveChart, removeItem, addItem, productInfo, backFromOrder;
        private List<Product> productList, cartList;
        private List<Discount> discountList;
        private StackPanel descriptionBox, addToChart, discountSum, discount, chartButtons, secondChartButtons, orderPanel;
        private decimal totalSum;
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        private void Start()
        {
            // Window options
            Title = "Butik";
            Width = 730;
            Height = 540;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Scrolling
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Content = root;

            // Main grid
            grid = new Grid();
            root.Content = grid;
            grid.Margin = new Thickness(5);

            //I main griden har vi skapat 6 rader.
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            //I main griden har vi skapat 3 kolumner
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            //LISTOR***********************************************************************************
            productList = new List<Product>(); //lägger in samtliga objekt ur csv filen hit.
            discountList = new List<Discount>(); //lägger in samtliga rabattkoder här.
            cartList = new List<Product>();  //Dokumentera om denna delen, att det var svårt att förstå att jag behövde lägga in saker från listProducts in i en ny lista (den här listan) för att kunna manipulera den i chartListBox.



            //KOLUMN 0**************************************************************************************
            productListLabel = new TextBlock //Detta är titeln som står högst upp "Produktlista"
            {
                FontSize = 20,
                Margin = new Thickness(2),
                TextAlignment = TextAlignment.Center,
                Width = 200,
                Text = "Produktlista"
            };
            grid.Children.Add(productListLabel);
            Grid.SetColumn(productListLabel, 0);
            Grid.SetRow(productListLabel, 0);

            productListBox = new ListBox //I den här ListBoxen ska vi visa användaren en lista på valbara produkter från en csv fil.
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200,
                Height = 350
            };
            grid.Children.Add(productListBox);
            Grid.SetColumn(productListBox, 0);
            Grid.SetRow(productListBox, 1);
            //lägger in titeln och priset för varje objekt av Product klassen utifrån csv fil in i en produktlistan som visas i vänster kolumn för användaren.

            addToChart = new StackPanel { Orientation = Orientation.Horizontal }; //Denna StackPanel är skapad för info och lägg till knapparna.
            grid.Children.Add(addToChart);
            Grid.SetRow(addToChart, 2);
            Grid.SetColumn(addToChart, 0);

            addItem = new Button //Denna knapp ska lägga till den markerade artikeln i varukorgen
            {
                Content = "Lägg till",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            addToChart.Children.Add(addItem);
            addItem.Click += ClickedAddToChart;

            //När användaren klickar här så ska produktbeskrivning visas i kolumn 1 
            productInfo = new Button
            {
                Content = "Visa info",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            addToChart.Children.Add(productInfo);
            productInfo.Click += ClickedInfo;

            //läser in produktlistan.
            ReadProductList();



            //KOLUMN 1 ************************************************************************
            productDescritpion = new TextBlock //Detta är titeln som står högst upp "Produktbeskrivning"
            {
                FontSize = 20,
                Margin = new Thickness(2),
                TextAlignment = TextAlignment.Center,
                Width = 210,
                Text = "Produktbeskrivning"
            };
            grid.Children.Add(productDescritpion);
            Grid.SetColumn(productDescritpion, 1);
            Grid.SetRow(productDescritpion, 0);

            descriptionBox = new StackPanel { Orientation = Orientation.Vertical };
            grid.Children.Add(descriptionBox);
            Grid.SetColumn(descriptionBox, 1);
            Grid.SetRow(descriptionBox, 1);

            image = CreateImage(@"Images\ost.jpg"); //Denna är hidden så att den inte syns vid programmets start.

            productDescription = new TextBlock
            {
                FontSize = 15,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 200,
                TextWrapping = TextWrapping.Wrap,
                Text = string.Empty
            };
            descriptionBox.Children.Add(productDescription);

            //KOLUMN 1, VID BESTÄLLNING ****************************************************************
            //Allt inuti orderPanel förblir 'Collapsed' för att hållas osynlig tills en beställning görs av användaren.
            orderPanel = new StackPanel { Orientation = Orientation.Vertical };
            grid.Children.Add(orderPanel);
            Grid.SetColumn(orderPanel, 1);
            Grid.SetRow(orderPanel, 2);
            orderPanel.Visibility = Visibility.Collapsed;

            //Beställda varukorgen
            orderedProductListBox = new ListBox
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 200,
            };
            orderPanel.Children.Add(orderedProductListBox);

            orderSum = new TextBlock //Summan vid order
            {
                Text = "Summa: ",
                Margin = new Thickness(50, 5, 0, 15),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200,
                Height = 30
            };
            orderPanel.Children.Add(orderSum);

            //Skapar en stackpanel som innehåller allt som har med rabatten att göra.
            discount = new StackPanel { Orientation = Orientation.Horizontal };
            grid.Children.Add(discount);
            Grid.SetRow(discount, 4);
            Grid.SetColumn(discount, 1);
            discount.Visibility = Visibility.Collapsed;

            addDiscount = new Button //Här är knappen för att lägga till rabattkoden till varukorgen
            {
                Content = "Lägg till",
                Width = 50,
                Margin = new Thickness(50, 10, 2, 0)
            };
            discount.Children.Add(addDiscount);
            addDiscount.Click += AddDiscount;

            discountBox = new TextBox //Inuti denna boxen matar användaren in rabattkoden
            {
                Foreground = Brushes.Gray,
                Background = Brushes.LightGoldenrodYellow,
                Text = "Rabattkod",
                Width = 148,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };
            discount.Children.Add(discountBox);
            discountBox.GotFocus += DiscountBoxHasBeenClicked;

            backFromOrder = new Button //Här är knappen för att lägga till rabattkoden till varukorgen
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = "Tillbaka",
                Width = 100,
                Margin = new Thickness(50, 2, 0, 0),
                Visibility = Visibility.Collapsed
            };
            grid.Children.Add(backFromOrder);
            Grid.SetRow(backFromOrder, 5);
            Grid.SetColumn(backFromOrder, 1);
            backFromOrder.Click += BackFromOrderClick;



            //KOLUMN 2,****************************************************************************************
            chart = new TextBlock //Detta är titeln som står högst upp "Varukorgen"
            {
                FontSize = 20,
                Margin = new Thickness(2),
                TextAlignment = TextAlignment.Center,
                Width = 200,
                Text = "Varukorgen"
            };
            grid.Children.Add(chart);
            Grid.SetColumn(chart, 2);
            Grid.SetRow(chart, 0);

            //Listbox som ska visa upp de tillagda artiklarna i varukorgen för användaren.
            chartListBox = new ListBox
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200,
                Height = 350
            };
            grid.Children.Add(chartListBox);
            Grid.SetColumn(chartListBox, 2);
            Grid.SetRow(chartListBox, 1);

            //stackpanel under varukorgen för att visa totalsumman
            discountSum = new StackPanel { Orientation = Orientation.Horizontal };
            grid.Children.Add(discountSum);
            Grid.SetRow(discountSum, 2);
            Grid.SetColumn(discountSum, 2);

            TextBlock totalSumLabelInChart = new TextBlock
            {
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Summa: ",
                Width = 50
            };
            discountSum.Children.Add(totalSumLabelInChart);

            chartSumLabel = new TextBlock
            {
                Foreground = Brushes.ForestGreen,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            discountSum.Children.Add(chartSumLabel);

            //stackPanel under stackPanelen ovan (Rad: 295).
            chartButtons = new StackPanel { Orientation = Orientation.Horizontal };
            grid.Children.Add(chartButtons);
            Grid.SetRow(chartButtons, 4);
            Grid.SetColumn(chartButtons, 2);

            removeItem = new Button //Ta bort enskild vald produkt från varukorgen.
            {
                Content = "Ta bort",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            chartButtons.Children.Add(removeItem);
            removeItem.Click += ClickedRemove;

            saveChart = new Button //Spara varukorgen till en csv fil i temp mappen.
            {
                Content = "Spara",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            chartButtons.Children.Add(saveChart);
            saveChart.Click += ClickedSaveChart;

            //En till stackpanel under stackPanel ovan.
            secondChartButtons = new StackPanel { Orientation = Orientation.Horizontal };
            grid.Children.Add(secondChartButtons);
            Grid.SetRow(secondChartButtons, 5);
            Grid.SetColumn(secondChartButtons, 2);

            emptyChart = new Button //Tömmer hela varukorgen, och nollställer programmet.
            {
                Content = "Töm",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            secondChartButtons.Children.Add(emptyChart);
            emptyChart.Click += ClickedEmptyAll;

            order = new Button //Lägg en beställning med valda produkter och vald rabatt.
            {
                Content = "Beställ",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            secondChartButtons.Children.Add(order);
            order.Click += ClickedOrder;

            //Läser in sparad varukorg.
            ReadSavedChart();
        }

        //Skapar dessa "Read"-metoder för att enkelt kunna testa dem.
        //Skapar en metod för inläsning av sparad varukorg (csv-fil).
        private void ReadSavedChart()
        {
            //Denna för att skapa filen "savedChart.csv" i temp mappen. Skapar en tom csv fil med namnet savedChart.csv. 
            File.AppendAllText(@"C:\Windows\Temp\savedChart.csv", string.Empty);

            //Läser in den sparade varukorgen.
            string[] savedChart = File.ReadAllLines(@"C:\Windows\Temp\savedChart.csv");

            foreach (string line in savedChart)
            {
                string[] columns = line.Split(',');
                string productName = columns[0];
                string productDescription = columns[1];
                decimal productPrice = decimal.Parse(columns[2].Replace('.', ','));
                string productPicture = columns[3];

                Product savedProduct = new Product(productName, productDescription, productPrice, productPicture);

                cartList.Add(savedProduct);

                //Lägger in de sparade produkterna i varukorgen
                chartListBox.Items.Add(savedProduct.Title + " | " + savedProduct.Price.ToString("C"));

                //Uppdaterar totalSum: 'Summa:' i raden under varukorgen.
                totalSum += savedProduct.Price;
                chartSumLabel.Text = totalSum.ToString("C");
            }
        }
        //Skapar metod för inläsning av produktlistan (csv-fil).
        private void ReadProductList()
        {
            //Läser in produktlistan ur csv.
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
        //Skapar metod för inläsning av rabakkoder (csv-fil).
        private void ReadDiscount()
        {
            string[] discountArray = File.ReadAllLines("rabattKoder.csv");
            foreach (string code in discountArray)
            {
                string[] columns = code.Split(',');
                string discountCode = columns[0];
                decimal discountPercentage = decimal.Parse(columns[1].Replace('.', ','));

                Discount discount = new Discount(discountCode, discountPercentage);
                discountList.Add(discount);
            }
        }

        private void AddDiscount(object sender, RoutedEventArgs e)
        {
            //Dokumentera om denna delen.. Den var sjukt jobbig att få till.
            //Rensar rabattlistan och läser in den på nytt här nedan.
            discountList.Clear();

            //Gör så att det inte spelar någon roll om användaren knappar in stora eller små bokstäver.
            discountBox.Text = discountBox.Text.ToUpper();

            //Läser in rabattkodslistan (csv-fil).
            ReadDiscount();

            foreach (Discount y in discountList)
            {
                if (discountBox.Text == y.Code)
                {
                    discountBox.Visibility = Visibility.Hidden;
                    addDiscount.Visibility = Visibility.Hidden;

                    orderedProductListBox.Items.Clear();

                    //deklarerar och initierar ordinaryPrice endast för att kunna skriva ut tidigare ord.pris för användaren.
                    decimal ordinaryPrice = 0;
                    foreach (Product x in cartList)
                    {
                        orderedProductListBox.Items.Add(x.Title + " | " + x.Price.ToString("C"));
                        totalSum -= x.Price * y.DiscountPercentage;

                        ordinaryPrice += x.Price;
                        orderSum.Text = "Summa: " + ordinaryPrice.ToString("C") + " | Ordinarie pris" + Environment.NewLine + "Summa: " + totalSum.ToString("C") + " | " + "Rabatt: " + (y.DiscountPercentage * 100) + "%";
                    }
                }
                else
                {
                    discountBox.Background = Brushes.OrangeRed;
                }
            }
        }
        private void DiscountBoxHasBeenClicked(object sender, RoutedEventArgs e)
        {
            //Denna metod gör så att TextBoxen där det står "Rabattkod" blir tom när man klickar på den.
            bool hasBeenClicked = false;
            if (!hasBeenClicked)
            {
                discountBox = sender as TextBox;
                discountBox.Foreground = Brushes.Black;
                discountBox.Background = Brushes.White;
                discountBox.Text = String.Empty;
                hasBeenClicked = true;
            }
        }
        private void BackFromOrderClick(object sender, RoutedEventArgs e)
        {
            //Kolumn 0: Gör hela Visible,
            productListLabel.Visibility = Visibility.Visible;
            productListBox.Visibility = Visibility.Visible;
            addToChart.Visibility = Visibility.Visible;

            //Kolumn 1: Visar "ursprungliga" kolumnen (den som visas vid programmets start)
            descriptionBox.Visibility = Visibility.Visible;
            productDescritpion.Visibility = Visibility.Visible;
            image.Visibility = Visibility.Hidden; //Håller den Hidden till dess att användaren klickar på "Visa Info"
            productDescription.Visibility = Visibility.Hidden; //Håller den Hidden till dess att användaren klickar på "Visa Info"

            //Kolumn 1: Collapsar/Döljer det som visas när användaren klickar på "Beställ"
            orderPanel.Visibility = Visibility.Collapsed;
            discount.Visibility = Visibility.Collapsed;
            backFromOrder.Visibility = Visibility.Collapsed;

            //Kolumn 2: Gör hela kolumnen Visible igen.
            chart.Visibility = Visibility.Visible;
            chartListBox.Visibility = Visibility.Visible;
            discountSum.Visibility = Visibility.Visible;
            chartButtons.Visibility = Visibility.Visible;
            secondChartButtons.Visibility = Visibility.Visible;

            //Återställer namnen på rubrikerna till ursprungstitel.
            productListLabel.Text = "Produktlista";
            productDescritpion.Text = "Produktbeskrivning";
            chart.Text = "Varukorgen";

            //Nollställer rabattfälten till sitt urpsrungstillstånd.
            discountBox.Foreground = Brushes.Gray;
            discountBox.Background = Brushes.LightGoldenrodYellow;
            discountBox.Text = "Rabattkod";

            //Tömmer/Rensar väsentliga listor och ListBox (som visas för användaren).
            chartListBox.Items.Clear();
            orderedProductListBox.Items.Clear();
            cartList.Clear();

            //Nollställer summan på totalsum.
            totalSum = 0;
            chartSumLabel.Text = totalSum.ToString("C");
        }
        private void ClickedOrder(object sender, RoutedEventArgs e)
        {
            //Kolumn 0: Vi vill Collapsa/Dölja denna när användaren trycker på "Beställ"
            //Notera att titeln/rubriken inte är Collapsed då vi gör strängen empty. (Rad: 525)
            productListBox.Visibility = Visibility.Collapsed;
            addToChart.Visibility = Visibility.Collapsed;

            //Kolumn 1: Vi gömmer det som visas i denna kolumnen vid programmets start och uppdaterar med ny information.
            //Notera att titeln/rubriken inte är Collapsed då vi ändrar på strängens innehåll (Rad: 526)
            descriptionBox.Visibility = Visibility.Collapsed;
            image.Visibility = Visibility.Collapsed;

            //Kolumn 1: Uppdaterad information när användaren klickat på "Beställ"
            orderPanel.Visibility = Visibility.Visible;
            discount.Visibility = Visibility.Visible;
            saveChart.Visibility = Visibility.Visible; //Denna rad visas inte trots att 'discount' (rad 507) är Visible därav denna kod. 
            discountBox.Visibility = Visibility.Visible; //Denna rad visas inte trots att 'discount' (rad 507) är Visible därav denna kod.
            addDiscount.Visibility = Visibility.Visible; //Denna rad visas inte trots att 'discount' (rad 507) är Visible därav denna kod.
            backFromOrder.Visibility = Visibility.Visible;

            //Kolumn 2: Collapsar/Döljer även denna vid beställning.
            //Notera att titeln/rubriken inte är collapsed då vi gör strängen empty (Rad: 527)
            chartListBox.Visibility = Visibility.Collapsed;
            discountSum.Visibility = Visibility.Collapsed;
            chartButtons.Visibility = Visibility.Collapsed;
            secondChartButtons.Visibility = Visibility.Collapsed;

            //Byter ut texten i samtliga kolumner för rad 0.
            productListLabel.Text = string.Empty;
            productDescritpion.Text = "Tack för din beställning";
            chart.Text = string.Empty;

            //Sätter en specifik höjd på beställningslistan.
            orderedProductListBox.Height = 350;

            //Skriver ut totalsumman för beställningen.
            orderSum.Text = "Summa: " + totalSum.ToString("C");

            //Lägger till alla beställda produkter ur cartList in i orderedProducts ListBox.
            foreach (Product p in cartList)
            {
                orderedProductListBox.Items.Add(p.Title + " | " + p.Price.ToString("C"));
            }
        }
        private void ClickedEmptyAll(object sender, RoutedEventArgs e)
        {
            MessageBoxResult warning = MessageBox.Show("Är du säker att du vill tömma varukorgen?", "Varning!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            switch (warning)
            {
                case MessageBoxResult.Yes:

                    //"nollställer/Gömmer" bild i kolumn 1 och beskrivning.
                    image.Visibility = Visibility.Hidden;
                    productDescription.Visibility = Visibility.Hidden;

                    //Rensar varukorgen.
                    chartListBox.Items.Clear();
                    cartList.Clear();

                    //Nollställer totalsumman.
                    totalSum = 0;
                    chartSumLabel.Text = totalSum.ToString("C");
                    break;

                case MessageBoxResult.No:
                    break;
            }
        }
        private void ClickedRemove(object sender, RoutedEventArgs e)
        {
            try
            {
                int foodIndex = chartListBox.SelectedIndex;
                chartListBox.Items.RemoveAt(foodIndex);

                //Dokumentera om denna delen, att den var sjukt svår.
                Product product = cartList[foodIndex];
                totalSum -= product.Price;
                chartSumLabel.Text = totalSum.ToString("C");
                //"C" Sätter currency baserat på valutan i ditt land. I mitt fall "kr"

                cartList.RemoveAt(foodIndex);
            }
            catch
            {
                MessageBoxResult info = MessageBox.Show("Vänligen markera produkt som du vill ta bort.", "Hoppsan!", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (info)
                {
                    case MessageBoxResult.OK:
                        break;
                }
            }
        }
        private void ClickedSaveChart(object sender, RoutedEventArgs e)
        {
            //Sparar varukorgen när användaren trycker på "Spara"
            var csv = new StringBuilder();
            foreach (Product product in cartList)
            {
                var title = product.Title;
                var description = product.Description;
                var price = product.Price;
                var image = product.Image;

                var newLine = string.Format("{0},{1},{2},{3}", title, description, price.ToString().Replace(',', '.'), image);
                csv.AppendLine(newLine);
            }
            File.WriteAllText(@"C:\Windows\Temp\savedChart.csv", csv.ToString());

            MessageBoxResult info = MessageBox.Show("Tack, din varukorg har nu sparats.", "Sparad varukorg", MessageBoxButton.OK, MessageBoxImage.Information);
            switch (info)
            {
                case MessageBoxResult.OK:
                    break;
            }
        }
        private void ClickedAddToChart(object sender, RoutedEventArgs e)
        {
            try
            {
                //Nollställer bild och beskrivning så att användaren får klicka på "Visa info" igen.
                image.Visibility = Visibility.Hidden;
                productDescription.Visibility = Visibility.Hidden;

                //Dokumentera om denna delen, att den var sjukt svår.
                chartListBox.Items.Clear();
                int selectedIndex = productListBox.SelectedIndex;

                Product product = productList[selectedIndex];
                cartList.Add(product);

                foreach (Product p in cartList)
                {
                    chartListBox.Items.Add(p.Title + " | " + p.Price.ToString("C"));
                }
                //För att visa priset av totalsumman i varukorgen!
                totalSum += product.Price;
                chartSumLabel.Text = totalSum.ToString("C");
            }
            catch
            {
                MessageBoxResult info = MessageBox.Show("Vänligen markera den produkt som du vill lägga till i varukorgen.", "Hoppsan!", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (info)
                {
                    case MessageBoxResult.OK:

                        //Loopen fungerar som en slags "felhantring". Ifall användaren inte markerat en produkt som ska läggas till så kommer det upp en informationsruta och sedan visas de redan tillagda produkterna igen. Utan loopen blir varukorgen tom.
                        foreach (Product p in cartList)
                        {
                            chartListBox.Items.Add(p.Title + " | " + p.Price.ToString("C"));
                        }
                        break;
                }
            }
        }
        private void ClickedInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                //läser in namnet/strängen som är kopplat till bilden. Ex: "ost.jpg".
                int selectedIndex = productListBox.SelectedIndex;
                string path = @"Images\" + productList[selectedIndex].Image;

                productDescription.Text = productList[selectedIndex].Description;
                productDescription.Visibility = Visibility.Visible;

                image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                image.Visibility = Visibility.Visible;

            }
            catch
            {
                MessageBoxResult info = MessageBox.Show("Vänligen markera produkt för att visa info.", "Hoppsan!", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (info)
                {
                    case MessageBoxResult.OK:
                        break;
                }
            }
        }
        private Image CreateImage(string filePath)
        {
            ImageSource source = new BitmapImage(new Uri(filePath, UriKind.Relative));
            image = new Image
            {
                Visibility = Visibility.Hidden,
                Source = source,
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 25, 0, 20)
            };
            descriptionBox.Children.Add(image);
            // A small rendering tweak to ensure maximum visual appeal.
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            return image;
        }
    }
}
