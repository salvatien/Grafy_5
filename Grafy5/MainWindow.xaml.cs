using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grafy5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //deklaracje wartości publicznych, widoczne w całym programie i w każdej funckji
        private AdjacencyMatrix adjacencyMatrix;
        public List<ComboBox> ListOfLeftComboBoxes;
        public List<ComboBox> ListOfRightComboBoxes;
        public List<TextBox> ListOfWeightTextBoxes;
        public int LeftComboBoxValue = 0;
        public int RightComboBoxValue = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (How_Many_Layers.Text != "")
            {
                How_Many_Layers.Background = Brushes.White;
                int v = 2; //ile jest wierzcholkow, na poczatku 2 (zrodlo i ujscie), potem dodamy warstwy
                //StackPanelWithConnections.Children.Clear();
                int N = Int32.Parse(How_Many_Layers.Text); //ile bedzie warstw
                Random r = new Random();
                //int[,] Layers = new int[N+1, N]; 
                int[] Layers = new int[N + 2]; //do N+2 bo chcemy zeby warstwa 0 i N+1 tez byly
                int[] SumOfPreviousLayers = new int[N + 2]; //ile bylo wierzcholkow we wszystkich wczesniejszych warstwach
                Layers[0] = 1;
                SumOfPreviousLayers[0] = 0;

                for (int i = 1; i <= N; i++)
                {
                    Layers[i] = r.Next(2, N+1);
                    v += Layers[i];
                    SumOfPreviousLayers[i] = SumOfPreviousLayers[i-1] + Layers[i-1];
                }
                Layers[N + 1] = 1;
                SumOfPreviousLayers[N + 1] = v - 1;
                //teraz w v mamy calkowita liczbe wierzcholkow, a w tablicy Layers - ile wierzcholkow jest w ktorej warstwie
                adjacencyMatrix = new AdjacencyMatrix(v);

                // i =0 - jestesmy w zrodle, chcemy je polaczyc z wierzcholkami z warstwy 1
                bool Connected = false;
                for (int j = 1; j < SumOfPreviousLayers[1] + Layers[1]; j++) //losowanie krawedzi wyjsciowych ze zrodla
                {
                    int probability = r.Next(0, 100);

                    if (probability > Int32.Parse(Probability_Of_Edge_Occurence.Text))
                    {
                        adjacencyMatrix.AdjacencyArray[0, j] = 0;
                    }
                    else
                    {
                        Connected = true;
                        adjacencyMatrix.AdjacencyArray[0, j] = 1;
                    }
                    adjacencyMatrix.AdjacencyArray[0, 0] = 0;
                }
                if(Connected == false)
                {
                    int VertexConnected = r.Next(1, Layers[1]+1);
                    adjacencyMatrix.AdjacencyArray[0, VertexConnected] = 1;
                }

                for (int i = 1; i <= N ; i++) //warstwy
                {
                    for (int j = SumOfPreviousLayers[i]; j < SumOfPreviousLayers[i] + Layers[i]; j++) //wierzcholki w danej warstwie
                    {
                        for (int k = SumOfPreviousLayers[i]; k < SumOfPreviousLayers[i] + Layers[i]; k++) //krawedzie wewnatrz warstwy
                        {
                            int probability = r.Next(0, 100);

                            if (probability > Int32.Parse(Probability_Of_Edge_Occurence.Text))
                            {
                                adjacencyMatrix.AdjacencyArray[j, k] = 0;
                            }
                            else
                            {
                                adjacencyMatrix.AdjacencyArray[j, k] = 1;
                            }
                            adjacencyMatrix.AdjacencyArray[k, k] = 0; 
                        }
                        for (int k = SumOfPreviousLayers[i+1]; k < SumOfPreviousLayers[i+1] + Layers[i+1]; k++) //krawedzie do nastepnej warstwy
                        {
                            int probability = r.Next(0, 100);

                            if (probability > Int32.Parse(Probability_Of_Edge_Occurence.Text))
                            {
                                adjacencyMatrix.AdjacencyArray[j, k] = 0;
                            }
                            else
                            {
                                adjacencyMatrix.AdjacencyArray[j, k] = 1;
                            }
                            adjacencyMatrix.AdjacencyArray[k, k] = 0; 
                        }
                    }
                }

                for(int i=1; i<=N; i++) //sprawdzenie czy spelniony jest warunek z laczeniem wierzcholkow z warstwy i oraz i+1
                {
                    for (int j = SumOfPreviousLayers[i]; j < SumOfPreviousLayers[i] + Layers[i]; j++)
                    {
                        bool outFlag = false;
                        bool inFlag = false;
                        for (int k = SumOfPreviousLayers[i]; k < SumOfPreviousLayers[i] + Layers[i]; k++)
                        {
                            if (adjacencyMatrix.AdjacencyArray[j, k] != 0)
                                outFlag = true;
                        }
                        for (int k = SumOfPreviousLayers[i - 1]; k < SumOfPreviousLayers[i] + Layers[i]; k++)
                        {
                            if (adjacencyMatrix.AdjacencyArray[k, j] != 0)
                                inFlag = true;
                        }
                        if(outFlag == false)
                        {
                            int VertexConnected;
                            while (true)
                            {
                                VertexConnected = r.Next(SumOfPreviousLayers[i], SumOfPreviousLayers[i] + Layers[i]);
                                if (VertexConnected != j)
                                    break;
                            }
                            adjacencyMatrix.AdjacencyArray[j, VertexConnected] = 1;
                        }

                        if (inFlag == false)
                        {

                            int VertexConnected;
                            while (true)
                            {
                                VertexConnected = r.Next(SumOfPreviousLayers[i - 1], SumOfPreviousLayers[i-1] + Layers[i-1]);
                                if (VertexConnected != j)
                                    break;
                            }
                            adjacencyMatrix.AdjacencyArray[VertexConnected, j] = 1;
                        }
                    }
                }
                for (int i = 1; i <= N+1; i++) //sprawdzanie czy warstwy sa polaczone
                {
                    bool LayersConnected = false;
                    for (int j = SumOfPreviousLayers[i]; j < SumOfPreviousLayers[i] + Layers[i]; j++)
                    {
                        for (int k = SumOfPreviousLayers[i - 1]; k < SumOfPreviousLayers[i]; k++)
                        {
                            if (adjacencyMatrix.AdjacencyArray[k, j] != 0)
                                LayersConnected = true;
                        }
                    }
                    if (LayersConnected == false)
                    {
                        int VertexFrom = r.Next(SumOfPreviousLayers[i - 1], SumOfPreviousLayers[i] );
                        int VertexTo = r.Next(SumOfPreviousLayers[i], SumOfPreviousLayers[i] + Layers[i]);
                        adjacencyMatrix.AdjacencyArray[VertexFrom, VertexTo] = 1;
                    }

                }

                adjacencyMatrix.Display(Layers, SumOfPreviousLayers, StackPanelForDisplayingAdjacencyMatrix, MyCanvas, StackPanelForDisplayingIncidenceMatrix, StackPanelForDisplayingAdjacencylist);
            }
            else
            {
                Probability_Of_Edge_Occurence.Background = Brushes.OrangeRed;
            }
        }

        private void NumbersOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

      


      
        private void ComputeShortestPath_Click(object sender, RoutedEventArgs e)
        {
            
        }


    }
}
