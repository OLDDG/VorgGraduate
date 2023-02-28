using System;
using System.Windows;
using System.Windows.Controls;
using Core;
using WPFSurfacePlot3D;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string regime;
        string reflection;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize surface plot objects
            //SurfacePlotModel viewModel = new SurfacePlotModel();
            //surfacePlotView.DataContext = viewModel;
        }

        private void reflection_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            reflection = pressed.Content.ToString();
        }

        private void regime_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            regime = pressed.Content.ToString();
        }

        private Item CreateItem()
        {
            Item item = new();
            item.Ffunc = Ffunc.Text;
            item.Gfunc = Gfunc.Text;
            item.RegisterSize = RegisterSize.Text;
            item.InputX = InputX.Text;
            item.InputY = InputY.Text;
            return item;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calculate calc = new(CreateItem());
            //int size = Int32.Parse(RegisterSize.Text);
            //double[] points
            if (regime == "последовательность")
            {
                if (reflection == "Мона")
                {
                    //for (double i = 3; i < Math.Pow(2, size); i++)
                    //{
                    //    for (double j = 3; j < Math.Pow(2, size); j++)
                    //    {
                    //        string Ffunction = Ffunc.Text.Replace("y", j.ToString());
                    //        Ffunction = Ffunction.Replace("x", i.ToString());
                    //        Parser pars = new(Ffunction);
                    //        double x = calc.Monna(calc.Two(i, 8));
                    //        double y = calc.Monna(calc.Two(j, 8));
                    //        double z = calc.Monna(calc.Two(pars.Calc(), 8));

                    //        //SurfacePlotModel mySurfacePlotModel = new();
                    //        //SurfacePlotView mySurfacePlotView = new();
                    //        //mySurfacePlotView.DataContext = mySurfacePlotModel;
                    //        //function = (x, y) => x * y;
                    //        //viewModel.PlotFunction(function, -1, 1);
                    //    }
                    //}
                    SurfacePlotModel viewModel = new SurfacePlotModel();
                    surfacePlotView.DataContext = viewModel;
                    //SurfacePlotView surfacePlotView = new ();
                    //SurfacePlotModel mySurfacePlotModel = new ();
                    //surfacePlotView.DataContext = mySurfacePlotModel;
                    //Func<double, double, double> sampleFunction = (x, y) => x * y;
                    //sampleFunction = (x, y) => 10 * Math.Sin(Math.Sqrt(x * x + y * y)) / Math.Sqrt(x * x + y * y);
                    //////mySurfacePlotModel.                    
                }
            }
        }
    }
}
