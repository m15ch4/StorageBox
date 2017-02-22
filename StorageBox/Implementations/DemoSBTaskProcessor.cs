using StorageBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageBox.Models;
using Caliburn.Micro;
using System.Threading;

namespace StorageBox.Implementations
{
    class DemoSBTaskProcessor : ISBTaskProcessor
    {
        private ISBTaskService _sbTaskService;
        private IEventAggregator _eventAggregator;

        public DemoSBTaskProcessor(ISBTaskService sbTaskService, IEventAggregator eventAggregator)
        {
            _sbTaskService = sbTaskService;
            _eventAggregator = eventAggregator;
        }

        public void process(SBTask sbtask)
        {
            _sbTaskService.SetRunning(sbtask);
            Thread.Sleep(2000);
            _eventAggregator.PublishOnUIThread(sbtask);
            _sbTaskService.SetCompleted(sbtask);
        }
    }
}
