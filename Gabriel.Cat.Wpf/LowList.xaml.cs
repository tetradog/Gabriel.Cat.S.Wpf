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

namespace Gabriel.Cat.Wpf
{
	/// <summary>
	/// Interaction logic for LowList.xaml
	/// </summary>
	public partial class LowList<TControl,TData> : UserControl 
		where TControl: ISetData<TData>,new()
	{
		Llista<TControl> lstControles;
		List<UserControl> lstControlesVisualizados;
		public LowList()
		{
			lstControles=new Llista<TControl>();
			lstControlesVisualizados=new List<UserControl>();
			lstControles.Updated+=ActualizaListaVisualizada;
	
		}

		public IList<TControl> Controles {
			get {
				return lstControles;
			}
		}

		void ActualizaListaVisualizada(object sender=null, EventArgs e=null)
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
	public interface ISetData<T>
	{
		void SetData(T data);
		void Clear();
		
	}
}