using System;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using Gabriel.Cat.S.Extension;
namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Lógica de interacción para SwitchImg.xaml
    /// </summary>
    public partial class SwitchImg : System.Windows.Controls.Image
    {
        bool estadoOn;
        ImageSource imgOn, imgOff;

        public event EventHandler<bool> SwitchChanged;

        public SwitchImg()
        {
            estadoOn = false;
            InitializeComponent();
            CambiarHaciendoClick = true;
        }

        public SwitchImg(Bitmap imgOn, Bitmap imgOff,bool estaOn=false):this()
        {
            this.imgOn = imgOn.ToImage().Source;
            this.imgOff = imgOff.ToImage().Source;
            
            EstadoOn = estaOn;
        }
        public bool EstadoOn
        {
            get
            {
                return estadoOn;
            }

            set
            {
                if (estadoOn != value)
                {
                    estadoOn = value;
                    PonImagen();
                }

            }
        }

        public ImageSource ImgOn
        {
            get
            {
                return imgOn;
            }

            set
            {
                imgOn = value;
                if (EstadoOn)
                    Source = imgOn;
            }
        }

        public ImageSource ImgOff
        {
            get
            {
                return imgOff;
            }

            set
            {
                imgOff = value;
                if (!EstadoOn)
                    Source = imgOff;
            }
        }

        public bool CambiarHaciendoClick { get; set; }

        private void Image_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CambiarHaciendoClick)
            {
                EstadoOn = !EstadoOn;
                SwitchChanged?.Invoke(this, EstadoOn);
            }

        }

        private void PonImagen()
        {  
            if (EstadoOn)
                Source = imgOn;
            else
                Source = imgOff;
        }
    }
}
