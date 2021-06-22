using Perfect_Scan.Manager;
using Perfect_Scan.Models;
using Perfect_Scan.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Contacts;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using static System.Net.Mime.MediaTypeNames;

namespace Perfect_Scan.ViewModel
{
    class ContactsViewModel : ViewModelBase
    {
        private ResourceLoader loader = new ResourceLoader();
        private Frame frame;
        private RelayCommand contactRelayCommand;
        private string contactsStackPanelVisibleState = "Visible";
        private string contactsStatusVisibleState = "Visible";
        private string contactsStatusText = "";
        private string contactsSearchVisibleState = "collapsed";
        private string contactsListVisibleState = "Collapsed";
        private string contactsListGridHeight = "Auto";
        private string contactsGroupVisibleState = "Collapsed";
        private string contactsGroupGridHeight = "Auto";
        private ObservableCollection<Contatos> contactItems = new ObservableCollection<Contatos>();
        private ObservableCollection<Contatos> listOfContacts = new ObservableCollection<Contatos>();
        private List<AlphaKeyGroup<Contatos>> groupsOfContacts;
        private ContactStore store;

        /// <summary>
        /// A view model class for the contacts panel/module.
        /// </summary>
        public ContactsViewModel()
        {
            contactsStatusText = loader.GetString("loadingContacts"); 
            UpdateContactListAsync(); 
        }

        internal void Loader(Frame frame)
        {
            this.frame = frame;
        }

        /// <summary>
        /// Ensures the UI is updated upon initilization of the cellular details singleton.
        /// Even if the contacts module is loaded before the singleton is done intializaing
        /// This is fired from the calling info singleton
        /// </summary>
        private async void CallingInfoInstance_CellInfoUpdateCompletedAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    UpdateContactListAsync();
                });
        }

        /// <summary>
        /// Updates the contacts view with information from the calling info singleton.
        /// The only item we care about here is the accent color.
        /// </summary>
        public async void UpdateContactListAsync()
        {
            await LoadContactsFromStoreAsync();
        }

        public ICommand ContactRelayCommand
        {
            get
            {
                if(contactRelayCommand == null)
                {
                    contactRelayCommand = new Manager.RelayCommand(p => this.OnContactRelayCommand());
                }
                return contactRelayCommand;
            }
        }

        private async void OnContactRelayCommand()
        {
            try
            {
                var picker = new ContactPicker();
                picker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                var contato = await picker.PickContactAsync();
                if (contato != null)
                {
                    Contatos c = new Contatos(contato);
                    ProcessOpenContactAsync(c);
                }
            }
            catch (Exception x) { Paginas.Root.RootApp.Instance.GetToast(x.Message); }
        }

        /// <summary>
        /// Loads up the contact store using the contactsRT APIs.
        /// Checks to make sure the store can be loaded and errors out accordingly
        /// </summary>
        private async Task LoadContactsFromStoreAsync()
        {
            //Try loading the contact atore
            try
            {
                store = await ContactManager.RequestStoreAsync(ContactStoreAccessType.AllContactsReadOnly);
            }
            catch 
            {
                ContactsStatusText = loader.GetString("loadingContactsError");
            }

            //If we can access the store without crashing (There seems to be a bug with the store).
            //Check to make sure we actually have access.
            if (store == null)
            {
                ContactsStatusText = loader.GetString("loadingContactsError");
                Debug.WriteLine("Could not open contact store, is app access disabled in privacy settings?", "error");
                return;
            }
            Debug.WriteLine("Contact store opened for reading successfully.", "informational");
            //Load the contacts into the ListView on the page
            ContactReader reader = store.GetContactReader();
            await DisplayContactsFromReaderAsync(reader, true);
            return;
        }

        /// <summary>
        /// Processes contact search.
        /// </summary>
        /// <param name="ContactFilter">Takes in the string inputed by the user</param>
        private async Task SearchForTextAsync(string ContactFilter)
        {
            if (store == null)
            {
                //Shouldn't happen, and I don't want to deal with opening the store in multiple locations
                await LoadContactsFromStoreAsync();
                return;
            }
            //A null query string is being treated as a query for "*"
            if (!string.IsNullOrWhiteSpace(ContactFilter))
            {
                ContactQueryOptions option = new ContactQueryOptions(ContactFilter, ContactQuerySearchFields.All);
                ContactReader reader = store.GetContactReader(option);
                await DisplayContactsFromReaderAsync(reader, false);
            }
            else
            {
                ContactReader reader = store.GetContactReader();
                await DisplayContactsFromReaderAsync(reader, true);
            }
            return;
        }

        /// <summary>
        /// Displays the items from the contact store in the contacts view.
        /// </summary>
        /// <param name="reader">Contact store reader</param>
        /// <param name="isGroup">
        /// Boolean to decide if to show the list in a group or a flat list
        /// Groups is shown by default and search
        /// Flast list is shown during search
        /// </param>
        private async Task DisplayContactsFromReaderAsync(ContactReader reader, bool isGroup)
        {
            contactItems.Clear();
            ContactBatch contactBatch = await reader.ReadBatchAsync();
            if (contactBatch.Contacts.Count == 0)
            {
                ContactsStatusText = loader.GetString("loadingContactsError");
                ShowContactListStatusText();
                return;
            }

            while (contactBatch.Contacts.Count != 0)
            {
                //should batch add to avoid triggering callbacks            
                foreach (Contact c in contactBatch.Contacts)
                {
                    Contatos contactToAdd = new Contatos(c);
                    contactToAdd.SetImageAsync();
                    contactItems.Add(contactToAdd);
                }
                contactBatch = await reader.ReadBatchAsync();
            }

            if (isGroup)
            {
                GroupsOfContacts = alphaGroupSorting(contactItems);
                ShowContactGroup();
            }
            else
            {
                ListOfContacts = contactItems;
                ShowContactList();
            }
            return;
        }

        /// <summary>
        /// Creating an aplabetical sorted group of the contacts in the store.
        /// </summary>
        /// <param name="items">Flat list of contacts</param>
        private List<AlphaKeyGroup<Contatos>> alphaGroupSorting(IEnumerable<Contatos> items)
        {

            var returnGroup = AlphaKeyGroup<Contatos>.CreateGroups(
                items,                                      // ungrouped list of items
                (Contatos s) => { return s.DisplayName; },  // the property to sort 
                true);                                      // order the items alphabetically 

            return returnGroup;
        }

        /// <summary>
        /// Shows a flat list of contacts.
        /// Hides irrelevant elements/controls
        /// </summary>
        private void ShowContactList()
        {
            ContactsStatusVisibleState = "Collapsed";
            ContactsGroupVisibleState = "Collapsed";
            ContactsGroupGridHeight = "Auto";
            ContactsSearchVisibleState = "Visible";
            ContactsListGridHeight = "*";
            ContactsListVisibleState = "Visible";
        }

        /// <summary>
        /// Shows a grouped list of contacts.
        /// Hides irrelevant elements/controls
        /// </summary>
        private void ShowContactGroup()
        {
            ContactsStatusVisibleState = "Collapsed";
            ContactsListVisibleState = "Collapsed";
            ContactsListGridHeight = "Auto";
            ContactsSearchVisibleState = "Visible";
            ContactsGroupGridHeight = "*";
            ContactsGroupVisibleState = "Visible";
        }

        /// <summary>
        /// Shows the status text in case of errors and additional info.
        /// Hides all contact list
        /// </summary>
        private void ShowContactListStatusText()
        {
            ContactsSearchVisibleState = "Visible";
            ContactsListVisibleState = "Collapsed";
            ContactsListGridHeight = "Auto";
            ContactsGroupVisibleState = "Collapsed";
            ContactsGroupGridHeight = "Auto";
            ContactsStatusVisibleState = "Visible";
        }

        /// <summary>
        /// Hides the status and search elements
        /// Used in the group zoomed out mode
        /// </summary>
        private void HideStatusAndSearch()
        {
            ContactsStackPanelVisibleState = "Collapsed";
        }

        /// <summary>
        /// Shows the status and search elements
        /// Used in the group zoomed in mode
        /// </summary>
        private void ShowStatusAndSearch()
        {
            ContactsStackPanelVisibleState = "Visible";
        }


        /// <summary>
        /// Processes the contact search event
        /// </summary>
        //Update to this when event bindings are working
        //public void ProcessContactSearch(object sender, TextChangedEventArgs e)
        public async void ProcessContactSearchAsync(string searchQuery)
        {
            await SearchForTextAsync(searchQuery);
        }

        public static string[] PHONE_KEYS = {"phone", "secondary_phone", "tertiary_phone"};

        private ApplicationDataContainer data = ApplicationData.Current.LocalSettings;
        public void ProcessOpenContactAsync(Contatos contact)
        {
            if (contact != null)
            {

                Contact ctt = contact.Contato;
                string id = ctt.Id;
                string nome = contact.DisplayName; 
                
                var encoder = new MECARDContactEncoder();
                List<string> postal = new List<string>();
                foreach(var postalCode in ctt.Addresses)
                {
                    postal.Add(postalCode.PostalCode);
                }
                List<string> numeros = new List<string>();
                foreach(var numero in ctt.Phones)
                {
                    numeros.Add(numero.Number);
                }
                List<string> Emails = new List<string>();
                foreach(var email in ctt.Emails)
                {
                    Emails.Add(email.Address);
                } 
                string[] _enc = encoder.Encode(new List<string>() { nome }, postal, numeros, Emails, ctt.Notes);
                if (!_enc[1].Equals(""))
                {
                    Gerador.With(_enc[0]).GerarCodigo(Data.Data.Codigo, frame);
                }

            } 
        }
        
        void Toast(string msg)
        {
            Paginas.Root.RootApp.Instance.GetToast(msg);
        }

        /// <summary>
        /// Shows/Hides different elements to maximize semantic zoom realestate
        /// </summary>
        public void MaximizeSemanticZoomOut(bool isZoomedInActive)
        {
            if (isZoomedInActive)
            {
                //Restore view
                ShowStatusAndSearch();
            }
            else
            {
                //Maximise view
                HideStatusAndSearch();
            }
        }

        #region Setters and Getters
        /// <summary>
        /// Gets and sets contacts stack panel visitble state
        /// </summary>
        public string ContactsStackPanelVisibleState
        {
            get
            {
                return contactsStackPanelVisibleState;
            }
            private set
            {
                if (contactsStackPanelVisibleState != value)
                {
                    contactsStackPanelVisibleState = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the contacts status text block visibile state.
        /// </summary>
        public string ContactsStatusVisibleState
        {
            get
            {
                return contactsStatusVisibleState;
            }
            private set
            {
                if (contactsStatusVisibleState != value)
                {
                    contactsStatusVisibleState = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the contact status text.
        /// </summary>
        public string ContactsStatusText
        {
            get
            {
                return contactsStatusText;
            }
            private set
            {
                if (contactsStatusText != value)
                {
                    contactsStatusText = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the contact search text box visibile state.
        /// </summary>
        public string ContactsSearchVisibleState
        {
            get
            {
                return contactsSearchVisibleState;
            }
            private set
            {
                if (contactsSearchVisibleState != value)
                {
                    contactsSearchVisibleState = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the contact list visible state.
        /// </summary>
        public string ContactsListVisibleState
        {
            get
            {
                return contactsListVisibleState;
            }
            private set
            {
                if (contactsListVisibleState != value)
                {
                    contactsListVisibleState = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the contacts list grid height.
        /// </summary>
        public string ContactsListGridHeight
        {
            get
            {
                return contactsListGridHeight;
            }
            private set
            {
                if (contactsListGridHeight != value)
                {
                    contactsListGridHeight = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the contacts group list visibile state.
        /// </summary>
        public string ContactsGroupVisibleState
        {
            get
            {
                return contactsGroupVisibleState;
            }
            private set
            {
                if (contactsGroupVisibleState != value)
                {
                    contactsGroupVisibleState = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the contact group list grid height.
        /// </summary>
        public string ContactsGroupGridHeight
        {
            get
            {
                return contactsGroupGridHeight;
            }
            private set
            {
                if (contactsGroupGridHeight != value)
                {
                    contactsGroupGridHeight = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the flat contacts list.
        /// </summary>
        public ObservableCollection<Contatos> ListOfContacts
        {
            get
            {
                return listOfContacts;
            }
            private set
            {
                if (listOfContacts != value)
                {
                    listOfContacts = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the grouped contacts list.
        /// </summary>
        public List<AlphaKeyGroup<Contatos>> GroupsOfContacts
        {
            get
            {
                return groupsOfContacts;
            }
            private set
            {
                if (groupsOfContacts != value)
                {
                    groupsOfContacts = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion
    }
}