namespace StorageBox.Shell.ViewModels
{
    using StorageBox.Framework;
    using System.Collections.Generic;
    using System.Composition;
    using System.Linq;

    public class ShellViewModel : Caliburn.Micro.Conductor<IWorkspace>.Collection.OneActive, IShell {



        [ImportingConstructor]
        public ShellViewModel(IEnumerable<IWorkspace> workspaces)
        {
            DisplayName = "StorageBox ver. 2.0";

            Items.AddRange(workspaces);
            ActivateItem(Items[0]);
        }

        public void Orders()
        {
            ActivateItem(Items[0]);
        }

        public void Additions()
        {
            ActivateItem(Items[1]);
        }

    }
}