using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
/*fuente:https://loboantonio.wordpress.com/2012/01/29/wpf-ventana-sin-bordes/*/
namespace Gabriel.Cat.Wpf
{
    /// <summary>
    /// Por desarrollar...
    /// </summary>
    public class WindowSinBordes:Window
    {
        Button btnClose, btnMinimize, btnMaximize;
        TextBlock txtTitle;
        Grid gridContenido;
        

        public WindowSinBordes()
        {
            Border border;
            DockPanel dockPanel;
            Grid gridBotones;
            Rectangle rctTitleBar;            
            DropShadowEffect effect;

            //Configuro la herencia
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            //pongo los botones para cerrar y minimizar
            border = new Border();
               border.Margin = new Thickness(10);
               border.BorderBrush = Brushes.Red;
               border.BorderThickness =new Thickness(0.5);
            border.MouseLeftButtonDown += titleBar_MouseLeftButtonDown;
            effect = new DropShadowEffect();
               effect.Color = Colors.Black;
               effect.Direction = 320;
               effect.BlurRadius = 15;
               effect.ShadowDepth = 3;
            border.Effect = effect;
            dockPanel = new DockPanel();
               dockPanel.LastChildFill = true;
               dockPanel.Background = Brushes.White;
            gridBotones = new Grid();
               gridBotones.Height = 25;
               dockPanel.Children.Add(gridBotones);
               DockPanel.SetDock(gridBotones, Dock.Top);
            txtTitle = new TextBlock();
               txtTitle.Text = "{Binding Path=Title, RelativeSource={RelativeSource Mode=FindAncestor, AncestorType=Window}}";
               txtTitle.HorizontalAlignment = HorizontalAlignment.Center;
            rctTitleBar = new Rectangle();
               rctTitleBar.Fill = Brushes.Transparent;
               rctTitleBar.MouseLeftButtonDown += titleBar_MouseLeftButtonDown;
               gridBotones.Children.Add(rctTitleBar);
            btnClose = new Button();
            //pongo el style btnClose
                btnClose.HorizontalAlignment = HorizontalAlignment.Right;
                btnClose.Width = 15;
                btnClose.Margin = new Thickness(0, 0,5, 0);
                btnClose.Height = 15;
                btnClose.Click += closeButton_Click;
                gridBotones.Children.Add(btnClose);
            btnMaximize = new Button();
            //pongo el style btnMaximize
              btnMaximize.HorizontalAlignment = HorizontalAlignment.Right;
              btnMaximize.Width = 15;
              btnMaximize.Height = 9;
              btnMaximize.Margin = new Thickness(0, 0, 30, 0);
              btnMaximize.Click += btnMaximize_Click;
              gridBotones.Children.Add(btnMaximize);
            btnMinimize = new Button();
            //pongo el style btnMinimize
               btnMinimize.HorizontalAlignment = HorizontalAlignment.Right;
               btnMinimize.Width = 15;
               btnMinimize.Height = 9;
               btnMinimize.Margin = new Thickness(0, 0, 55, 0);
               btnMinimize.Click += btnMaximize_Click;
               gridBotones.Children.Add(btnMinimize);
            gridContenido = new Grid();
            gridContenido.MouseLeftButtonDown += titleBar_MouseLeftButtonDown;
                dockPanel.Children.Add(gridContenido);
            border.Child = dockPanel;
            this.AddChild(border);
        }
        /*poner para ocultar los botones*/
        public Button BtnClose
        {
            get
            {
                return btnClose;
            }


        }

        public Button BtnMinimize
        {
            get
            {
                return btnMinimize;
            }


        }

        public Button BtnMaximize
        {
            get
            {
                return btnMaximize;
            }

        }

        public TextBlock TxtTitle
        {
            get
            {
                return txtTitle;
            }


        }
        public Grid GridContenido
        {
            get
            {
                return gridContenido;
            }

        }
        protected override void AddChild(object value)
        {
            base.AddChild(value);
        }


        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }
    }
}
