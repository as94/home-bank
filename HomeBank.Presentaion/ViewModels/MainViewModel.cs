using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using HomeBank.Domain.Infrastructure;
using HomeBank.Presentaion.EventArguments;
using HomeBank.Presentaion.Infrastructure;

namespace HomeBank.Presentaion.ViewModels
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
            IEventBus eventBus,
            ViewModel[] childrens,
            ICategoryRepository categoryRepository,
            ITransactionRepository transactionRepository)
            : base(eventBus)
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

            CategoryRepository = categoryRepository;
            TransactionRepository = transactionRepository;

            EventBus.EventOccured += (type, args) =>
            {
                switch (type)
                {
                    case EventType.CategoryOperationExecuted:
                        {
                            var opArgs = args as CategoryOperationEventArgs;
                            if (opArgs != null && opArgs.Category.OperationType == Presentaion.Enums.OperationType.Remove)
                            {
                                return;
                            }
                            SelectedChildren = opArgs.Category;
                            break;
                        }

                    case EventType.CategoryFilterChanged:
                    case EventType.CategoryItemOperationExecuted:
                    case EventType.CategoryBackExecuted:
                        SelectedChildren = Childrens.First(c => c.ViewModelName == nameof(CategoryViewModel));
                        break;

                    case EventType.TransactionOperationExecuted:
                        {
                            var opArgs = args as TransactionOperationEventArgs;
                            if (opArgs != null && opArgs.Transaction.OperationType == Presentaion.Enums.OperationType.Remove)
                            {
                                return;
                            }
                            SelectedChildren = opArgs.Transaction;
                            break;
                        }

                    case EventType.TransactionFilterChanged:
                    case EventType.TransactionItemOperationExecuted:
                    case EventType.TransactionBackExecuted:
                        SelectedChildren = Childrens.First(c => c.ViewModelName == nameof(TransactionViewModel));
                        break;

                    case EventType.CategoryStatisticFilterChanged:
                        SelectedChildren = Childrens.First(c => c.ViewModelName == nameof(StatisticViewModel));
                        break;
                }
            };

            SelectedChildren = Childrens[0];
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
