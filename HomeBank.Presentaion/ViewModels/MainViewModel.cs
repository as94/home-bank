using HomeBank.Domain.Infrastructure;
using HomeBank.Presentaion.Infrastructure;
using HomeBank.Presentaion.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HomeBank.Presentation.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public override string ViewModelName => nameof(MainViewModel);

        public ObservableCollection<ViewModel> Childrens { get; set; }

        private ViewModel _selectedChildren;
        public ViewModel SelectedChildren
        {
            get => _selectedChildren;
            set
            {
                if (_selectedChildren == value) return;
                _selectedChildren = value;
                OnPropertyChanged();
            }
        }

        public ICategoryRepository CategoryRepository { get; }
        public ITransactionRepository TransactionRepository { get; }

        public MainViewModel(
            ViewModel[] childrens,
            ICategoryRepository categoryRepository,
            ITransactionRepository transactionRepository)
        {
            if (childrens == null)
            {
                throw new ArgumentNullException(nameof(childrens));
            }

            if (childrens.Length == 0)
            {
                throw new ArgumentException(nameof(childrens));
            }

            if (categoryRepository == null)
            {
                throw new ArgumentNullException(nameof(categoryRepository));
            }

            if (transactionRepository == null)
            {
                throw new ArgumentNullException(nameof(transactionRepository));
            }

            Childrens = new ObservableCollection<ViewModel>(childrens);
            SelectedChildren = Childrens[0];

            CategoryRepository = categoryRepository;
            TransactionRepository = transactionRepository;
        }

        private ICommand _accountShowCommand;
        public ICommand AccountShowCommand
        {
            get
            {
                return _accountShowCommand ?? (_accountShowCommand = new ActionCommand(vm =>
                {
                    SelectedChildren = Childrens.First(c => c.ViewModelName == nameof(AccountViewModel));
                }));
            }
        }

        private ICommand _settingsShowCommand;
        public ICommand SettingsShowCommand
        {
            get
            {
                return _settingsShowCommand ?? (_settingsShowCommand = new ActionCommand(vm =>
                {
                    SelectedChildren = Childrens.First(c => c.ViewModelName == nameof(SettingsViewModel));
                }));
            }
        }

        private ICommand _menuItemShowCommand;
        public ICommand MenuItemShowCommand
        {
            get
            {
                return _menuItemShowCommand ?? (_menuItemShowCommand = new ActionCommand(vm =>
                {
                    var menuItemIdx = (int)vm;

                    SelectedChildren = Childrens[menuItemIdx];
                }));
            }
        }
    }
}
