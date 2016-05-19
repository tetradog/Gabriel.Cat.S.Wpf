using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Gabriel.Cat.Extension;
namespace Gabriel.Cat.Wpf
{
   public class ImagePointerLocated
    {
        System.Windows.Controls.Image imagen;
        LlistaOrdenada<int, Point> colorLocatedList;
        byte[] bytesImg;

        

        public ImagePointerLocated(System.Windows.Controls.Image img)
        {
            colorLocatedList = new LlistaOrdenada<int, Point>();
            Imagen = img;

        }
        public System.Windows.Controls.Image Imagen
        {
            get
            {
                return imagen;
            }
            set {
                imagen = value;
                bytesImg = imagen.ToBitmap().GetBytes();
                colorLocatedList.Buida();
            }
        }
        public Point GetPoint(Color color)
        {
            int colorInt = color.ToArgb();
            Point location;
            if (colorLocatedList.Existeix(colorInt))
                location = colorLocatedList[colorInt];
            else
                location = GetPoint(colorInt);
            return location;
        }

        public  Point GetPoint(int colorInt)
        {
            const int ARGB = 4;
            int posicion;
            byte[] bytesColor;
            Point location=default(Point);
            bool encontrado=false;
            if (colorLocatedList.Existeix(colorInt))
                location = colorLocatedList[colorInt];
            else
            {
                bytesColor = Serializar.GetBytes(colorInt);
                for (int y = 0, yFin =Convert.ToInt32(imagen.Height), xFin =Convert.ToInt32(imagen.Width) * ARGB; y<yFin&&!encontrado;y++)
                    for(int x=0;x<xFin&&!encontrado;x+= ARGB)
                    {
                        posicion = x + (y * xFin);
                        encontrado = bytesImg[posicion] == bytesColor[0] && bytesImg[posicion + 1] == bytesColor[1] && bytesImg[posicion + 2] == bytesColor[2] && bytesImg[posicion + 3] == bytesColor[3];
                        if (encontrado)
                            location = new Point(x, y);
                    }
                if (!encontrado)
                    throw new ArgumentOutOfRangeException("El color no esta dentro de la imagen!");
                else colorLocatedList.Afegir(colorInt, location);
            }
            return location;
        }
    }
}
