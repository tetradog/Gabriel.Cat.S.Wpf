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
    public delegate void ObjViewerEventHandler(ObjViewer visor);
    /// <summary>
    /// Lógica de interacción para ObjViewer.xaml
    /// </summary>
    public partial class ObjViewer: UserControl
    {
        Object obj;
        public event ObjViewerEventHandler ObjSelected;
        public ObjViewer(Object obj)
        {
            InitializeComponent();
            Object = obj;
        }

        public Object Object
        {
            get
            {
                return obj;
            }

            set
            {
                obj = value;
                if(obj!=default)
                   txBlToStringObj.Text = obj.ToString();
            }
        }
        public void CambiarColorLetra(System.Windows.Media.Color color)
        {
        	txBlToStringObj.Foreground=new SolidColorBrush(color);
        }
        public void CambiarFondo(System.Windows.Media.Color color)
        {
            Background =new SolidColorBrush(color);
        }
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ObjSelected?.Invoke(this);
        }
        public override string ToString()
        {
            return txBlToStringObj?.Text;
        }
    }
}
