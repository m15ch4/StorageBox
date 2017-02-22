using Caliburn.Micro;
using StorageBox.Models;
using System.Collections.Generic;

namespace StorageBox.Contracts
{
    public interface IEMailService
    {
        void sendAvailabilityWarning(List<ProductSKU> underThresholdSKU);
    }
}
