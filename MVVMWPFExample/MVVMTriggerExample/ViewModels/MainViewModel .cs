using GalaSoft.MvvmLight.CommandWpf;
using MVVMExample.API;
using MVVMExample.DataModal;
using MVVMExample.Models;
using MVVMExample.UavModal;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

/// <summary>
/// This is an example of a viewmodel from my wpf demo app. I would be more than happy to
/// demonstrate the application in it's entirety during an interview. In that application,
/// I show 2 different modal windows, animation, async, powershell, linkedlist, user controls,
/// interfaces, json data serialization, service consumption and many other design ideas.
/// </summary>

namespace MVVMExample.ViewModels
{
    public class MainViewModel : OnPropertyChangedBase, INotifyPropertyChanged
    {
        #region Constatant String
        private const string ConnectionString = "Data Source=DAVIDS-LAPTOP\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;TrustServerCertificate=true;";

        #endregion Constatant String

        private readonly IWindowService _windowService;

        #region Properties
        private WeatherInfo _weatherInfo;
        public WeatherInfo WeatherInfo
        {
            get { return _weatherInfo; }
            set
            {
                _weatherInfo = value;
                OnPropertyChanged(nameof(WeatherInfo));
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        private bool _isButtonEnabled;
        public bool IsButtonEnabled
        {
            get { return _isButtonEnabled; }
            set
            {
                if (_isButtonEnabled != value)
                {
                    _isButtonEnabled = value;
                    OnPropertyChanged(nameof(IsButtonEnabled));
                }
            }
        }

        private bool _isLabelVisible;
        public bool IsLabelVisible
        {
            get { return _isLabelVisible; }
            set
            {
                if (_isLabelVisible != value)
                {
                    _isLabelVisible = value;
                    OnPropertyChanged(nameof(IsLabelVisible));
                }
            }
        }

        private bool _isSpreadsheetLabelVisible;
        public bool IsSpreadsheetLabelVisible
        {
            get { return _isSpreadsheetLabelVisible; }
            set
            {
                if (_isSpreadsheetLabelVisible != value)
                {
                    _isSpreadsheetLabelVisible = value;
                    OnPropertyChanged(nameof(_isSpreadsheetLabelVisible));
                }
            }
        }

        private ObservableCollection<Customer> _allCustomers;
        public ObservableCollection<Customer> AllCustomers
        {
            get { return _allCustomers; }
            set
            {
                _allCustomers = value;
                OnPropertyChanged(nameof(AllCustomers));
            }
        }

        private ObservableCollection<Customer> _filteredCustomers;
        public ObservableCollection<Customer> FilteredCustomers
        {
            get { return _filteredCustomers; }
            set
            {
                _filteredCustomers = value;
                OnPropertyChanged(nameof(FilteredCustomers));
            }
        }

        private string _searchLastName = "";
        public string SearchLastName
        {
            get { return _searchLastName; }
            set
            {
                if (_searchLastName != value)
                {
                    _searchLastName = value;
                    OnPropertyChanged(nameof(SearchLastName));
                    if (_searchLastName == "Data") 
                    { 
                        IsSpreadsheetLabelVisible = true;
                        _searchLastName = string.Empty;
                    }
                    else if (_searchLastName == "Zap" || !IsLabelVisible)
                    {
                        IsSpreadsheetLabelVisible = false;
                    }
                    OnPropertyChanged(nameof(IsSpreadsheetLabelVisible));
                    FilterCustomers();
                }
            }
        }

        private string _userName = "";
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));

                    // If the UserName is not empty, change the Button content
                    if (!string.IsNullOrEmpty(value))
                    {
                        ButtonContent = value;
                    }
                }
            }
        }

        private string _buttonContent = "My 3D Button";
        public string ButtonContent
        {
            get { return _buttonContent; }
            set
            {
                if (_buttonContent != value)
                {
                    _buttonContent = value;
                    OnPropertyChanged(nameof(ButtonContent));
                }
            }
        }

        private Point _carPosition = new Point(0, 0);
        public Point CarPosition
        {
            get { return _carPosition; }
            set
            {
                _carPosition = value;
                OnPropertyChanged(nameof(CarPosition));
            }
        }

        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private string _loadingStatus;
        public string LoadingStatus
        {
            get { return _loadingStatus; }
            set
            {
                if (_loadingStatus != value)
                {
                    _loadingStatus = value;
                    OnPropertyChanged(nameof(LoadingStatus));
                }
            }
        }

        private double _progress;
        public double Progress
        {
            get { return _progress; }
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }

        #endregion Properties



        public ICommand LoadDataCommand { get; }
        public ICommand ToggleVisibilityCommand { get; }
        public ICommand OpenJsonModalCommand { get; }
        public ICommand ShowUavPathCommand { get; }
        public ICommand GetWeatherCommand { get; private set; }


        private void ToggleVisibility(object parameter)
        {
            IsLabelVisible = !IsLabelVisible;
        }

        #region Constructor(s)

        public MainViewModel(IWindowService? windowService = null)
        {
            _windowService = windowService ?? new WindowService(); // fallback if null

            IsButtonEnabled = true;
            IsLabelVisible = false;
            IsSpreadsheetLabelVisible = false;

            ToggleVisibilityCommand = new RelayCommand(ToggleVisibility);
            OpenJsonModalCommand = new RelayCommand(OpenJsonModal);

            ShowUavPathCommand = new RelayCommand(async(parameter) =>
            {
                _windowService.ShowUavPathModal();
            });

            // Other logic...
            LoadAllCustomers();
            LoadingStatus = "Simulate asynchronous data loading with a delay";

            LoadDataCommand = new RelayCommand(async (parameter) =>
            {
                await LoadDataAsync();
            });

            _weatherService = new WeatherService();
            GetWeatherCommand = new RelayCommand<string>(async (location) => await GetWeatherAsync());
        }

        #endregion

        #region private methods
        private WeatherService _weatherService;

        private async Task GetWeatherAsync()
        {
            if (!string.IsNullOrWhiteSpace(Location))
            {
                // WeatherService API method
                WeatherInfo = await _weatherService.GetWeatherAsync(Location);
            }
        }

        private async Task LoadDataAsync()
        {
            // Show loading status
            LoadingStatus = "Loading data...";

            // Reset progress
            Progress = 0;

            // Offload the synchronous part of the operation to a background thread
            await Task.Run(() =>
            {
                // Simulate synchronous data loading with a delay
                for (int i = 0; i <= 100; i++)
                {
                    Thread.Sleep(50); // Simulating a 50ms delay

                    // Update progress
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Progress = i;
                    });
                }
            });

            // Set progress to maximum
            Progress = 100;

            // Update loading status
            LoadingStatus = "Data loaded!";
        }

        private void LoadAllCustomers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT FirstName, LastName, CompanyName FROM SalesLT.Customer";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        DataTable dataTable = new DataTable();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);

                            AllCustomers = new ObservableCollection<Customer>();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                AllCustomers.Add(new Customer
                                {
                                    FirstName = row["FirstName"].ToString(),
                                    LastName = row["LastName"].ToString(),
                                    CompanyName = row["CompanyName"].ToString()
                                });
                            }
                        }
                    }
                }

                // Initially populate the grid with all customers
                Customers = AllCustomers;
            }
            catch (SqlException sqlEx)
            {
                // Log the error details to the Output Window for developers or admins
                LogError(sqlEx);

                // Inform the developer about the error in the Output Window
                Debug.WriteLine($"SQL Exception: {sqlEx}");
            }
            catch (Exception ex)
            {
                // Log any unexpected errors to the Output Window
                LogError(ex);

                // Inform the developer about the unexpected error in the Output Window
                Debug.WriteLine($"Unexpected Error: {ex}");
            }
        }

        private void FilterCustomers()
        {
            if (string.IsNullOrEmpty(SearchLastName) || SearchLastName == "Data")
            {
                // If TextBox is empty, show all customers
                Customers = AllCustomers;
            }
            else
            {
                Customers = new ObservableCollection<Customer>(AllCustomers.Where(c => c.LastName.Contains(SearchLastName)));
            }
        }

        private void OpenJsonModal(object parameter)
        {
            // Open the modal window
            var jsonModal = new JsonModalWindow();
            jsonModal.ShowDialog();
        }

        private void LogError(Exception ex)
        {
            Debug.WriteLine($"Error Message: {ex.Message}");
            Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Debug.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
                Debug.WriteLine($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
            }
        }

        #endregion private methods
    }
}

