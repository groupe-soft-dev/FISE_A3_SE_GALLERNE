using BackUp.Model;
using System.ComponentModel;

namespace BackUp.ViewModel
{
    public class ManageBackUpServices
    {
        public ManageBackUpServices()
        {

        }
    }
    /*
    
    public class ManageBackUpServicesViewModel : IManageBackUpServices, INotifyPropertyChanged
    {
        // 1) Propri�t�s expos�es (contrat de l�interface)
        public ICommand CreateBackUpCommand { get; }
        public ICommand AlterBackUpCommand { get; }
        public ICommand DeleteBackUpCommand { get; }
        public ICommand SelectBackUpCommand { get; }
        public ICommand SelectAllJobsCommand { get; }

        // 2) Champs priv�s pour stocker les RelayCommand (optionnel mais pratique pour RaiseCanExecuteChanged)
        private readonly RelayCommand _createCmd;
        private readonly RelayCommand _alterCmd;
        private readonly RelayCommand _deleteCmd;
        private readonly RelayCommand _selectCmd;
        private readonly RelayCommand _selectAllCmd;

        // Un service m�tier inject� pour r�ellement r�aliser les backups
        private readonly IJobs _jobsModel;

        // Une propri�t� pour la sauvegarde s�lectionn�e
        private BackUpJob _selectedBackUp;
        public BackUpJob SelectedBackUp
        {
            get => _selectedBackUp;
            set
            {
                if (_selectedBackUp != value)
                {
                    _selectedBackUp = value;
                    OnPropertyChanged(nameof(SelectedBackUp));
                    // chaque fois qu�on change de s�lection, on r��value CanExecute
                    _alterCmd.RaiseCanExecuteChanged();
                    _deleteCmd.RaiseCanExecuteChanged();
                }
            }
        }

        public ManageBackUpServices(IJobs service)
        {
            _jobsModel = service;

            // 3) Cr�ation des RelayCommand en pointant sur les m�thodes
            _createCmd = new RelayCommand(
                _ => CreateBackUp(),
                _ => CanCreateBackUp()
            );
            _alterCmd = new RelayCommand(
                _ => AlterBackUp(),
                _ => CanAlterBackUp()
            );
            _deleteCmd = new RelayCommand(
                _ => DeleteBackUp(),
                _ => CanDeleteBackUp()
            );
            _selectCmd = new RelayCommand(
                param => SelectBackUp((BackUpJob)param),
                param => param is BackUpJob
            );

            // 1) On affecte nos champs RelayCommand aux propri�t�s ICommand
            CreateBackUpCommand = _createCmd;
            AlterBackUpCommand = _alterCmd;
            DeleteBackUpCommand = _deleteCmd;
            SelectBackUpCommand = _selectCmd;
        }

        // 4) M�thodes appel�es par Execute
        private void CreateBackUp()
        {
            var newBackup = _jobsModel.Create();
            // par ex. Actualiser une liste, etc.
        }
        private bool CanCreateBackUp() => true;  // toujours possible ici

        private void AlterBackUp()
        {
            if (SelectedBackUp != null)
                _jobsModel.Alter(SelectedBackUp);
        }
        private bool CanAlterBackUp() => SelectedBackUp != null;

        private void DeleteBackUp()
        {
            if (SelectedBackUp != null)
                _jobsModel.Delete(SelectedBackUp.Id);
        }
        private bool CanDeleteBackUp() => SelectedBackUp != null;

        private void SelectBackUp(BackUpJob backup)
        {
            SelectedBackUp = backup;
        }

        // INotifyPropertyChanged pour les bindings
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }*/
}
