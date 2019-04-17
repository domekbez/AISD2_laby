
using System;

namespace ASD
{

    //
    // Za kazdy z algorytmow jest 1 pkt.
    //

    public class SortingMethods : MarshalByRefObject
    {

        public int[] QuickSort(int[] tab)
        {
            if (tab.Length == 0) return tab;
            QuickSortpom(0, tab.Length - 1,tab);

            return tab;
        }
        public static void QuickSortpom(int l, int r, int[] tab)
        {
            int i = l;
            int j = r;
            int sr = tab[(l + r) / 2];
            while (i < j)
            {
                while (tab[i] < sr) i++;
                while (tab[j] > sr) j--;
                if (i <= j)
                {
                    int tmp = tab[i];
                    tab[i++] = tab[j];  
                    tab[j--] = tmp;
                }
            }
            if (l < j) QuickSortpom(l, j,tab);
            if (i < r) QuickSortpom(i, r,tab);
        }
        public int[] ShellSort(int[] tab)
        {
            if (tab.Length == 0) return tab;
            int poz = 2;
            int n = tab.Length - 1;
            while (poz - 1 < n / 2)
                poz *= 2;
            poz--;
            while (poz >= 1)
            {
                for (int j = poz; j <= n; j++)
                {
                    int val = tab[j];
                    int i = j - poz;
                    while (i >= 0 && tab[i] > val)
                    {
                        tab[i + poz] = tab[i];
                        i -= poz;
                    }
                    tab[i + poz] = val;
                }
                poz = (poz + 1) / 2 - 1;
            }

            return tab;
        }

        public void DownHeap(int i, int n, int[] tab)
        {
            if (tab.Length == 0) return;
            int k;
            if (i == 0) k = 1;
            else k = 2 * i;
            int val = tab[i];
            while (k < n)
            {
                if (k + 1 < n)
                    if (tab[k + 1] > tab[k])
                        k++;
                if (tab[k] > val)
                {
                    tab[i] = tab[k];
                    i = k;
                    k = 2 * i;
                }
                else break;
            }
            tab[i] = val;
        }
        public int[] HeapSort(int[] tab)
        {
            if (tab.Length == 0) return tab;
            int hl = tab.Length;
            for (int i = hl / 2; i >= 0; i--)
                DownHeap(i, hl, tab);
            while (hl > 0)
            {
                int pom = tab[0];
                tab[0] = tab[hl - 1];
                tab[hl - 1] = pom;
                hl--;
                DownHeap(0, hl, tab);

            }

            return tab;
        }

        public int[] MergeSort(int[] tab)
        {

            if (tab.Length == 0) return tab;
            MergeSortpom(0, tab.Length - 1, tab);
            return tab;
        }

        public void MergeSortpom(int l, int r, int[] tab)
        {
            if (l == r) return;

            int m = (l + r) / 2;

            MergeSortpom(l, m, tab);
            MergeSortpom(m + 1, r, tab);
            Merge(l, m, m + 1, r, tab);
        }
        public void Merge(int l1, int r1, int l2, int r2, int[] tab)
        {
            int count = r2 - l1 + 1;
            int[] tabpom = new int[count];
            int lewy1 = l1;
            int lewy2 = l2;
            int i = 0;
            for (; i < count && lewy1 <= r1 && lewy2 <= r2; i++)
            {
                if (tab[lewy1] <= tab[lewy2])
                {
                    tabpom[i] = tab[lewy1];
                    lewy1++;
                }
                else
                {
                    tabpom[i] = tab[lewy2];
                    lewy2++;
                }

            }
            if (lewy1 > r1)
                for (; i < count; i++, lewy2++)
                    tabpom[i] = tab[lewy2];


            if (lewy2 > r2)
                for (; i < count; i++, lewy1++)
                    tabpom[i] = tab[lewy1];


            lewy1 = l1;
            for (int j = 0; j < count; j++, lewy1++)
            {
                tab[lewy1] = tabpom[j];

            }
        }
    }
}
