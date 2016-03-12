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
using System.Xml;
using System.Threading;

namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Lógica de interacción para RicoTextBox.xaml
    /// </summary>
    public partial class RicoTextBox : UserControl
    {
        public event TextChangedEventHandler TextoCambiado;
        Tiket<TextChangedEventArgs> eventoTextoCambiado;
        public RicoTextBox()
        {
            InitializeComponent();
            imgCopiar.SetImage(Resource1.copiar);
            imgPegar.SetImage(Resource1.pegar);
            imgCortar.SetImage(Resource1.cortar);
            eventoTextoCambiado = new Tiket<TextChangedEventArgs>(()=> { });
        }
        public string Text
        {
            get
            {
                return rtText.GetText();
            }
            set
            {

                if (value == null)
                    value = "";

                rtText.SetText(value);
            }
        }
        public string TextWithFormat
        {
            get
            {
                return rtText.ToStringRtf();
            }
            set
            {
                rtText.LoadRtf(value);
            }
        }

        private void rtText_TextChanged(object sender, TextChangedEventArgs e)
        {

            eventoTextoCambiado.AbortaTrabajo();
            eventoTextoCambiado = new Tiket<TextChangedEventArgs>((o) =>
            {
                Thread.Sleep(1000);//espero un segundo
                if (TextoCambiado != null)
                    TextoCambiado(sender, e);
            }, null);
            eventoTextoCambiado.HazFaenaAsync();

        }
    }
}
