﻿using HomeBank.Domain.Infrastructure;
using HomeBank.Presentaion.EventArguments;
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
                }
            };

            SelectedChildren = Childrens[0];
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
