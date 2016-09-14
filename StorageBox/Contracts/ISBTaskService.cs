using Caliburn.Micro;
using StorageBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Contracts
{
    public interface ISBTaskService
    {
        BindableCollection<SBTask> CreateSBTasks(BindableCollection<WishListItem> orderQueue, IBoxService boxService);
    }
}
