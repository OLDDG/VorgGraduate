using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Core;
using WPFChart3D;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string regime;
        string reflection;
        string regimeValues;


        // transform class object for rotate the 3d model
        public WPFChart3D.TransformMatrix m_transformMatrix = new WPFChart3D.TransformMatrix();

        // ***************************** 3d chart ***************************
        private WPFChart3D.Chart3D m_3dChart;       // data for 3d chart
        public int m_nChartModelIndex = -1;         // model index in the Viewport3d
        public int m_nSurfaceChartGridNo = 100;     // surface chart grid no. in each axis
        public int m_nScatterPlotDataNo = 5000;     // total data number of the scatter plot

        // ***************************** selection rect ***************************
        ViewportRect m_selectRect = new ViewportRect();
        public int m_nRectModelIndex = -1;

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
            RadioButton pressed1 = (RadioButton)sender;
            regime = pressed1.Content.ToString();
        }

        private void regimeValues_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            regimeValues = pressed.Content.ToString();
        }

        private Item CreateItem()
        {
            Item item = new();
            item.Ffunc = Ffunc.Text;
            item.RegisterSize = RegisterSize.Text;
            item.InputX = InputX.Text;
            item.InputY = InputY.Text;
            return item;
        }

        public void OnViewportMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            Point pt = args.GetPosition(mainViewport);
            if (args.ChangedButton == MouseButton.Left)         // rotate or drag 3d model
            {
                m_transformMatrix.OnLBtnDown(pt);
            }
            else if (args.ChangedButton == MouseButton.Right)   // select rect
            {
                m_selectRect.OnMouseDown(pt, mainViewport, m_nRectModelIndex);
            }
        }

        // this function is used to rotate, drag and zoom the 3d chart
        private void TransformChart()
        {
            if (m_nChartModelIndex == -1) return;
            ModelVisual3D visual3d = (ModelVisual3D)(this.mainViewport.Children[m_nChartModelIndex]);
            if (visual3d.Content == null) return;
            Transform3DGroup group1 = visual3d.Content.Transform as Transform3DGroup;
            group1.Children.Clear();
            group1.Children.Add(new MatrixTransform3D(m_transformMatrix.m_totalMatrix));
        }

        public void OnViewportMouseMove(object sender, System.Windows.Input.MouseEventArgs args)
        {
            Point pt = args.GetPosition(mainViewport);

            if (args.LeftButton == MouseButtonState.Pressed)                // rotate or drag 3d model
            {
                m_transformMatrix.OnMouseMove(pt, mainViewport);

                TransformChart();
            }
            else if (args.RightButton == MouseButtonState.Pressed)          // select rect
            {
                m_selectRect.OnMouseMove(pt, mainViewport, m_nRectModelIndex);
            }
        }

        public void OnViewportMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            Point pt = args.GetPosition(mainViewport);
            if (args.ChangedButton == MouseButton.Left)
            {
                m_transformMatrix.OnLBtnUp();
            }
            else if (args.ChangedButton == MouseButton.Right)
            {
                if (m_nChartModelIndex == -1) return;
                // 1. get the mesh structure related to the selection rect
                MeshGeometry3D meshGeometry = WPFChart3D.Model3D.GetGeometry(mainViewport, m_nChartModelIndex);
                if (meshGeometry == null) return;

                // 2. set selection in 3d chart
                m_3dChart.Select(m_selectRect, m_transformMatrix, mainViewport);

                // 3. update selection display
                m_3dChart.HighlightSelection(meshGeometry, Color.FromRgb(200, 200, 200));
            }
        }

        // zoom in 3d display
        public void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs args)
        {
            m_transformMatrix.OnKeyDown(args);
            TransformChart();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            Calculate calc = new(CreateItem());
            //double xxxx = calc.Reverse("101.0000000", 6);
            int size = Int32.Parse(RegisterSize.Text);
            List<double> x = new ();
            List<double> y = new ();
            List<double> z = new ();
            if (regime == "последовательность")
            {
                if (reflection == "Мона")
                {
                    for (int i = 0; i < Math.Pow(2, size); i++)
                    {
                        x.Add(calc.Monna(calc.Two(i, size), size));
                        y.Add(calc.Monna(calc.Two(i, size), size));
                    }
                    for (int i = 0; i < Math.Pow(2, size); i++)
                    {
                        for (int j = 0; j < Math.Pow(2, size); j++)
                        {
                            string Ffunction = Ffunc.Text.Replace("y", j.ToString());
                            Ffunction = Ffunction.Replace("x", i.ToString());
                            Parser pars = new(Ffunction);
                            z.Add(calc.Monna(calc.Two(pars.Calc(), 8), size));
                        }
                    }
                    TestScatterPlot(x, y, z, (int)Math.Pow(2, size), false);
                }
                else
                {
                    for (int i = 0; i < Math.Pow(2, size); i++)
                    {
                        x.Add(calc.Reverse(calc.Two(i, size), size));
                        y.Add(calc.Reverse(calc.Two(i, size), size));
                    }
                    for (int i = 0; i < Math.Pow(2, size); i++)
                    {
                        for (int j = 0; j < Math.Pow(2, size); j++)
                        {
                            string Ffunction = Ffunc.Text.Replace("y", j.ToString());
                            Ffunction = Ffunction.Replace("x", i.ToString());
                            Parser pars = new(Ffunction);
                            z.Add(calc.Reverse(calc.Two(pars.Calc(), 8), size));
                        }
                    }
                    TestScatterPlot(x, y, z, (int)Math.Pow(2, size), false);
                }
            }
            else
            {
                int inputX = Int32.Parse(InputX.Text);
                int inputY = Int32.Parse(InputY.Text);
                string Ffunction = Ffunc.Text.Replace("y", inputY.ToString());
                Ffunction = Ffunction.Replace("x", inputX.ToString());
                x.Add(inputX);
                y.Add(inputY);
                Parser pars = new(Ffunction);
                z.Add(pars.Calc());
                for (int i = 0; i < Math.Pow(2, size); i++)
                {
                    for (int j = 0; j < Math.Pow(2, size); j++)
                    {
                        if (regimeValues == "1")
                        {
                            x.Add(y[(int)(i * Math.Pow(2, size) + j)]);
                            y.Add(z[(int)(i * Math.Pow(2, size) + j)]);
                            Ffunction = Ffunc.Text.Replace("y", y[(int)(i * Math.Pow(2, size) + j + 1)].ToString());
                            Ffunction = Ffunction.Replace("x", x[(int)(i * Math.Pow(2, size) + j + 1)].ToString());
                            Parser parser = new(Ffunction);
                            z.Add(parser.Calc());
                        }
                        else
                        {
                            double tmpX = y[(int)(i * Math.Pow(2, size) + j)];
                            double tmpY = z[(int)(i * Math.Pow(2, size) + j)];
                            
                            Ffunction = Ffunc.Text.Replace("y", tmpY.ToString());
                            Ffunction = Ffunction.Replace("x", tmpX.ToString());
                            Parser parser = new(Ffunction);
                            double tmpZ = parser.Calc();
                            x.Add(tmpZ);

                            Ffunction = Ffunc.Text.Replace("y", tmpZ.ToString());
                            Ffunction = Ffunction.Replace("x", tmpY.ToString());
                            parser = new(Ffunction);
                            double tmpZ2 = parser.Calc();
                            y.Add(tmpZ2);

                            Ffunction = Ffunc.Text.Replace("y", tmpZ2.ToString());
                            Ffunction = Ffunction.Replace("x", tmpZ.ToString());
                            parser = new(Ffunction);
                            z.Add(parser.Calc());
                        }
                    }
                }
                TestScatterPlot(x, y, z, (int)Math.Pow(2, size), true);
            }
        }

        private void UpdateModelSizeInfo(ArrayList meshs)
        {
            int nMeshNo = meshs.Count;
            int nChartVertNo = 0;
            int nChartTriangelNo = 0;
            for (int i = 0; i < nMeshNo; i++)
            {
                nChartVertNo += ((Mesh3D)meshs[i]).GetVertexNo();
                nChartTriangelNo += ((Mesh3D)meshs[i]).GetTriangleNo();
            }
        }

        // function for testing 3d scatter plot
        public void TestScatterPlot(List<double> x, List<double> y, List<double> z, int size, bool isSubseq)
        {
            // 1. set scatter chart data no.
            m_3dChart = new ScatterChart3D();
            m_3dChart.SetDataNo(size * size);

            // 2. set property of each dot (size, position, shape, color)
            Random randomObject = new Random();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    ScatterPlotItem plotItem = new();

                    if (isSubseq)
                    {
                        plotItem.w = (float)0.5;//(float)0.01;
                        plotItem.h = (float)0.5;//(float)0.01;
                    }
                    else
                    {
                        plotItem.w = (float)0.01;//(float)0.01;
                        plotItem.h = (float)0.01;//(float)0.01;
                    }
                    plotItem.x = (float)x[i];
                    plotItem.y = (float)y[j];
                    plotItem.z = (float)z[(int)(i * size + j)];
                    
                    plotItem.shape = 2;//randomObject.Next(4);

                    Byte nR = (Byte)randomObject.Next(2);
                    Byte nG = (Byte)randomObject.Next(2);
                    Byte nB = (Byte)randomObject.Next(2);

                    plotItem.color = Color.FromRgb(nR, nG, nB);
                    ((ScatterChart3D)m_3dChart).SetVertex((int)(i * size + j), plotItem);
                }
            }

            // 3. set the axes
            m_3dChart.GetDataRange();
            if (!isSubseq)
            {
                m_3dChart.SetAxes(1);
            }
            else
            {
                m_3dChart.SetAxes(size);
            }
            // 4. get Mesh3D array from the scatter plot
            ArrayList meshs = ((ScatterChart3D)m_3dChart).GetMeshes();

            // 5. display model vertex no and triangle no
            UpdateModelSizeInfo(meshs);

            // 6. display scatter plot in Viewport3D
            WPFChart3D.Model3D model3d = new WPFChart3D.Model3D();
            m_nChartModelIndex = model3d.UpdateModel(meshs, null, m_nChartModelIndex, this.mainViewport);

            // 7. set projection matrix
            float viewRange = (float)1;
            m_transformMatrix.CalculateProjectionMatrix(0, viewRange, 0, viewRange, 0, viewRange, 0.3);
            TransformChart();
        }


    }
}
