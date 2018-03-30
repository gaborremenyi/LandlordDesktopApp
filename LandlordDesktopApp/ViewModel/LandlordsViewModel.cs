using LandlordDesktopApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LandlordDesktopApp.ViewModel
{
    public delegate void LandlordChangedEventHandler(Landlord landlord, EventArgs e);

    public class LandlordsViewModel
    {
        private readonly IDataService _dataService;

        private ObservableCollection<Landlord> _landlords;
        public IEnumerable<Landlord> Landlords
        {
            get { return _landlords; }
        }

        // event for selection change
        public event LandlordChangedEventHandler OnLandlordChanged;

        private Landlord _selectedLandlord;
        public Landlord SelectedLandlord
        {
            get
            {
                return _selectedLandlord;
            }
            set
            {
                _selectedLandlord = value;

                // fire selection change event
                OnLandlordChanged(_selectedLandlord, new EventArgs());
            }
        }

        public LandlordsViewModel(IDataService dataService)
        {
            _dataService = dataService;

            RefreshGrid();
        }

        internal void RefreshGrid()
        {
            if (_dataService != null)
            {
                _dataService.GetLandlords(
                    (landlords, exception) =>
                    {
                        if (landlords != null)
                            _landlords = landlords;

                    // TODO error handling
                });
            }
        }
    }
}
