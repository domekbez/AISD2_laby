using System;
using System.Collections.Generic;
using ASD.Graphs;
using System.Linq;
using System.Collections;

namespace ASD
{
    public class RoutePlanner : MarshalByRefObject
    {

        // mozna dodawac metody pomocnicze

        /// <summary>
        /// Znajduje dowolny cykl skierowany w grafie lub stwierdza, że go nie ma.
        /// </summary>
        /// <param name="g">Graf wejściowy</param>
        /// <returns>Kolejne wierzchołki z cyklu lub null, jeśli cyklu nie ma.</returns>
        //public int[] FindCycle(Graph g)
        //{
        //    int[] tab;
        //    Stack s = new Stack();
        //    for (int i = 0; i < g.VerticesCount; i++)
        //    {
        //        tab = FindCycleRec(g, i, s);
        //        if (tab != null)
        //           return tab;
        //        s.Clear();
        //    }
        //    return null;

        //}
        public int[] FindCycle(Graph g)
        {
       
            List<int> cykl = new List<int>();
            int a = 0;
            Predicate<int> dodaj = delegate (int v)
            {
                cykl.Add(v);
                return true;
            };

            Predicate<int> usun = delegate (int v)
            {
                cykl.Remove(v);
                return true;
            };

            Predicate<Edge> sprawdz = delegate (Edge e)
            {
                if (cykl.Contains(e.To))
                {
                    a = e.To;
                    return false;
                }
                return true;
            };
            g.GeneralSearchAll<EdgesStack>(dodaj, usun, sprawdz, out int pom);
            if (cykl.Count==0) return null;
            int i = 0;
            while(cykl[i]!=a)
            {
                cykl.RemoveAt(i);
            }
            return cykl.ToArray();

        }
        //public int[] FindCycleRec(Graph g, int w, Stack s)
        //{




        //    s.Push(w);

        //    foreach (var el in g.OutEdges(w))
        //    {


        //        if (s.Contains(el.To))
        //        {
        //            List<int> pom = new List<int>();
        //            int b = 1000;
        //            while(b!=el.To)
        //            {
        //                b = (int)s.Pop();
        //                pom.Add(b);

        //            }
        //            pom.Reverse();
        //            return pom.ToArray();
        //        }

        //        return FindCycleRec(g,el.To,s);


        //    }
        //    s.Clear();
        //    return null;


        //}
        /// <summary>
        /// Rozwiązanie wariantu 1.
        /// </summary>
        /// <param name="g">Graf połączeń, które trzeba zrealizować</param>
        /// <returns>Lista tras autobusów lub null, jeśli zadanie nie ma rozwiązania</returns>
        public int[][] FindShortRoutes(Graph g)
        {
            for (int i = 0; i < g.VerticesCount; i++)
            {
                if ((g.InDegree(i) +  g.OutDegree(i)) % 2 == 1) return null;
            }
            Graph pom = g.Clone();
            
            List<int[]> listapom = new List<int[]>();
            while(true)
            {
                int[] cykl = FindCycle(pom);
                
                if (cykl == null ) break;
                listapom.Add(cykl);
                for (int i = 0; i <cykl.Length-1;i++)
                {
                    Edge e = new Edge(cykl[i], cykl[i + 1]);
                    pom.DelEdge(e);
          
                }
                Edge ee=new Edge(cykl[cykl.Length - 1], cykl[0]);
                pom.DelEdge(ee);
            }
            if (listapom.Count == 0||pom.EdgesCount!=0) return null;
            return listapom.ToArray();
            
        }

        /// <summary>
        /// Rozwiązanie wariantu 2.
        /// </summary>
        /// <param name="g">Graf połączeń, które trzeba zrealizować</param>
        /// <returns>Lista tras autobusów lub null, jeśli zadanie nie ma rozwiązania</returns>
        /// </summary>
        public int[][] FindLongRoutes(Graph g)
        {
            for(int i=0;i<g.VerticesCount;i++)
            {
                if ((g.OutDegree(i) + g.InDegree(i)) % 2 == 1) return null;
            }
            Graph pom = g.Clone();

            int[][] cykle = FindShortRoutes(g);
            if (cykle == null) return null;
            List<List<int>> wszystkiecykle = new List<List<int>>();
            for (int i = 0; i < cykle.GetLength(0); i++)
            {
                wszystkiecykle.Add(cykle[i].ToList());
            }
            bool skrocono = false;
            do
            {
                skrocono = false;


                for (int z = 0; z < wszystkiecykle.Count; z++)
                {
                    for (int i = z + 1; i < wszystkiecykle.Count; i++)
                    {
                        for (int j = 0; j < wszystkiecykle[i].Count; j++)
                        {
                            if (wszystkiecykle[z].Contains(wszystkiecykle[i][j]))
                            {
                                int pomlicz = wszystkiecykle[i][j];
                                List<int> nowa = new List<int>();
                                int ii = 0;
                                for (; wszystkiecykle[i][ii] != wszystkiecykle[i][j]; ii++)
                                {
                                    nowa.Add(wszystkiecykle[i][ii]);
                                }
                                int jj = 0;
                                for (; wszystkiecykle[z][jj] != wszystkiecykle[i][j]; jj++)
                                {

                                }
                                for (; jj < wszystkiecykle[z].Count; jj++)
                                {
                                    nowa.Add(wszystkiecykle[z][jj]);
                                }
                                jj = 0;
                                for (; wszystkiecykle[z][jj] != wszystkiecykle[i][j]; jj++)
                                {
                                    nowa.Add(wszystkiecykle[z][jj]);
                                }
                                nowa.Add(pomlicz);
                                ii++;
                                for (; ii < wszystkiecykle[i].Count; ii++)
                                {
                                    nowa.Add(wszystkiecykle[i][ii]);
                                }

                                wszystkiecykle.Remove(wszystkiecykle[i]);
                                wszystkiecykle.Remove(wszystkiecykle[z]);

                                wszystkiecykle.Add(nowa);


                                skrocono = true;
                                break;
                            }
                            if (skrocono == true) break;
                        }
                        if (skrocono == true) break;

                    }
                    if (skrocono == true) break;

                }


            } while (skrocono == true);
            int[][] ret = new int[wszystkiecykle.Count][];
            for (int i = 0; i < wszystkiecykle.Count; i++)
            {
                ret[i] = wszystkiecykle[i].ToArray();
            }
          
            return ret;
        }

    }

}
