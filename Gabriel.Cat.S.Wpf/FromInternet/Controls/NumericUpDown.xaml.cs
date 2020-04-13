using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gabriel.Cat.S.Wpf.FromInternet.Controls
{
    /// <summary>
    /// source https://stackoverflow.com/questions/841293/where-is-the-wpf-numeric-updown-control
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        private double _numValue = 0;
        public event EventHandler ValueChange;
        public NumericUpDown()
        {
            InitializeComponent();
            Margen = 0.1;
            txtNum.Text = _numValue.ToString();
        }
        

        public double NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                txtNum.Text = value.ToString();
            }
        }

        public double Margen { get; set; }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue+=Margen;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            NumValue -= Margen;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            double valueAnt;
            if (txtNum != null)
            {
                valueAnt = _numValue;
                if (double.TryParse(txtNum.Text, out _numValue))
                {
                    if (valueAnt!=_numValue&&ValueChange != null)
                        ValueChange(this, new EventArgs());
                }else txtNum.Text = _numValue.ToString();
            }
        }
    }
}
