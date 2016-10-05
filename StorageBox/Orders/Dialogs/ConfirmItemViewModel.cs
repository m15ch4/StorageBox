using Caliburn.Micro;
using StorageBox.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Orders.Dialogs
{
    public class ConfirmItemViewModel : Screen, IDialog
    {
        

        public ConfirmItemViewModel()
        {
            
        }


        public void ItemNOTOK()
        {
            TryClose(false);
        }

        public void ItemOK()
        {
            TryClose(true);
        }
    }
}
