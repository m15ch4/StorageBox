namespace StorageBox.Additions.ViewModels
{
    using Caliburn.Micro;
    using Framework;
    using System.Collections.Generic;

    public class AdditionsViewModel : Conductor<object>.Collection.OneActive, IWorkspace
    {
        public AdditionsViewModel(IEnumerable<IAddition> additions)
        {
            Items.AddRange(additions);
            ActivateItem(Items[0]);
        }

        public void Categories()
        {
            ActivateItem(Items[0]);
        }

        public void Products()
        {
            ActivateItem(Items[1]);
        }

        public void Options()
        {
            ActivateItem(Items[2]);
        }

        public void SKUs()
        {
            ActivateItem(Items[3]);
        }

        public void OptionValues()
        {
            ActivateItem(Items[4]);
        }

        public void SKUValues()
        {
            ActivateItem(Items[5]);
        }

        public void Boxes()
        {
            ActivateItem(Items[6]);
        }

        public void FillBoxes()
        {
            ActivateItem(Items[7]);
        }

        public void Users()
        {
            ActivateItem(Items[8]);
        }
    }
}
