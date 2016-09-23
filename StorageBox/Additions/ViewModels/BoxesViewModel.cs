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
    public class BoxesViewModel : Screen, IAddition
    {
        private IBoxService _boxService;
        private BindableCollection<BoxSize> _boxSizes;
        private BoxSize _boxSizesSelectedItem;
        private string _boxSizeName;
        private string _row;
        private string _column;
        private BindableCollection<Box> _boxes;

        public BoxesViewModel(IBoxService boxService)
        {
            _boxService = boxService;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Boxes = _boxService.GetAll();
            BoxSizes = _boxService.GetAllBoxSizes();
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
            }
        }

        public string Row
        {
            get { return _row; }
            set
            {
                _row = value;
                NotifyOfPropertyChange(() => Row);
            }
        }

        public string Column
        {
            get { return _column; }
            set
            {
                _column = value;
                NotifyOfPropertyChange(() => Column);
            }
        }

        public void CreateBox()
        {
            Int16 r = Convert.ToInt16(Row);
            Int16 c = Convert.ToInt16(Column);

            if (_boxService.Get(Convert.ToInt32(Row), Convert.ToInt32(Column)) == null)
            {
                _boxService.CreateBox(r, c, BoxSizesSelectedItem);
                Boxes = _boxService.GetAll();
            }

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

        public void RemoveBox(Box box)
        {
            _boxService.Remove(box);
            Boxes = _boxService.GetAll();
        }
    }
}
