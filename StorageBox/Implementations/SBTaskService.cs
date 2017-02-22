using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using StorageBox.Models;
using System.Windows.Media.Imaging;

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
                IEnumerable<Box> boxes = boxService.Get(wishListItem.ProductSKU);
                for (int i = 0; i < wishListItem.Count; i++)
                {
                    SBTask sbTask = new SBTask()
                    {
                        SBTaskStatus = SBTaskStatus.Queued,
                        SBTaskType = SBTaskType.Order,
                        DateAdded = DateTime.Now,
                        ProductSKU = wishListItem.ProductSKU,
                        Box = boxes.ElementAt(i),
                        SBUser = UserSession.sbuser,
                        CategoryName = wishListItem.ProductSKU.Product.Category.CategoryName,
                        ProductName = wishListItem.ProductSKU.Product.ProductName,
                        SKU = wishListItem.ProductSKU.Sku,
                        UserName = UserSession.sbuser != null ? UserSession.sbuser.UserName : ""
                    };

                    _context.SBTasks.Add(sbTask);
                    _context.SaveChanges();
                    sbTasks.Add(sbTask);
                }
            }

            return sbTasks;
        }

        public BindableCollection<SBTask> GetAll()
        {
            List<SBTask> sbtasks = _context.SBTasks.ToList();
            return new BindableCollection<SBTask>(sbtasks);
        }

        public void processSBTask(SBTask task)
        {

        }


        public void SetCompleted(SBTask sbtask)
        {
            sbtask.SetStatus = SBTaskStatus.Completed;
            sbtask.DateEnded = DateTime.Now;
            _context.SaveChanges();
        }

        public void SetFailed(SBTask sbtask)
        {
            sbtask.SetStatus = SBTaskStatus.Failed;
            sbtask.DateEnded = DateTime.Now;
            _context.SaveChanges();
        }

        public void SetRunning(SBTask sbtask)
        {
            sbtask.SetStatus = SBTaskStatus.Running;
            sbtask.DateStarted = DateTime.Now;
            _context.SaveChanges();
        }

        public void SetValid(SBTask sbtask, bool isvalid)
        {
            sbtask.IsValid = isvalid;
            _context.SaveChanges();
        }

        public List<ProductSKU> taskedSKUs(BindableCollection<SBTask> sbtasks)
        {
            List<ProductSKU> result = new List<ProductSKU>();
            foreach (SBTask sbtask in sbtasks)
            {
                if (!result.Contains(sbtask.ProductSKU))
                {
                    result.Add(sbtask.ProductSKU);
                }
            }
            return result;
        }
    }
}
