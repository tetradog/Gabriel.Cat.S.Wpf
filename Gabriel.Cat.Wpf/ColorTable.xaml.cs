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
using WPFColorPickerLib;

namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Interaction logic for ColorTable.xaml
    /// </summary>
    public partial class ColorTable : UserControl
    {
        public event EventHandler<ColorSelectedArgs> ColorSelected;
        public event EventHandler<ColorChangedArgs> ColorChanged;
        Color[] colors;
        public ColorTable(params Color[] colors)
        {
            InitializeComponent();
            Colors = colors;
        }
        public Color[] Colors
        {
            get { return colors; }
            set
            {
                Image imgColor;

                if (value != null && value.Length > 0)
                {
                    colors = value;
                    ugColors.Children.Clear();
                    for(int i=0;i<colors.Length;i++)
                    {
                        imgColor = new Image();
                        imgColor.MouseLeftButtonDown += (s, e) =>
                        {
                            if (ColorSelected != null)
                                ColorSelected(this, new ColorSelectedArgs((Color)((Image)s).Tag));
                        };
                        imgColor.MouseRightButtonDown += (s, e) =>
                        {
                            Color colorAnt;
                            ColorDialog pickColor;
                            Image imgColorToChange = (Image)s;
                            colorAnt = (Color)imgColorToChange.Tag;
                            pickColor= new ColorDialog();
                            pickColor.ShowDialog();
                            imgColorToChange.Tag = pickColor.SelectedColor;
                            imgColorToChange.SetImage(pickColor.SelectedColor.ToBitmap(10, 10));
                            //actualizo el color
                            if (ColorChanged != null)
                                ColorChanged(this, new ColorChangedArgs(colorAnt,(Color) imgColorToChange.Tag));
                        };
                        imgColor.SetImage(colors[i].ToBitmap(10,10));
                        imgColor.Tag = colors[i];
                        ugColors.Children.Add(imgColor);
                    }
                }
                else
                    throw new ArgumentException();
            }
        }
    }

    public class ColorChangedArgs:EventArgs
    {
        Color colorAnt;
        Color colorAct;
        public ColorChangedArgs(Color colorAnt,Color colorAct)
        {
            this.colorAnt = colorAnt;
            this.colorAct = colorAct;

       }
        public Color ColorAnt{
            get { return colorAnt; }
            }
        public Color ColorAct
        {
            get { return colorAct; }
        }
    }
    public class ColorSelectedArgs : EventArgs
    {
        Color color;
        public ColorSelectedArgs(Color color)
        {
            this.color = color;

        }
        public Color Color
        {
            get { return color; }
        }
    }
}
