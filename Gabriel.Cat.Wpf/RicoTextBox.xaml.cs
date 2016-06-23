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
using System.Xml;
using System.Threading;
using System.Drawing;

namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Lógica de interacción para RicoTextBox.xaml
    /// </summary>
    public partial class RicoTextBox : RichTextBox
    {
        private ulong textChangedTimes;
     
        public RicoTextBox():base()
        {
            try
            {
                ContextMenu = new RichTextBoxContextMenu(this);
                TextChanged += (s, o) => TextChangedTimes++;
            }
            catch { }
        }
        public string Text
        {
            get
            {
                string txt = "";
                Action act = () => txt = this.GetText();
                Dispatcher.BeginInvoke(act).Wait();
                return txt;
            }
            set
            {
                Action act;
                if (value == null)
                    value = "";
                act = () => this.SetText(value);
                Dispatcher.BeginInvoke(act).Wait();
                TextChangedTimes++;
            }
        }
        public string TextWithFormat
        {
            get
            {
                string txtWithformat = "";
                Action act = () => txtWithformat = this.ToStringRtf();
                Dispatcher.BeginInvoke(act).Wait();
                return txtWithformat;
            }
            set
            {
                Action act = () => this.LoadStringRtf(value);
                Dispatcher.BeginInvoke(act).Wait();
                TextChangedTimes++;
            }
        }
        public ulong TextChangedTimes
        {
            get { return textChangedTimes; }
            private set { textChangedTimes = value; }
        }

    }
}
