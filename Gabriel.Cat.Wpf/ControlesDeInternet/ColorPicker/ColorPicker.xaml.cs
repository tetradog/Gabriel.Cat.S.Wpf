using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Gabriel.Cat.Extension;
using Gabriel.Cat;
using Gabriel.Cat.Wpf;
//sacado de http://www.codeproject.com/Articles/33001/WPF-A-Simple-Color-Picker-With-Preview
namespace WPFColorPickerLib
{
    /// <summary>
    /// A simple WPF color picker.  The basic idea is to use a Color swatch image and then pick out a single
    /// pixel and use that pixel's RGB values along with the Alpha slider to form a SelectedColor.
    /// 
    /// This class is from Sacha Barber at http://sachabarber.net/?p=424 and http://www.codeproject.com/KB/WPF/WPFColorPicker.aspx.
    /// 
    /// This class borrows an idea or two from the following sources:
    ///  - AlphaSlider and Preview box; Based on an article by ShawnVN's Blog; 
    ///    http://weblogs.asp.net/savanness/archive/2006/12/05/colorcomb-yet-another-color-picker-dialog-for-wpf.aspx.
    ///  - 1*1 pixel copy; Based on an article by Lee Brimelow; http://thewpfblog.com/?p=62.
    /// 
    /// Enhanced by Mark Treadwell (1/2/10):
    ///  - Left click to select the color with no mouse move
    ///  - Set tab behavior
    ///  - Set an initial color (note that the search to set the cursor ellipse delays the initial display)
    ///  - Fix single digit hex displays
    ///  - Add Mouse Wheel support to change the Alpha value
    ///  - Modify color select dragging behavior
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        #region Data

        private DrawingAttributes drawingAttributes = new DrawingAttributes();
        private Color selectedColor = Colors.Transparent;
        private Boolean IsMouseDown = false;
        ImagePointerLocated imagenActual;
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor that initializes the ColorPicker to Black.
        /// </summary>
        public ColorPicker()
          : this(Colors.Black)
        { }

        /// <summary>
        /// Constructor that initializes to ColorPicker to the specified color.
        /// </summary>
        /// <param name="initialColor"></param>
        public ColorPicker(Color initialColor)
        {
            InitializeComponent();
            this.selectedColor = initialColor;
            this.ColorImage.SetImage(Gabriel.Cat.Wpf.Resource1.ColorSwatchSquare1);
            this.ImgCircle1.SetImage(Gabriel.Cat.Wpf.Resource1.ColorSwatchCircle);
            this.ImgCircle1.Tag = new ImagePointerLocated(this.ImgCircle1);
            this.ImgSqaure1.SetImage(Gabriel.Cat.Wpf.Resource1.ColorSwatchSquare1);
            this.ImgSqaure1.Tag = new ImagePointerLocated(this.ImgSqaure1);
            this.ImgSqaure2.SetImage(Gabriel.Cat.Wpf.Resource1.ColorSwatchSquare2);
            this.ImgSqaure2.Tag = new ImagePointerLocated(this.ImgSqaure2);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or privately sets the Selected Color.
        /// </summary>
        public Color SelectedColor
        {
            get { return selectedColor; }
            private set
            {
                if (selectedColor != value)
                {
                    this.selectedColor = value;
                    CreateAlphaLinearBrush();
                    UpdateTextBoxes();
                    UpdateInk();

                }
            }
        }

        /// <summary>
        /// Sets the initial Selected Color.
        /// </summary>
        public Color InitialColor
        {
            set
            {
                SelectedColor = value;
                CreateAlphaLinearBrush();
                AlphaSlider.Value = value.A;
                UpdateCursorEllipse(value);
            }
        }

        #endregion

        #region Control Events

        /// <summary>
        /// 
        /// </summary>
        private void AlphaSlider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int change = e.Delta / Math.Abs(e.Delta);
            AlphaSlider.Value = AlphaSlider.Value + (double)change;
        }

        /// <summary>
        /// Update SelectedColor Alpha based on Slider value.
        /// </summary>
        private void AlphaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SelectedColor = Color.FromArgb((byte)AlphaSlider.Value, SelectedColor.R, SelectedColor.G, SelectedColor.B);
        }

        /// <summary>
        /// Update the SelectedColor if moving the mouse with the left button down.
        /// </summary>
        private void CanvasImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown) UpdateColor();
        }

        /// <summary>
        /// Handle MouseDown event.
        /// </summary>
        private void CanvasImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsMouseDown = true;
            UpdateColor();
        }

        /// <summary>
        /// Handle MouseUp event.
        /// </summary>
        private void CanvasImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseDown = false;
            //UpdateColor();
        }

        /// <summary>
        /// Apply the new Swatch image based on user requested swatch.
        /// </summary>
        private void Swatch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (sender as Image);
            ColorImage.Source = img.Source;
            imagenActual = img.Tag as ImagePointerLocated;
            UpdateCursorEllipse(SelectedColor);
        }

        #endregion // Control Events

        #region Private Methods

        /// <summary>
        /// Creates a new LinearGradientBrush background for the Alpha area slider.  This is based on the current color.
        /// </summary>
        private void CreateAlphaLinearBrush()
        {
            Color startColor = Color.FromArgb((byte)0, SelectedColor.R, SelectedColor.G, SelectedColor.B);
            Color endColor = Color.FromArgb((byte)255, SelectedColor.R, SelectedColor.G, SelectedColor.B);
            LinearGradientBrush alphaBrush = new LinearGradientBrush(startColor, endColor, new Point(0, 0), new Point(1, 0));
            AlphaBorder.Background = alphaBrush;
        }

        /// <summary>
        /// Sets a new Selected Color based on the color of the pixel under the mouse pointer.
        /// </summary>
        private void UpdateColor()
        {

            // Test to ensure we do not get bad mouse positions along the edges
            int imageX = (int)Mouse.GetPosition(canvasImage).X;
            int imageY = (int)Mouse.GetPosition(canvasImage).Y;
            bool actualizar = !((imageX < 0) || (imageY < 0) || (imageX > ColorImage.Width - 1) || (imageY > ColorImage.Height - 1));

            // Get the single pixel under the mouse into a bitmap and copy it to a byte array
            CroppedBitmap cb;
            byte[] pixels;
            if (actualizar)
            {
                cb = new CroppedBitmap(ColorImage.Source as BitmapSource, new Int32Rect(imageX, imageY, 1, 1));
                pixels = new byte[4];
                cb.CopyPixels(pixels, 4, 0);
                // Update the mouse cursor position and the Selected Color
                ellipsePixel.SetValue(Canvas.LeftProperty, (double)(Mouse.GetPosition(canvasImage).X - (ellipsePixel.Width / 2.0)));
                ellipsePixel.SetValue(Canvas.TopProperty, (double)(Mouse.GetPosition(canvasImage).Y - (ellipsePixel.Width / 2.0)));
                canvasImage.InvalidateVisual();
                // Set the Selected Color based on the cursor pixel and Alpha Slider value
                SelectedColor = Color.FromArgb((byte)AlphaSlider.Value, pixels[2], pixels[1], pixels[0]);
            }
        }

        /// <summary>
        /// Update the mouse cursor ellipse position.
        /// </summary>
        private void UpdateCursorEllipse(Color searchColor)
        {
            Point pointEncontrado;
            double searchX, searchY;
            try
            {
                // Default to the top left if no match is found
                 pointEncontrado= imagenActual.GetPoint(searchColor.ToDrawingColor()).ToWindowsPoint();
                searchX = pointEncontrado.X;
                searchY = pointEncontrado.Y;
            }catch
            {
                searchX = 0;
                searchY = 0;
            }
                // Update the mouse cursor ellipse position
                ellipsePixel.SetValue(Canvas.LeftProperty, ((double)searchX - (ellipsePixel.Width / 2.0)));
                ellipsePixel.SetValue(Canvas.TopProperty, ((double)searchY - (ellipsePixel.Width / 2.0)));
            
        }

        /// <summary>
        /// Update text box values based on the Selected Color.
        /// </summary>
        private void UpdateTextBoxes()
        {
            txtAlpha.Text = SelectedColor.A.ToString();
            txtAlphaHex.Text = SelectedColor.A.ToString("X2");
            txtRed.Text = SelectedColor.R.ToString();
            txtRedHex.Text = SelectedColor.R.ToString("X2");
            txtGreen.Text = SelectedColor.G.ToString();
            txtGreenHex.Text = SelectedColor.G.ToString("X2");
            txtBlue.Text = SelectedColor.B.ToString();
            txtBlueHex.Text = SelectedColor.B.ToString("X2");
            txtAll.Text = String.Format("#{0}{1}{2}{3}", txtAlphaHex.Text, txtRedHex.Text, txtGreenHex.Text, txtBlueHex.Text);
        }

        /// <summary>
        /// Updates the Ink strokes based on the Selected Color.
        /// </summary>
        private void UpdateInk()
        {
            drawingAttributes.Color = SelectedColor;
            drawingAttributes.StylusTip = StylusTip.Ellipse;
            drawingAttributes.Width = 5;
            // Update drawing attributes on previewPresenter
            for (int i = 0; i < previewPresenter.Strokes.Count; i++)
            {
                previewPresenter.Strokes[i].DrawingAttributes = drawingAttributes;
            }
        }

        #endregion // Update Methods

        private void txtAll_TextChanged(object sender, TextChangedEventArgs e)
        {
            const int CARACTERESBYTEMAX = 2;
            const int PARTESCOLORARGB = 4;
            const int CARACTERESBYTESARGB = CARACTERESBYTEMAX * PARTESCOLORARGB;
            TextBox txt = (TextBox)sender;
            //valido que los campos Hex esten bien escritos :)
            switch (txt.Name)
            {

                case "txtRedHex":
                case "txtGreenHex":
                case "txtBlueHex":
                case "txtAlphaHex":
                    e.Handled = !Gabriel.Cat.Hex.ValidaString(txt.Text) && txt.Text.Length > CARACTERESBYTEMAX;
                    if (!e.Handled)
                    {
                        if (txt.Text.Length == 0)
                            txt.Text = "0";
                        else
                        {
                            txt.Text = txt.Text.ToUpper();
                            txt.CaretIndex = txt.Text.Length;
                        }
                    }

                    break;
                case "txtAll":
                    if (txt.Text[0] == '#')
                        txt.Text = txt.Text.Remove(0, 1);
                    e.Handled = !Gabriel.Cat.Hex.ValidaString(txt.Text) && txt.Text.Length > (CARACTERESBYTESARGB);
                    if (!e.Handled) {
                        if (txt.Text.Length == 0)
                            txt.Text = "0";
                        else
                        {
                            txt.Text = '#' + txt.Text.ToUpper();
                            txt.CaretIndex = txt.Text.Length;
                        }
                    }

                    break;


            }

            if (!e.Handled)//si esta bien escrito valido los otros campos
            {
                try
                {
                    switch (txt.Name)
                    {
                        case "txtAlpha":
                            SelectedColor = Color.FromArgb(Convert.ToByte(txt.Text), SelectedColor.R, SelectedColor.G, SelectedColor.B);
                            txtAlphaHex.Text = (Hex)SelectedColor.A;
                            break;
                        case "txtAlphaHex":
                            SelectedColor = Color.FromArgb((byte)((Hex)txt.Text), SelectedColor.R, SelectedColor.G, SelectedColor.B);
                            txtAlpha.Text = "" + SelectedColor.A;
                            break;

                        case "txtRed":
                            SelectedColor = Color.FromArgb(SelectedColor.A, Convert.ToByte(txt.Text), SelectedColor.G, SelectedColor.B);
                            txtRedHex.Text = (Hex)SelectedColor.R;
                            break;
                        case "txtRedHex":
                            SelectedColor = Color.FromArgb(SelectedColor.A, (byte)((Hex)txt.Text), SelectedColor.G, SelectedColor.B);
                            txtRed.Text = "" + SelectedColor.R;
                            break;

                        case "txtGreen":
                            SelectedColor = Color.FromArgb(SelectedColor.A, SelectedColor.R, Convert.ToByte(txt.Text), SelectedColor.B);
                            txtGreenHex.Text = (Hex)SelectedColor.G;
                            break;
                        case "txtGreenHex":
                            SelectedColor = Color.FromArgb(SelectedColor.A, SelectedColor.R, (byte)((Hex)txt.Text), SelectedColor.B);
                            txtGreen.Text = "" + SelectedColor.G;
                            break;

                        case "txtBlue":
                            SelectedColor = Color.FromArgb(SelectedColor.A, SelectedColor.R, SelectedColor.G, Convert.ToByte(txt.Text));
                            txtBlueHex.Text = (Hex)SelectedColor.B;
                            break;
                        case "txtBlueHex":
                            SelectedColor = Color.FromArgb(SelectedColor.A, SelectedColor.R, SelectedColor.G, (byte)((Hex)txt.Text));
                            txtBlue.Text = "" + SelectedColor.B;
                            break;
                        case "txtAll":
                            SelectedColor = Serializar.ToColor(Serializar.GetBytes((int)(Hex)txtAll.Text)).ToMediaColor();
                            txtAlphaHex.Text = (Hex)SelectedColor.A;
                            txtAlpha.Text = "" + SelectedColor.A;
                            txtRedHex.Text = (Hex)SelectedColor.R;
                            txtRed.Text = "" + SelectedColor.R;
                            txtGreenHex.Text = (Hex)SelectedColor.G;
                            txtGreen.Text = "" + SelectedColor.G;
                            txtBlueHex.Text = (Hex)SelectedColor.B;
                            txtBlue.Text = "" + SelectedColor.B;
                            break;
                    }
                }catch { e.Handled = true; }//si peta en el convert To Byte es que se han pasado con el numero!
            }
        }
    }

}
