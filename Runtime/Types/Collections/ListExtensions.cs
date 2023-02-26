using System;
using System.Collections.Generic;
using System.Text;

namespace ch.sycoforge.Util.Collections
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n));
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
