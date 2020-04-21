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
using Gabriel.Cat.S.Utilitats;

namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Interaction logic for ToggleButton.xaml
    /// </summary>
    public partial class ToggleButton : UserControl
    {
        int indice;
        public event EventHandler<ToggleButtonArgs> ChangeIndex;
        public ToggleButton()
        {
            ImagenesButton = new Llista<Image>();
            ImagenesButton.Updated +=(s,e)=>{
                int index = Index;
                Index = index;
            };
            InitializeComponent();
            Index = 0;
        }
        public ToggleButton(params System.Drawing.Bitmap[] bmps):this()
        {
            if (bmps != null)
                for (int i = 0; i < bmps.Length; i++)
                    ImagenesButton.Add(bmps[i].ToImage());
            Index = 0;
        }
        public Llista<Image> ImagenesButton  { get; private set; }
        public int Index
        {
            get { return indice; }
            set
            {
                if (ImagenesButton.Count != 0)
                {
                    indice = value % ImagenesButton.Count;
                    imgButton.Source = ImagenesButton[indice].Source;
                }
                else {
                    imgButton.Source = new Image().Source;
                    indice = -1;
                }
            }
        }

        private void imgButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Index++;
            ChangeIndex?.Invoke(this, new ToggleButtonArgs(Index));
        }
    }
    public class ToggleButtonArgs:EventArgs
    {
        public int Index { get; private set; }
        public ToggleButtonArgs(int index)
        {
            Index = index;
        }
    }
}
