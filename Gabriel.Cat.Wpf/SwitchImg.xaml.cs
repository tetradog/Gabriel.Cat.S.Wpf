using System;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using Gabriel.Cat.Extension;
namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Lógica de interacción para SwitchImg.xaml
    /// </summary>
    public partial class SwitchImg : System.Windows.Controls.Image
    {
        bool cambiarHaciendoClick;
        bool estadoOn;
        ImageSource imgOn, imgOff;

        public event EventHandler<bool> SwitchChanged;

        public SwitchImg()
        {
            estadoOn = false;
            InitializeComponent();
            cambiarHaciendoClick = true;
        }

        public SwitchImg(Bitmap imgOn, Bitmap imgOff):this()
        {
            this.imgOn = imgOn.ToImage().Source;
            this.SetImage(imgOff);
            this.imgOff = Source;
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
                if (estadoOn)
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
                if (!estadoOn)
                    Source = imgOff;
            }
        }

        public bool CambiarHaciendoClick
        {
            get
            {
                return cambiarHaciendoClick;
            }

            set
            {
                cambiarHaciendoClick = value;
            }
        }

        private void Image_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (cambiarHaciendoClick)
            {
                estadoOn = !estadoOn;
                PonImagen();

                if (SwitchChanged != null)
                    SwitchChanged(this, estadoOn);
            }

        }

        private void PonImagen()
        {  
            if (estadoOn)
                Source = imgOn;
            else
                Source = imgOff;
        }
    }
}
