using Perfect_Scan.Manager;
using Perfect_Scan.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources; 
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls; 
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Extensions;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class EscanearImagem : Page
    {
        private ResourceLoader loader = new ResourceLoader(); 
        private ImageEscannearViewModel vm;

        public EscanearImagem()
        {
            this.InitializeComponent(); 
            vm = ViewModelDispatcher.ImageEscannear;
            DataContext = vm;
        
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Data.Data.AppViewPager = "EscanearImagem";
            if (e.Parameter == e.Parameter as IActivatedEventArgs)
            {
                var args = e.Parameter as Windows.ApplicationModel.Activation.IActivatedEventArgs;
                if (args != null)
                {
                    if (args.Kind == Windows.ApplicationModel.Activation.ActivationKind.File)
                    {
                        try
                        {
                            var fileargs = args as Windows.ApplicationModel.Activation.FileActivatedEventArgs;
                            string sxt = fileargs.Files[0].Path;
                            var file = (StorageFile)fileargs.Files[0];
                            if (file != null)
                            {
                                vm.ShowNavigationFile(file);
                            }
                        }
                        catch
                        {
                            Root.RootApp.Instance.GetToast(loader.GetString("errorFileLoader"), Tools.ModoColor.Error);
                        }
                    }
                }
             }
            else if (e.Parameter == e.Parameter as RandomAccessStreamReference)
            {
                var args = e.Parameter as RandomAccessStreamReference;
                vm.ShowNavigationFile(args);
            }
            else
            {
                var str = e.Parameter as string;
                if(str != null)
                { 
                    var path = str;
                   
                        try
                        {
                            var file = await StorageFile.GetFileFromPathAsync(path);
                            if (file != null)
                            {
                                vm.ShowNavigationFile(file);
                            }
                            else
                            {
                                Root.RootApp.Instance.GetToast(loader.GetString("errorFileLoader"), Tools.ModoColor.Error);
                            }
                        }
                        catch
                        {
                            Root.RootApp.Instance.GetToast(loader.GetString("errorFileLoader"), Tools.ModoColor.Error);
                        }
                    
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            vm.Shot(imageCropper);
        }
    }
}
