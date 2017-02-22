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
    public class BoxesViewModel : Screen, IAddition
    {
        private IBoxService _boxService;
        private BindableCollection<BoxSize> _boxSizes;
        private BoxSize _boxSizesSelectedItem;
        private string _boxSizeName;
        private string _row;
        private string _column;
        private BindableCollection<Box> _boxes;
        private Box _boxesSelectedItem;

        public BoxesViewModel(IBoxService boxService)
        {
            _boxService = boxService;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Boxes = _boxService.GetAll();
            BoxSizes = _boxService.GetAllBoxSizes();
            Row = "";
            Column = "";
        }

        public BindableCollection<BoxSize> BoxSizes
        {
            get { return _boxSizes; }
            set
            {
                _boxSizes = value;
                NotifyOfPropertyChange(() => BoxSizes);
            }
        }

        public BoxSize BoxSizesSelectedItem
        {
            get { return _boxSizesSelectedItem; }
            set
            {
                _boxSizesSelectedItem = value;
                NotifyOfPropertyChange(() => BoxSizesSelectedItem);
                NotifyOfPropertyChange(() => CanCreateBox);
            }
        }

        public string Row
        {
            get { return _row; }
            set
            {
                _row = value;
                NotifyOfPropertyChange(() => Row);
                NotifyOfPropertyChange(() => CanCreateBox);
            }
        }

        public string Column
        {
            get { return _column; }
            set
            {
                _column = value;
                NotifyOfPropertyChange(() => Column);
                NotifyOfPropertyChange(() => CanCreateBox);
            }
        }

        public void CreateBox()
        {
            byte r = Convert.ToByte(Row);
            byte c = Convert.ToByte(Column);

            try
            {
                if (_boxService.Get(Convert.ToByte(Row), Convert.ToByte(Column)) == null)
                {
                    _boxService.CreateBox(r, c, BoxSizesSelectedItem);
                    Boxes = _boxService.GetAll();
                    Row = "";
                    Column = "";
                    MessageBox.Show("Dodano nową skrzynkę.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                MessageBox.Show("Nie dodano nowej skrzynki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public bool CanCreateBox
        {
            get { return ((Row != "") && (Column != "") && (BoxSizesSelectedItem != null)); }
        }

        public string BoxSizeName
        {
            get { return _boxSizeName; }
            set
            {
                _boxSizeName = value;
                NotifyOfPropertyChange(() => BoxSizeName);
            }
        }

        public void CreateBoxSize()
        {
            if (BoxSizeName != "")
            {
                _boxService.CreateBoxSize(BoxSizeName);
                BoxSizeName = "";
                BoxSizes = _boxService.GetAllBoxSizes();
            }
        }

        public BindableCollection<Box> Boxes
        {
            get { return _boxes; }
            set
            {
                _boxes = value;
                NotifyOfPropertyChange(() => Boxes);
            }
        }

        public Box BoxesSelectedItem
        {
            get { return _boxesSelectedItem; }
            set
            {
                _boxesSelectedItem = value;
                NotifyOfPropertyChange(() => BoxesSelectedItem);
                NotifyOfPropertyChange(() => CanRemoveBox);
            }
        }

        public bool CanRemoveBox
        {
            get { return (BoxesSelectedItem != null); }
        }

        public void RemoveBox(Box box)
        {
            try
            {
                _boxService.Remove(box);
                Boxes = _boxService.GetAll();
                MessageBox.Show("Usunięto wybraną skrzynkę.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                MessageBox.Show("Nie usunięto skrzynki. Opróżnij skrzynkę i spróbuj ponownie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
