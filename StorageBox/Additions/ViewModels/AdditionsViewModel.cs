namespace StorageBox.Additions.ViewModels
{
    using Caliburn.Micro;
    using Framework;
    using System.Collections.Generic;

    public class AdditionsViewModel : Conductor<object>.Collection.OneActive, IWorkspace
    {
        private string _title;

        public AdditionsViewModel(IEnumerable<IAddition> additions)
        {
            Items.AddRange(additions);
            Categories();
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        public void Categories()
        {
            Title = "Kategorie produktów";
            ActivateItem(Items[0]);
        }

        public void Products()
        {
            Title = "Przedmioty";
            ActivateItem(Items[1]);
        }

        public void Options()
        {
            Title = "Parametry przedmiotów";
            ActivateItem(Items[2]);
        }

        public void SKUs()
        {
            Title = "SKU";
            ActivateItem(Items[3]);
        }

        public void OptionValues()
        {
            Title = "Wartości parametrów";
            ActivateItem(Items[4]);
        }

        public void SKUValues()
        {
            Title = "SKU <=> Wartości parametrów";
            ActivateItem(Items[5]);
        }

        public void Boxes()
        {
            Title = "Skrzynki";
            ActivateItem(Items[6]);
        }

        public void FillBoxes()
        {
            Title = "Zawartość skrzynek";
            ActivateItem(Items[7]);
        }

        public void Users()
        {
            Title = "Użytkownicy";
            ActivateItem(Items[8]);
        }
    }
}
