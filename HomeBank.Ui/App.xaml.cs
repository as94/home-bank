using HomeBank.Data.Memory.Store;
using HomeBank.Domain.DomainModel;
using HomeBank.Domain.Infrastructure;
using HomeBank.Presentation.ViewModels;
using HomeBank.Ui.Views;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HomeBank.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel _mainViewModel;
        private MainView _mainView;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var categories = new List<Category>
            {
                new Category(Guid.NewGuid(), "Work", "Programmer", Domain.Enums.CategoryType.Income),
                new Category(Guid.NewGuid(), "Business", "Store", Domain.Enums.CategoryType.Income),

                new Category(Guid.NewGuid(), "Products", "Apple", Domain.Enums.CategoryType.Expenditure),
                new Category(Guid.NewGuid(), "Products", "Orange", Domain.Enums.CategoryType.Expenditure),
            };

            var transactions = new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2018, 2, 3), 10, categories[0]),
                new Transaction(Guid.NewGuid(), new DateTime(2018, 2, 4), 30, categories[1]),
                new Transaction(Guid.NewGuid(), new DateTime(2018, 3, 20), 50, categories[2]),
                new Transaction(Guid.NewGuid(), new DateTime(2018, 6, 9), 1000, categories[3]),
                new Transaction(Guid.NewGuid(), new DateTime(2018, 6, 10), 100, categories[3]),
            };

            var categoryRepository = new CategoryRepository(categories);
            var transactionRepository = new TransactionRepository(transactions);

            if (_mainView == null)
            {
                _mainViewModel = new MainViewModel(categoryRepository, transactionRepository);
                _mainView = new MainView(_mainViewModel);

                _mainView.Show();
            }
        }
    }
}
