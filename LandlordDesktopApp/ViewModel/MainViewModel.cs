using GalaSoft.MvvmLight;
using LandlordDesktopApp.Model;
using System;

namespace LandlordDesktopApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        private LandlordsViewModel _landlordsViewModel;
        public LandlordsViewModel Landlords
        {
            get { return _landlordsViewModel; }
            set { _landlordsViewModel = value; }
        }

        private PropertiesViewModel _propertiesViewModel;
        public PropertiesViewModel Properties
        {
            get { return _propertiesViewModel; }
            set { _propertiesViewModel = value; }
        }

        private PropertyViewModel _propertyViewModel;
        public PropertyViewModel Property
        {
            get { return _propertyViewModel; }
            set { _propertyViewModel = value; }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;

            _landlordsViewModel = new LandlordsViewModel(_dataService);
            _propertiesViewModel = new PropertiesViewModel(_dataService, _landlordsViewModel);
            _propertyViewModel = new PropertyViewModel(_dataService, _landlordsViewModel, _propertiesViewModel);
        }
    }
}