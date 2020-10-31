﻿using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
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
        public int DiscountPercentage;

        public Discount(string code, int discountPercentage)
        {
            Code = code;
            DiscountPercentage = discountPercentage;
        }
    }
    public partial class MainWindow : Window
    {
        private Image image;
        private Grid grid;
        private ListBox productListBox, chartListBox;
        private TextBox discountBox;
        private TextBlock chart, productDescritpion, productList, descriptionBox, totalSumInChart;
        private Button addDiscount, order, empty, save, remove, addItem, info;
        private List<Product> listProducts;
        private List<Discount> discountList;
        private StackPanel DescBox;
        private decimal totalSum;
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        private void Start()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            listProducts = new List<Product>(); //lägger in samtliga objekt ur csv filen hit.
            discountList = new List<Discount>(); //lägger in samtliga rabattkoder här.

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

            // FÖRSTA KOLUMNEN I GRIDDEN*****************************************************************

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

            //Läser in från rabattKoder.csv // RABATTKODER
            string[] discountArray = File.ReadAllLines(@"C:\Users\orhan\source\repos\Lektion18GUIhändelser\FirstApp\rabattKoder.csv");
            foreach (string code in discountArray)
            {
                string[] columns = code.Split(',');
                string discountCode = columns[0];
                int discountPercentage = int.Parse(columns[1]);

                Discount y = new Discount(discountCode, discountPercentage);
                discountList.Add(y);
            }

            //Läser in från produktLista.csv // PRODUKTLISTAN
            string[] productArray = File.ReadAllLines(@"C:\Users\orhan\source\repos\Lektion18GUIhändelser\FirstApp\produktLista.csv");

            //Separerar alla ',' och lägger in de i diverse titel här nedan.
            foreach (string line in productArray)
            {
                string[] columns = line.Split(',');
                string titleName = columns[0];
                string descriptionProduct = columns[1];
                decimal productPrice = decimal.Parse(columns[2]);
                string pictures = columns[3];

                //För varje rad i csv filen skapar vi ett nytt objekt (x) av klassen.
                Product x = new Product(titleName, descriptionProduct, productPrice, pictures);
                //Lägger till objektet i en lista (titelnamn, beskrivning och pris)
                listProducts.Add(x);
            }
            //Här säger vi att för varje objekt (x) i klassen Product, skriv ut x.Title och x.Price i productListBox.
            foreach (Product x in listProducts)
            {
                productListBox.Items.Add(x.Title + " (" + x.Price + ")kr");
            }

            //KOLUMN 2 PRODUKTBESKRIVNING************************************************************************
            productDescritpion = new TextBlock //Detta är titeln som står högst upp "Produktbeskrivning"
            {
                FontSize = 20,
                Margin = new Thickness(2),
                TextAlignment = TextAlignment.Center,
                Width = 200,
                Text = "Produktbeskrivning"
            };
            grid.Children.Add(productDescritpion);
            Grid.SetColumn(productDescritpion, 1);
            Grid.SetRow(productDescritpion, 0);

            DescBox = new StackPanel { Orientation = Orientation.Vertical };
            grid.Children.Add(DescBox);
            Grid.SetColumn(DescBox, 1);
            Grid.SetRow(DescBox, 1);

            image = CreateImage(@"Images\ost.jpg"); //Denna är hidden

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

            //VARUKORGEN Texten högst upp ************************************************************************
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

            //STACK PANEL FÖR KNAPPARNA I PRODUKTLISTAN*****************************************************************************
            StackPanel addToChart = new StackPanel { Orientation = Orientation.Horizontal }; //Denna StackPanel är skapad för info och lägg till knapparna.
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

            //LISTA FÖR VARUKORGEN**************************************************************************************************

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

            //*********************************************************************************************************************
            //Created a panel just under chartList to show the total sum of the products.

            StackPanel discountSum = new StackPanel { Orientation = Orientation.Horizontal };
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

            //in med metod här istället för kod nedan.

            totalSumInChart = new TextBlock
            {
                Foreground = Brushes.ForestGreen,
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            discountSum.Children.Add(totalSumInChart);

            //totalSumInChart = new TextBlock
            //{
            //    Margin = new Thickness(0, 2, 0, 0),
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    Text = "**Priset ska visas här**"
            //};
            //discountSum.Children.Add(totalSumInChart);

            //**********************************************************************************************************************

            StackPanel discount = new StackPanel { Orientation = Orientation.Horizontal }; //Skapade en ny stackPanel för rabattfäleten
            grid.Children.Add(discount);
            Grid.SetRow(discount, 3);
            Grid.SetColumn(discount, 2);

            addDiscount = new Button //Här är knappen för att lägga till rabattkoden till varukorgen
            {
                Content = "Lägg till",
                Width = 50,
                Margin = new Thickness(0, 2, 2, 0)
            };
            discount.Children.Add(addDiscount);

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

            //KNAPPAR INUTI STACKPANELEN FÖR VARUKORGEN*******************************************************************************
            StackPanel buttonChart = new StackPanel { Orientation = Orientation.Horizontal }; // skapad för att lägga till knapparna under varukorgen
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

            StackPanel secondButtonChart = new StackPanel { Orientation = Orientation.Horizontal }; // skapad för att lägga till knapparna under varukorgen
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
            //***********************************************************************************************
        }
        private void ClickedEmptyAll(object sender, RoutedEventArgs e)
        {
            MessageBoxResult warning = MessageBox.Show("Är du säker att du vill tömma varukorgen? Ja/Nej.", "Varning!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            switch (warning)
            {
                case MessageBoxResult.Yes:
                    chartListBox.Items.Clear();

                    totalSum = 0; //Rensar totalsumman och sätter den på 0
                    totalSumInChart.Text = totalSum.ToString() + "kr"; //Vi sätter även Summan: 0.

                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        private void ClickedRemove(object sender, RoutedEventArgs e)
        {
            try
            {
                //int selectedIndex = chartListBox.SelectedIndex;

                int selectedIndex = chartListBox.SelectedIndex;
                chartListBox.Items.RemoveAt(selectedIndex);

                totalSum -= chartListBox.SelectedItems.;
                //Kod här som ska räkna ut hur mycket totalSum är när man tagit bort produkt från chartListBox.
                //totalSum -= decimal.Parse(selectedIndex).Price;
                totalSumInChart.Text = Math.Round(totalSum, 2).ToString() + "kr";
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
                int selectedIndex = productListBox.SelectedIndex;
                chartListBox.Items.Add(listProducts[selectedIndex].Title + " | " + listProducts[selectedIndex].Price + "kr");

                //För att visa priset av totalsumman i varukorgen!
                totalSum += listProducts[selectedIndex].Price;
                totalSumInChart.Text = Math.Round(totalSum, 2).ToString() + "kr";

                //Kod nedan sorterar det som läggs till i varukorgen i bokstavsordning.
                //    chartList.Items.SortDescriptions.Add(
                //new System.ComponentModel.SortDescription("",
                //System.ComponentModel.ListSortDirection.Ascending));
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
        //private string TotalSumInChart()
        //{

        //    //totalSumInChart = new TextBlock
        //    //{
        //    //    Margin = new Thickness(0, 2, 0, 0),
        //    //    HorizontalAlignment = HorizontalAlignment.Center,
        //    //};

        //    foreach (decimal x in chartListBox.Items)
        //    {
        //        totalSum += x;
        //    }
        //    Math.Round(totalSum, 2);
        //    return totalSumInChart.Text = totalSum.ToString();
        //}
    }
}