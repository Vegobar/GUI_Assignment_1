using System.Windows;

namespace GUI_Assignment_1
{
    /// <summary>
    /// Interaction logic for TotalDebtView.xaml
    /// </summary>
    public partial class TotalDebtView : Window
    {
        public TotalDebtView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as TotalDebtViewModel;
            DialogResult = true;
        }
    }
}