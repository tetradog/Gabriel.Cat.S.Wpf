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
        Llista<Image> lstBmps;
        int indice;
        public event EventHandler<ToggleButtonArgs> ChangeIndex;
        public ToggleButton()
        {
            lstBmps = new Llista<Image>();
            lstBmps.Updated +=(s,e)=>{
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
        public Llista<Image> ImagenesButton
        {
            get { return lstBmps; }
        }
        public int Index
        {
            get { return indice; }
            set
            {
                if (lstBmps.Count != 0)
                {
                    indice = value % lstBmps.Count;
                    imgButton.Source = lstBmps[indice].Source;
                }
                else {
                    imgButton.Source = new Image().Source;
                    indice = 0;
                }
            }
        }

        private void imgButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Index++;
            if (ChangeIndex != null)
                ChangeIndex(this, new ToggleButtonArgs(Index));
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
