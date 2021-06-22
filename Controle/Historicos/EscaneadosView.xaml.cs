using Perfect_Scan.Database.Sql;
using Perfect_Scan.ViewModel;
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

namespace Perfect_Scan.Controle.Historicos
{
    public sealed partial class EscaneadosView : UserControl
    {
        private EscaneadosViewModel vm;

        public EscaneadosView(Button sorting, Button delete, Pivot paginas)
        {
            this.InitializeComponent();
            vm = ViewModelDispatcher.EscaneadosView;
            DataContext = vm;

            sorting.Click += (sender, args) =>
            {
                vm.Sorting(paginas.SelectedIndex);
            };

            delete.Click += (sender, args) =>
            {
                vm.DeleteAll(paginas.SelectedIndex);
            };
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem item = (MenuFlyoutItem)sender as MenuFlyoutItem;
            Escaneados escaneado = (Escaneados)item.DataContext as Escaneados;
            vm.OnEditar(escaneado);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = DialogEdit;
            vm.Shot(dialog, EditName);
            vm.AtualizarLista();
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem item = (MenuFlyoutItem)sender as MenuFlyoutItem;
            Escaneados escaneado = (Escaneados)item.DataContext as Escaneados;
            vm.OnDelete(escaneado);
        }
    }
}
