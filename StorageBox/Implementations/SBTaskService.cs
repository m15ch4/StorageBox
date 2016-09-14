using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using StorageBox.Models;

namespace StorageBox.Implementations
{
    public class SBTaskService :  ISBTaskService
    {
        private MyDBContext _context;

        public SBTaskService(MyDBContext context) 
        {
            _context = context;
        }



        public BindableCollection<SBTask> CreateSBTasks(BindableCollection<WishListItem> orderQueue, IBoxService boxService)
        {
            BindableCollection<SBTask> sbTasks = new BindableCollection<SBTask>();

            foreach (WishListItem wishListItem in orderQueue)
            {
                IEnumerable<Box> boxes = boxService.Get(wishListItem.ProductVariant.ProductSKU);
                for (int i = 0; i < wishListItem.Count; i++)
                {
                    SBTask sbTask = new SBTask()
                    {
                        SBTaskStatus = SBTaskStatus.Queued,
                        SBTaskType = SBTaskType.Order,
                        DateAdded = DateTime.Now,
                        ProductVariant = wishListItem.ProductVariant,
                        ProductSKU = wishListItem.ProductVariant.ProductSKU,
                        // TODO: zabezpieczenie czy nie za dużo lub mało skrzynek
                        Box = boxes.ElementAt(i)
                    };

                    _context.SBTasks.Add(sbTask);
                    _context.SaveChanges();
                    sbTasks.Add(sbTask);
                }
            }

            return sbTasks;
        }

        public void processSBTask(SBTask task)
        {

        }

    }
}
