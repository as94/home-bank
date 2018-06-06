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
        private ICategoryRepository _categoryRepository = new CategoryRepository(new List<Category>
        {
            new Category(Guid.NewGuid(), "Work", "Programmer", Domain.Enums.CategoryType.Income),
            new Category(Guid.NewGuid(), "Business", "Store", Domain.Enums.CategoryType.Income),

            new Category(Guid.NewGuid(), "Products", "Apple", Domain.Enums.CategoryType.Expenditure),
            new Category(Guid.NewGuid(), "Products", "Orange", Domain.Enums.CategoryType.Expenditure),
        });

        private MainViewModel _mainViewModel;
        private MainView _mainView;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (_mainView == null)
            {
                _mainViewModel = new MainViewModel(_categoryRepository);
                _mainView = new MainView(_mainViewModel);

                _mainView.Show();
            }
        }
    }
}
