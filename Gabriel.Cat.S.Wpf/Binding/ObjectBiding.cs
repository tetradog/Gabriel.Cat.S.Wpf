using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Gabriel.Cat.S.Wpf
{
    /// <summary>
    /// Sirve para simplificar la creación de objectos con DataBinding, Solo se tiene que llamar despues del set a OnPropertyChanged();
    /// </summary>
    public abstract class ObjectBinding : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
