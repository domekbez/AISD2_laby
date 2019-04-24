using System;
using System.Collections.Generic;
using System.Linq;
using ASD.Graphs;

namespace ASD
{
    public class Lab08 : MarshalByRefObject
    {
        /// <summary>
        /// Znajduje cykl rozpoczynający się w stolicy, który dla wybranych miast,
        /// przez które przechodzi ma największą sumę liczby ludności w tych wybranych
        /// miastach oraz minimalny koszt.
        /// </summary>
        /// <param name="cities">
        /// Graf miast i połączeń między nimi.
        /// Waga krawędzi jest kosztem przejechania między dwoma miastami.
        /// Koszty transportu między miastami są nieujemne.
        /// </param>
        /// <param name="citiesPopulation">Liczba ludności miast</param>
        /// <param name="meetingCosts">
        /// Koszt spotkania w każdym z miast.
        /// Dla części pierwszej koszt spotkania dla każdego miasta wynosi 0.
        /// Dla części drugiej koszty są nieujemne.
        /// </param>
        /// <param name="budget">Budżet do wykorzystania przez kandydata.</param>
        /// <param name="capitalCity">Numer miasta będącego stolicą, z której startuje kandydat.</param>
        /// <param name="path">
        /// Tablica dwuelementowych krotek opisująca ciąg miast, które powinen odwiedzić kandydat.
        /// Pierwszy element krotki to numer miasta do odwiedzenia, a drugi element decyduje czy
        /// w danym mieście będzie organizowane spotkanie wyborcze.
        /// 
        /// Pierwszym miastem na tej liście zawsze będzie stolica (w której można, ale nie trzeba
        /// organizować spotkania).
        /// 
        /// Zakładamy, że po odwiedzeniu ostatniego miasta na liście kandydat wraca do stolicy
        /// (na co musi mu starczyć budżetu i połączenie między tymi miastami musi istnieć).
        /// 
        /// Jeżeli kandydat nie wyjeżdża ze stolicy (stolica jest jedynym miastem, które odwiedzi),
        /// to lista `path` powinna zawierać jedynie jeden element: stolicę (wraz z informacją
        /// czy będzie tam spotkanie czy nie). Nie są wtedy ponoszone żadne koszty podróży.
        /// 
        /// W pierwszym etapie drugi element krotki powinien być zawsze równy `true`.
        /// </param>
        /// <returns>
        /// Liczba mieszkańców, z którymi spotka się kandydat.
        /// </returns>
        /// 
        double[] meetingCostspom;
        int[] CitiesPopulationpom;
        int stolica;
        Graph citiespom;
        double bestbudget = 0;
      

        public int ComputeElectionCampaignPath(Graph cities, int[] citiesPopulation,
            double[] meetingCosts, double budget, int capitalCity, out (int, bool)[] path)
        {

            path = null;

            meetingCostspom = meetingCosts;
            CitiesPopulationpom = citiesPopulation;
            citiespom = cities;
            stolica = capitalCity;
            int glosy = 0;
            bool[] zazn = new bool[citiesPopulation.Length];
            List<(int, bool)> sciezka = new List<(int, bool)>();
            List<(int, bool)> sciezkapom = new List<(int, bool)>();
            sciezkapom.Add((capitalCity, false));
            Rekurencja(budget, ref glosy, 0, capitalCity, ref sciezka, sciezkapom, zazn);
            if (budget >= meetingCosts[capitalCity])
            {
                if (glosy == 0)
                    glosy = citiesPopulation[capitalCity];
                sciezkapom = new List<(int, bool)> {};
                sciezkapom.Add((capitalCity, true));
             
                Rekurencja(budget - meetingCosts[capitalCity], ref glosy, citiesPopulation[capitalCity], capitalCity, ref sciezka, sciezkapom, zazn);
            }
            if (glosy == 0)
            {
                path = new (int, bool)[1];
                path[0] = (capitalCity, false);
                return glosy;
            }
            if (glosy == citiesPopulation[capitalCity])
            {
                path = new (int, bool)[1];
                path[0] = (capitalCity, true);
                return glosy;
            }
            path = sciezka.ToArray();
            return glosy;
        }
        public void Rekurencja(
            double budget, ref int bestvotes, int curvotes, int curvert,
            ref List<(int, bool)> bestpath,
            List<(int, bool)> curpath, bool[] zaznaczone)
        {
            
            foreach (var el in citiespom.OutEdges(curvert))
            {
                if (budget < el.Weight) continue;
                if (zaznaczone[el.To] == true) continue;
                if (el.To == stolica)
                {
                    if (curvotes < bestvotes) continue;
                    if (curvotes > bestvotes || (curvotes == bestvotes && budget - el.Weight > bestbudget))
                    {
                        bestbudget = budget - el.Weight;
                        bestvotes = curvotes;                       
                        bestpath = new List<(int, bool)>();
                        for (int i = 0; i < curpath.Count; i++)
                        {
                            bestpath.Add(curpath[i]);
                        }

                    }
                    continue;
                }
                if (meetingCostspom[el.To] == 0)
                {

                    zaznaczone[el.To] = true;
                    budget -= el.Weight;
                    curvotes += CitiesPopulationpom[el.To];
                    curpath.Add((el.To, true));
                    curvert = el.To;
                    Rekurencja(budget, ref bestvotes
                    , curvotes, curvert, ref bestpath, curpath, zaznaczone);
                    zaznaczone[el.To] = false;
                    budget += el.Weight;
                    curvotes -= CitiesPopulationpom[el.To];
                    curvert = el.From;
                    curpath.RemoveAt(curpath.Count - 1);
                    continue;
                }

                zaznaczone[el.To] = true;
                budget -= el.Weight;
                curpath.Add((el.To, false));
                curvert = el.To;
                Rekurencja(budget, ref bestvotes
                    , curvotes, curvert, ref bestpath, curpath, zaznaczone);
                if (budget - meetingCostspom[el.To] <= 0)
                {

                    zaznaczone[el.To] = false;
                    budget += el.Weight;
                    curpath.RemoveAt(curpath.Count - 1);
                    curvert = el.From;
                    continue;
                }

                curpath.RemoveAt(curpath.Count - 1);
                zaznaczone[el.To] = true;
                budget -= meetingCostspom[el.To];
                curvotes += CitiesPopulationpom[el.To];
                curpath.Add((el.To, true));
                curvert = el.To;
                Rekurencja(budget, ref bestvotes
                    , curvotes, curvert, ref bestpath, curpath, zaznaczone);

                zaznaczone[el.To] = false;
                budget += el.Weight;
                budget += meetingCostspom[el.To];
                curvotes -= CitiesPopulationpom[el.To];
                curpath.RemoveAt(curpath.Count - 1);
                curvert = el.From;
            }


        }



        // mozna dopisywa pomocnicze pola i metody
        // a nawet pomocnicze klasy!

    }

}






