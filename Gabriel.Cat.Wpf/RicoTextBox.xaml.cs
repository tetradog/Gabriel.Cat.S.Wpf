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
        //poder añadir una imagen desde el menu porque el copiar y pegar ya lo permite :)
        //poner marcado
        public event TextChangedEventHandler TextoCambiado;
        static TwoKeysList<string, string, System.Windows.Controls.Image> imgDiccionary;
        static RicoTextBox()
        {
            imgDiccionary = new TwoKeysList<string, string, System.Windows.Controls.Image>();
            System.Windows.Media.Color[] colores = Colores.ListaColores;
            System.Windows.Controls.Image img;
            for (int i = 0; i < colores.Length; i++)
            {
                img = colores[i].ToImage(20, 15);
                imgDiccionary.Add(colores[i].GetName(), colores[i].ToString(), img);
            }

        }
        public RicoTextBox()
        {
            MenuItem item;
            System.Windows.Media.Color[] colores = Colores.ListaColores;
            TextAlignment[] alienamientos = (TextAlignment[])Enum.GetValues(typeof(TextAlignment));
            double[] tamaños = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 74 };
            System.Windows.Media.FontFamily[] fontFamilies = Fonts.SystemFontFamilies.ToArray();//cojo las familias de algun lado...
            
            InitializeComponent();
            rtText.AutoWordSelection = false;
            imgCopiar.SetImage(Resource1.copiar);
            imgPegar.SetImage(Resource1.pegar);
            imgCortar.SetImage(Resource1.cortar);
            imgSubrallado.SetImage(Resource1.subrallado);
            imgNegrita.SetImage(Resource1.negrita);
            imgCursiva.SetImage(Resource1.cursiva);
            imgNormal.SetImage(Resource1.normal);
            imgMarcado.SetImage(Resource1.marcador);
            for (int i = 0; i < colores.Length; i++)
            {
                item = new MenuItem();
                item.Header = imgDiccionary.ObtainTkey1WhithTkey2(colores[i].ToString());

                item.Click += CambiarColorTextoSeleccionado;
                item.Icon = imgDiccionary.ObtainValueWithKey2(colores[i].ToString());
                item.Visibility = Visibility.Visible;
                item.Tag = colores[i];
                menuColorLetra.Items.Add(item);
            }
            menuColorLetra.UpdateLayout();
            for (int i = 0; i < colores.Length; i++)
            {
                item = new MenuItem();
                item.Header = imgDiccionary.ObtainTkey1WhithTkey2(colores[i].ToString());

                item.Click += PonMarcadorDeEsteColor;
                item.Icon = imgDiccionary.ObtainValueWithKey2(colores[i].ToString());
                item.Visibility = Visibility.Visible;
                item.Tag = colores[i];
                menuMarcador.Items.Add(item);
            }
            menuMarcador.UpdateLayout();
            for (int i = 0; i < alienamientos.Length; i++)
            {
                item = new MenuItem();
                item.Header = alienamientos[i].ToString();
                item.Click += PonAlineamientoTextoSeleccionado;
                item.Tag = alienamientos[i];
                item.Visibility = Visibility.Visible;
                menuAlineamientoLetra.Items.Add(item);
            }
            menuAlineamientoLetra.UpdateLayout();
            for (int i = 0; i < tamaños.Length; i++)
            {
                item = new MenuItem();
                item.Header = tamaños[i].ToString();
                item.Click += PonTamañoTextoSeleccionado;
                item.Tag = tamaños[i];
                item.Visibility = Visibility.Visible;
                menuTamañoLetra.Items.Add(item);
            }
            menuTamañoLetra.UpdateLayout();
            for (int i = 0; i < fontFamilies.Length; i++)
            {
                item = new MenuItem();
                item.Header = fontFamilies[i].ToString();
                item.FontFamily = fontFamilies[i];
                item.Click += PonFuenteTextoSelecciondado;
                item.Tag = fontFamilies[i];
                item.Visibility = Visibility.Visible;
                menuTipoLetra.Items.Add(item);
            }
            menuTipoLetra.UpdateLayout();


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
            rtText.SelectedColor((System.Windows.Media.Color)((MenuItem)sender).Tag);
        }

        private void Cursiva_Click(object sender, RoutedEventArgs e)
        {
            rtText.ItalicSelection();
        }
        private void Negrita_Click(object sender, RoutedEventArgs e)
        {
            rtText.BoldSelection();
        }
        private void Subrallado_Click(object sender, RoutedEventArgs e)
        {
            rtText.UndeLineSelection();
        }
        private void PonMarcadorDeEsteColor(object sender, RoutedEventArgs e)
        {
            rtText.SelectionMarcador((System.Windows.Media.Color)((MenuItem)sender).Tag);
        }
        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            rtText.NormalSelection();
        }

        private void PonTamañoTextoSeleccionado(object sender, RoutedEventArgs e)
        {
            rtText.SelectionSize(Math.Round((double)((MenuItem)sender).Tag, 2));
        }

        private void PonAlineamientoTextoSeleccionado(object sender, RoutedEventArgs e)
        {
            rtText.SelectionAligment((TextAlignment)((MenuItem)sender).Tag);
        }
        private void PonFuenteTextoSelecciondado(object sender, RoutedEventArgs e)
        {
            rtText.FontFamilySelection((System.Windows.Media.FontFamily)((MenuItem)sender).Tag);
        }
    }
}
