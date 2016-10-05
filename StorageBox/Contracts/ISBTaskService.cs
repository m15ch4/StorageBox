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
        BindableCollection<SBTask> GetAll();
        BindableCollection<SBTask> CreateSBTasks(BindableCollection<WishListItem> orderQueue, IBoxService boxService);
        void SetCompleted(SBTask sbtask);
        void SetRunning(SBTask sbtask);
        void SetFailed(SBTask sbtask);
        void SetValid(SBTask sbtask, bool isvalid);
    }
}
