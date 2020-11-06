﻿using Microsoft.VisualBasic.CompilerServices;
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
        private TextBlock chart, productDescritpion, productList, descriptionBox, orderSum, totalSumInChart;
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
            grid.Visibility = Visibility.Visible;

            //LISTOR***********************************************************************************
            listProducts = new List<Product>(); //lägger in samtliga objekt ur csv filen hit.
            discountList = new List<Discount>(); //lägger in samtliga rabattkoder här.
            cartList = new List<Product>();  //Dokumentera om denna delen, att det var svårt att förstå att jag behövde lägga in saker från listProducts in i en ny lista (den här listan) för att kunna manipulera den i chartListBox.
  


            //KOLUMN 0**************************************************************************************
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
            info = new Button
            {
                Content = "Visa info",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            addToChart.Children.Add(info);
            info.Click += ClickedInfo;

            //läser in produktlistan
            string[] productArray = File.ReadAllLines("produktLista.csv");

            //Separerar alla ',' och lägger in de i diverse titel.
            foreach (string line in productArray)
            {
                string[] columns = line.Split(',');
                string titleName = columns[0];
                string descriptionProduct = columns[1];
                decimal productPrice = decimal.Parse(columns[2].Replace('.', ','));
                string pictures = columns[3];

                //För varje rad i csv filen skapar vi ett nytt objekt (x) av klassen.
                Product productList = new Product(titleName, descriptionProduct, productPrice, pictures);
                //Lägger till objektet i en lista (titelnamn, beskrivning och pris)
                listProducts.Add(productList);
            }
            foreach (Product p in listProducts)
            {
                productListBox.Items.Add(p.Title + " | " + p.Price.ToString("C"));
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

            //Ändra namnet på DescBox till descriptionPanel
            DescBox = new StackPanel { Orientation = Orientation.Vertical };
            grid.Children.Add(DescBox);
            Grid.SetColumn(DescBox, 1);
            Grid.SetRow(DescBox, 1);

            image = CreateImage(@"Images\ost.jpg"); //Denna är hidden så att den inte syns vid programmets start.

            // KOLUMN 1 *************************************************************************
            descriptionBox = new TextBlock //I den här ListBoxen ska vi visa användaren en lista på valbara produkter från en csv fil.
            {
                FontSize = 15,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 200,
                TextWrapping = TextWrapping.Wrap,
                Text = string.Empty
            };
            DescBox.Children.Add(descriptionBox);

            //KOLUMN 1, VID BESTÄLLNING ****************************************************************
            //Allt inuti orderPanel förblir 'Collapsed' för att hållas osynlig tills en beställning görs av användaren.
            orderPanel = new StackPanel { Orientation = Orientation.Vertical };
            grid.Children.Add(orderPanel);
            Grid.SetColumn(orderPanel, 1);
            Grid.SetRow(orderPanel, 2);
            orderPanel.Visibility = Visibility.Collapsed;

            //Beställda varukorgen
            orderedProducts = new ListBox
            {
                Background = Brushes.AliceBlue,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 200,
            };
            orderPanel.Children.Add(orderedProducts);

            //Summan vid order
            orderSum = new TextBlock
            {   
                Text = "Summa: ",
                Margin = new Thickness(50, 5, 0, 15),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200,
                Height = 30
            };
            orderPanel.Children.Add(orderSum);

            //Skapar en stackpanel som innehåller allt med rabatten att göra.
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

            totalSumInChart = new TextBlock
            {
                Foreground = Brushes.ForestGreen,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            discountSum.Children.Add(totalSumInChart);

            //stackPanel under stackPanelen ovan.
            buttonChart = new StackPanel { Orientation = Orientation.Horizontal };
            grid.Children.Add(buttonChart);
            Grid.SetRow(buttonChart, 4);
            Grid.SetColumn(buttonChart, 2);

            remove = new Button //Ta bort enskild vald produkt från varukorgen.
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
            save.Click += ClickedSaveChart;

            //En till stackpanel under stackPanel ovan.
            secondButtonChart = new StackPanel { Orientation = Orientation.Horizontal };
            grid.Children.Add(secondButtonChart);
            Grid.SetRow(secondButtonChart, 5);
            Grid.SetColumn(secondButtonChart, 2);

            empty = new Button //Tömmer hela varukorgen, och nollställer programmet.
            {
                Content = "Töm",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            secondButtonChart.Children.Add(empty);
            empty.Click += ClickedEmptyAll;

            order = new Button //Lägg en beställning med valda produkter och vald rabatt.
            {
                Content = "Beställ",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            secondButtonChart.Children.Add(order);
            order.Click += ClickedOrder;

            //Läser in sparad varukorg
            //Denna för att skapa filen "savedChart.csv" i temp mappen. Skapar en tom csv fil med namnet savedChart.csv om det inte finns någon sparad varukorg.
            File.AppendAllText(@"C:\Windows\Temp\savedChart.csv", string.Empty);

            //Läser in den sparade varukorgen.
            string[] savedChart = File.ReadAllLines(@"C:\Windows\Temp\savedChart.csv");

            foreach (string line in savedChart)
            {
                string[] columns = line.Split(',');
                string titleName = columns[0];
                string productDescription = columns[1];
                decimal price = decimal.Parse(columns[2].Replace('.', ','));
                string pictures = columns[3];

                Product savedProduct = new Product(titleName, productDescription, price, pictures);

                cartList.Add(savedProduct);

                //Lägger in de sparade produkterna i varukorgen
                chartListBox.Items.Add(savedProduct.Title + " | " + savedProduct.Price.ToString("C"));

                //Uppdaterar totalSum: 'Summa:' i raden under varukorgen.
                totalSum += savedProduct.Price;
                totalSumInChart.Text = totalSum.ToString("C");
            }
        }
        private void ClickedSaveChart(object sender, RoutedEventArgs e)
        {
            //Sparar varukorgen när användaren trycker på "Spara"
            var csv = new StringBuilder();
            foreach (Product p in cartList)
            {
                var title = p.Title;
                var description = p.Descprition;
                var price = p.Price;
                var image = p.Image;

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
        private void BackFromOrderClick(object sender, RoutedEventArgs e)
        {
            //Visar ursprunget i Kolumn 0
            productListBox.Visibility = Visibility.Visible;
            addToChart.Visibility = Visibility.Visible;

            //Visar Ursprunget i Kolumn 1
            DescBox.Visibility = Visibility.Visible;
            backFromOrder.Visibility = Visibility.Collapsed;

            //Visar ursprunget i hela kolumn 2.
            buttonChart.Visibility = Visibility.Visible;
            secondButtonChart.Visibility = Visibility.Visible;
            chartListBox.Visibility = Visibility.Visible;
            discount.Visibility = Visibility.Collapsed;
            discountSum.Visibility = Visibility.Visible;

            //Visar den nya panelen i kolumn 2 (vid beställning)
            orderPanel.Visibility = Visibility.Collapsed;

            save.Visibility = Visibility.Visible;
            discountBox.Visibility = Visibility.Visible;
            addDiscount.Visibility = Visibility.Visible;

            //"Nollställer även bild och beskrivning
            image.Visibility = Visibility.Hidden;
            descriptionBox.Visibility = Visibility.Hidden;

            //Återställer namnen på rubrikerna till ursprungstitel.
            productList.Text = "Produktlista";
            productDescritpion.Text = "Produktbeskrivning";
            chart.Text = "Varukorgen";

            //Nollställer rabattexten som användaren får knappa in till sitt urpsrungstillstånd
            discountBox.Foreground = Brushes.Gray;
            discountBox.Background = Brushes.LightGoldenrodYellow;
            discountBox.Text = "Rabattkod";

            //Tömmer/Rensar väsentliga listor och ListBox (som visas för användaren).
            chartListBox.Items.Clear();
            orderedProducts.Items.Clear();
            cartList.Clear();
            listProducts.Clear();

            //Läser in listan på nytt eftersom listan rensats och vi vill hämta ursprungspriset igen (från csv-filen: produktLista) efter att en rabattkod har matats in.
            string[] productArray = File.ReadAllLines("produktLista.csv");
            foreach (string line in productArray)
            {
                string[] columns = line.Split(',');
                string titleName = columns[0];
                string descriptionProduct = columns[1];
                decimal productPrice = decimal.Parse(columns[2].Replace('.', ','));
                string pictures = columns[3];

                Product product = new Product(titleName, descriptionProduct, productPrice, pictures);
                listProducts.Add(product);
            }

            //Nollställer summan på totalsum.
            totalSum = 0;
            totalSumInChart.Text = totalSum.ToString("C");
        }
        private void ClickedOrder(object sender, RoutedEventArgs e)
        {
            save.Visibility = Visibility.Visible;
            backFromOrder.Visibility = Visibility.Visible;
            //Byter ut texten i samtliga kolumner för rad 0.
            productList.Text = string.Empty;
            productDescritpion.Text = "Tack för din beställning";
            chart.Text = string.Empty;

            //Gömmer allt i kolumn 0.
            productListBox.Visibility = Visibility.Collapsed;
            addToChart.Visibility = Visibility.Collapsed;

            //Gömmer kolumn 1.
            DescBox.Visibility = Visibility.Collapsed;

            //Gömmer hela kolumn 2.
            buttonChart.Visibility = Visibility.Collapsed;
            secondButtonChart.Visibility = Visibility.Collapsed;
            chartListBox.Visibility = Visibility.Collapsed;
            discountSum.Visibility = Visibility.Collapsed;

            //Visar den nya panelen i kolumn 2 (vid beställning)
            orderPanel.Visibility = Visibility.Visible;
            discount.Visibility = Visibility.Visible;

            //discountEnabled.Foreground = Brushes.Black; //Kolla upp denna också
            orderedProducts.Height = 350;

            orderSum.Text = "Summa: " + totalSum.ToString("C");

            foreach (Product p in cartList)
            {
                orderedProducts.Items.Add(p.Title + " | " + p.Price.ToString("C"));
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

                Discount discount = new Discount(discountCode, discountPercentage);
                discountList.Add(discount);
            }

            foreach (Discount y in discountList)
            {
                if (discountBox.Text == y.Code)
                {
                    discountBox.Visibility = Visibility.Hidden;
                    addDiscount.Visibility = Visibility.Hidden;

                    orderedProducts.Items.Clear();
                    foreach (Product x in cartList)
                    {
                        orderedProducts.Items.Add(x.Title + " | " + x.Price.ToString("C"));
                        totalSum -= x.Price * y.DiscountPercentage;
                    }

                    //Skapar denna foreach med ordinaryPrice endast för att kunna skriva ut tidigare ord.pris
                    decimal ordinaryPrice = 0;
                    foreach (Product x in cartList)
                    {
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
        private void ClickedEmptyAll(object sender, RoutedEventArgs e)
        {
            MessageBoxResult warning = MessageBox.Show("Är du säker att du vill tömma varukorgen? Även rabattkod kommer att återställas. Du kommer behöva knappa in din rabattkod på nytt.", "Varning!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            switch (warning)
            {
                case MessageBoxResult.Yes:

                    save.Visibility = Visibility.Visible;
                    discountBox.Visibility = Visibility.Visible;
                    addDiscount.Visibility = Visibility.Visible;

                    //"nollställer även bild i kolumn 1 och beskrivning.
                    image.Visibility = Visibility.Hidden;
                    descriptionBox.Visibility = Visibility.Hidden;

                    //Nollställer rabattexten som användaren får knappa in till sitt urpsrungstillstånd
                    discountBox.Foreground = Brushes.Gray;
                    discountBox.Background = Brushes.LightGoldenrodYellow;
                    discountBox.Text = "Rabattkod";

                    //Rensar listor, och listboxen som visar upp tillagda artiklar i listboxen.
                    chartListBox.Items.Clear();
                    cartList.Clear();
                    listProducts.Clear();
                    discountList.Clear();

                    //Läser in listan på nytt eftersom listan rensats och vi vill hämta ursprungspriset igen (från csv-filen) efter att en rabattkod har matats in.
                    string[] productArray = File.ReadAllLines("produktLista.csv");
                    foreach (string line in productArray)
                    {
                        string[] columns = line.Split(',');
                        string titleName = columns[0];
                        string descriptionProduct = columns[1];
                        decimal productPrice = decimal.Parse(columns[2].Replace('.', ','));
                        string pictures = columns[3];

                        Product product = new Product(titleName, descriptionProduct, productPrice, pictures);
                        listProducts.Add(product);
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
                totalSumInChart.Text = totalSum.ToString("C");
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
        private void ClickedAddToChart(object sender, RoutedEventArgs e)
        {
            try
            {
                //Nollställer bild och beskrivning
                image.Visibility = Visibility.Hidden;
                descriptionBox.Visibility = Visibility.Hidden;

                //Dokumentera om denna delen, att den var sjukt svår.
                chartListBox.Items.Clear();
                int selectedIndex = productListBox.SelectedIndex;
                //cartList.Add(listProducts[selectedIndex]);
                Product product = listProducts[selectedIndex];
                cartList.Add(product);

                foreach (Product p in cartList)
                {
                    chartListBox.Items.Add(p.Title + " | " + p.Price.ToString("C"));
                }
                //För att visa priset av totalsumman i varukorgen!
                totalSum += product.Price;
                totalSumInChart.Text = totalSum.ToString("C");
            }
            catch
            {
                MessageBoxResult info = MessageBox.Show("Vänligen markera den produkt som du vill lägga till i varukorgen.", "Hoppsan!", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (info)
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
                descriptionBox.Visibility = Visibility.Visible;

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
