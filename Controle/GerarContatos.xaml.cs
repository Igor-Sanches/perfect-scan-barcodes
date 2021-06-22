using Perfect_Scan.Models;
using Perfect_Scan.ViewModel; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace Perfect_Scan.Controle
{
    public sealed partial class GerarContatos : UserControl
    {
        ContactsViewModel contactsVM;
        private Frame frame;

        /// <summary>
        /// A user control that hosts the contacts panel within the main pivot/tab.
        /// </summary>
        public GerarContatos(Frame frame)
        {
        
            this.InitializeComponent();
            this.frame = frame;
            contactsVM = ViewModelDispatcher.ContactsViewModel;
            DataContext = contactsVM;
           
        }

        /// <summary>
        /// Open a contacts page upon click.
        /// </summary>
        private void OnContactItemClick(object sender, ItemClickEventArgs e)
        {
            contactsVM.ProcessOpenContactAsync((Contatos)e.ClickedItem);
        }

        /// <summary>
        /// Collapses all unncessary elements when the user is in semantic zoom out mode
        /// This allows the alphabet grid to be displayed in a bigger space.
        /// </summary>
        private void UpdateCollapseState(object sender, SemanticZoomViewChangedEventArgs e)
        {
            contactsVM.MaximizeSemanticZoomOut(ContactGroupView.IsZoomedInViewActive);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            contactsVM.Loader(frame);

        }
    }
}
