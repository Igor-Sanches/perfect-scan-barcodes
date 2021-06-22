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
using Perfect_Scan.ViewModel;
using Windows.System;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace Perfect_Scan.Controle
{
    public sealed partial class DisplayProEncView : UserControl
    {

        private Settings.Settings _stt = new Settings.Settings();
        private ProdutoEncomendaViewModel vm;
        private Frame frame;
        public DisplayProEncView(Frame frame)
        {
            this.InitializeComponent();
            this.frame = frame;
            vm = ViewModelDispatcher.ProdutoEncomendaView;
            DataContext = vm;
            setDialPadHeight();
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            try
            {
                if (Data.Data.PaginaViewIndex == 2)
                {
                    if (Data.Data.IsProduto)
                    {
                        var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                        if (ctrl.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
                        {
                            switch (args.VirtualKey)
                            {
                                case VirtualKey.C:
                                    if (!vm.DigiteProduto.Equals(""))
                                    {
                                        vm.ProdutoCopyDigitado();
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            switch (args.VirtualKey)
                            {
                                case VirtualKey.Back: vm.ProdutoDeleteDigitado(); break;
                                case VirtualKey.Delete: vm.ProdutoDeleteAllDigitado(); break;
                                case VirtualKey.Number0: vm.Digit = "0"; break;
                                case VirtualKey.Number1: vm.Digit = "1"; break;
                                case VirtualKey.Number2: vm.Digit = "2"; break;
                                case VirtualKey.Number3: vm.Digit = "3"; break;
                                case VirtualKey.Number4: vm.Digit = "4"; break;
                                case VirtualKey.Number5: vm.Digit = "5"; break;
                                case VirtualKey.Number6: vm.Digit = "6"; break;
                                case VirtualKey.Number7: vm.Digit = "7"; break;
                                case VirtualKey.Number8: vm.Digit = "8"; break;
                                case VirtualKey.Number9: vm.Digit = "9"; break;
                            }
                        }
                    }
                    else
                    {
                        var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                        if (ctrl.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
                        {
                            switch (args.VirtualKey)
                            {
                                case VirtualKey.C:
                                    if (!vm.DigiteProduto.Equals(""))
                                    {
                                        vm.OnCopy();
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            if (args.VirtualKey == VirtualKey.Back)
                            {
                                vm.DeleteInDial();
                            }
                            else if (args.VirtualKey == VirtualKey.Delete)
                            {
                                vm.DeleteAllInDial();
                            }
                            if (vm.DialerVisibility.Equals("Visible"))
                            {
                                switch (args.VirtualKey)
                                {
                                    case VirtualKey.Q: vm.DigitLetra = "Q"; break;
                                    case VirtualKey.W: vm.DigitLetra = "W"; break;
                                    case VirtualKey.E: vm.DigitLetra = "E"; break;
                                    case VirtualKey.R: vm.DigitLetra = "R"; break;
                                    case VirtualKey.T: vm.DigitLetra = "T"; break;
                                    case VirtualKey.Y: vm.DigitLetra = "Y"; break;
                                    case VirtualKey.U: vm.DigitLetra = "U"; break;
                                    case VirtualKey.I: vm.DigitLetra = "I"; break;
                                    case VirtualKey.O: vm.DigitLetra = "O"; break;
                                    case VirtualKey.P: vm.DigitLetra = "P"; break;
                                    case VirtualKey.A: vm.DigitLetra = "A"; break;
                                    case VirtualKey.S: vm.DigitLetra = "S"; break;
                                    case VirtualKey.D: vm.DigitLetra = "D"; break;
                                    case VirtualKey.F: vm.DigitLetra = "F"; break;
                                    case VirtualKey.G: vm.DigitLetra = "G"; break;
                                    case VirtualKey.H: vm.DigitLetra = "H"; break;
                                    case VirtualKey.J: vm.DigitLetra = "J"; break;
                                    case VirtualKey.K: vm.DigitLetra = "K"; break;
                                    case VirtualKey.L: vm.DigitLetra = "L"; break;
                                    case VirtualKey.Z: vm.DigitLetra = "Z"; break;
                                    case VirtualKey.X: vm.DigitLetra = "X"; break;
                                    case VirtualKey.C: vm.DigitLetra = "C"; break;
                                    case VirtualKey.V: vm.DigitLetra = "V"; break;
                                    case VirtualKey.B: vm.DigitLetra = "B"; break;
                                    case VirtualKey.N: vm.DigitLetra = "N"; break;
                                    case VirtualKey.M: vm.DigitLetra = "M"; break;
                                }
                            }
                            else
                            {

                                switch (args.VirtualKey)
                                {
                                    case VirtualKey.Number0: vm.DigitNumber = "0"; break;
                                    case VirtualKey.Number1: vm.DigitNumber = "1"; break;
                                    case VirtualKey.Number2: vm.DigitNumber = "2"; break;
                                    case VirtualKey.Number3: vm.DigitNumber = "3"; break;
                                    case VirtualKey.Number4: vm.DigitNumber = "4"; break;
                                    case VirtualKey.Number5: vm.DigitNumber = "5"; break;
                                    case VirtualKey.Number6: vm.DigitNumber = "6"; break;
                                    case VirtualKey.Number7: vm.DigitNumber = "7"; break;
                                    case VirtualKey.Number8: vm.DigitNumber = "8"; break;
                                    case VirtualKey.Number9: vm.DigitNumber = "9"; break;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

        }


        private void setDialPadHeight()
        {
            var bounds = Window.Current.Bounds;
            int windowHeight = (int)bounds.Height;
            Dialpad.Height = windowHeight - 350;
            Dialpad2.Height = windowHeight - 350;
        }


        private void NumberToDial_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void OnBackspaceHolding(object sender, HoldingRoutedEventArgs e)
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            vm.SetCompomentes(frame);
        }

        private void EncTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.Data.SttEncTypes = EncTypes.SelectedIndex;
            vm.Shot();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Data.Data.IsProduto = ProdutoView.IsOn;
            vm.OnToggleButton();
            Paginas.GerarCodigos.Instance.GetPositionComboBox();
        }
    }
}
