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
        bool estaOff;
        public event EventHandler Changed;
        public SwitchButtonLabel()
        {
            brushOn = Brushes.LightSalmon;
            brushOff = Brushes.LightGreen;
            InitializeComponent();
            gLeftOff.Background = new ImageBrush(Resource1.SwitchBottonOffL.ToImage().Source);
            gRightOff.Background = new ImageBrush(Resource1.SwitchBottonOffR.ToImage().Source);
            gLeft.Background = new ImageBrush(Resource1.SwitchBottonL.ToImage().Source);
            gRight.Background = new ImageBrush(Resource1.SwitchBottonR.ToImage().Source);
            EstaOff = true;
        }
        public bool EstaOff
        {
            get { return estaOff; }
            set
            {
                estaOff = value;
                if (estaOff)
                {
                    if (!gRight.IsVisible)
                    {
                        gRight.Visibility = Visibility.Visible;
                        gLeft.Visibility = Visibility.Hidden;
                        txtLabel.Background = brushOff;

                    }
                }else
                {
                    if (!gLeft.IsVisible)
                    {
                        gLeft.Visibility = Visibility.Visible;
                        gRight.Visibility = Visibility.Hidden;
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
            EstaOff = true;
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EstaOff = !EstaOff;
            if (Changed != null)
                Changed(this, new EventArgs());
        }

        private void gRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EstaOff = false;
            if (Changed != null)
                Changed(this, new EventArgs());
        }
    }
}
