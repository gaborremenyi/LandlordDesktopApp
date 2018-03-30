using System;
using System.Collections.Generic;
using LandlordDesktopApp.Model;
using LandlordDesktopApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModels.ViewModel;
using LandlordDesktopApp.Utils;
using System.Collections.ObjectModel;

namespace LandlordDesktopApp.Test
{
    [TestClass]
    public class UnitTest
    {
        IDataService dataService = null;
        ObservableObject observableObject;

        MainViewModel mainViewModel;

        public UnitTest()
        {
            observableObject = new ObservableObject();
            mainViewModel = new MainViewModel(dataService);
        }

        [TestMethod]
        public void PropertyChangedTest()
        {
            observableObject.PropertyChanged += (s, e) =>
            {
                var landlord = s as Landlord;

                Assert.IsNotNull(landlord);
                Assert.AreEqual(1, landlord.LandlordId);
            };

            mainViewModel.Property.SelectedLandlord = new Landlord() { LandlordId = 1 };
        }

        [TestMethod]
        public void ToObservableCollectionTest()
        {
            var list1 = new List<int>() { 1, 2, 3 }.ToObservableCollection();

            Assert.IsInstanceOfType(list1, typeof(ObservableCollection<int>));
            Assert.AreEqual(3, list1.Count);
        }

        [TestCleanup]
        public void CleanUp()
        {
        }
    }
}
