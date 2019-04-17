using System;
namespace ASD
{
    public enum TaxAction
    {
        Empty,
        TakeMoney,
        TakeCarrots
    }

    public class TaxCollectorManager : MarshalByRefObject
    {
        public int CollectMaxTax(int[] dist, int[] money, int[] carrots, int maxCarrots, int startingCarrots, out TaxAction[] collectingPlan)
        {
            int n = money.Length;
            collectingPlan = new TaxAction[n];
            int max = -1;
            int zas2 = maxCarrots + 1;
            int[,] dynam = new int[n, zas2];
            TaxAction[,] plany = new TaxAction[n, zas2];
            int[,] ilemarchewek = new int[n,zas2];

            
            

            for (int j = 0; j < n; j++)
                for (int i = 0; i < zas2; i++)
                    dynam[j,i] = -1;
            dynam[0,startingCarrots]= money[0];
            plany[0,startingCarrots] = TaxAction.TakeMoney;
            int oby = startingCarrots + carrots[0];
            if (oby >= zas2) oby = maxCarrots;
            dynam[0,oby]= 0;
            plany[0,oby] = TaxAction.TakeCarrots;
            ilemarchewek[0,oby] = oby - startingCarrots;
            for (int i = 1; i < n; i++)
            {

                int iii = i - 1;
                for (int j = 1; j < zas2; j++)
                {
                    if (dynam[iii,j] < 0) continue;
                    int nac = j - dist[i];

                    if (nac < 0) continue;

                    int nac7 = dynam[iii,j] + money[i];


                    int zas = nac + carrots[i];
                    if (dynam[i,nac] < nac7)
                    {
                        plany[i,nac] = TaxAction.TakeMoney;
                        dynam[i,nac]= nac7;
                    }
                    if (zas >= zas2) zas = maxCarrots;
                    if (dynam[iii,j] > dynam[i,zas])
                    {
                        plany[i,zas] = TaxAction.TakeCarrots;
                        dynam[i,zas] = dynam[iii,j];
                        ilemarchewek[i,zas] = zas - nac;
                    }

                }
            }
            int indMax = -1;
            for (int j = startingCarrots; j < zas2; j++)
            {
                if (dynam[n - 1,j] > max)
                {
                    indMax = j;
                    max = dynam[n - 1,j];
                }
            }
            if (max == -1)
            {
                collectingPlan = null;
                return max;
            }
            for (int i = n - 1; i >= 0; i--)
            {
                collectingPlan[i] = plany[i,indMax];
                indMax += dist[i];
                if (collectingPlan[i] == TaxAction.TakeCarrots)
                    indMax = indMax - ilemarchewek[i,indMax - dist[i]];
            }
            return max;
        }
    }
}