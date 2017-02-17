using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StorageBox.Additions.ViewModels
{
    public class AddCategoryViewModel : Screen, IAddition
    {
        private ICategoryService _categoryService;
        private BindableCollection<Category> _categories;
        private string _categoryName;
        private Category _categoriesSelecteItem;

        protected override void OnActivate()
        {
            base.OnActivate();
            CategoryName = "";
        }

        public AddCategoryViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _categories = _categoryService.GetAll();
        }

        public void AddCategory(string categoryName)
        {
            try
            {
                _categoryService.Add(categoryName);
                Categories = _categoryService.GetAll();
                CategoryName = null;
                MessageBox.Show("Utworzono nową kategorię: " + categoryName, "Utworzono", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                MessageBox.Show("Nie dodano nowego produktu. Spróbuj ponownie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CanAddCategory
        {
            get { return CategoryName != ""; }
        }

        public BindableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                NotifyOfPropertyChange(() => Categories);
            }
        }

        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
                NotifyOfPropertyChange(() => CategoryName);
                NotifyOfPropertyChange(() => CanAddCategory);
            }
        }

        public Category CategoriesSelectedItem
        {
            get { return _categoriesSelecteItem; }
            set
            {
                _categoriesSelecteItem = value;
                NotifyOfPropertyChange(() => CategoriesSelectedItem);
                NotifyOfPropertyChange(() => CanDeleteCategory);
            }
        }

        public bool CanDeleteCategory
        {
            get { return (CategoriesSelectedItem != null);  }
        }

        public void DeleteCategory(Category category)
        {
            if (_categoriesSelecteItem != null)
            {
                try
                {
                    var categoryName = _categoriesSelecteItem.CategoryName;
                    _categoryService.Delete(_categoriesSelecteItem);
                    Categories.Remove(_categoriesSelecteItem);
                    MessageBox.Show("Usunięto wybraną kategorię: " + categoryName, "Usunięto", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    MessageBox.Show("Nie usunięto kategorii. Sprawdź czy nie istnieją obiekty zależne od wybranej kategorii.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
