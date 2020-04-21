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
using Gabriel.Cat.S.Extension;
namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Interaction logic for SwitchButtonLabel.xaml
    /// </summary>
    public partial class SwitchButtonLabel : UserControl
    {
        Brush brushOn,brushOff;
        bool estaOn;
        public event EventHandler Changed;
        public SwitchButtonLabel()
        {
            brushOn = Brushes.LightGreen;
            brushOff = Brushes.LightSalmon; 
            InitializeComponent();
            EstaOn = true;
        }
        public bool EstaOn
        {
            get { return estaOn; }
            set
            {
                estaOn = value;
                if (estaOn)
                {//se tiene que apagar

                        txtLabel.Background = brushOff;
                    
                }else
                {//se tiene que encender
                    
                        txtLabel.Background = brushOn;

                    
                }

            }
        }
        public TextBlock Label
        {
            get { return txtLabel; }
        }

        public Brush BrushOn
        {
            get
            {
                return brushOn;
            }

            set
            {
                brushOn = value;
                EstaOn = EstaOn;
            }
        }

        public Brush BrushOff
        {
            get
            {
                return brushOff;
            }

            set
            {
                brushOff = value;
                EstaOn = EstaOn;
            }
        }

        private void MouseLeftButtonUp_Click(object sender, MouseButtonEventArgs e)
        {
            EstaOn = !EstaOn;
            Changed?.Invoke(this, new EventArgs());
        }


    }
}
