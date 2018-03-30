using LandlordDesktopApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ViewModels.ViewModel;

namespace LandlordDesktopApp.ViewModel
{
    public delegate void PropertyChangedEventHandler(Property property, EventArgs e);

    public class PropertiesViewModel : ObservableObject
    {
        private readonly IDataService _dataService;

        private ObservableCollection<Property> _properties;
        public IEnumerable<Property> Properties
        {
            get { return _properties; }
        }

        // event for selection change
        public event PropertyChangedEventHandler OnPropertyChanged;

        private Property _selectedProperty;
        public Property SelectedProperty
        {
            get
            {
                return _selectedProperty;
            }
            set
            {
                _selectedProperty = value;

                // fire selection change event
                OnPropertyChanged(_selectedProperty, new EventArgs());
            }
        }

        public PropertiesViewModel(IDataService dataService, LandlordsViewModel landlordsViewModel)
        {
            _dataService = dataService;

            RefreshGrid();

            // subscribe landlord selection change event
            landlordsViewModel.OnLandlordChanged += LandlordsViewModel_OnLandlordChanged;
        }

        /// <summary>
        /// If the selection of the Landlords grid changes refresh Properties grid.
        /// </summary>
        /// <param name="landlord"></param>
        /// <param name="e"></param>
        private void LandlordsViewModel_OnLandlordChanged(Landlord landlord, EventArgs e)
        {
            if (landlord != null)
                RefreshGrid(landlord.LandlordId);
            else
                RefreshGrid();
        }

        /// <summary>
        /// Refreshes the Properties grid from database.
        /// </summary>
        /// <param name="landlordId">If supplied used as a filter.</param>
        internal void RefreshGrid(int? landlordId = null)
        {
            if (_dataService != null)
            {
                _dataService.GetProperties(
                landlordId,
                (properties, exception) =>
                {
                    if (properties != null)
                    {
                        _properties = properties;
                        RaisePropertyChangedEvent(nameof(Properties));
                    }
                    // TODO error handling
                });
            }
        }
    }
}
