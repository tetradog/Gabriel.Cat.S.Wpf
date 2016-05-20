using System;
using System.Windows;
using System.Windows.Media;
using Gabriel.Cat.Extension;
using System.Windows.Controls;
//sacado de http://www.codeproject.com/Articles/33001/WPF-A-Simple-Color-Picker-With-Preview
namespace WPFColorPickerLib
{
	/// <summary>
	/// Holds a ColorPicker control, and exposes the ColorPicker SelectedColor.
	/// 
	/// Enhanced by Mark Treadwell (1/2/10) to include:
	///  - Added ability to set ColorPicker initial color via constructor or property
	///  - Use of Button's IsDefault and IsCancel properties
	///  - Setting tab behavior
	/// </summary>
	public partial class ColorDialog : Window
	{
		#region Constructors
        public bool EstaCancelado { get; private set; }
		/// <summary>
		/// Default constructor initializes to Black.
		/// </summary>
		public ColorDialog()
			: this(Colors.Black)
		{
		}

		/// <summary>
		/// Constructor with an initial color.
		/// </summary>
		/// <param name="initialColor">Color to set the ColorPicker to.</param>
		public ColorDialog(Color initialColor)
		{
			Image imgIco = new Image();
			InitializeComponent();
			imgIco.SetImage(Gabriel.Cat.Wpf.Resource1.ColorSwatchSquare1);
			Icon = imgIco.Source;
			colorPicker.SelectedColor = initialColor;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets/sets the ColorDialog color.
		/// </summary>
        public ColorPicker ColorPicker
        { get { return colorPicker; } }
		#endregion

		#region Event Handlers

		/// <summary>
		/// Close ColorDialog, accepting color selection.
		/// </summary>
		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			EstaCancelado = false;
            this.Hide();
		}

		/// <summary>
		///  Close ColorDialog, rejecting color selection.
		/// </summary>
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			EstaCancelado = true;
		}

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            EstaCancelado = true;

        }
    }
}
