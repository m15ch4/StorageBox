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
    }
}
