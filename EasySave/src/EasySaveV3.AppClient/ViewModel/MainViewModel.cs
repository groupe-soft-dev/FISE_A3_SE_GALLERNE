using EasySaveV3.AppClient.Commands;
using EasySaveV3.AppClient.Interfaces;
using EasySaveV3.AppClient.Notifiers;
using EasySaveV3.AppClient.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace EasySaveV3.AppClient.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IMessageBoxNotifier _notifier;

        public ObservableCollection<Core.Model.BackUpJob> JobsList { get; }

        public ICommand SettingsCommand { get; }
        public ICommand ShowLogsCommand { get; }
        public ICommand CreateJobCommand { get; }
        public ICommand ModifyJobCommand { get; }
        public ICommand DeleteJobCommand { get; }
        public ICommand ExecuteSelectedJobsCommand { get; }
        public ICommand MonitoringCommand { get; }


        private bool _areAllSelected;
        public bool AreAllSelected
        {
            get => _areAllSelected;
            set
            {
                if (_areAllSelected != value)
                {
                    _areAllSelected = value;
                    foreach (var job in JobsList)
                        job.IsSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel(INavigationService navigation, IMessageBoxNotifier notifier, SocketClientService socketClient) 
            : base(socketClient)
        {
            _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
            _navigationService = navigation;
            LoadAvailableLanguagesAsync();
            LoadCurrentLanguageAsync();
            LoadJobsFromServerAsync();

            ExecuteSelectedJobsCommand = new RelayCommand(_ => ExecuteSelectedJobs());
            ShowLogsCommand = new RelayCommand(_ => ShowLogs());
            SettingsCommand = new RelayCommand(_ => Settings());
            CreateJobCommand = new RelayCommand(_ => CreateJob());
            ModifyJobCommand = new RelayCommand(job =>
            {
                //if (job is Core.Model.BackUpJob backupJob)
                    //AlterJob(backupJob);
            });
            DeleteJobCommand = new RelayCommand(job =>
            {
               // if (job is Core.Model.BackUpJob backupJob)
                //    DeleteJob(backupJob);
            });
            MonitoringCommand = new RelayCommand(job =>
            { 
                if (job is Core.Model.BackUpJob backupJob) 
                    MonitoringJob(backupJob);
            });
        }

        public async Task LoadCurrentLanguageAsync()
        {
            string json = await _socketClient.SendCommandAsync("GET_CURRENT_LANGUAGE", null);
            CurrentLanguage = JsonSerializer.Deserialize<string>(json);
            _notifier.ShowSuccess($"Current Language : {CurrentLanguage}");
        }

        public async Task LoadAvailableLanguagesAsync()
        {
            string json = await _socketClient.SendCommandAsync("GET_AVAILABLE_LANGUAGES", null);
            AvailableLanguages = JsonSerializer.Deserialize<IReadOnlyList<string>>(json);
            if (AvailableLanguages != null)
            {
                _notifier.ShowSuccess($"Available languages: {string.Join(", ", AvailableLanguages)}");
            }
            else
            {
                _notifier.ShowError("Failed to parse available languages.");
            }
        }


        private void MonitoringJob(Core.Model.BackUpJob job)
        {
            if (job == null)
                return;
            MonitoringViewModel viewModel = new MonitoringViewModel(_navigationService, _notifier, _socketClient);
            viewModel.LoadFromExistingJob(job);
            _navigationService.NavigateToMonitoring(viewModel);
        }

        private async void ExecuteSelectedJobs()
        {
            var selectedJobs = GetSelectedJobs();

            try
            {
                await _socketClient.SendCommandAsync("EXECUTE_SELECTED_JOBS", selectedJobs);
                _notifier.ShowSuccess("Command sent to server.");
            }
            catch (Exception ex)
            {
                _notifier.ShowError($"Failed to send job to server: {ex.Message}");
            }
        }

        
        public List<Core.Model.BackUpJob> GetSelectedJobs()
        {
            //Core.Model
            List<Core.Model.BackUpJob> selectedJobs = JobsList.Where(job => job.IsSelected).ToList();
            _notifier.ShowSuccess($"{this["job_selected"]} {string.Join(", ", selectedJobs.Select(j => j.Id))}");

            return selectedJobs;


        }


        public async Task LoadJobsFromServerAsync()
        {
            string json = await _socketClient.SendCommandAsync("GET_JOBS", null);

            var jobs = JsonSerializer.Deserialize<List<Core.Model.BackUpJob>>(json);
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                JobsList.Clear();
                foreach (var job in jobs)
                {
                    JobsList.Add(job);
                    _notifier.ShowSuccess($"{job.Id}");
                }
            });
        }


        private void Settings()
        {
            _navigationService.NavigateToSettings();
            _navigationService.CloseMenu();
        }

        private async void ShowLogs()
        {
            try
            {
                await _socketClient.SendCommandAsync("SHOW_LOGS", null);
            }
            catch (Exception ex) 
            {
                _notifier.ShowError(ex.ToString());
            }
        }

        private void CreateJob()
        {
            /*
            BackUpViewModel viewModel = new BackUpViewModel(_navigationService, _notifier)
            {
                IsEditMode = false,
            };

            _navigationService.NavigateToBackUp(viewModel);
            _navigationService.CloseMenu();*/
        }
        /*
        private void AlterJob(Core.Model.BackUpJob selectedJob)
        {
            BackUpViewModel viewModel = new BackUpViewModel(_navigationService, _notifier)
            {
                IsEditMode = true
            };
            viewModel.LoadFromExistingJob(selectedJob);

            _navigationService.NavigateToBackUp(viewModel);
            _navigationService.CloseMenu();
        }

        private void DeleteJob(Core.Model.BackUpJob selectedJob)
        {
            
            if (_notifier.ShowWarning($"{this["delete_job"]}{selectedJob.Id}. {selectedJob.Name}"))
            {
                try
                {
                    selectedJob.DeleteJob();
                    JobsList.Remove(selectedJob);
                    //RefreshJobs();
                }
                catch (Exception ex)
                {
                    _notifier.ShowError(ex.Message);
                }
            }            

        }
        */

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private IReadOnlyList<string> _availableLanguages;
        public IReadOnlyList<string> AvailableLanguages
        {
            get => _availableLanguages;
            private set
            {
                if (_availableLanguages != value)
                {
                    _availableLanguages = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _currentLanguage;
        public string CurrentLanguage
        {
            get => _currentLanguage;
            private set => SetProperty(ref _currentLanguage, value);
        }



        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged();

                    // Changer la langue et recharger les traductions
                    _ = SetCurrentLanguageAsync(value);
                }
            }
        }


        // Méthode publique pour changer la langue (à appeler depuis la vue ou le contrôleur)
        public async Task SetCurrentLanguageAsync(string newLanguage)
        {
            if (_currentLanguage == newLanguage) return;

            CurrentLanguage = newLanguage; // modifie la propriété, déclenche OnPropertyChanged

            // Appelle le chargement des traductions (async)
            await LoadTranslationsAsync();
            OnPropertyChanged("Item[]");


            // Affiche la notification uniquement quand les traductions sont bien chargées
            _notifier.ShowSuccess($"{this["settings"]}");
        }


    }
}