using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafy5
{
    class AdjacencyMatrix
    {
        // Brzydkoszybka enkapsulacja
        public int[,] AdjacencyArray;
        public int[,] AdjacencyArrayWeights;
        public double[,] Positions;

        // Konstruktor
        public AdjacencyMatrix(int n)
        {
            AdjacencyArray = new int[n, n];
            AdjacencyArrayWeights = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    AdjacencyArray[i, j] = 0;
                    AdjacencyArrayWeights[i, j] = 0;
                }
            }
        }

        // Wyświetlanie macierzy na stack panelu 
        public void Display(int [] Layers, int[] SumOfPreviousLayers,StackPanel StackPanelForDisplayingAdjacencyMatrix, Canvas MyCanvas, StackPanel StackPanelForDisplayingIncidenceMatrix, StackPanel StackPanelForDisplayingAdjacencylist)
        {
            StackPanelForDisplayingAdjacencyMatrix.Children.Clear();

            string myString = "";

            for (int i = 0; i < AdjacencyArray.GetLength(0); i++)
            {
                for (int j = 0; j < AdjacencyArray.GetLength(1); j++)
                {
                    myString += AdjacencyArray[i, j].ToString() + "  ";
                }
                myString += "\n";
            }

            TextBlock myBlock = new TextBlock();
            myBlock.Text = myString;
            myBlock.FontSize = 16;
            StackPanelForDisplayingAdjacencyMatrix.Children.Add(myBlock);

            DrawGraph(AdjacencyArray.GetLength(0), Layers, SumOfPreviousLayers, MyCanvas);

            StackPanelForDisplayingAdjacencyMatrix.Children.Clear();

            myString = "";

            for (int i = 0; i < AdjacencyArray.GetLength(0); i++)
            {
                for (int j = 0; j < AdjacencyArray.GetLength(1); j++)
                {
                    myString += AdjacencyArray[i, j].ToString() + "  ";
                }
                myString += "\n";
            }

            myBlock = new TextBlock();
            myBlock.Text = myString;
            myBlock.FontSize = 16;
            StackPanelForDisplayingAdjacencyMatrix.Children.Add(myBlock);

            // Nowa maceirz incydencji
            //IncidenceMatrix incidenceMatix = new IncidenceMatrix(this);
            //incidenceMatix.Display(StackPanelForDisplayingIncidenceMatrix, StackPanelForDisplayingAdjacencylist);
        }

        // Wizualizacja grafu na macierzy
        private void DrawGraph(int num_v, int [] Layers, int[] SumOfPreviousLayers, Canvas MyCanvas)
        {
            MyCanvas.Children.Clear();

            var width = MyCanvas.Width;
            var height = MyCanvas.Height;
            Positions = new double[num_v, 2];

            Rectangle myRectangle = new Rectangle();
            myRectangle.Height = 300;
            myRectangle.Width = 500;
            myRectangle.Fill = Brushes.Transparent;
            myRectangle.StrokeThickness = 2;
            myRectangle.Stroke = Brushes.LightGray;
            Canvas.SetLeft(myRectangle, width / 2 - 200);
            Canvas.SetTop(myRectangle, height / 2 - 100);
            MyCanvas.Children.Add(myRectangle);


            var x_m = width / 2;    //x middle
            var y_m = height / 2;   //y middle


            for(int i=1; i<Layers.GetLength(0)-1; i++)
            {
                Line myLine = new Line();
                myLine.Stroke = Brushes.LightGray;
                myLine.X1 = width / 2 - 200 +  myRectangle.Width / (Layers.GetLength(0)-1) * i;
                myLine.X2 = width / 2 - 200 + myRectangle.Width / (Layers.GetLength(0)-1) * i;
                myLine.Y1 = height / 2 - 100;
                myLine.Y2 = height / 2 - 100 + myRectangle.Height;
                myLine.HorizontalAlignment = HorizontalAlignment.Left;
                myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 2;
                MyCanvas.Children.Add(myLine);
            }

            var x_source = width / 2 - 200;
            var y_source = height / 2 - 100 + myRectangle.Height/2;

            Ellipse Source = new Ellipse();
            Source.Height = 8;
            Source.Width = 8;
            Source.Fill = Brushes.Black;
            Source.StrokeThickness = 1;
            Source.Stroke = Brushes.Black;
            Canvas.SetLeft(Source, x_source - 6);
            Canvas.SetTop(Source, y_source - 6);
            MyCanvas.Children.Add(Source);
            TextBlock SourceNumber = new TextBlock();
            SourceNumber.Text = "Source";//(1).ToString();
            SourceNumber.RenderTransform = new TranslateTransform
            {
                X = x_source,
                Y = y_source
            };
            Positions[0, 0] = x_source;
            Positions[0, 1] = y_source;
            MyCanvas.Children.Add(SourceNumber);


            var x_sink = width / 2 - 200 + myRectangle.Width / (Layers.GetLength(0) - 1) * (Layers.GetLength(0) - 1);
            var y_sink = height / 2 - 100 + myRectangle.Height / 2;

            Ellipse Sink = new Ellipse();
            Sink.Height = 8;
            Sink.Width = 8;
            Sink.Fill = Brushes.Black;
            Sink.StrokeThickness = 1;
            Sink.Stroke = Brushes.Black;
            Canvas.SetLeft(Sink, x_sink-6);
            Canvas.SetTop(Sink, y_sink-6);
            MyCanvas.Children.Add(Sink);
            TextBlock SinkNumber = new TextBlock();
            SinkNumber.Text = "Sink";//(num_v).ToString();
            SinkNumber.RenderTransform = new TranslateTransform
            {
                X = x_sink,
                Y = y_sink
            };
            Positions[num_v -1 , 0] = x_sink;
            Positions[num_v-1 , 1] = y_sink;
            MyCanvas.Children.Add(SinkNumber);

            for (int i = 1; i < Layers.GetLength(0) - 1; i++)
            {
                for (int j = SumOfPreviousLayers[i]; j < SumOfPreviousLayers[i] + Layers[i]; j++)
                {
                    var x = width / 2 - 200 + myRectangle.Width / (Layers.GetLength(0) - 1) * i;
                    var y = height / 2 - 100 + (j - SumOfPreviousLayers[i]) * myRectangle.Height / Layers[i];
                    Ellipse smallPoint = new Ellipse();
                    smallPoint.Height = 8;
                    smallPoint.Width = 8;
                    smallPoint.Fill = Brushes.Black;
                    smallPoint.StrokeThickness = 1;
                    smallPoint.Stroke = Brushes.Black;
                    Canvas.SetLeft(smallPoint, x - 6);
                    Canvas.SetTop(smallPoint, y - 6);
                    TextBlock smallPointNumber = new TextBlock();
                    smallPointNumber.Text = j.ToString();
                    smallPointNumber.RenderTransform = new TranslateTransform
                    {
                        X = x,
                        Y = y
                    };
                    Positions[j,0] = x;
                    Positions[j, 1] = y;
                    MyCanvas.Children.Add(smallPoint);
                    MyCanvas.Children.Add(smallPointNumber);
                }
            }

            Random r = new Random();
            for(int i = 0; i < num_v; i++)
            {
                for (int j = 0; j < num_v; j++)
                {
                    if (AdjacencyArray[i, j] !=0)
                    {
                        AdjacencyArray[i, j] = r.Next(1, 11);

                        // drawing arrow line
                        Point point11 = new Point(Positions[i, 0], Positions[i,1]);
                        Point point12 = new Point(Positions[j, 0], Positions[j, 1]);
                        DrawArrow(point11, point12, AdjacencyArray[i, j], MyCanvas, Brushes.Black, Brushes.PaleVioletRed, 0);
                    }
                }


            }
        }

        private void DrawArrow(Point p1, Point p2, int weight, Canvas MyCanvas, Brush lineColor, Brush numberColor, double offset)
        {
            GeometryGroup lineGroup = new GeometryGroup();
            double theta = Math.Atan2((p2.Y - p1.Y), (p2.X - p1.X)) * 180 / Math.PI;

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            Point p = new Point(p1.X + ((p2.X - p1.X) / 1.35), p1.Y + ((p2.Y - p1.Y) / 1.35));
            pathFigure.StartPoint = p;

            Point lpoint = new Point(p.X + 6, p.Y + 15);
            Point rpoint = new Point(p.X - 6, p.Y + 15);
            LineSegment seg1 = new LineSegment();
            seg1.Point = lpoint;
            pathFigure.Segments.Add(seg1);

            LineSegment seg2 = new LineSegment();
            seg2.Point = rpoint;
            pathFigure.Segments.Add(seg2);

            LineSegment seg3 = new LineSegment();
            seg3.Point = p;
            pathFigure.Segments.Add(seg3);

            pathGeometry.Figures.Add(pathFigure);

            RotateTransform transform = new RotateTransform();
            transform.Angle = theta + 90;
            transform.CenterX = p.X;
            transform.CenterY = p.Y;
            pathGeometry.Transform = transform;
            lineGroup.Children.Add(pathGeometry);

            LineGeometry connectorGeometry = new LineGeometry();
            connectorGeometry.StartPoint = p1;
            connectorGeometry.EndPoint = p2;
            lineGroup.Children.Add(connectorGeometry);

            Path path = new Path();
            path.Data = lineGroup;
            path.StrokeThickness = 2;
            path.Stroke = path.Fill = lineColor;//Brushes.Black;

            MyCanvas.Children.Add(path);

            Label label = new Label();
            label.Foreground = numberColor;//Brushes.PaleVioletRed;
            label.Content = weight.ToString();
            label.FontWeight = FontWeights.Bold;
            label.FontSize = 15;
            Canvas.SetLeft(label, p.X + offset);
            Canvas.SetTop(label, p.Y + offset);
            MyCanvas.Children.Add(label);
        }

        

        public int FordFurkelson(int v, int [,] graph, Canvas MyCanvas)
        {
            Queue<int>myQ = new Queue<int>();
            int[,] Capacities = new int[v, v];
            int[,] Flows = new int[v, v];
            int[] Previous = new int[v];
            int[] PathsMinCapacity = new int[v];
            int MaxFlow = 0;
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    Capacities[i, j] = graph[i, j];
                    Flows[i,j] = 0;
                }
            }
            bool whileStop = false;
            while(true)
            {
                Previous[0] = -100; //zrodlo
                for (int i = 1; i < v; i++)
                    Previous[i] = -1; //-1 bedzie oznaczac ze w tym wierzcholku jeszcze nie bylismy (w tej sciezce)
                for (int i=0; i<v; i++)
                    PathsMinCapacity[i] = 1000000; //przepustowosc sciezek, idac przez krawedz o mniejszej przepustowosci bedziemy to zmniejszac
                while (myQ.Count != 0) myQ.Dequeue();
                myQ.Enqueue(0);
                whileStop = false;
                while(myQ.Count != 0)
                {
                    int fromQueue = myQ.Dequeue();
                    for(int vert = 1; vert<v; vert++)
                    {
                        int edgeCapacity = Capacities[fromQueue, vert] - Flows[fromQueue, vert];
                        if(edgeCapacity!=0 && (Previous[vert] == -1))
                        {
                            Previous[vert] = fromQueue;
                            PathsMinCapacity[vert] = Math.Min(PathsMinCapacity[fromQueue], edgeCapacity);
                            if (vert == (v - 1))
                            {
                                MaxFlow += PathsMinCapacity[v - 1];
                                int update = v - 1;
                                while (update != 0)
                                {
                                    int temp = Previous[update];
                                    Flows[temp, update] += PathsMinCapacity[v - 1];
                                    Flows[update, temp] -= PathsMinCapacity[v - 1];
                                    update = temp;
                                }
                                whileStop = true;
                                break;
                            }
                            myQ.Enqueue(vert);
                        }
                    }
                    if (whileStop == true) break;
                }
                if (whileStop == false) break;
            }
            for(int i=0; i<v; i++)
            {
                for(int j=0; j<v; j++)
                {
                    if(Flows[i,j]>0)
                    {
                        Point point11 = new Point(Positions[i, 0], Positions[i, 1]);
                        Point point12 = new Point(Positions[j, 0], Positions[j, 1]);
                        DrawArrow(point11, point12, Flows[i, j], MyCanvas, Brushes.Blue, Brushes.DeepSkyBlue, -7);
                    }
                }
            }
            return MaxFlow;

        }

    }
}
