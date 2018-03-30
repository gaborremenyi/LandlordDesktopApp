using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LandlordDesktopApp.Model
{
    public interface IDataService
    {
        void GetLandlords(Action<ObservableCollection<Landlord>, Exception> callback);

        void GetProperties(int? landlordId, Action<ObservableCollection<Property>, Exception> callback);

        void SaveProperty(Property property, Action<bool, Exception> callback);
    }
}
