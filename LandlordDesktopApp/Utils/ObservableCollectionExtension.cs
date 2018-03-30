using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LandlordDesktopApp.Utils
{
    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source is null");

            return new ObservableCollection<T>(source);
        }
    }
}
