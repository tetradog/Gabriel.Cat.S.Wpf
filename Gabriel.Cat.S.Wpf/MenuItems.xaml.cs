using Gabriel.Cat.S.Extension;
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
    /// Interaction logic for ItemsAdmin.xaml
    /// </summary>
    public partial class MenuItems : UserControl
    {
        public event EventHandler ClickLeftButton;
        public event EventHandler ClickCenterButton;
        public event EventHandler ClickRightButton;
        public MenuItems()
        {
            InitializeComponent();
           
        }
        public ToggleButton Left
        {
            get { return tgLeft; }
        }
        public ToggleButton Right
        {
            get { return tgRight; }
        }
        public ToggleButton Center
        {
            get { return tgCenter; }
        }
        private void tgLeft_ChangeIndex(object sender, ToggleButtonArgs e)
        {
            if (ClickLeftButton != null)
                ClickLeftButton(this, new EventArgs());
        }

        private void tgCenter_ChangeIndex(object sender, ToggleButtonArgs e)
        {
            if (ClickCenterButton != null)
                ClickCenterButton(this, new EventArgs());
        }

        private void tgRight_ChangeIndex(object sender, ToggleButtonArgs e)
        {
            if (ClickRightButton != null)
                ClickRightButton(this, new EventArgs());
        }
    }
}
