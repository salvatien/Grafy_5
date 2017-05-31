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
            double [,] Positions = new double[num_v, 2];

            Rectangle myRectangle = new Rectangle();
            myRectangle.Height = 200;
            myRectangle.Width = 400;
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
                        DrawArrow(point11, point12, AdjacencyArray[i, j], MyCanvas);
                    }
                }


            }
        }

        private void DrawArrow(Point p1, Point p2, int weight, Canvas MyCanvas)
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
            path.Stroke = path.Fill = Brushes.Black;

            MyCanvas.Children.Add(path);

            Label label = new Label();
            label.Foreground = Brushes.PaleVioletRed;
            label.Content = weight.ToString();
            label.FontWeight = FontWeights.Bold;
            label.FontSize = 15;
            Canvas.SetLeft(label, p.X);
            Canvas.SetTop(label, p.Y);
            MyCanvas.Children.Add(label);
        }

        //========================================================
        // Algorytm Kosaraju
        //========================================================

        // Algorytm przejścia DFStack
        private void DFStack(int v, bool[] visited, Stack<int> S, int[,] graph, TextBlock SCC)
        {
            visited[v] = true;
            for (int u = 0; u < graph.GetLength(0); u++)
                if (graph[v, u] == 1)
                    if (visited[u] == false)
                        DFStack(u, visited, S, graph, SCC);
            S.Push(v);
            //SCC.Text += "\nPushed";
        }

        // Algorytm przejścia DFSprint
        private void DFSprint(int v, bool[] visited, int[,] graph, TextBlock SCC, List<int> currCompList)
        {
            visited[v] = true;
            //
            // Pisz v
            SCC.Text += (v + 1).ToString() + " ";
            currCompList.Add(v);
            //
            for (int u = 0; u < graph.GetLength(0); u++)
                if (graph[v, u] == 1)
                    if (visited[u] == false)
                        DFSprint(u, visited, graph, SCC, currCompList);
        }

        public List<List<int>> KosarajuAlgorithm(TextBlock SCC)
        {
            SCC.Text = "";

            int n = AdjacencyArray.GetLength(0);

            // Kopiujemy macierz sąsiedztwa do nowego grafu
            int[,] graph = new int[AdjacencyArray.GetLength(0), AdjacencyArray.GetLength(1)];
            for (int i = 0; i < AdjacencyArray.GetLength(0); i++)
                for (int j = 0; j < AdjacencyArray.GetLength(1); j++)
                    graph[i, j] = AdjacencyArray[i, j];

            bool[] visited = new bool[n];   // domyślnie wszystko jest ustawione na false

            Stack<int> S = new Stack<int>();

            for (int v = 0; v < n; v++)
                if (visited[v] == false)
                    DFStack(v, visited, S, graph, SCC);

            TransposeGraph(graph, n);

            for (int i = 0; i < n; i++)
                visited[i] = false;

            List<List<int>> listOfSSC = new List<List<int>>();

            int cn = 0;     // licznik silnie spójnych składowych
            while (S.Count != 0)
            {
                int v = S.Peek();
                S.Pop();

                if (visited[v] != true)
                {
                    cn++;
                    List<int> currCompList = new List<int>();
                    //
                    // Pisz SCC, cn, :
                    SCC.Text += "SCC " + cn.ToString() + ": ";
                    //
                    DFSprint(v, visited, graph, SCC, currCompList);
                    SCC.Text += "\n";
                    listOfSSC.Add(currCompList);
                }
            }
            return listOfSSC;
        }

        private void TransposeGraph(int[,] graph, int n)
        {
            int[,] tmpGraph = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    tmpGraph[i, j] = graph[i, j];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    graph[i, j] = tmpGraph[j, i];
        }

        //Stwórz silnie spójny digraf
        internal void RandomStronglyConnectedDigraph(TextBlock SCC, int num_of_e, int probability)
        {
            int n = AdjacencyArray.GetLength(0);

            Random r = new Random();
            int i_to_draw = r.Next(0, n);
            int j_to_draw = r.Next(0, n);
            while (num_of_e > 0)
            {
                //int probability_curr = r.Next(0, 100);
                // if (probability_curr < probability)
                //  {
                while ((AdjacencyArray[i_to_draw, j_to_draw] != 0) || (i_to_draw == j_to_draw))
                {
                    i_to_draw = r.Next(0, n);
                    j_to_draw = r.Next(0, n);
                }
                AdjacencyArray[i_to_draw, j_to_draw] = 1;
                num_of_e--;
                // }
            }


            //=================================
            List<List<int>> listOfSCC = KosarajuAlgorithm(SCC);

            for (int i = 0; i < listOfSCC.Count - 1; i++)
            {
                AdjacencyArray[listOfSCC[i][0], listOfSCC[i + 1][0]] = 1;
                AdjacencyArray[listOfSCC[i + 1][listOfSCC[i + 1].Count - 1], listOfSCC[i][listOfSCC[i].Count - 1]] = 1;
            }
            KosarajuAlgorithm(SCC);


            //=================================
            // losowanie wag na łukach
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (AdjacencyArray[i, j] == 1)
                        //=================================================ZMIANAAAAA
                        AdjacencyArrayWeights[i, j] = r.Next(-5, 11);
                    //AdjacencyArrayWeights[i, j] = r.Next(1, 11);
                }
            }
        }


        //========================================================
        // Algorytm Bellmana-Forda
        //========================================================

        internal bool BellmanFordAlorithm(TextBlock SP, int v, ref Int32[] d)
        {
            int n = AdjacencyArray.GetLength(0);

            // Kopiujemy macierz sąsiedztwa do nowego grafu
            int[,] graph = new int[AdjacencyArray.GetLength(0), AdjacencyArray.GetLength(1)];
            for (int i = 0; i < AdjacencyArray.GetLength(0); i++)
                for (int j = 0; j < AdjacencyArray.GetLength(1); j++)
                    graph[i, j] = AdjacencyArray[i, j];

            //elementy pomocnicze
            bool test;

            //utworzenie n-elementowej tablicy d i wypełnienie jej wartościami maksymalnymi
            //oraz n-elementowej tablicy p i wypełnienie jej wartościami -1
            d = new Int32[n];
            Int32[] p = new Int32[n];
            for (int i = 0; i < n; i++)
            {
                d[i] = Int32.MaxValue;
                p[i] = -1;
            }
            d[v] = 0;
            for (int i = 1; i <= n - 1; i++)
            {
                test = true;
                for (int x = 0; x < n; x++)
                {
                    for (int y = 0; y < n; y++)
                    {
                        if (graph[x, y] == 1)
                        {
                            if ((d[y] > d[x] + AdjacencyArrayWeights[x, y]) && d[x] != Int32.MaxValue)
                            {
                                test = false;
                                d[y] = d[x] + AdjacencyArrayWeights[x, y];
                                p[y] = x;
                            }
                        }
                    }
                }
                if (test == true)
                    return true;
            }
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    if (graph[x, y] == 1)
                    {
                        if (d[y] > d[x] + AdjacencyArrayWeights[x, y])
                            return false;
                    }
                }
            }

            return true;
        }

        public void WriteShortestPaths(Int32[] d, TextBlock SP, int v)
        {
            SP.Text = "Najkrótsze ścieżki od wierzchołka " + (v + 1).ToString() + ":\n ";
            for (int i = 0; i < d.Length; i++)
            {
                SP.Text += " do " + (i + 1).ToString() + " : " + d[i] + "\n ";
            }
        }


        /*
        //========================================================
        // Algorytm Dijkstry
        //========================================================

        internal void DijkstraAlgorithm(TextBlock SP, int v)
        {
            int n = AdjacencyArray.GetLength(0);

            // Kopiujemy macierz sąsiedztwa do nowego grafu
            int[,] graph = new int[AdjacencyArray.GetLength(0), AdjacencyArray.GetLength(1)];
            for (int i = 0; i < AdjacencyArray.GetLength(0); i++)
                for (int j = 0; j < AdjacencyArray.GetLength(1); j++)
                    graph[i, j] = AdjacencyArray[i, j];

            List<int> S = new List<int>();

            List<int> Q = new List<int>();
            for (int i = 0; i < n; i++)
                Q.Add(i);

            //tablica na koszty dojścia inicjalizowana największymi wartościami
            Int32[] d = new Int32[n];
            //tablica poprzedników na ścieżkach inizjalizowana -1
            Int32[] p = new Int32[n];
            //boole
            bool[] sptSet = new bool[n];

            for (int i = 0; i < n; i++)
            {
                d[i] = Int32.MaxValue;
                p[i] = -1;
                sptSet[i] = false;
            }
            d[v] = 0;   //koszt do samego siebie

            /*
            while(Q.Count > 0)
            {
                //var u = Q.IndexOf(Q.Min());
                //S.Add(Q[u]);
                //Q.RemoveAt(u);
                int u = -1;
                int min_d = Int32.MaxValue;
                for(int i = 0; i < n; i++)
                    if(d[i] < min_d)
                    {
                        u = i;
                        min_d = d[i];
                    }

                Q.RemoveAt(u);
                sptSet[u] = true;

                for (int w = 0; w < n; w++)
                    if (graph[u, w] == 1)
                    {
                        //if (Q.FindIndex(item => item == w) >= 0)
                        bool flag = false;
                        for (int i = 0; i < Q.Count; i++)
                            if (Q[i] == w)
                                flag = true;
                        if (flag == true)
                        {
                            if ((d[w] > d[u] + AdjacencyArrayWeights[u, w]) && (d[u] != Int32.MaxValue) && (sptSet[w] == false))
                            {
                                d[w] = d[u] + AdjacencyArrayWeights[u, w];
                                p[w] = u;
                            }
                        }
                    }
            }
            

            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    if (graph[x, y] == 1)
                    {
                        if ((d[y] > d[x] + AdjacencyArrayWeights[x, y]) && d[x] != Int32.MaxValue)
                        {
                            d[y] = d[x] + AdjacencyArrayWeights[x, y];
                            p[y] = x;
                        }
                    }
                }
            }

            WriteShortestPaths(d, SP, v);
        }
        */

        //========================================================
        // Algorytm Johnsona
        //========================================================
        internal void JohnsonAlgorithm(TextBlock SP)
        {
            // miejsce na nową macierz
            // n - 1 to indeks nowego wierzchołka
            int n = AdjacencyArray.GetLength(0) + 1;

            // Kopiujemy macierz sąsiedztwa do nowego grafu większego o jeden wierzchołek
            int[,] graph = new int[n, n];
            //Kopiujemy macierz wag do nowej macierzy dla nowego grafu
            int[,] graphWeights = new int[n, n];

            for (int i = 0; i < n - 1; i++)
                for (int j = 0; j < n - 1; j++)
                {
                    graph[i, j] = AdjacencyArray[i, j];
                    graphWeights[i, j] = AdjacencyArrayWeights[i, j];
                }

            //dokładamy połączenia od nowego wierzchołka do każdego istniejącego wierzchołka
            //dodajemy również zerowe wagi połączeń
            for (int i = 0; i < n; i++)
            {
                graph[n - 1, i] = 1;
                graph[i, n - 1] = 0;
                graphWeights[n - 1, i] = graphWeights[i, n - 1] = 0;
            }

            //Aktualizujemy macierz w klasie oraz jej wagi - wierzchołek jest teraz dodany
            //jest połączony ze wszystkimi istniejącymi wcześniej wierzchołkami
            //i jest pomiędzy każdą taką parą połączenie o wadze 0
            AdjacencyArray = graph;
            AdjacencyArrayWeights = graphWeights;

            //Algorytmem Bellmana-Forda szukamy najkrószych ścieżek między dodanym wierzchołkiem, a resztą
            Int32[] d = new Int32[0];
            if (BellmanFordAlorithm(SP, n - 1, ref d))
            {
                //Teraz przewagowywuwywuwyjemy graf tak, by pozbyć się ujemnych wag
                //nie zmieniając wartości najkrótszych ścieżek
                for (int u = 0; u < n; u++)
                    for (int v = 0; v < n; v++)
                        AdjacencyArrayWeights[u, v] = AdjacencyArrayWeights[u, v] + d[u] - d[v];

                //Teraz usuwamy początkowo dodany węzeł
                //czyli ponownie wielkość będzie taka, jak na początku
                n = AdjacencyArray.GetLength(0) - 1;

                graph = new int[n, n];
                graphWeights = new int[n, n];

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        graph[i, j] = AdjacencyArray[i, j];
                        graphWeights[i, j] = AdjacencyArrayWeights[i, j];
                    }

                AdjacencyArray = graph;
                AdjacencyArrayWeights = graphWeights;

                //Aktualizujemy teraz najkrótsze ścieżki algorytmem Dijkstry dla każdego wierzchołka
                //który jest uparty i nie daje się zaimplementować, 
                //więc posłużymy się bardziej procesożernym bratem Bellmanem-Fordem
                //UWAGA!!! Tak jest w oryginalnym algorytmie, ale tutaj nie potrzebujemy tego wywoływać;
                //          wystarczy, że użytkownik może raz jeszcze sobie odpalić algorytm
                //for (int i = 0; i < n; i++) ;
                // BellmanFordAlorithm(SP, i, ref d);
            }
            else
            {
                SP.Text = "Wykryto ujemny cykl.";
            }


        }

    }
}
