using Caliburn.Micro;
using StorageBox.Contracts;
using StorageBox.Framework;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Additions.ViewModels
{
    public class AddCategoryViewModel : Screen, IAddition
    {
        private ICategoryService _categoryService;
        private BindableCollection<Category> _categories;
        private string _categoryName;

        public AddCategoryViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _categories = _categoryService.GetAll();
        }

        public void AddCategory(string categoryName)
        {
            _categoryService.Add(categoryName);
            Categories = _categoryService.GetAll();
            CategoryName = null;
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
            }
        }
    }
}
