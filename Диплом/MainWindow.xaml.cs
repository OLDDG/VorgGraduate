using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Core;
using WPFSurfacePlot3D;
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
            else
            {
                /*
                String s1;
                Point pt2 = m_transformMatrix.VertexToScreenPt(new Point3D(0.5, 0.5, 0.3), mainViewport);
                s1 = string.Format("Screen:({0:d},{1:d}), Predicated: ({2:d}, H:{3:d})", 
                    (int)pt.X, (int)pt.Y, (int)pt2.X, (int)pt2.Y);
                this.statusPane.Text = s1;
                */
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
            int size = Int32.Parse(RegisterSize.Text);
            List<double> x = new ();
            List<string> x2 = new();
            List<double> y = new ();
            List<double> z = new ();
            if (regime == "последовательность")
            {
                if (reflection == "Мона")
                {
                    for (int i = 0; i < Math.Pow(2, size); i++)
                    {
                        x2.Add(calc.Two(i, size));
                        x.Add(calc.Monna(calc.Two(i, size)));
                        y.Add(calc.Monna(calc.Two(i, size)));
                    }

                    //var g = calc.Monna(x2[4]);

                    
                    for (int i = 0; i < Math.Pow(2, size); i++)
                    {
                        double curX = x[i];
                        for (int j = 0; j < Math.Pow(2, size); j++)
                        {
                            double curY = y[j];
                            string Ffunction = Ffunc.Text.Replace("y", j.ToString());
                            Ffunction = Ffunction.Replace("x", i.ToString());
                            Parser pars = new(Ffunction);
                            double curZ = calc.Monna(calc.Two(pars.Calc(), 8));
                            z.Add(calc.Monna(calc.Two(pars.Calc(), 8)));
                            //newDataArray[i, j] = new Point3D(curX, curY, curZ);
                        }
                    }
                    TestScatterPlot(x, y, z, (int)Math.Pow(2, size));
                    //SurfacePlotModel viewModel = new(newDataArray);
                    ////viewModel.PlotData(newDataArray);
                    //surfacePlotView.DataContext = viewModel;
                    ///Create pen.
                }
            }
        }


        // function for testing surface chart
        public void TestSurfacePlot(int nGridNo)
        {
            int nXNo = nGridNo;
            int nYNo = nGridNo;
            // 1. set the surface grid
            m_3dChart = new UniformSurfaceChart3D();
            ((UniformSurfaceChart3D)m_3dChart).SetGrid(nXNo, nYNo, -100, 100, -100, 100);

            // 2. set surface chart z value
            double xC = m_3dChart.XCenter();
            double yC = m_3dChart.YCenter();
            int nVertNo = m_3dChart.GetDataNo();
            double zV;
            for (int i = 0; i < nVertNo; i++)
            {
                Vertex3D vert = m_3dChart[i];

                double r = 0.15 * Math.Sqrt((vert.x - xC) * (vert.x - xC) + (vert.y - yC) * (vert.y - yC));
                if (r < 1e-10) zV = 1;
                else zV = Math.Sin(r) / r;

                m_3dChart[i].z = (float)zV;
            }
            m_3dChart.GetDataRange();

            // 3. set the surface chart color according to z vaule
            double zMin = m_3dChart.ZMin();
            double zMax = m_3dChart.ZMax();
            for (int i = 0; i < nVertNo; i++)
            {
                Vertex3D vert = m_3dChart[i];
                double h = (vert.z - zMin) / (zMax - zMin);

                Color color = WPFChart3D.TextureMapping.PseudoColor(h);
                m_3dChart[i].color = color;
            }

            // 4. Get the Mesh3D array from surface chart
            ArrayList meshs = ((UniformSurfaceChart3D)m_3dChart).GetMeshes();

            // 5. display vertex no and triangle no of this surface chart
            UpdateModelSizeInfo(meshs);

            // 6. Set the model display of surface chart
            WPFChart3D.Model3D model3d = new WPFChart3D.Model3D();
            Material backMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Gray));
            m_nChartModelIndex = model3d.UpdateModel(meshs, backMaterial, m_nChartModelIndex, this.mainViewport);

            // 7. set projection matrix, so the data is in the display region
            float xMin = m_3dChart.XMin();
            float xMax = m_3dChart.XMax();
            m_transformMatrix.CalculateProjectionMatrix(xMin, xMax, xMin, xMax, zMin, zMax, 0.5);
            TransformChart();
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
            //labelVertNo.Content = String.Format("Vertex No: {0:d}", nChartVertNo);
            //labelTriNo.Content = String.Format("Triangle No: {0:d}", nChartTriangelNo);
        }

        // function for testing 3d scatter plot
        public void TestScatterPlot(List<double> x, List<double> y, List<double> z, int size)
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

                    plotItem.w = (float)0.01;
                    plotItem.h = (float)0.01;
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
            m_3dChart.SetAxes();

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
