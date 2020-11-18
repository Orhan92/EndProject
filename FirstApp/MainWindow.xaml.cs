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
        private ListBox productListBox, cartListBox, orderedProductListBox;
        private TextBox discountBox;
        private TextBlock cart, productDescritpion, productListLabel, productDescription, orderSum, chartSumLabel;
        private Button addDiscount, order, emptyCart, saveCart, removeItem, addItem, productInfo, backFromOrder;
        private List<Product> productList, cartList;
        private List<Discount> discountList;
        private StackPanel descriptionBox, addToCart, discountSum, discount, cartButtons, secondCartButtons, orderPanel;
        private decimal totalSum;
        private string[] productArray, discountArray;
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

            //LISTOR*****************************************************************

            //lägger in samtliga objekt ur csv 
            productList = new List<Product>();

            //lägger in samtliga rabattkoder här.
            discountList = new List<Discount>(); 

            //Lägger in tillagda artiklar från productList hit.
            cartList = new List<Product>();  


            //KOLUMN 0****************************************************************

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

            //Denna StackPanel är skapad för info och lägg till knapparna.
            addToCart = new StackPanel { Orientation = Orientation.Horizontal }; 
            grid.Children.Add(addToCart);
            Grid.SetRow(addToCart, 2);
            Grid.SetColumn(addToCart, 0);

            //Denna knapp ska lägga till den markerade artikeln i varukorgen
            addItem = new Button 
            {
                Content = "Lägg till",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            addToCart.Children.Add(addItem);
            addItem.Click += ClickedAddToCart;

            //När användaren klickar här så ska produktbeskrivning visas i kolumn 1 
            productInfo = new Button
            {
                Content = "Visa info",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            addToCart.Children.Add(productInfo);
            productInfo.Click += ClickedInfo;



            //KOLUMN 1 ************************************************************************
            //Detta är titeln som visas högst upp i kolumnen "Produktbeskrivning"
            productDescritpion = new TextBlock 
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

            //Stackpanel för beskrivning och bild.
            descriptionBox = new StackPanel { Orientation = Orientation.Vertical };
            grid.Children.Add(descriptionBox);
            Grid.SetColumn(descriptionBox, 1);
            Grid.SetRow(descriptionBox, 1);

            //Denna är hidden så att den inte syns vid programmets start.
            image = CreateImage(@"Images\ost.jpg"); 

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

            //KOLUMN 1 VID BESTÄLLNING *************************************************

            //orderPanel förblir 'Collapsed' för att hållas osynlig tills användaren klickar "Beställ"
            orderPanel = new StackPanel { Orientation = Orientation.Vertical };
            grid.Children.Add(orderPanel);
            Grid.SetColumn(orderPanel, 1);
            Grid.SetRow(orderPanel, 2);
            orderPanel.Visibility = Visibility.Collapsed;

            //Beställda varor i varukorgen visas i denna ListBox
            orderedProductListBox = new ListBox
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 200,
            };
            orderPanel.Children.Add(orderedProductListBox);

            //Vi visar användaren totalsumman på ordervärdet.
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

            //Här är knappen för att lägga till rabattkoden till varukorgen
            addDiscount = new Button 
            {
                Content = "Lägg till",
                Width = 50,
                Margin = new Thickness(50, 10, 2, 0)
            };
            discount.Children.Add(addDiscount);
            addDiscount.Click += AddDiscount;

            //Inuti denna boxen matar användaren in rabattkoden
            discountBox = new TextBox 
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

            //Här är knappen för att lämna orderfönstret och gå tillbaka till beställ-fönstret.
            backFromOrder = new Button 
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



            //KOLUMN 2,************************************************************************
            //Detta är titeln som står högst upp "Varukorgen"
            cart = new TextBlock 
            {
                FontSize = 20,
                Margin = new Thickness(2),
                TextAlignment = TextAlignment.Center,
                Width = 200,
                Text = "Varukorgen"
            };
            grid.Children.Add(cart);
            Grid.SetColumn(cart, 2);
            Grid.SetRow(cart, 0);

            //Listbox som ska visa upp de tillagda artiklarna i varukorgen för användaren.
            cartListBox = new ListBox
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200,
                Height = 350
            };
            grid.Children.Add(cartListBox);
            Grid.SetColumn(cartListBox, 2);
            Grid.SetRow(cartListBox, 1);

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

            //För att visa totalpriset i varukorgen.
            chartSumLabel = new TextBlock
            {
                Foreground = Brushes.ForestGreen,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            discountSum.Children.Add(chartSumLabel);

            //stackPanel för knappar under varukorgen.
            cartButtons = new StackPanel { Orientation = Orientation.Horizontal };
            grid.Children.Add(cartButtons);
            Grid.SetRow(cartButtons, 4);
            Grid.SetColumn(cartButtons, 2);

            //Ta bort enskild vald produkt från varukorgen.
            removeItem = new Button 
            {
                Content = "Ta bort",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            cartButtons.Children.Add(removeItem);
            removeItem.Click += ClickedRemove;

            //Spara varukorgen till en csv fil i temp mappen.
            saveCart = new Button 
            {
                Content = "Spara",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            cartButtons.Children.Add(saveCart);
            saveCart.Click += ClickedSaveCart;

            //Stackpanel för knappar under varukorgen
            secondCartButtons = new StackPanel { Orientation = Orientation.Horizontal };
            grid.Children.Add(secondCartButtons);
            Grid.SetRow(secondCartButtons, 5);
            Grid.SetColumn(secondCartButtons, 2);

            //Tömmer hela varukorgen, och nollställer programmet.
            emptyCart = new Button 
            {
                Content = "Töm",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            secondCartButtons.Children.Add(emptyCart);
            emptyCart.Click += ClickedEmptyAll;

            //För att beställa tillagda artiklar
            order = new Button 
            {
                Content = "Beställ",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            secondCartButtons.Children.Add(order);
            order.Click += ClickedOrder;

            //läser in produktlistan. Se metod för funktion.
            ReadProductList();
            //Läser in sparad varukorg när programmet startas.
            ReadSavedCart();
        }

        //Skapar en metod för inläsning av sparad varukorg (csv-fil).
        private void ReadSavedCart()
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
                cartListBox.Items.Add(savedProduct.Title + " | " + savedProduct.Price.ToString("C"));

                //Uppdaterar totalSum: 'Summa:' i raden under varukorgen.
                totalSum += savedProduct.Price;
                chartSumLabel.Text = totalSum.ToString("C");
            }
        }

        //METODER som kollar om vi har en sparad CSV fil, om inte så hämtar vi "original"-filen.
        private void ReadProductList()
        {
            //Läser in produktlistan ur csv OM den finns i Temp..
            try
            {
                productArray = File.ReadAllLines(@"C:\Windows\Temp\savedEditedProducts.csv");
                ReadProductListFromCSV(); //Skapat metod för att läsa in csv fil.
            }

            //Om den INTE finns i Temp så läser vi in från "programmets" csv-fil.
            catch
            {
                productArray = File.ReadAllLines("produktLista.csv");
                ReadProductListFromCSV();
            }
        }
        private void ReadDiscount()
        {
            //Läser in rabattlistan ur csv OM den finns i Temp..
            try
            {
                discountArray = File.ReadAllLines(@"C:\Windows\Temp\savedDiscountList.csv");
                ReadDiscountListFromCSV();
            }

            //Om den INTE finns i Temp så läser vi in från "programmets" csv-fil.
            catch
            {
                discountArray = File.ReadAllLines("rabattKoder.csv");
                ReadDiscountListFromCSV();
            }
        }

        //METODER FÖR ATT LÄSA IN CSV-FILER
        private void ReadProductListFromCSV()
        {
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
        private void ReadDiscountListFromCSV()
        {
            foreach (string code in discountArray)
            {
                string[] columns = code.Split(',');
                string discountCode = columns[0];
                decimal discountPercentage = decimal.Parse(columns[1].Replace('.', ','));

                Discount discount = new Discount(discountCode, discountPercentage);
                discountList.Add(discount);
            }
        }

        //METODER FÖR KNAPPTRYCK.
        private void AddDiscount(object sender, RoutedEventArgs e)
        {
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

                    //Deklarerar/initierar ordinaryPrice för att kunna skriva ut tidigare ord.pris för användaren.
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

        //TILLBAKA FRÅN ORDER / ORDER
        private void BackFromOrderClick(object sender, RoutedEventArgs e)
        {
            //Kolumn 0: Gör hela Visible,
            productListLabel.Visibility = Visibility.Visible;
            productListBox.Visibility = Visibility.Visible;
            addToCart.Visibility = Visibility.Visible;

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
            cart.Visibility = Visibility.Visible;
            cartListBox.Visibility = Visibility.Visible;
            discountSum.Visibility = Visibility.Visible;
            cartButtons.Visibility = Visibility.Visible;
            secondCartButtons.Visibility = Visibility.Visible;

            //Återställer namnen på rubrikerna till ursprungstitel.
            productListLabel.Text = "Produktlista";
            productDescritpion.Text = "Produktbeskrivning";
            cart.Text = "Varukorgen";

            //Nollställer rabattfälten till sitt urpsrungstillstånd.
            discountBox.Foreground = Brushes.Gray;
            discountBox.Background = Brushes.LightGoldenrodYellow;
            discountBox.Text = "Rabattkod";

            //Tömmer/Rensar väsentliga listor och ListBox (som visas för användaren).
            cartListBox.Items.Clear();
            orderedProductListBox.Items.Clear();
            cartList.Clear();

            //Nollställer summan på totalsum.
            totalSum = 0;
            chartSumLabel.Text = totalSum.ToString("C");
        }
        private void ClickedOrder(object sender, RoutedEventArgs e)
        {
            //Kolumn 0: Vi vill Collapsa/Dölja denna när användaren trycker på "Beställ"
            productListBox.Visibility = Visibility.Collapsed;
            addToCart.Visibility = Visibility.Collapsed;

            //Kolumn 1: Vi gömmer det som visas i denna kolumnen vid programmets start och uppdaterar med ny information.
            descriptionBox.Visibility = Visibility.Collapsed;
            image.Visibility = Visibility.Collapsed;

            //Kolumn 1: Uppdaterad information när användaren klickat på "Beställ"
            orderPanel.Visibility = Visibility.Visible;
            discount.Visibility = Visibility.Visible;
            saveCart.Visibility = Visibility.Visible; 
            discountBox.Visibility = Visibility.Visible; 
            addDiscount.Visibility = Visibility.Visible; 
            backFromOrder.Visibility = Visibility.Visible;

            //Kolumn 2: Collapsar/Döljer även denna vid beställning.
            cartListBox.Visibility = Visibility.Collapsed;
            discountSum.Visibility = Visibility.Collapsed;
            cartButtons.Visibility = Visibility.Collapsed;
            secondCartButtons.Visibility = Visibility.Collapsed;

            //Byter ut texten i samtliga kolumner för rad 0.
            productListLabel.Text = string.Empty;
            productDescritpion.Text = "Tack för din beställning";
            cart.Text = string.Empty;

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

        //TÖM / TA BORT
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
                    cartListBox.Items.Clear();
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
                //Tar bort den markerade raden i varukorgen. SelectedIndex; för att veta vilken produkt.
                int foodIndex = cartListBox.SelectedIndex;
                cartListBox.Items.RemoveAt(foodIndex);

                //Tar bort objektet i varukorgen och priset för objektet i totalSum.
                Product product = cartList[foodIndex];
                totalSum -= product.Price;
                chartSumLabel.Text = totalSum.ToString("C");
                //"C" Sätter currency baserat på valutan i ditt land. I mitt fall "kr"

                //Tar bort objektet från varukorgsListan.
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

        //SPARA / LÄGG TILL / INFO
        private void ClickedSaveCart(object sender, RoutedEventArgs e)
        {
            //Sparar varukorgen i Temp mappen när användaren trycker på "Spara"
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
        private void ClickedAddToCart(object sender, RoutedEventArgs e)
        {
            try
            {
                //Nollställer bild och beskrivning så att användaren får klicka på "Visa info" igen.
                image.Visibility = Visibility.Hidden;
                productDescription.Visibility = Visibility.Hidden;

                //Rensar ListBoxen som visar upp varukorgen för användaren.
                //Detta för att undvika dubletter av listan som visas för användaren.
                cartListBox.Items.Clear();
                int selectedIndex = productListBox.SelectedIndex;

                //Lägger till den markerade produkten från productList in till cartList.
                Product product = productList[selectedIndex];
                cartList.Add(product);

                foreach (Product p in cartList)
                {
                    cartListBox.Items.Add(p.Title + " | " + p.Price.ToString("C"));
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
                            cartListBox.Items.Add(p.Title + " | " + p.Price.ToString("C"));
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

                //Visar bilden.
                image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                image.Visibility = Visibility.Visible;

                //Visar produktbeskrivning.
                productDescription.Text = productList[selectedIndex].Description;
                productDescription.Visibility = Visibility.Visible;
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

        //LÄSER IN BILD
        private Image CreateImage(string filePath)
        {
            ImageSource source = new BitmapImage(new Uri(filePath, UriKind.Relative));
            image = new Image
            {
                //Håller den hidden till dess att användaren klickar på "Visa Info".
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

        //STATIC METODER FÖR TESTING
        public static bool SaveToCsvCart(string csv)
        {
            if (!string.IsNullOrEmpty(csv))
            {
                File.WriteAllText(@"C:\Windows\Temp\savedChart.csv", csv);
                return true;
            }
            return false;
        }
        public static List<Product> ReadProductListWithoutGUI()
        {
            //Skapade en lista för att skicka till min unit test
            var unitTestList = new List<Product>();

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
                var products = new Product(productName, productDescription, productPrice, productImage);

                unitTestList.Add(products);
            }
            return unitTestList;
        }
        public static List<Discount> ReadDiscountTest()
        {
            var unitTestDiscountList = new List<Discount>();
            string[] discountArray = File.ReadAllLines("rabattKoder.csv");

            foreach (string code in discountArray)
            {
                string[] columns = code.Split(',');
                string discountCode = columns[0];
                decimal discountPercentage = decimal.Parse(columns[1].Replace('.', ','));

                Discount discount = new Discount(discountCode, discountPercentage);
                unitTestDiscountList.Add(discount);
            }

            return unitTestDiscountList;
        }
        public static List<Product> ReadSavedCartTest()
        {
            //kod som läser om filen
            //lägger till listan
            // och gör dem til product objekt

            //Denna för att skapa filen "savedChart.csv" i temp mappen. Skapar en tom csv fil med namnet savedChart.csv. 
            File.AppendAllText(@"C:\Windows\Temp\savedChart.csv", string.Empty);
            //Läser in den sparade varukorgen.
            string[] savedChart = File.ReadAllLines(@"C:\Windows\Temp\savedChart.csv");

            var testList = new List<Product>();

            var savedProduct = new Product("", "", 0, "");
            foreach (string line in savedChart)
            {
                string[] columns = line.Split(',');
                string productName = columns[0];
                string productDescription = columns[1];
                decimal productPrice = decimal.Parse(columns[2].Replace('.', ','));
                string productPicture = columns[3];

                savedProduct = new Product(productName, productDescription, productPrice, productPicture);

                //var mainWindow = new MainWindow();

                //mainWindow.GUIExtra(savedProduct);

                testList.Add(savedProduct);
            }
            return testList;
        }

    }
}
