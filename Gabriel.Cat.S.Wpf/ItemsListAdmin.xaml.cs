using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Interaction logic for ItemsListAdmin.xaml
    /// </summary>
    public partial class ItemsListAdmin : UserControl
    {
        public ItemsListAdmin()
        {
            InitializeComponent();
        }
        public MenuItems Menu
        {
            get { return menu; }
        }
        public StackPanel StackItems
        {
            get { return stkItems; }
        }
    }
}
