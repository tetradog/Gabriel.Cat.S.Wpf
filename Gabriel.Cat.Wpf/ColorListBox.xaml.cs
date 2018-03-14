using System;
using System.Collections;
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
using Gabriel.Cat.S.Extension;
using Gabriel.Cat.S.Utilitats;

namespace Gabriel.Cat.Wpf
{
    public delegate void ItemSelectedEventHandler(Object objSelected, ItemArgs arg);
    /// <summary>
    /// Interaction logic for ColorListBox.xaml
    /// </summary>
    public partial class ColorListBox : UserControl, IEnumerable<ItemColorList>
    {
        public enum TipoSeleccion
        {
            One, More
        }
        Llista<ItemColorList> objectes;//si se pudiese indexar por la referencia del objeto
        bool onlyOneSelecction;
        System.Windows.Media.Color colorPorDefecto;
        System.Windows.Media.Color colorSeleccionadoPorDefecto;
        ItemColorList itemSelectedActual;
        Llista<ItemColorList> itemsSelectedActual;
        TipoSeleccion tipo;
        public event ItemSelectedEventHandler ItemSelected;
        bool keyCtrModificaTipoSeleccion;
        private bool pulsaControl;
        private bool ordenada;

        public ColorListBox()
        {
            ordenada = false;
            itemsSelectedActual = new Llista<ItemColorList>();
            tipo = TipoSeleccion.More;
            colorPorDefecto = SystemColors.ControlColor;
            colorSeleccionadoPorDefecto = SystemColors.ControlDarkColor;
            onlyOneSelecction = true;
            objectes = new Llista<ItemColorList>();
            InitializeComponent();
            keyCtrModificaTipoSeleccion = true;
            Keyboard.AddKeyDownHandler(this, SiPulsaControl);
            Keyboard.AddKeyUpHandler(this, SiNoPulsaControl);
        }
        public bool OnlyOneSelecction
        {
            get
            {
                return onlyOneSelecction;
            }

            set
            {
                onlyOneSelecction = value;
                if (onlyOneSelecction)
                    SelectionType = TipoSeleccion.One;
                else SelectionType = TipoSeleccion.More;
            }
        }

        public TipoSeleccion SelectionType
        {
            get
            {
                return tipo;
            }

            set
            {
                tipo = value;//asi asta que no se haga clic en un elemento no se actualiza!!
            }
        }

        public bool KeyCtrModificaTipoSeleccion
        {
            get
            {
                return keyCtrModificaTipoSeleccion;
            }

            set
            {
                keyCtrModificaTipoSeleccion = value;
            }
        }
        public bool Sorted
        {
            get { return ordenada; }
            set
            {
                ordenada = value;
                if (ordenada)
                { Ordena(); }
            }
        }
        public ItemColorList this[int pos]
        {
            get { return objectes[pos]; }
            set { objectes[pos] = value; }
        }
        private void SiNoPulsaControl(object sender, KeyEventArgs e)
        {
            if (KeyCtrModificaTipoSeleccion && pulsaControl)
            {
                pulsaControl = e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl;
                SelectionType = !pulsaControl ? ColorListBox.TipoSeleccion.More : ColorListBox.TipoSeleccion.One;
            }
        }

        private void SiPulsaControl(object sender, KeyEventArgs e)
        {
            if (KeyCtrModificaTipoSeleccion)
            {
                pulsaControl = e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl;
                SelectionType = pulsaControl ? ColorListBox.TipoSeleccion.More : ColorListBox.TipoSeleccion.One;
            }
        }

        public void SelectItem(object obj)
        {
            objectes.WhileEach((item) =>
            {
                bool salir;
                if (item.Object == obj)
                {
                   
                    item.Seleccionado = false;
                    ElementoSeleccionado(item, null); salir = false;
                }
                else salir = true;
                return salir;
            });
        }
        public void UnSelectItem(object obj)
        {
            
            objectes.WhileEach((item) =>
            {
                bool salir;
                if (item.Object == obj)
                {
                    item.Seleccionado = true;
                    ElementoSeleccionado(item, null); salir = false;
                }
                else salir = true;
                return salir;
            });
        }
        public void Add(Object obj)
        { Add(obj, colorPorDefecto, colorSeleccionadoPorDefecto); }
        public void Add(Object obj, System.Windows.Media.Color color)
        { Add(obj, color, colorSeleccionadoPorDefecto); }
        public void Add(Object obj, System.Windows.Media.Color color, System.Windows.Media.Color colorSeleccionado)
        {
            Action act = () =>
            {
                ItemColorList item = new ItemColorList(obj);
                item.Color = color;
                item.ColorSeleccionado = colorSeleccionado;
                Add(item);
            };
            Dispatcher.BeginInvoke(act);
        }
        public void Add(Object obj, System.Windows.Media.Color color, bool invertirColorAlSeleccionarlo)
        {
            Action act = () =>
            {
                ItemColorList item = new ItemColorList(obj);
                item.Color = color;
                item.InvertirColorAlSeleccionar = invertirColorAlSeleccionarlo;
                Add(item);
            };
            Dispatcher.BeginInvoke(act);



        }
        public void Add(ItemColorList item)
        {
            Action act = () =>
         {
             item.HorizontalAlignment = HorizontalAlignment.Left;
             objectes.Add(item);
             stkPanel.Children.Add(item);
             if (Sorted)
                 Ordena();
             //le pongo el click
             item.MouseLeftButtonUp += ElementoSeleccionado;
         };
            Dispatcher.BeginInvoke(act);
        }



        private void ElementoSeleccionado(object sender, MouseButtonEventArgs e)
        {

            ItemColorList item = sender as ItemColorList;

            if (item != null)
            {//han seleccionado un item!
                //tengo cambiar la seleccion del item
                //si esta seleccionado y solo tiene que haber uno deselecciono el anterior

                item.Seleccionado = !item.Seleccionado;
                if (item.Seleccionado)
                {
                    if (tipo == TipoSeleccion.One)
                    {
                        if (itemSelectedActual != null)
                            itemSelectedActual.Seleccionado = false;
                        itemSelectedActual = item;

                    }
                    else
                        itemsSelectedActual.Add(item);
                }
                else if (item == itemSelectedActual)
                    itemSelectedActual = null;
                else if (tipo == TipoSeleccion.More)
                    itemsSelectedActual.Remove(item);

                if (tipo == TipoSeleccion.One && itemsSelectedActual.Count > 0)
                {
                    for (int i = 0; i < itemsSelectedActual.Count; i++)
                        itemsSelectedActual[i].Seleccionado = false;
                    itemsSelectedActual.Clear();
                    if (itemSelectedActual != null)
                        itemSelectedActual.Seleccionado = false;
                    ElementoSeleccionado(sender, e);
                    item.Seleccionado = true;
                }
                if (ItemSelected != null && item.Seleccionado)
                    ItemSelected(item.Object, new ItemArgs(stkPanel.Children.IndexOf(item), item));
            }
        }

        public void AddRange(IEnumerable<Object> objs)
        { AddRange(objs, colorPorDefecto); }
        public void AddRange(IEnumerable<Object> objs, System.Windows.Media.Color color)
        {
            AddRange(objs, color, colorSeleccionadoPorDefecto);
        }
        public void AddRange(IEnumerable<Object> objs, System.Windows.Media.Color color, System.Windows.Media.Color colorSeleccionado)
        {
            foreach (Object obj in objs)
                Add(obj, color, colorSeleccionado);
        }
        public void AddRange(IEnumerable<Object> objs, System.Windows.Media.Color color, bool invertirColorSeleccionado)
        {
            foreach (Object obj in objs)
                Add(obj, color, invertirColorSeleccionado);
        }
        public void Remove(IEnumerable<Object> objs)
        {
            foreach (Object obj in objs)
                Remove(obj);
        }
        public void Remove(Object obj)
        {
            Action act = () =>
            {
                ItemColorList itemToRemove = null;
                objectes.WhileEach((item) =>
                {
                    if (item.Object == obj)
                        itemToRemove = item;
                    return itemToRemove == null;

                });
                if (itemToRemove != null)
                {
                    objectes.Remove(itemToRemove);
                    //quito el objeto del control
                    itemToRemove.MouseLeftButtonUp -= ElementoSeleccionado;
                    stkPanel.Children.Remove(itemToRemove);
                }
            };
            Dispatcher.BeginInvoke(act);
        }
        public void RemoveAt(int posicion)
        {
            Action act = () =>
            {
                if (posicion < 0 || posicion > objectes.Count)
                    throw new ArgumentOutOfRangeException();
                ItemColorList item = objectes[posicion];
                objectes.Remove(item);
                //elimino de los controles
                stkPanel.Children.Remove(item);
                item.MouseLeftButtonUp -= ElementoSeleccionado;
            };
            Dispatcher.BeginInvoke(act);
        }
        public void Remove(System.Windows.Media.Color elementsWithColorBackGround)
        {
            Remove(GetElementsWithBackGroundColor(elementsWithColorBackGround));
        }

        public Object[] GetElementsWithBackGroundColor(System.Windows.Media.Color elementsWithColorBackGround)
        {
            Llista<Object> objectsWithColor = new Llista<object>();
            for (int i = 0; i < stkPanel.Children.Count; i++)
                if (((ItemColorList)stkPanel.Children[i]).ColorBackGround.Equals(elementsWithColorBackGround))
                    objectsWithColor.Add(((ItemColorList)stkPanel.Children[i]).Object);
            return objectsWithColor.ToArray();
        }
        //poder hacer insertAt(int posicion)
        public void Clear()
        {
            Action act = () =>
            {
                for (int i = 0; i < objectes.Count; i++)
                {
                    objectes[i].MouseLeftButtonUp -= ElementoSeleccionado;
                }
                objectes.Clear();
                itemSelectedActual = null;
                //vacio la lista de controles del control...
                stkPanel.Children.Clear();
            };
            Dispatcher.BeginInvoke(act);
        }
        public Object SelectedItem()
        {
            Object obj = null;

            if (itemSelectedActual != null)
                obj = itemSelectedActual.Object;
            else if (tipo == TipoSeleccion.More)
                objectes.WhileEach((item) =>
                {
                    if (item.Seleccionado)
                        obj = item.Object;
                    return obj == null;

                });
            return obj;
        }
        public Object[] SelectedItems()
        {
            Llista<Object> objectesSeleccionats = new Llista<object>();
            ItemColorList itemActual;
            for (int i = 0; i < objectes.Count; i++)
            {
                itemActual = objectes[i];
                if (itemActual.Seleccionado)
                    objectesSeleccionats.Add(itemActual.Object);
            }
            return objectesSeleccionats.ToArray();
        }

        public void CambiarColor(object obj, System.Windows.Media.Color color)
        {
            Action act = () =>
            {
                ItemColorList item = null;
                objectes.WhileEach((itemHaComprovar) =>
                {

                    if (itemHaComprovar.Object == obj)
                        item = itemHaComprovar;
                    return item == null;
                });
                if (item != null)
                    item.Color = color;
            };
            Dispatcher.BeginInvoke(act);
        }

        public void ClearSelection()
        {
            for (int i = 0; i < objectes.Count; i++)
                objectes[i].Seleccionado = false;
            itemSelectedActual = null;
        }
        public void SelectAt(int posicion)
        {
            if (posicion < 0 || posicion > objectes.Count)
                throw new ArgumentOutOfRangeException();

            ElementoSeleccionado(objectes[posicion], null);
            objectes[posicion].Seleccionado = false;
        }
        public void UnSelectAt(int posicion)
        {
            if (posicion < 0 || posicion > objectes.Count)
                throw new ArgumentOutOfRangeException();
            objectes[posicion].Seleccionado = true;
            ElementoSeleccionado(objectes[posicion], null);

        }

        public IEnumerator<ItemColorList> GetEnumerator()
        {
            return objectes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Ordena()
        {
            Action act = () => stkPanel.Children.Sort();
            Dispatcher.BeginInvoke(act);
        }

        public void VolverASeleccionar()
        {
            if (itemSelectedActual != null)
            {
                if (stkPanel.Children.Contains(itemSelectedActual))
                    itemSelectedActual.Seleccionado = true;
            }
            else if (itemsSelectedActual.Count > 0)
            {
                Llista<ItemColorList> itemsAQuitar = new Llista<ItemColorList>();
                for (int i = 0; i < itemsSelectedActual.Count; i++)
                {
                    if (stkPanel.Children.Contains(itemsSelectedActual[i]))
                    {
                        itemsSelectedActual[i].Seleccionado = true;
                    }
                    else
                        itemsAQuitar.Add(itemsSelectedActual[i]);
                }
                itemsSelectedActual.RemoveRange(itemsAQuitar);
            }
        }

        public bool Existe(object obj)
        {
            bool existe = false;
            objectes.WhileEach((item) => { existe = item.Object == obj; return !existe; });
            return existe;
        }
    }
    public class ItemColorList : UserControl, IComparable, IComparable<ItemColorList>
    {
        TextBlock txtTexto;
        System.Windows.Media.Color color;
        System.Windows.Media.Color colorSeleccionado;
        bool seleccionado;
        bool invertirColorAlSeleccionar;
        object obj;//objeto al que hacen referencia :D
        public ItemColorList()
        {
            Viewbox view = new Viewbox();
            txtTexto = new TextBlock();
            view.Child = txtTexto;
            colorSeleccionado = Colors.Aquamarine;
            seleccionado = false;
            invertirColorAlSeleccionar = false;
            Color = Colors.White;
            //Añado la etiqueta
            AddChild(view);
            ActualizaTexto();


        }
        public ItemColorList(Object obj)
            : this()
        {
            Object = obj;
        }
        public object Object
        {
            get { return obj; }
            set
            {
                obj = value;
                ActualizaTexto();
            }
        }

        public System.Windows.Media.Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                PonColor();
            }
        }



        public System.Windows.Media.Color ColorSeleccionado
        {
            get
            {
                return colorSeleccionado;
            }

            set
            {
                colorSeleccionado = value;
                PonColor();
            }
        }

        public bool Seleccionado
        {
            get
            {
                return seleccionado;
            }

            set
            {
                if (seleccionado != value)
                {
                    seleccionado = value;
                    PonColor();
                }
            }
        }

        public bool InvertirColorAlSeleccionar
        {
            get
            {
                return invertirColorAlSeleccionar;
            }

            set
            {
                invertirColorAlSeleccionar = value; PonColor();
            }
        }

        private void ActualizaTexto()
        {
            Action act = () =>
            {
                if (obj != null)
                {
                    //pongo el toString
                    txtTexto.Text = obj.ToString();
                }
                else
                {
                    //pongo NoItem
                    txtTexto.Text = "NoItem";
                }
            };
            Dispatcher.BeginInvoke(act);

        }
        private void PonColor()
        {
            Action act = () =>
            {
                if (Seleccionado)
                {
                    //pongo el colorSeleccionado
                    if (invertirColorAlSeleccionar)
                    {
                        //pongo el colorContrario al normal
                        this.Background = new SolidColorBrush(color.Invertir());
                    }
                    else
                    {
                        //pongo el colorSeleccionado
                        this.Background = new SolidColorBrush(colorSeleccionado);
                    }
                }
                else
                {
                    //pongo el colorNormal
                    this.Background = new SolidColorBrush(color);
                }
                //si el color es claro se ponen letras oscuras sino blancas
                if (ColorBackGround.EsClaro())
                {
                    txtTexto.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    txtTexto.Foreground = new SolidColorBrush(Colors.White);
                }

            };
            Dispatcher.BeginInvoke(act);
        }
        public System.Windows.Media.Color ColorBackGround
        {
            get { return ((SolidColorBrush)this.Background).Color; }
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as ItemColorList);
        }

        public int CompareTo(ItemColorList other)
        {
            if (other != null)
                return String.Compare(txtTexto.Text, other.txtTexto.Text);
            else
                return -1;
        }
    }
    public class ItemArgs : EventArgs
    {
        int posicion;
        ItemColorList item;

        public ItemArgs(int posicion, ItemColorList item)
        {
            this.Posicion = posicion;
            this.Item = item;
        }
        public int Posicion
        {
            get
            {
                return posicion;
            }

            private set
            {
                posicion = value;
            }
        }
        public ItemColorList Item
        {
            get
            {
                return item;
            }

            private set
            {
                item = value;
            }
        }
    }

}
