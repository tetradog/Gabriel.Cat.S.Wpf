using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Gabriel.Cat.Extension;
using Gabriel.Cat.S.Utilitats;

namespace Gabriel.Cat.Wpf
{
    public class ImagePointerLocated : BitmapPointerLocated
    {
        public ImagePointerLocated(System.Windows.Controls.Image img) : base(img.ToBitmap())
        {

        }
    }

}
