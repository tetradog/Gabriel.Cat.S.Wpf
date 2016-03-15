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
using System.Drawing;

namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Lógica de interacción para RicoTextBox.xaml
    /// </summary>
    public partial class RicoTextBox : UserControl
    {
        public event TextChangedEventHandler TextoCambiado;
        static  LlistaOrdenada<string, System.Windows.Controls.Image> imgDiccionary = new LlistaOrdenada<string, System.Windows.Controls.Image>();
        static RicoTextBox()
        {
            System.Windows.Media.Color[] colores = Colores.GetColors().ToArray();
            System.Windows.Controls.Image img;
            for (int i = 0; i < colores.Length; i++)
            {
                img = new System.Windows.Controls.Image();
                img.SetImage(colores[i].ToBitmap(20,15));
                imgDiccionary.Afegir(colores[i].ToString(), img);
            }

        }
        public RicoTextBox()
        {
            MenuItem item;
            System.Windows.Media.Color[] colores = Colores.GetColors().ToArray();
          
            InitializeComponent();
            imgCopiar.SetImage(Resource1.copiar);
            imgPegar.SetImage(Resource1.pegar);
            imgCortar.SetImage(Resource1.cortar);
            for (int i = 0; i < colores.Length; i++)
            {
                item = new MenuItem();                
                item.Header = colores[i].GetName();
                
                item.Click += CambiarColorTextoSeleccionado;
                item.Icon =imgDiccionary[colores[i].ToString()];
                item.Visibility = Visibility.Visible;
                item.Tag = colores[i];
                menuColorLetra.Items.Add(item);
            }
            menuColorLetra.UpdateLayout();
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
            {//no se porque le aparece enters
                rtText.LoadStringRtf(value);
            }
        }

        private void rtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextoCambiado != null)
                TextoCambiado(sender, e);
        }

        private void Cortar_Click(object sender, RoutedEventArgs e)
        {
            rtText.Cut();
        }
        private void Copiar_Click(object sender, RoutedEventArgs e)
        {
            rtText.Copy();
        }
        private void Pegar_Click(object sender, RoutedEventArgs e)
        {
            rtText.Paste();
        }
        private void CambiarColorTextoSeleccionado(object sender, RoutedEventArgs e)
        {
            TextRange txtRange = new TextRange(rtText.Selection.Start, rtText.Selection.End);
            txtRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush((System.Windows.Media.Color)((MenuItem)sender).Tag));
        }

    }
}
