using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class GeralEditor : UserControl
    {
        private Frame frame;
        private string protocol = "";
        public GeralEditor(string protocol, Frame frame)
        {
            this.InitializeComponent();
            this.frame = frame;
            this.protocol = protocol;
            DataContext = ViewModel.ViewModelDispatcher.GeralEditorViewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            ViewModel.GeralViewModel vm = (ViewModel.GeralViewModel)DataContext;
            vm.LoadFonts(protocol, FontFaceMenuFlyout, FontSizeMenuFlyout, ButtonFontFlyout, CheckBoxTextItalic, GeralPlanilhaEditor, frame); ;
        }
    }
}
