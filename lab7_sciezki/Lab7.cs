using System;
using System.Collections.Generic;
using System.Linq;
using ASD.Graphs;


namespace Lab7
{

    public class BestCitiesSolver : MarshalByRefObject
    {
        
            public (int c1, int c2, int? bypass, double time, Edge[] path)? FindBestCitiesPair(Graph times, double[] passThroughCityTimes, int[] nominatedCities, bool buildBypass)
            {
            int n = times.VerticesCount;

            
            int c1 = -1, c2 = -1;
            Graph g = times.IsolatedVerticesGraph(true,n);
 

            for (int i = 0; i < n; i++)
            {
                foreach (var e in times.OutEdges(i))
                {
                    g.AddEdge(e.From, e.To, e.Weight + passThroughCityTimes[e.From]);

                }
            }

            PathsInfo[] opt = null;

            PathsInfo[][] sciezki = new PathsInfo[nominatedCities.Length][];

            double dystans = double.MaxValue;
            for (int i = 0; i < nominatedCities.Length; i++)
            {
                foreach (var e in g.OutEdges(nominatedCities[i]))
                {
                    g.ModifyEdgeWeight(e.From, e.To, -passThroughCityTimes[nominatedCities[i]]);
                }
                PathsInfo[] pi;

                g.DijkstraShortestPaths(nominatedCities[i], out pi);
                sciezki[i] = pi;

                for (int k = 0; k < nominatedCities.Length; k++)
                {
                    if (k == i) continue;
                    if (GraphHelperExtender.IsNaN(pi[nominatedCities[k]].Dist) == true) continue;
                    if (pi[nominatedCities[k]].Dist < dystans)
                    {
                        dystans = pi[nominatedCities[k]].Dist;
                        c1 = nominatedCities[i];
                        c2 = nominatedCities[k];
                        opt = pi;
                    }
                }

                foreach (var e in g.OutEdges(nominatedCities[i]))
                {
                    g.ModifyEdgeWeight(e.From, e.To, passThroughCityTimes[nominatedCities[i]]);
                }

            }
            if (dystans == double.MaxValue) return null;
            Edge[] sciezka = PathsInfo.ConstructPath(c1, c2, opt);
            if (buildBypass == false)
                return (c1, c2, null, dystans, sciezka);


            //ETAP 2
            int obwodnica = -1;
            PathsInfo p1=new PathsInfo();
            PathsInfo p2= new PathsInfo();



            for (int i = 0; i < nominatedCities.Length; i++)
            {
                for (int j = 0; j < nominatedCities.Length; j++)
                {
                    if (i == j) continue;
                    for (int k = 0; k < n; k++)
                    {
                        if (k == nominatedCities[i] || k == nominatedCities[j]) continue;
                        if (sciezki[i][k].Dist+sciezki[j][k].Dist<dystans)
                        {
                            dystans= sciezki[i][k].Dist + sciezki[j][k].Dist;
                            obwodnica = k;
                            //sciezki[i][k].Dist -= passThroughCityTimes[k];
                            c1 = nominatedCities[i];
                            c2 = nominatedCities[j];
                            p1 = sciezki[i][k];
                            p2 = sciezki[j][k];


                            //p1 = sciezki[i];
                            //sciezki[i][k].Dist += passThroughCityTimes[k];



                        }

                    }
                }
            }
           

            if (obwodnica==-1)
                return (c1, c2, null, dystans, sciezka);

            //sciezka = PathsInfo.ConstructPath(c1, c2,p1);

            int indc1 = -1;
            int indc2 = -1;
            for(int i=0;i<n;i++)
            {
                if(nominatedCities[i]==c1)
                {
                    indc1 = i;
                    break;
                }
            }
            for (int j = 0; j < n; j++)
            {
                if (nominatedCities[j] == c2)
                {
                    indc2 = j;
                    break;
                }
            }



            List<Edge> sciezkapom = new List<Edge>();
            sciezkapom.Add((Edge)p1.Last);
            int ostatniwierzch= ((Edge)p1.Last).From;
            while(ostatniwierzch!=c1)
            {
                sciezkapom.Add((Edge)sciezki[indc1][ostatniwierzch].Last);
                ostatniwierzch = ((Edge)sciezki[indc1][ostatniwierzch].Last).From;
            }
            sciezkapom.Reverse();
            sciezkapom.Add(new Edge(((Edge)p2.Last).To, ((Edge)p2.Last).From, ((Edge)p2.Last).Weight));
            ostatniwierzch= ((Edge)p2.Last).From;
            while(ostatniwierzch!=c2)
            {


                sciezkapom.Add(new Edge(((Edge)sciezki[indc2][ostatniwierzch].Last).To, ((Edge)sciezki[indc2][ostatniwierzch].Last).From, ((Edge)sciezki[indc2][ostatniwierzch].Last).Weight));
                ostatniwierzch = ((Edge)sciezki[indc2][ostatniwierzch].Last).From;
            }
            sciezka = sciezkapom.ToArray();
            return (c1, c2, obwodnica, dystans, sciezka);
        }
        

    }

}

