using LandlordDesktopApp.Model;
using LandlordDesktopApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModels.ViewModel;

namespace LandlordDesktopApp.ViewModel
{
    public class PropertyViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private LandlordsViewModel _landlordsViewModel;
        private PropertiesViewModel _propertiesViewModel;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private ObservableCollection<Landlord> _landlords;
        public IEnumerable<Landlord> Landlords
        {
            get { return _landlords; }
        }

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

                RaisePropertyChangedEvent(nameof(SelectedLandlord));
            }
        }

        private Property _property;
        public Property Property
        {
            get { return _property; }
            set { _property = value; }
        }

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => SaveButtonClicked()));
            }
        }

        public PropertyViewModel(IDataService dataService, LandlordsViewModel landlordsViewModel, PropertiesViewModel propertiesViewModel)
        {
            _title = "Add Property";
            _dataService = dataService;
            _landlordsViewModel = landlordsViewModel;
            _propertiesViewModel = propertiesViewModel;

            _property = new Property() { AvailableFrom = DateTime.Now };

            if (_dataService != null)
            {
                _dataService.GetLandlords(
                    (landlords, exception) =>
                    {
                        if (landlords != null)
                            _landlords = landlords;
                    });
            }

            // subscribe landlord selection change event
            propertiesViewModel.OnPropertyChanged += PropertiesViewModel_OnPropertyChanged;
        }

        /// <summary>
        /// If the selection of the Properties grid changes refresh Property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="e"></param>
        private void PropertiesViewModel_OnPropertyChanged(Property property, EventArgs e)
        {
            if (property == null)
            {
                _title = "Add Property";
                _property = new Property()
                {
                    AvailableFrom = DateTime.Now,
                    LandlordId = 0
                };
            }
            else
            {
                _title = "Edit Property";
                _property = new Property()
                {
                    PropertyId = property.PropertyId,
                    Housenumber = property.Housenumber,
                    Street = property.Street,
                    Town = property.Town,
                    PostCode = property.PostCode,
                    Status = property.Status,
                    AvailableFrom = property.AvailableFrom,
                    LandlordId = property.LandlordId,
                    Landlord = property.Landlord
                };

                _selectedLandlord = _property.Landlord;
            }

            RaisePropertyChangedEvent(nameof(Title));
            RaisePropertyChangedEvent(nameof(Property));
            RaisePropertyChangedEvent(nameof(SelectedLandlord));
        }

        /// <summary>
        /// Save button action to trigger database insert or update.
        /// </summary>
        private void SaveButtonClicked()
        {
            if (_property != null)
            {
                _property.Landlord = _selectedLandlord;
                _property.LandlordId = _selectedLandlord.LandlordId;

                _dataService.SaveProperty(_property,
                (success, exception) =>
                {
                    if (success)
                    {
                        _landlordsViewModel.RefreshGrid();
                        _propertiesViewModel.RefreshGrid();
                        Clear();
                    }
                    // TODO error handling
                });
            }
        }

        /// <summary>
        /// Clears form.
        /// </summary>
        private void Clear()
        {
            _title = "Add Property";
            _property = new Property()
            {
                AvailableFrom = DateTime.Now,
                LandlordId = 0
            };
            _selectedLandlord = null;

            RaisePropertyChangedEvent(nameof(Title));
            RaisePropertyChangedEvent(nameof(Property));
            RaisePropertyChangedEvent(nameof(SelectedLandlord));
        }
    }
}
