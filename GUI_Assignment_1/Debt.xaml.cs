using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;

namespace GUI_Assignment_1
{
    /// <summary>
    /// Interaction logic for Debt.xaml
    /// </summary>
    public partial class Debt : Window
    {
        public Debt()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as DebtViewModel;
            if (vm.IsValid)
            {
                DialogResult = true;
                btnOk.IsEnabled = true;
            }
            else
                System.Windows.MessageBox.Show("You need to enter values for name and debt value", "Missing data");
        }
    }
}