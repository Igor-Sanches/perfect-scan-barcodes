using Perfect_Scan.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
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
    public sealed partial class DisplayNumeroView : UserControl
    {
        private ResourceLoader loader = new ResourceLoader();
        private string tel;
        private GerarNumeroViewModel vm;
        private Frame frame;
        public DisplayNumeroView(string v, Frame frame)
        {
            this.InitializeComponent();
            this.frame = frame;
            tel = v;
            vm = ViewModelDispatcher.DialerViewModel; ;
            DataContext = vm;
            setDialPadHeight();
            Clipboard.ContentChanged += async (ss, aa) =>
            {
                DataPackageView view = Clipboard.GetContent();
                Paste.Visibility = await Convert.Convert.ToVisibilityByClipboardAsync(view);
            };
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            if(tel!= "")
            {
                vm.Shot(tel);
            }


        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            try
            {
                if (Data.Data.PaginaViewIndex == 3)
                {
                    GerarNumeroViewModel vm = (GerarNumeroViewModel)DataContext;
                    var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                    if (ctrl.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
                    {
                        switch (args.VirtualKey)
                        { 
                            case VirtualKey.V:
                                vm.DialerPhoneNumber.BackPasteInDial();
                                break; 
                            case VirtualKey.C:
                                if (!NumberToDial.SelectedText.Equals(""))
                                {
                                    NumberToDial.CopySelectionToClipboard();
                                    Paginas.Root.RootApp.Instance.GetToast(loader.GetString("CopyText"), Tools.ModoColor.Succes);
                                }
                                break;
                        }
                    }
                    else
                    {

                        switch (args.VirtualKey)
                        {
                            case VirtualKey.Number0:
                                vm.DialerPhoneNumber.SetDialNumberKey("0");
                                break;
                            case VirtualKey.Number1:
                                vm.DialerPhoneNumber.SetDialNumberKey("1");
                                break;
                            case VirtualKey.Number2:
                                vm.DialerPhoneNumber.SetDialNumberKey("2");
                                break;
                            case VirtualKey.Number3:
                                vm.DialerPhoneNumber.SetDialNumberKey("3");
                                break;
                            case VirtualKey.Number4:
                                vm.DialerPhoneNumber.SetDialNumberKey("4");
                                break;
                            case VirtualKey.Number5:
                                vm.DialerPhoneNumber.SetDialNumberKey("5");
                                break;
                            case VirtualKey.Number6:
                                vm.DialerPhoneNumber.SetDialNumberKey("6");
                                break;
                            case VirtualKey.Number7:
                                vm.DialerPhoneNumber.SetDialNumberKey("7");
                                break;
                            case VirtualKey.Number8:
                                vm.DialerPhoneNumber.SetDialNumberKey("8");
                                break;
                            case VirtualKey.Number9:
                                vm.DialerPhoneNumber.SetDialNumberKey("9");
                                break;
                            case VirtualKey.Delete:
                                vm.DialerPhoneNumber.SetBackSpaceCommandDialClear();
                                break;
                            case VirtualKey.Back:
                                vm.DialerPhoneNumber.SetBackSpaceCommand();
                                break;
                        }
                    }
                }

            }
            catch { }

        }

        /// <summary>
        /// sets the dialpad control height based on screen resolution.
        /// </summary>

        private void setDialPadHeight()
        {
            var bounds = Window.Current.Bounds;
            int windowHeight = (int)bounds.Height;
            Dialpad.Height = windowHeight - 350;
        }

        /// <summary>
        /// Auto scrolls the dialer number heap to the end for long phone numbers.
        /// </summary>
        private void OnDialerNumberControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double scrollToEndOffset = dialerNumberControlScrollviewer.ScrollableWidth;
            dialerNumberControlScrollviewer.ChangeView(scrollToEndOffset, null, null);
        }

        /// <summary>
        /// Processes press and hold for the back button
        /// This clears the dialer number heap all at once.
        /// </summary>
        private void OnBackspaceHolding(object sender, HoldingRoutedEventArgs e)
        {
            GerarNumeroViewModel vm = (GerarNumeroViewModel)DataContext;
            if ((vm != null) && (e.HoldingState == Windows.UI.Input.HoldingState.Started))
            {
                vm.ProcessBackSpaceHolding.Execute(null);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            vm.Loader(frame);
        }
    }
}
