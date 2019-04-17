using System;
using System.Collections;
using System.Collections.Generic;
using ASD.Graphs;

namespace ASD
{
    

    public class Lab03 : MarshalByRefObject
    {
        // Część 1
        //  Sprawdzenie czy podany ciąg stopni jest grafowy
        //  0.5 pkt

        public bool IsGraphic(int[] sequence)
        {
            int sum = 0;
            if (sequence.Length == 1 && sequence[0] != 0) return false;

            List<int> pom = new List<int>(sequence);
            pom.Sort();
            pom.Reverse();

            while (pom.Count>0)
            {
                if (pom[0] +1 > pom.Count) return false;
                for(int i=1;i<pom[0]+1;i++)
                {
                    pom[i]--;
                }
                pom.RemoveAt(0);
                
                pom.Sort();
                pom.Reverse();
                foreach (var el in pom)
                {
                    if (el < 0) return false;
                    sum += el;
                }
                if (sum == 0) return true;
                if(sum%2==1)
                {

                    return false;
                }
                sum=0;
            }
            return false;
        }

        //Część 2
        // Konstruowanie grafu na podstawie podanego ciągu grafowego
        // 1.5 pkt
        public Graph ConstructGraph(int[] sequence)
        {

            Graph wynik = new AdjacencyListsGraph<SimpleAdjacencyList>(false, sequence.Length);
            if (IsGraphic(sequence) == false) return null;

               List < KeyValuePair<int, int>> listapar = new List<KeyValuePair<int, int>>();
            for(int i=0;i<sequence.Length;i++)
            {
                KeyValuePair<int, int> el = new KeyValuePair<int, int>(i, sequence[i]);
                listapar.Add(el);
            }

            while (listapar.Count > 0)
            {
                listapar.Sort((x, y) =>
                {
                    return -x.Value.CompareTo(y.Value);

                });
                for(int j=1;j<=listapar[0].Value&&j<listapar.Count;j++)
                {
                    wynik.AddEdge(listapar[0].Key, listapar[j].Key);
                    wynik.AddEdge(listapar[j].Key, listapar[0].Key);

                    listapar[j]= new KeyValuePair<int, int>(listapar[j].Key, listapar[j].Value-1);
                }
                listapar.RemoveAt(0);

            }
            

            return wynik;
        }

        //Część 3
        // Wyznaczanie minimalnego drzewa (bądź lasu) rozpinającego algorytmem Kruskala
        // 2 pkt
        public Graph MinimumSpanningTree(Graph graph, out double min_weight)
        {
            
            MinimHeapp edges = new MinimHeapp(graph.EdgesCount);
            UnionFind union = new UnionFind(graph.VerticesCount);

            Graph wynik = graph.IsolatedVerticesGraph();

            if (graph.Directed)
                throw new ArgumentException("Graf skierowany");
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                foreach (var el in graph.OutEdges(i))
                {
                    if (el.From > el.To)
                        edges.Add(el);
                   
                }
            }
            min_weight = 0;

            while (edges.Empty() == false)
            {
                Edge e = edges.RemoveMin();

                int nrset1 = union.Find(e.From);
                int nrset2 = union.Find(e.To);
                if (nrset1 != nrset2)
                {
                    union.Union(nrset1, nrset2);
                    min_weight += e.Weight;
                    wynik.AddEdge(e);


                }
            }

            return wynik;
            
            
        }
    }
    class MinimHeapp
    {
        private Edge[] tab;
        private int count;
        public MinimHeapp(int size)
        {
            tab = new Edge[size];
            count = 0;
        }

        public void Add(Edge element)
        {
            tab[count] = element;
            UpHeap(count);
            count++;
        }

        public Edge RemoveMin()
        {
            Edge ret = tab[0];
            tab[0] = tab[--count];
            DownHeap(0, count);


            return ret;
        }
        public bool Empty()
        {
            if (count == 0) return true;
            return false;
        }
        private void UpHeap(int i)
        {

            Edge v = tab[i];
            while (tab[i / 2].Weight > v.Weight)
            {

                tab[i] = tab[i / 2];
                if (i == 0) break;
                if (i != 1)
                    i = i / 2;
                else
                    i = 0;
            }
            tab[i] = v;
        }
        private void DownHeap(int i, int n)
        {
            int k = 1;
            Edge v = tab[i];
            while (k <= n)
            {
                if (k + 1 <= n)
                    if (tab[k + 1].Weight < tab[k].Weight)
                        k = k + 1;
                if (tab[k].Weight < v.Weight)
                {
                    tab[i] = tab[k];
                    i = k;
                    k = 2 * i;
                }
                else
                    break;
            }
            tab[i] = v;
        }

    }
}