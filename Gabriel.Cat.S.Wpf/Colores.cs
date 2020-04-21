using Gabriel.Cat.S.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Gabriel.Cat.S.Utilitats;
namespace Gabriel.Cat.Wpf
{
    public static class Colores
    {
        static TwoKeysList<string, string, System.Windows.Media.Color> colores;
        public static readonly System.Windows.Media.Color[] ListaColores;
        static Colores()
        {
            System.Windows.Media.Color color;
            System.Reflection.PropertyInfo[] pInfo = typeof(System.Windows.Media.Colors).GetProperties();


            colores = new TwoKeysList<string, string, System.Windows.Media.Color>();

            for (int i = 0; i < pInfo.Length; i++)
            {
                try {
                    color = (System.Windows.Media.Color)pInfo[i].GetGetMethod().Invoke(null, null);
                    colores.Add(pInfo[i].Name, color.ToString(), color);
                }
                catch { }
            }
            ListaColores = colores.ValueToArray();
        }
        public static IEnumerable<System.Windows.Media.Color> GetColors()
        {
            return ListaColores;
        }
        public static string GetName(System.Windows.Media.Color color)
        {
            return colores.GetTkey1WhithTkey2(color.ToString());
        }

        public static System.Windows.Media.Color GetColor(string name)
        {
            return colores.GetValueWithKey1(name);
        }

    }
}
