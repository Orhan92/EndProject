using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Lektion18GUIhändelser
{
    public class Person
    {
        public string Name;
        public string LastName;
        public string Email;
        public long PersonalID;
        public long Phone;
        public string Swedish;
        public Person(string name, string lastName, string email, long id, long phoneNumber, string swedish)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            PersonalID = id;
            Phone = phoneNumber;
            Swedish = swedish;
        }
    }
    public partial class MainWindow : Window
    {
        private Grid grid;
        private Grid addedGrid; //If you want another grid.
        private Grid infoGrid;
        private TextBox namn;
        private TextBox efternamn;
        private TextBox email;
        private TextBox telefon;
        private TextBox personnummer;
        private List<Person> personList;
        private CheckBox svensk;
        private ListBox persons;
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            // Window options
            Title = "GUI App";
            Width = 450;
            Height = 450;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Scrolling
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Content = root;

            // Main grid
            grid = new Grid();
            //If you want a grid background color : grid = new Grid { Background = Brushes.Black };
            root.Content = grid;
            grid.Margin = new Thickness(5);

            //Skapar 7 rader
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            //skapar 2 kolumner
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); //Du väljer längden själv.
            grid.Visibility = Visibility.Visible;

            //************************************************************GRID 2***********************************************************************************

            addedGrid = new Grid(); //Skapar en ny grid.
            grid.Children.Add(addedGrid); //Vi gör den till huvudgridens (grid) Child.
            Grid.SetColumn(addedGrid, 0); //Vi startar den nya gridden från kolumn 0.
            Grid.SetRow(addedGrid, 7); //Den nya gridden (addedGrid) börjar från rad 7.
            Grid.SetColumnSpan(addedGrid, 6); //Denna gör att addedGrid sträcker sig över 6 kolumner.
            //Grid.SetRowSpan(addedGrid, 6); //Denna gör att addedGrid sträcker genom 6 rader.

            //Skapar 3 rader
            addedGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            addedGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            addedGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            addedGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            //skapar 6 kolumner
            addedGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            addedGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            addedGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            addedGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            addedGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            addedGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            addedGrid.Visibility = Visibility.Hidden;
            //**********************************************************INFO GRID***********************************************************************************

            infoGrid = new Grid();
            grid.Children.Add(infoGrid);
            Grid.SetColumn(infoGrid, 0);
            Grid.SetRow(infoGrid, 7);
            Grid.SetColumnSpan(infoGrid, 6);

            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            infoGrid.Visibility = Visibility.Hidden;
            //****************************************************************************************************************************************************

            personList = new List<Person>();

            addLabel("Name:", 0, 0);
            namn = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
            };
            grid.Children.Add(namn);
            Grid.SetColumn(namn, 1);
            Grid.SetRow(namn, 0);

            addLabel("Surname:", 1, 0);
            efternamn = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
            };
            grid.Children.Add(efternamn);
            Grid.SetColumn(efternamn, 1);
            Grid.SetRow(efternamn, 1);

            addLabel("Email:", 2, 0);
            email = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
            };
            grid.Children.Add(email);
            Grid.SetColumn(email, 1);
            Grid.SetRow(email, 2);

            addLabel("ID:", 3, 0);
            personnummer = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
            };
            grid.Children.Add(personnummer);
            Grid.SetColumn(personnummer, 1);
            Grid.SetRow(personnummer, 3);

            addLabel("Phone:", 4, 0);
            telefon = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Center,
            };
            grid.Children.Add(telefon);
            Grid.SetColumn(telefon, 1);
            Grid.SetRow(telefon, 4);

            addLabel("Swedish:", 5, 0);
            svensk = new CheckBox
            {
                VerticalAlignment = VerticalAlignment.Center,
            };
            grid.Children.Add(svensk);
            Grid.SetColumn(svensk, 1);
            Grid.SetRow(svensk, 5);

            button("Add", 6, 0, HorizontalAlignment.Right, 1).Click += ClickedSave;
            button("Clear", 6, 1, HorizontalAlignment.Left, 1).Click += ClickedClear; ;
            ButtonAddedGrid("Remove", 2, 0, HorizontalAlignment.Left, 1).Click += ClickedRemove;
            ButtonAddedGrid("Show info", 2, 1, HorizontalAlignment.Right, 1).Click += ClickedShowInfo;
            Button back = new Button
            {
                // If you want another text color : Foreground = Brushes.Yellow,
                // If you want another background color : Background = Brushes.Yellow,
                Content = "Back",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(1),
                Width = 200
            };
            infoGrid.Children.Add(back);
            Grid.SetColumn(back, 0);
            Grid.SetRow(back, 2);
            back.Click += ClickedBack;
        }

        private void ClickedBack(object sender, RoutedEventArgs e)
        {
            infoGrid.Visibility = Visibility.Hidden;
            addedGrid.Visibility = Visibility.Visible;
        }

        private void ClickedShowInfo(object sender, RoutedEventArgs e)
        {
            addedGrid.Visibility = Visibility.Hidden;
            infoGrid.Visibility = Visibility.Visible;

            try
            {
                TextBox infoBlock = new TextBox
                {
                    Text = "Name: " + personList[persons.SelectedIndex].Name + Environment.NewLine + "Surname: " + personList[persons.SelectedIndex].LastName + Environment.NewLine + "Email: " + personList[persons.SelectedIndex].Email + Environment.NewLine + "ID: " + personList[persons.SelectedIndex].PersonalID + Environment.NewLine + "Phone: +46" + personList[persons.SelectedIndex].Phone + Environment.NewLine + "Swedish Citizen: " + personList[persons.SelectedIndex].Swedish,
                    Height = 200,
                    Width = 403,
                };
                infoGrid.Children.Add(infoBlock);
                Grid.SetColumn(infoBlock, 0);
                Grid.SetColumnSpan(infoBlock, 6);
                Grid.SetRow(infoBlock, 1);
            }
            catch
            {
                TextBox message = new TextBox
                {
                    Foreground = Brushes.Red,
                    Text = "Nothing to show/or you did not select an item to show.",
                    Height = 200,
                    Width = 403
                };
                infoGrid.Children.Add(message);
                Grid.SetColumn(message, 0);
                Grid.SetColumnSpan(message, 6);
                Grid.SetRow(message, 1);
            }
        }

        private void ClickedRemove(object sender, RoutedEventArgs e)
        {
            try
            {
                persons.Background = Brushes.White;
                int selectedIndex = persons.SelectedIndex;
                personList.RemoveAt(selectedIndex); 
                persons.Items.RemoveAt(selectedIndex); 
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("You did not mark an item to remove/or there is nothing to remove.", "Something went wrong!", MessageBoxButton.OK, MessageBoxImage.Information);       
                switch (result)
                {
                    case MessageBoxResult.OK:
                        break;
                }
            }
        }

        private void ClickedClear(object sender, RoutedEventArgs e)
        {
            namn.Clear();
            efternamn.Clear();
            email.Clear();
            telefon.Clear();
            personnummer.Clear();
            svensk.IsChecked = false;
        }

        private void ClickedSave(object sender, RoutedEventArgs e)
        {
            addedGrid.Visibility = Visibility.Visible;
            infoGrid.Visibility = Visibility.Hidden;

            string förnamn;
            string eftNamn;
            string epost;
            long nummer;
            long persnummer;

            string svenne;
            try
            {
                telefon.Background = Brushes.White;
                telefon.Foreground = Brushes.Black;

                förnamn = namn.Text;
                eftNamn = efternamn.Text;
                epost = email.Text.ToLower();
                nummer = long.Parse(telefon.Text);
                persnummer = long.Parse(personnummer.Text);


                if ((bool)svensk.IsChecked)
                {
                    svenne = "Yes";
                }
                else
                {
                    svenne = "No";
                }

                var p = new Person(förnamn, eftNamn, epost, persnummer, nummer, svenne);
                personList.Add(p);

                persons = new ListBox
                {
                    Height = 200,
                    Width = 403
                };
                foreach (Person x in personList)
                {
                    persons.Items.Add(x.Name + " " + x.LastName);
                }
                addedGrid.Children.Add(persons);
                Grid.SetColumn(persons, 0);
                Grid.SetColumnSpan(persons, 6);
                Grid.SetRow(persons, 1);
                int selectedIndex = persons.SelectedIndex;

            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("You have to fill in information to Add.", "Something went wrong!", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        break;
                }
                addedGrid.Visibility = Visibility.Visible;
                infoGrid.Visibility = Visibility.Hidden;
            }
        }
        private Label addLabel(string content, int row, int column) //Adds for grid
        {
            Label x = new Label
            {
                Content = content,
                //Foreground = Brushes.Yellow, //Gives the text a yellow color
            };
            grid.Children.Add(x);
            Grid.SetColumn(x, column);
            Grid.SetRow(x, row);

            return x;
        }

        private Button button(string content, int row, int column, HorizontalAlignment alignment, int margin) //adds for grid
        {
            Button x = new Button
            {
                // If you want another text color : Foreground = Brushes.Yellow,
                // If you want another background color : Background = Brushes.Yellow,
                Content = content,
                HorizontalAlignment = alignment,
                Margin = new Thickness(margin),
                Width = 200
            };
            grid.Children.Add(x);
            Grid.SetColumn(x, column);
            Grid.SetRow(x, row);
            return x;
        }

        private Button ButtonAddedGrid(string content, int row, int column, HorizontalAlignment alignment, int margin) //adds for grid
        {
            Button x = new Button
            {
                // If you want another text color : Foreground = Brushes.Yellow,
                // If you want another background color : Background = Brushes.Yellow,
                Content = content,
                HorizontalAlignment = alignment,
                Margin = new Thickness(margin),
                Width = 200
            };
            addedGrid.Children.Add(x);
            Grid.SetColumn(x, column);
            Grid.SetRow(x, row);
            return x;
        }
    }
}
