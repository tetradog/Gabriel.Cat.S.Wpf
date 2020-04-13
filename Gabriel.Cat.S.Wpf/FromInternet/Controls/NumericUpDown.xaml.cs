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
            NumValue = 0;
        }


        public double NumValue
        {
            get { return _numValue; }
            set
            {
                double numAnt = _numValue;
                _numValue = value;
                txtNum.TextChanged -= txtNum_TextChanged;
                txtNum.Text = _numValue.ToString();
                txtNum.TextChanged += txtNum_TextChanged;
                if (numAnt != _numValue && ValueChange != null)
                    ValueChange(this, new EventArgs());
            }
        }

       
        public double Margen { get; set; }
 

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue=Math.Round(NumValue+ Margen,3);
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            NumValue = Math.Round(NumValue - Margen, 3);
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            double newValue;
            bool correcto;
            if (txtNum != null)
            {
                correcto = double.TryParse(txtNum.Text, out newValue);
                if (correcto)
                    NumValue = newValue;
                else NumValue = 0;

              

            }
        }
    }
}
