using System;
using System.Collections.Generic;

namespace ASD
{
    public class Squares : MarshalByRefObject
    {
        /// <param name="n">Długość boku działki, którą dzielimy</param>
        /// <param name="sizes">Dopuszczalne długości wynikowych działek</param>
        /// <param name="solution">Tablica n*n z znalezionym podziałem, każdy element to unikalny dodatni identyfikator kwadratu</param>
        /// <returns>Liczba kwadratów na jakie została podzielona działka lub 0 jeśli poprawny podział nie istnieje </returns>
        public int FindDisivion(int n, int[] sizes, out int[,] solution)
        {
            bool czynp = false;
            List<int> pom = new List<int>();
            for (int i = 0; i < sizes.Length; i++)
            {
                if (sizes[i] <= n)
                {
                    if (sizes[i] % 2 == 1)
                        czynp = true;
                    
                    pom.Add(sizes[i]);
                }
            }
            if(n%2==1 && czynp==false)
            {
                solution = null;
                return 0;
            }
            pom.Sort();
            pom.Reverse();
            sizes = pom.ToArray();
            solution = new int[n, n];
            int[,] obsolution = new int[n, n];

            int opt = int.MaxValue;

            int wyn = 0;
            bool[,] zaznaczone = new bool[n, n];
            CheckRec(n, sizes, zaznaczone, ref wyn, ref solution, obsolution, ref opt, 0);


            return wyn;
        }

        public void CheckRec(int n, int[] sizes, bool[,] zaznaczone, ref int wyn, ref int[,] sol, int[,] obsol, ref int opt, int obecny)
        {

            bool flaga2 = false;
            bool flaga = false;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (zaznaczone[i, j] == false)
                    {
                        for (int k = 0; k < sizes.Length; k++)
                        {
                            if (i + sizes[k] <= n && j + sizes[k] <= n)
                            {
                                for (int z = 0; z < sizes[k]; z++)
                                {
                                    for (int s = 0; s < sizes[k]; s++)
                                    {
                                        if (zaznaczone[i + z, j + s] == true) flaga = true;
                                    }
                                }
                                if (flaga != true)
                                {

                                    if (opt < obecny) continue;

                                    for (int z = 0; z < sizes[k]; z++)
                                    {
                                        for (int s = 0; s < sizes[k]; s++)
                                        {
                                            zaznaczone[i + z, j + s] = true;

                                            obsol[i + z, j + s] = obecny + 1;
                                        }
                                    }



                                    
                                    flaga2 = false;
                                    for (int pp = 0; pp < n; pp++)
                                        for (int mm = 0; mm < n; mm++)
                                        {
                                            if (zaznaczone[pp, mm] == false) flaga2 = true;
                                        }
                                    if (flaga2 == false)
                                    {
                                        wyn = 1;
                                        if (obecny < opt)
                                        {
                                            sol = (int[,])obsol.Clone();
                                            opt = obecny;
                                        }
                                        for (int z = 0; z < sizes[k]; z++)
                                        {
                                            for (int s = 0; s < sizes[k]; s++)
                                            {
                                                zaznaczone[i + z, j + s] = false;
                                                obsol[i + z, j + s] = 0;

                                            }
                                        }
                                        obecny--;
                                        return;
                                    }
                                    obecny++;
                                    CheckRec(n, sizes, zaznaczone, ref wyn, ref sol, obsol, ref opt, obecny);

                                    obecny--;


                                    for (int z = 0; z < sizes[k]; z++)
                                    {
                                        for (int s = 0; s < sizes[k]; s++)
                                        {
                                            zaznaczone[i + z, j + s] = false;
                                            obsol[i + z, j + s] = 0;

                                        }
                                    }

                                }
                                flaga = false;
                            }

                            
                        }
                        return;
                    }

                }
            }

        }
    }
}