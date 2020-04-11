/*
 * Creado por SharpDevelop.
 * Usuario: Pikachu240
 * Fecha: 21/11/2017
 * Hora: 20:55
 * Licencia GNU GPL V3
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Gabriel.Cat.S.Utilitats;
using Gabriel.Cat.S.Extension;
namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Interaction logic for LowList.xaml
    /// </summary>
    public partial class LowList : UserControl
    {
        Llista<IControlListaData> lstControles;
        List<ControlLista> lstControlesVisualizados;
        public LowList()
        {
            lstControlesVisualizados = new List<ControlLista>();

        }

        public Llista<IControlListaData> Controles
        {
            get
            {
                return lstControles;
            }
            set
            {
                if (lstControles != null)
                    lstControles.Updated -= ActualizaListaVisualizada;
                lstControles = value;
                lstControles.Updated += ActualizaListaVisualizada;
                ActualizaListaVisualizada();

            }

        }

        void ActualizaListaVisualizada(object sender = null, EventArgs e = null)
        {
            //obtengo el primer elemento que se ve segun el scroll
            //voy poniendo los elementos (si hace falta más controles hacer más)
            //parar cuando el ultimo elemento no se vea.

        }
        void ScrollViwerLista_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ActualizaListaVisualizada();
        }
        void ScrollViwerLista_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ActualizaListaVisualizada();
        }
    }
    public interface IControlListaData
    {
        /// <summary>
        /// returns dictorionary propertyName,value
        /// </summary>
        /// <returns></returns>
        IDictionary<string, object> GetData();

    }
    public class ControlListaData : IControlListaData
    {
        #region IControlListaData implementation
        public IDictionary<string, object> GetData()
        {
            SortedList<string, object> dic = new SortedList<string, object>();
            IList<Propiedad> propiedades = this.GetPropiedades();//mirar si coge los valores de los hijos... 
            for (int i = 0; i < propiedades.Count; i++)
                dic.Add(propiedades[i].Info.Nombre, propiedades[i].Objeto);
            return dic;

        }
        #endregion

    }
    public abstract class ControlLista : UIElement
    {
        public abstract void Clear();
        public void SetData(IControlListaData data)
        {
            IList<Propiedad> propiedades = this.GetPropiedades();
            IDictionary<string, object> dic = data.GetData();
            for (int i = 0; i < propiedades.Count; i++)
            {
                if (dic.ContainsKey(propiedades[i].Info.Nombre))
                    this.SetProperty(propiedades[i].Info.Nombre, dic[propiedades[i].Info.Nombre]);
            }
        }

    }
}