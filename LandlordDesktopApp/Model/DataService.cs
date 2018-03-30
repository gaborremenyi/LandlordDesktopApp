using System;
using System.Linq;
using System.Collections.Generic;
using LandlordDesktopApp.Utils;
using System.Collections.ObjectModel;

namespace LandlordDesktopApp.Model
{
    public class DataService : IDataService, IDisposable
    {
        dgcodetest_GREntities1 databaseContext;

        public DataService()
        {
            databaseContext = new dgcodetest_GREntities1();
        }

        public void GetLandlords(Action<ObservableCollection<Landlord>, Exception> callback)
        {
            try
            {
                var landlords = databaseContext.Landlords.Select(x => x).ToObservableCollection();

                callback(landlords, null);
            }
            catch (Exception e) { callback(null, e); }
        }

        public void GetProperties(int? landlordId, Action<ObservableCollection<Property>, Exception> callback)
        {
            try
            {
                var properties = (from p in databaseContext.Properties
                                  where (landlordId == null || p.LandlordId == landlordId)
                                  select p).ToObservableCollection();

                callback(properties, null);
            }
            catch (Exception e) { callback(null, e); }
        }

        public void SaveProperty(Property property, Action<bool, Exception> callback)
        {
            try
            {
                var existingProperty = (from p in databaseContext.Properties
                                        where p.PropertyId == property.PropertyId
                                        select p).FirstOrDefault();

                if (existingProperty == null)
                    databaseContext.Properties.Add(property);
                else
                    databaseContext.Entry(existingProperty).CurrentValues.SetValues(property);

                databaseContext.SaveChanges();

                callback(true, null);
            }
            catch (Exception e) { callback(false, e); }
        }

        public void Dispose()
        {
            if (databaseContext != null)
                databaseContext.Dispose();
        }

        ~DataService()
        {
            Dispose();
        }
    }
}