using System;
using System.Collections.Generic;
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

namespace FirstApp
{
    public partial class MainWindow : Window
    {
        private ListBox productListBox;
        private ListBox ChartList;
        private TextBox discountBox;
        private TextBlock chart;
        private TextBlock productDescritpion;
        private TextBlock productList;
        private Button addDiscount;
        private Button order;
        private Button empty;
        private Button save;
        private Button remove;
        private Button addItem;
        private Button info;
        
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
            Height = 500;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Scrolling
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Content = root;

            // Main grid
            Grid grid = new Grid();
            root.Content = grid;
            grid.Margin = new Thickness(5);

            //I main griden har vi skapat 5 rader.
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

            productListBox = new ListBox //I den här ListBoxen ska vi visa användaren en lista på valbara produkter från en csv fil.
            {
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200,
                Height = 350
            };
            grid.Children.Add(productListBox);
            Grid.SetColumn(productListBox, 0);
            Grid.SetRow(productListBox, 1);

            //STACK PANEL FÖR KNAPPARNA I PRODUKTLISTAN*****************************************************************************
            StackPanel addToChart = new StackPanel { Orientation = Orientation.Horizontal }; //Denna StackPanel är skapad för info och lägg till knapparna.
            grid.Children.Add(addToChart);
            Grid.SetRow(addToChart, 2);
            Grid.SetColumn(addToChart, 0);

            info = new Button //När användaren klickar här så ska produktbeskrivning visas i kolumn 1 (mitten kolumnen)
            {
                Content = "Visa info",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            addToChart.Children.Add(info);

            addItem = new Button //Denna knapp ska lägga till den markerade artikeln i varukorgen
            {
                Content = "Lägg till",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            addToChart.Children.Add(addItem);

            //LISTA FÖR VARUKORGEN***************************************************************************************************

            ChartList = new ListBox //Listbox som ska visa upp de tillagda artiklarna i varukorgen.
            {
                Margin = new Thickness(0, 2, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 200,
                Height = 350
            };
            grid.Children.Add(ChartList);
            Grid.SetColumn(ChartList, 2);
            Grid.SetRow(ChartList, 1);

            StackPanel discount = new StackPanel { Orientation = Orientation.Horizontal }; //Skapade en ny stackPanel för rabattfäleten
            grid.Children.Add(discount);
            Grid.SetRow(discount, 2);
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
            Grid.SetRow(buttonChart, 3);
            Grid.SetColumn(buttonChart, 2);

            remove = new Button //Ta bort en produkt (genom att markera den och klicka på denna knappen)
            {
                Content = "Ta bort",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            buttonChart.Children.Add(remove);

            save = new Button //Spara varukorgen till en csv fil i temp mappen.
            {
                Content = "Spara",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            buttonChart.Children.Add(save);

            StackPanel secondButtonChart = new StackPanel { Orientation = Orientation.Horizontal }; // skapad för att lägga till knapparna under varukorgen
            grid.Children.Add(secondButtonChart);
            Grid.SetRow(secondButtonChart, 4);
            Grid.SetColumn(secondButtonChart, 2);

            empty = new Button //TÖM Knappen
            {
                Content = "Töm",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            secondButtonChart.Children.Add(empty);

            order = new Button //Beställ knappen som ska leda till att användaren får en "Tack förbeställningen"-ruta med detaljer för beställningen
            {
                Content = "Beställ",
                Width = 100,
                Margin = new Thickness(0, 2, 0, 0)
            };
            secondButtonChart.Children.Add(order);
            //***********************************************************************************************
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

    }
}
