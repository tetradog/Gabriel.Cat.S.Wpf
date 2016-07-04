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
using Gabriel.Cat.Extension;
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
            gLeftOff.Background = new ImageBrush(Resource1.SwitchBottonOffL.ToImage().Source);
            gRightOff.Background = new ImageBrush(Resource1.SwitchBottonOffR.ToImage().Source);
            gLeft.Background = new ImageBrush(Resource1.SwitchBottonL.ToImage().Source);
            gRight.Background = new ImageBrush(Resource1.SwitchBottonR.ToImage().Source);
            EstaOn = true;
        }
        public bool EstaOn
        {
            get { return estaOn; }
            set
            {
                estaOn = value;
                if (estaOn)
                {//se tiene que 
                    if (!gLeft.IsVisible)
                    {
                        gLeft.Visibility = Visibility.Visible;
                        gRight.Visibility = Visibility.Hidden;
                        txtLabel.Background = brushOff;
                    }
                }else
                {//se tiene que encender
                    if (!gRight.IsVisible)
                    {
                        gRight.Visibility = Visibility.Visible;
                        gLeft.Visibility = Visibility.Hidden;
                        txtLabel.Background = brushOn;

                    }
                }

            }
        }
        public TextBlock Label
        {
            get { return txtLabel; }
        }

        private void gLeft_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EstaOn = true;
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EstaOn = !EstaOn;
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void gRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EstaOn = false;
            if (Changed != null)
                Changed(this, new EventArgs());
        }
    }
}
