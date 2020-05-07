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
using System.Xml;
using Gabriel.Cat.S.Utilitats;
using Gabriel.Cat.S.Wpf;

namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Lógica de interacción para RichTextBoxContextMenu.xaml
    /// </summary>
    public partial class RichTextBoxContextMenu : ContextMenu
    {
        //hay un bug que a veces no carga los colores...mirar de clonar las imagenes :)
        //poder añadir una imagen desde el menu porque el copiar y pegar ya lo permite :)
        //poner marcado
        delegate void MetodoSinParametros();
        static ContextMenu context;
        static TwoKeysList<string, string, System.Drawing.Bitmap> imgDiccionary;
        RichTextBox rtbParent;

        static RichTextBoxContextMenu()
        {
            context = new ContextMenu();
            imgDiccionary = new TwoKeysList<string, string, System.Drawing.Bitmap>();
            System.Windows.Media.Color[] colores = Colores.ListaColores;
            System.Drawing.Bitmap img;
            for (int i = 0; i < colores.Length; i++)
            {
                img = colores[i].ToBitmap(20, 15);
                try
                {
                    imgDiccionary.Add(colores[i].GetName(), colores[i].ToString(), img);
                }
                catch { }
            }

        }
        public RichTextBoxContextMenu() : this(null) { }
        public RichTextBoxContextMenu(RichTextBox rtbParent):base()
        {

            MenuItem item;
            System.Windows.Media.Color[] colores;
            TextAlignment[] alienamientos;
            KeyValuePair<string, MetodoSinParametros>[] metodosExtra;
            double[] tamaños;
            System.Windows.Media.FontFamily[] fontFamilies;
            Image imgCopiar = new Image(), imgPegar = new Image(), imgCortar = new Image(), imgSubrallado = new Image(), imgNegrita = new Image(), imgCursiva = new Image(), imgNormal = new Image(), imgMarcado = new Image();
            MenuItem menuColorLetra = new MenuItem(), menuMarcador = new MenuItem(), menuAlineamientoLetra = new MenuItem(), menuTamañoLetra = new MenuItem(), menuTipoLetra = new MenuItem(), menuExtra = new MenuItem();
            MenuItem menuCopiar = new MenuItem(), menuPegar = new MenuItem(), menuCortar = new MenuItem(), menuSubrallado = new MenuItem(), menuNegrita = new MenuItem(), menuCursiva = new MenuItem(), menuNormal = new MenuItem();
            Separator sp1 = new Separator(), sp2 = new Separator();

            
            InitializeComponent();
            this.RtbParent = rtbParent;
            Items.AddRange(new Control[] { menuCortar, menuCopiar, menuPegar, sp1, menuNormal, menuSubrallado, menuNegrita, menuMarcador, menuExtra, sp2, menuTamañoLetra, menuTipoLetra, menuAlineamientoLetra, menuColorLetra });
            imgCopiar.SetImage(Resource1.copiar);
            imgPegar.SetImage(Resource1.pegar);
            imgCortar.SetImage(Resource1.cortar);
            imgSubrallado.SetImage(Resource1.subrallado);
            imgNegrita.SetImage(Resource1.negrita);
            imgCursiva.SetImage(Resource1.cursiva);
            imgNormal.SetImage(Resource1.normal);

            
            menuCortar.Header = "Cortar";
            menuCortar.Icon = imgCortar;
            menuCortar.Click += Cortar_Click;
            menuCopiar.Header = "Copiar";
            menuCopiar.Icon = imgCopiar;
            menuCopiar.Click += Copiar_Click;
            menuPegar.Header = "Pegar"; 
            menuPegar.Icon = imgPegar;
            menuPegar.Click += Pegar_Click;

            menuNormal.Header = "Normal"; 
            menuNormal.Icon = imgNormal;
            menuNormal.Click += Normal_Click;
            menuSubrallado.Header = "Subrallado";
            menuSubrallado.Icon = imgSubrallado;
            menuSubrallado.Click += Subrallado_Click;
            menuNegrita.Header = "Negrita";
            menuNegrita.Icon = imgNegrita;
            menuNegrita.Click += Negrita_Click;
            menuMarcador.Header = "Marcador";
            menuMarcador.Icon = imgMarcado;
            menuExtra.Header = "Extra";

            menuTamañoLetra.Header = "Tamaño";
            menuTipoLetra.Header = "Familia";
            menuAlineamientoLetra.Header = "Alineamiento";
            menuColorLetra.Header = "Color";
            colores = Colores.ListaColores;
            alienamientos = (TextAlignment[])Enum.GetValues(typeof(TextAlignment));
            tamaños = new double[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 74 };
            fontFamilies = Fonts.SystemFontFamilies.ToArray();
            metodosExtra = new KeyValuePair<string, MetodoSinParametros>[] {
                new KeyValuePair<string, MetodoSinParametros>("Oblique", ObliqueSelection),

                new KeyValuePair<string, MetodoSinParametros>("Baseline", BaselineSelection),
                new KeyValuePair<string, MetodoSinParametros>("OverLine", OverLineSelection),
                new KeyValuePair<string, MetodoSinParametros>("Strikethrough", StrikethroughSelection),
                new KeyValuePair<string, MetodoSinParametros>("SemiBold", SemiBoldSelection),

                new KeyValuePair<string, MetodoSinParametros>("Black", BlackSelection),
                new KeyValuePair<string, MetodoSinParametros>("ExtraBlack", ExtraBlackSelection),
                new KeyValuePair<string, MetodoSinParametros>("UltraBlack", UltraBlackSelection),
                
                new KeyValuePair<string, MetodoSinParametros>("DemiBold", DemiBoldSelection),
                new KeyValuePair<string, MetodoSinParametros>("ExtraBold",ExtraBoldSelection),
                new KeyValuePair<string, MetodoSinParametros>("UltraBold", UltraBoldSelection),

                new KeyValuePair<string, MetodoSinParametros>("Light", LightSelection),
                new KeyValuePair<string, MetodoSinParametros>("ExtraLight",ExtraLightSelection),
                new KeyValuePair<string, MetodoSinParametros>("UltraLight", UltraLightSelection),

                new KeyValuePair<string, MetodoSinParametros>("Thin", ThinSelection),
                new KeyValuePair<string, MetodoSinParametros>("Heavy", HeavySelection),
  
                new KeyValuePair<string, MetodoSinParametros>("Medium", MediumSelection),
                new KeyValuePair<string, MetodoSinParametros>("Regular", RegularSelection)
                

               
                
            };

            rtbParent.AutoWordSelection = false;
            imgMarcado.SetImage(Resource1.marcador);
            for (int i = 0; i < colores.Length; i++)
            {
                item = new MenuItem();
                item.Header = imgDiccionary.GetTkey1WhithTkey2(colores[i].ToString());

                item.Click += CambiarColorTextoSeleccionado;
                item.Icon = imgDiccionary.GetValueWithKey2(colores[i].ToString()).ToImage();
                item.Visibility = Visibility.Visible;
                item.Tag = colores[i];
                menuColorLetra.Items.Add(item);
            }
            menuColorLetra.UpdateLayout();
            for (int i = 0; i < colores.Length; i++)
            {
                item = new MenuItem();
                item.Header = imgDiccionary.GetTkey1WhithTkey2(colores[i].ToString());

                item.Click += PonMarcadorDeEsteColor;
                item.Icon = imgDiccionary.GetValueWithKey2(colores[i].ToString()).ToImage();
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
            for (int i = 0; i < metodosExtra.Length; i++)
            {
                item = new MenuItem();
                item.Header = metodosExtra[i].Key.ToString();
                item.Click += HazMetodoExtra;
                item.Tag = metodosExtra[i].Value;
                item.Visibility = Visibility.Visible;
                menuExtra.Items.Add(item);
            }
            menuExtra.UpdateLayout();


        }
        public RichTextBox RtbParent
        {
            get { return rtbParent; }
            set
            {
                rtbParent = value;
                rtbParent.ContextMenu = this;
            }
        }
        private void RegularSelection()
        {
            rtbParent.RegularSelection();
        }

        private void MediumSelection()
        {
            rtbParent.MediumSelection();
        }

        private void HeavySelection()
        {
            rtbParent.HeavySelection();
        }

        private void ThinSelection()
        {
            rtbParent.ThinSelection();
        }

        private void UltraLightSelection()
        {
            rtbParent.UltraLightSelection();
        }

        private void ExtraLightSelection()
        {
            rtbParent.ExtraLightSelection();
        }

        private void LightSelection()
        {
            rtbParent.LightSelection();
        }

        private void UltraBoldSelection()
        {
            rtbParent.UltraBoldSelection();
        }

        private void ExtraBoldSelection()
        {
            rtbParent.ExtraBoldSelection();
        }

        private void DemiBoldSelection()
        {
            rtbParent.DemiBoldSelection();
        }

        private void UltraBlackSelection()
        {
            rtbParent.UltraBlackSelection();
        }

        private void ExtraBlackSelection()
        {
            rtbParent.ExtraBlackSelection();
        }

        private void BlackSelection()
        {
            rtbParent.BlackSelection();
        }

        private void SemiBoldSelection()
        {
            rtbParent.SemiBoldSelection();
        }

        private void StrikethroughSelection()
        {
            rtbParent.StrikethroughSelection();
        }

        private void OverLineSelection()
        {
            rtbParent.OverLineSelection();
        }

        private void BaselineSelection()
        {
            rtbParent.BaselineSelection();
        }


        private void ObliqueSelection()
        {
            rtbParent.ObliqueSelection();
        }

        private void HazMetodoExtra(object sender, RoutedEventArgs e)
        {
            MetodoSinParametros metodo = ((MenuItem)sender).Tag as MetodoSinParametros;
            metodo();
        }



        private void Cortar_Click(object sender, RoutedEventArgs e)
        {
            rtbParent.Cut();
        }
        private void Copiar_Click(object sender, RoutedEventArgs e)
        {
            rtbParent.Copy();
        }
        private void Pegar_Click(object sender, RoutedEventArgs e)
        {
            rtbParent.Paste();
        }
        private void CambiarColorTextoSeleccionado(object sender, RoutedEventArgs e)
        {
            rtbParent.SelectedColor((System.Windows.Media.Color)((MenuItem)sender).Tag);
        }

        private void Cursiva_Click(object sender, RoutedEventArgs e)
        {
            rtbParent.ItalicSelection();
        }
        private void Negrita_Click(object sender, RoutedEventArgs e)
        {
            rtbParent.BoldSelection();
        }
        private void Subrallado_Click(object sender, RoutedEventArgs e)
        {
            rtbParent.UndeLineSelection();
        }
        private void PonMarcadorDeEsteColor(object sender, RoutedEventArgs e)
        {
            rtbParent.SelectionMarcador((System.Windows.Media.Color)((MenuItem)sender).Tag);
        }
        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            rtbParent.NormalSelection();
        }

        private void PonTamañoTextoSeleccionado(object sender, RoutedEventArgs e)
        {
            rtbParent.SelectionSize(Math.Round((double)((MenuItem)sender).Tag, 2));
        }

        private void PonAlineamientoTextoSeleccionado(object sender, RoutedEventArgs e)
        {
            rtbParent.SelectionAligment((TextAlignment)((MenuItem)sender).Tag);
        }
        private void PonFuenteTextoSelecciondado(object sender, RoutedEventArgs e)
        {
            rtbParent.FontFamilySelection((System.Windows.Media.FontFamily)((MenuItem)sender).Tag);
        }

    }
}
