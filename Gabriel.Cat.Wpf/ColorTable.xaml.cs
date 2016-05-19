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
        System.Drawing.Color[] colors;
        private bool isAlfaSuported;
        private bool isTextReadOnly;
        ColorDialog pickColor;
        public ColorTable():this(new System.Drawing.Color[] { }) { }
        public ColorTable(params System.Drawing.Color[] colors)
        {
            InitializeComponent();
            Colors = colors;
            pickColor = new ColorDialog();
        }
        public System.Drawing.Color[] Colors
        {
            get { return colors; }
            set
            {
                Image imgColor;
                
                if (value != null)
                {
                    colors = value;
                    ugColors.Children.Clear();
                    for(int i=0;i<colors.Length;i++)
                    {
                        imgColor = new Image();
                    
                        imgColor.MouseLeftButtonDown += (s, e) =>
                        {
                            ColorPos colorPos;
                            colorPos= (ColorPos) ((Image)s).Tag;
                            if (ColorSelected != null)
                                ColorSelected(this, new ColorSelectedArgs(colorPos.Color,colorPos.Posicion));
                        };
                        imgColor.MouseRightButtonDown += (s, e) =>
                        {
                           
                            System.Drawing.Color colorAnt;
                           
                            Image imgColorToChange = (Image)s;
                            ColorPos colorPos= (ColorPos)imgColorToChange.Tag;
                            colorAnt = colorPos.Color;
                            pickColor.ColorPicker.SelectedColor =Color.FromArgb(colorPos.Color.A, colorPos.Color.R, colorPos.Color.G, colorPos.Color.B);
                            pickColor.ShowDialog();
                            if (pickColor.DialogResult.Value)
                            {
                                colors[colorPos.Posicion] = System.Drawing.Color.FromArgb(pickColor.ColorPicker.SelectedColor.A, pickColor.ColorPicker.SelectedColor.R, pickColor.ColorPicker.SelectedColor.G, pickColor.ColorPicker.SelectedColor.B);
                                imgColorToChange.Tag = new ColorPos(colors[colorPos.Posicion], colorPos.Posicion);
                                imgColorToChange.SetImage(pickColor.ColorPicker.SelectedColor.ToBitmap(10, 10));
                                //actualizo el color
                                if (ColorChanged != null)
                                    ColorChanged(this, new ColorChangedArgs(colorAnt, ((ColorPos)imgColorToChange.Tag).Color, ((ColorPos)imgColorToChange.Tag).Posicion));
                            }
                        };
                        imgColor.SetImage( System.Windows.Media.Color.FromArgb(colors[i].A, colors[i].R, colors[i].G, colors[i].B).ToBitmap(10,10));
                        imgColor.Tag =new ColorPos(colors[i],i);
                        ugColors.Children.Add(imgColor);
                    }
                }
                else
                    throw new ArgumentException();
            }
        }
        public ColorDialog ColorDialog
        {
            get { return pickColor; }
        }
    }
    public class ColorPos
    {
        public System.Drawing.Color Color;
        public int Posicion;
        public ColorPos(System.Drawing.Color color ,int pos)
        {
            this.Color = color;
            this.Posicion = pos;
        }
    }
    public class ColorChangedArgs:EventArgs
    {
        System.Drawing.Color colorAnt;
        System.Drawing.Color colorAct;
        int pos;
        public ColorChangedArgs(System.Drawing.Color colorAnt, System.Drawing.Color colorAct,int pos)
        {
            this.colorAnt = colorAnt;
            this.colorAct = colorAct;
            this.pos = pos;

       }
        public int Posicion
        {
            get { return pos; }
        }
        public System.Drawing.Color ColorAnt{
            get { return colorAnt; }
            }
        public System.Drawing.Color ColorAct
        {
            get { return colorAct; }
        }
    }
    public class ColorSelectedArgs : EventArgs
    {
        System.Drawing.Color color;
        int pos;
        public ColorSelectedArgs(System.Drawing.Color color,int pos)
        {
            this.color = color;
            this.pos = pos;

        }
        public System.Drawing.Color Color
        {
            get { return color; }
        }
        public int Posicion
        {
            get { return pos; }
        }
    }
}
