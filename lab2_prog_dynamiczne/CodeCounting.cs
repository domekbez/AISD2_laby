
using System;
using System.Collections.Generic;

namespace ASD
{

public class CodesCounting : MarshalByRefObject
    {

    public int CountCodes(string text, string[] codes, out int[][] solutions)
        {
            
            int[] tab = new int[text.Length+1];
            tab[0] = 1;
            List<List<int>>[] tablicapom = new List<List<int>>[text.Length+1];
            for (int i = 0; i < text.Length + 1; i++)
                tablicapom[i] = new List<List<int>>();

            for (int i = 0; i < text.Length+1; i++)
            {
                for (int j = 0; j < codes.Length; j++)
                {
                    if (i - codes[j].Length < 0) continue;
                    if (text.Substring(i - codes[j].Length, codes[j].Length) == codes[j])
                    {
                        tab[i] += tab[i - codes[j].Length];
                        if ((i - codes[j].Length) == 0)
                        {
                            List<int> p = new List<int>();
                            p.Add(j);
                            tablicapom[i].Add(p);
                        }
                        else
                        {
                            for (int g = 0; g < tablicapom[i - codes[j].Length].Count; g++)
                            {
                                List<int> pom = new List<int>(tablicapom[i - codes[j].Length][g]);
                                pom.Add(j);
                                tablicapom[i].Add(pom);
                            }
                        }
                    }
                     
                }
            }
            solutions = new int[tab[text.Length]][];
            for(int g=0;g<tablicapom[text.Length].Count;g++)
            {
                solutions[g] = tablicapom[text.Length][g].ToArray();
            }
            return tab[text.Length];
        }
    }
}
