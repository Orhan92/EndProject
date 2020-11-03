using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
using System.Reflection;
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


namespace FirstApp
{
    public class Product
    {
        public string Title;
        public string Descprition;
        public decimal Price;
        public string Image;
        public Product(string title, string description, decimal price, string pictures)
        {
            Title = title;
            Descprition = description;
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
        private ListBox productListBox, chartListBox, orderedProducts;
        private TextBox discountBox;
        private TextBlock chart, productDescritpion, productList, descriptionBox, discountEnabled, orderSum, totalSumInChart;
        private Button addDiscount, order, empty, save, remove, addItem, info, backFromOrder;
        private List<Product> listProducts, cartList;
        private List<Discount> discountList;
        private StackPanel DescBox, addToChart, discountSum, discount, buttonChart, secondButtonChart, orderPanel;
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

            //I main griden har vi skapat 5 rader.
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
            grid.Visibility = Visibility.Visible;

            //*************************************************************************************

            listProducts = new List<Product>(); //lägger in samtliga objekt ur csv filen hit.
            discountList = new List<Discount>(); //lägger in samtliga rabattkoder här.
            cartList = new List<Product>();  //Dokumentera om denna delen, att det var svårt att förstå att jag behövde lägga in saker från listProducts in i en ny lista (den här listan) för att kunna manipulera den i chartListBox.

            // FÖRSTA KOLUMNEN 0 GRIDDEN***********************************************************

            //läser in produktlistan
            string[] productArray = File.ReadAllLines("produktLista.csv");
            //Separerar alla ',' och lägger in de i diverse titel här nedan.

            foreach (string line in productArray)
            {
                string[] columns = line.Split(',');
                string titleName = columns[0];
                string descriptionProduct = columns[1];
                decimal productPrice = decimal.Parse(columns[2].Replace('.', ','));
                string pictures = columns[3];

                //För varje rad i csv filen skapar vi ett nytt objekt (x) av klassen.
                Product x = new Product(titleName, descriptionProduct, productPrice, pictures);
                //Lägger till objektet i en lista (titelnamn, beskrivning och pris)
                listProducts.Add(x);
            }

            productList = new TextBlock //Detta är titeln som står högst upp "Produktlista"
            {
                FontSize = 20,
                Margin = new Thickness(2),
                TextAlignment = TextAlignment.Center,
                Width = 200,
                Text = "Produktlista"
            };
            grid.Children.Add(productList);
            Grid.SetColumn(productList, 0);
            Grid.SetRow(productList, 0);

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
            //lägger in titeln och priset för varje objekt av Product klassen utifrån csv fil in i en ListBox
            foreach (Product x in listProducts)
            {
                productListBox.Items.Add(x.Title + " | " + x.Price.ToString("C"));
            }

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

            DescBox = new StackPanel { Orientation = Orientation.Vertical };
            grid.Children.Add(DescBox);
            Grid.SetColumn(DescBox, 1);
            Grid.SetRow(DescBox, 1);

            image = CreateImage(@"Images\ost.jpg"); //Denna är hidden så att den inte syns vid programmets start.

            // KOLUMN 2 VID BESTÄLLNING********************************************************
            descriptionBox = new TextBlock //I den här ListBoxen ska vi visa användaren en lista på valbara produkter från en csv fil.
            {
                FontSize = 15,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 200,
                //Height = 350,
                TextWrapping = TextWrapping.Wrap,
                Text = string.Empty
            };
            DescBox.Children.Add(descriptionBox);

            //STACKPANEL VID BESTÄLLNING I KOLUMN 2.
            orderPanel = new StackPanel { Orientation = Orientation.Vertical };
            grid.Children.Add(orderPanel);
            Grid.SetColumn(orderPanel, 1);
            Grid.SetRow(orderPanel, 1);
            orderPanel.Visibility = Visibility.Collapsed;

            //Denna förblir hidden till dess att användaren trycker på beställ.
            orderedProducts = new ListBox
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0, 20, 0, 0),
                //Padding = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 200,
            };
            orderPanel.Children.Add(orderedProducts);

            orderSum = new TextBlock
            {
                Margin = new Thickness(50, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200
            };
            orderPanel.Children.Add(orderSum);

            backFromOrder = new Button //Här är knappen för att lägga till rabattkoden till varukorgen
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = "Tillbaka",
                Width = 100,
                Margin = new Thickness(50, 20, 0, 0)

            };
            orderPanel.Children.Add(backFromOrder);
            backFromOrder.Click += BackFromOrderClick;

            //VARUKORGEN Texten högst upp *********************************************************
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

            //STACK PANEL FÖR KNAPPARNA I PRODUKTLISTAN********************************************
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

            info = new Button //När användaren klickar här så ska produktbeskrivning visas i kolumn 1 (mitten kolumnen)
            {
                Content = "Visa info",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            addToChart.Children.Add(info);
            info.Click += ClickedInfo; //Visar information om den markerade produkten

            //LISTA FÖR VARUKORGEN*****************************************************************
            chartListBox = new ListBox //Listbox som ska visa upp de tillagda artiklarna i varukorgen.
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
            //*************************************************************************************
            //Created a panel just under chartList to show the total sum of the products.

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

            totalSumInChart = new TextBlock
            {
                Foreground = Brushes.ForestGreen,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            discountSum.Children.Add(totalSumInChart);

            //*************************************************************************************
            discount = new StackPanel { Orientation = Orientation.Horizontal }; //Skapade en ny stackPanel för rabattfäleten
            grid.Children.Add(discount);
            Grid.SetRow(discount, 3);
            Grid.SetColumn(discount, 2);

            discountEnabled = new TextBlock
            {
                TextAlignment = TextAlignment.Left,
                Foreground = Brushes.Black,
                Width = 200,
                Margin = new Thickness(0, 2, 2, 5),
                Visibility = Visibility.Collapsed,
                IsEnabled = false
            };
            discount.Children.Add(discountEnabled);

            addDiscount = new Button //Här är knappen för att lägga till rabattkoden till varukorgen
            {
                Content = "Lägg till",
                Width = 50,
                Margin = new Thickness(0, 2, 2, 0)
            };
            discount.Children.Add(addDiscount);
            addDiscount.Click += AddDiscount;

            discountBox = new TextBox //Inuti denna boxen matar användaren in rabattkoden,      Denna är initierad högs upp
            {
                Foreground = Brushes.Gray,
                Background = Brushes.LightGoldenrodYellow,
                Text = "Rabattkod",
                Width = 148,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 2, 0, 0)
            };
            discount.Children.Add(discountBox);
            discountBox.GotFocus += HasBeenClicked; //Skapar en metod för vad som ska hända när användaren klickar i rabatt rutan.

            //KNAPPAR INUTI STACKPANELEN FÖR VARUKORGEN************************************************************************
            buttonChart = new StackPanel { Orientation = Orientation.Horizontal }; // skapad för att lägga till knapparna under varukorgen
            grid.Children.Add(buttonChart);
            Grid.SetRow(buttonChart, 4);
            Grid.SetColumn(buttonChart, 2);

            remove = new Button //Ta bort en produkt (genom att markera den och klicka på denna knappen)
            {
                Content = "Ta bort",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            buttonChart.Children.Add(remove);
            remove.Click += ClickedRemove;

            save = new Button //Spara varukorgen till en csv fil i temp mappen.
            {
                Content = "Spara",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            buttonChart.Children.Add(save);

            secondButtonChart = new StackPanel { Orientation = Orientation.Horizontal }; // skapad för att lägga till knapparna under varukorgen
            grid.Children.Add(secondButtonChart);
            Grid.SetRow(secondButtonChart, 5);
            Grid.SetColumn(secondButtonChart, 2);

            empty = new Button //TÖM Knappen
            {
                Content = "Töm",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            secondButtonChart.Children.Add(empty);
            empty.Click += ClickedEmptyAll;

            order = new Button //Beställ knappen som ska leda till att användaren får en "Tack förbeställningen"-ruta med detaljer för beställningen
            {
                Content = "Beställ",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            secondButtonChart.Children.Add(order);
            order.Click += ClickedOrder;
            //***********************************************************************************************
        }

        private void BackFromOrderClick(object sender, RoutedEventArgs e)
        {
            //Visar ursprunget i Kolumn 0
            productListBox.Visibility = Visibility.Visible;
            addToChart.Visibility = Visibility.Visible;

            //Visar Ursprunget i Kolumn 1
            DescBox.Visibility = Visibility.Visible;

            //Visar ursprunget i hela kolumn 2.
            buttonChart.Visibility = Visibility.Visible;
            secondButtonChart.Visibility = Visibility.Visible;
            chartListBox.Visibility = Visibility.Visible;
            discount.Visibility = Visibility.Visible;
            discountSum.Visibility = Visibility.Visible;
            discountEnabled.Visibility = Visibility.Collapsed;

            //Visar den nya panelen i kolumn 2 (vid beställning)
            orderPanel.Visibility = Visibility.Collapsed;

            discountBox.Visibility = Visibility.Visible;
            addDiscount.Visibility = Visibility.Visible;

            productList.Text = "Produktlista";
            productDescritpion.Text = "Produktbeskrivning";
            chart.Text = "Varukorgen";

            //Nollställer rabattexten som användaren får knappa in till sitt urpsrungstillstånd
            discountBox.Foreground = Brushes.Gray;
            discountBox.Background = Brushes.LightGoldenrodYellow;
            discountBox.Text = "Rabattkod";

            //Rensar listan och listboxen som visar upp tillagda artiklar i listboxen.
            chartListBox.Items.Clear();
            orderedProducts.Items.Clear();
            cartList.Clear();
            listProducts.Clear();
            discountEnabled.Text = string.Empty;

            //Läser in listan på nytt eftersom listan rensats och vi vill hämta ursprungspriset igen (från csv-filen) efter att en rabattkod har matats in.
            string[] productArray = File.ReadAllLines("produktLista.csv");
            foreach (string line in productArray)
            {
                string[] columns = line.Split(',');
                string titleName = columns[0];
                string descriptionProduct = columns[1];
                decimal productPrice = decimal.Parse(columns[2].Replace('.', ','));
                string pictures = columns[3];

                Product x = new Product(titleName, descriptionProduct, productPrice, pictures);
                listProducts.Add(x);
            }

            //Nollställer summan på totalsum.
            totalSum = 0;
            totalSumInChart.Text = totalSum.ToString("C");
        }

        private void ClickedOrder(object sender, RoutedEventArgs e)
        {

            //Byter ut texten i samtliga kolumner för rad 0.
            productList.Text = string.Empty;
            productDescritpion.Text = "Tack för din beställning";
            chart.Text = string.Empty;

            //Gömmer allt i kolumn 0.
            productListBox.Visibility = Visibility.Collapsed;
            addToChart.Visibility = Visibility.Collapsed;

            //Gömmer TextBox för beskrivning och bild i kolumn 1.
            DescBox.Visibility = Visibility.Collapsed;

            //Gömmer hela kolumn 2.
            buttonChart.Visibility = Visibility.Collapsed;
            secondButtonChart.Visibility = Visibility.Collapsed;
            chartListBox.Visibility = Visibility.Collapsed;
            discount.Visibility = Visibility.Collapsed;
            discountSum.Visibility = Visibility.Collapsed;

            //Visar den nya panelen i kolumn 2 (vid beställning)
            orderPanel.Visibility = Visibility.Visible;

            discountEnabled.Foreground = Brushes.Black;
            orderedProducts.Height = 350;

            orderSum.Text = "Summa: " + totalSum.ToString("C") + Environment.NewLine + discountEnabled.Text;

            foreach (Product x in cartList)
            {
                orderedProducts.Items.Add(x.Title + " | " + x.Price.ToString("C"));
            }
        }

        private void AddDiscount(object sender, RoutedEventArgs e)
        {
            //Dokumentera om denna delen.. Den var sjukt jobbig att få till.
            discountList.Clear();
            //Gör så att det inte spelar någon roll om användaren knappar in stora eller små bokstäver.
            discountBox.Text = discountBox.Text.ToUpper();
            string[] discountArray = File.ReadAllLines("rabattKoder.csv");
            foreach (string code in discountArray)
            {
                string[] columns = code.Split(',');
                string discountCode = columns[0];
                decimal discountPercentage = decimal.Parse(columns[1].Replace('.', ','));

                Discount y = new Discount(discountCode, discountPercentage);
                discountList.Add(y);
            }

            foreach (Discount y in discountList)
            {
                if (discountBox.Text == y.Code)
                {
                    discountBox.Background = Brushes.LightGreen;

                    discountBox.Visibility = Visibility.Collapsed;
                    addDiscount.Visibility = Visibility.Collapsed;
                    discountEnabled.Visibility = Visibility.Visible;

                    foreach (Product x in cartList)
                    {
                        chartListBox.Items.Clear();
                        totalSum -= x.Price * y.DiscountPercentage;
                    }

                    foreach (Product x in listProducts)
                    {
                        x.Price -= x.Price * y.DiscountPercentage;
                    }
                    totalSumInChart.Text = Math.Round(totalSum, 2).ToString("C");
                    discountEnabled.Text = "Rabatt: " + (y.DiscountPercentage * 100) + "%";

                    foreach (Product x in cartList)
                    {
                        chartListBox.Items.Add(x.Title + " | " + x.Price.ToString("C"));
                    }
                }
                else
                {
                    discountBox.Background = Brushes.OrangeRed;
                }
            }         
        }
        private void ClickedEmptyAll(object sender, RoutedEventArgs e)
        {
            MessageBoxResult warning = MessageBox.Show("Är du säker att du vill tömma varukorgen? Även rabattkod kommer att återställas. Du kommer behöva knappa in din rabattkod på nytt.", "Varning!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            switch (warning)
            {
                case MessageBoxResult.Yes:

                    discountBox.Visibility = Visibility.Visible;
                    addDiscount.Visibility = Visibility.Visible;
                    discountEnabled.Visibility = Visibility.Collapsed; //Så att den inte tar någon plats alls i main griden.

                    //Nollställer rabattexten som användaren får knappa in till sitt urpsrungstillstånd
                    discountBox.Foreground = Brushes.Gray;
                    discountBox.Background = Brushes.LightGoldenrodYellow;
                    discountBox.Text = "Rabattkod";

                    //Rensar listan och listboxen som visar upp tillagda artiklar i listboxen.
                    chartListBox.Items.Clear();
                    cartList.Clear();
                    listProducts.Clear();

                    //Läser in listan på nytt eftersom listan rensats och vi vill hämta ursprungspriset igen (från csv-filen) efter att en rabattkod har matats in.
                    string[] productArray = File.ReadAllLines("produktLista.csv");
                    foreach (string line in productArray)
                    {
                        string[] columns = line.Split(',');
                        string titleName = columns[0];
                        string descriptionProduct = columns[1];
                        decimal productPrice = decimal.Parse(columns[2].Replace('.', ','));
                        string pictures = columns[3];

                        Product x = new Product(titleName, descriptionProduct, productPrice, pictures);
                        listProducts.Add(x);
                    }

                    //Nollställer summan på totalsum.
                    totalSum = 0;
                    totalSumInChart.Text = totalSum.ToString("C");
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
                Product p = cartList[foodIndex];
                totalSum -= p.Price;
                totalSumInChart.Text = Math.Round(totalSum, 2).ToString("C");
                //"C" Sätter currency baserat på valutan i ditt land. I mitt fall "kr"

                cartList.RemoveAt(foodIndex);
            }
            catch
            {
                MessageBoxResult warning = MessageBox.Show("Vänligen markera produkt som du vill ta bort.", "Hoppsan!", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (warning)
                {
                    case MessageBoxResult.OK:
                        break;
                }
            }
        }
        private void ClickedAddToChart(object sender, RoutedEventArgs e)
        {
            try
            {
                //Dokumentera om denna delen, att den var sjukt svår.
                chartListBox.Items.Clear();
                int selectedIndex = productListBox.SelectedIndex;
                //cartList.Add(listProducts[selectedIndex]);
                Product p = listProducts[selectedIndex];
                cartList.Add(p);

                foreach (Product x in cartList)
                {
                    chartListBox.Items.Add(x.Title + " | " + x.Price.ToString("C"));
                }
                //För att visa priset av totalsumman i varukorgen!
                totalSum += p.Price;
                totalSumInChart.Text = Math.Round(totalSum, 2).ToString("C");
            }
            catch
            {
                MessageBoxResult warning = MessageBox.Show("Vänligen markera den produkt som du vill lägga till i varukorgen.", "Hoppsan!", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (warning)
                {
                    case MessageBoxResult.OK:
                        break;
                }
            }
        }
        private void ClickedInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = productListBox.SelectedIndex;
                string path = @"Images\" + listProducts[selectedIndex].Image;

                descriptionBox.Text = listProducts[selectedIndex].Descprition;

                image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                image.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBoxResult warning = MessageBox.Show("Vänligen markera produkt för att visa info.", "Hoppsan!", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (warning)
                {
                    case MessageBoxResult.OK:
                        break;
                }
            }
        }
        private void HasBeenClicked(object sender, RoutedEventArgs e) //Denna metod gör så att boxen blir tömd när man klickar på den.
        {
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
            DescBox.Children.Add(image);
            // A small rendering tweak to ensure maximum visual appeal.
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            return image;
        }
    }
}
