using Perfect_Scan.Database.Sql;
using Perfect_Scan.Paginas.Visualizador;
using Perfect_Scan.ViewModel; 
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls; 

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace Perfect_Scan.Controle.Historicos
{
    public sealed partial class GeradosView : UserControl
    {
        private GeradosViewModel vm;
        private Frame frame;
        public GeradosView(Button sorting, Button delete, Pivot paginas, Frame frame)
        {
            this.InitializeComponent();
            this.frame = frame;
            vm = ViewModelDispatcher.GeradosView;
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
            Gerados gerado = (Gerados)item.DataContext as Gerados;
            vm.OnEditar(gerado);
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
            Gerados gerado = (Gerados)item.DataContext as Gerados;
            vm.OnDelete(gerado);
        }

        private void ItemClicked_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender as Button;
            Gerados gerado = (Gerados)btn.DataContext as Gerados;

            frame.Navigate(typeof(Paginas.Visualizador.Visualizador), gerado);

        }
    }
}
