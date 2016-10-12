using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RangeMinimumUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] tokens = Console.ReadLine().Split();
            long n = long.Parse(tokens[0]);
            long q = long.Parse(tokens[1]);

            SegmentTree sg = new SegmentTree(n);
            while (n > 0)
            {
                string[] array = Console.ReadLine().Split();
                sg.Add(array);
                n -= array.Length;
            }
            sg.ConstructSegmentTree(n);
       //     string[] array = Console.ReadLine().Split();
          //  SegmentTree sg = new SegmentTree(array, n);

            while (q-- != 0)
            {
                string[] query = Console.ReadLine().Split();

                if (query[0] == "q")
                {
                    Console.WriteLine(sg.MinimumInSubArray(long.Parse(query[1]), long.Parse(query[2])));
                }
                else
                {
                    sg.UpdateArray(long.Parse(query[1]), long.Parse(query[2]));
                }
            }
        }
    }

    internal class SegmentTree
    {
        private long[] array;
        private long[] starray;

        public SegmentTree(string[] tokens, long n)
        {
            long i = 0;
            array = new long[n];
            foreach(string s in tokens)
            {
                array[i++] = long.Parse(s);
            }

            long sn = n * 2 - 1;
            starray = new long[sn];
            this.ConstructSegmentTree(0, n - 1, 0);
        }

        public SegmentTree(long n)
        {
            array = new long[n];
            starray = new long[n * 2 - 1];
        }

        internal long MinimumInSubArray(long qstart, long qend)
        {
            return this.FindRangeMinimum(0, array.Length - 1, qstart - 1, qend - 1, 0);
        }

        private long FindRangeMinimum(long astart, long aend, long qstart, long qend, long sindex)
        {
            if (qstart <= astart && qend >= aend)
            {
                return starray[sindex];
            }

            if (qend < astart || qstart > aend)
            {
                return long.MaxValue;
            }

            long mid = (astart + aend) / 2;
            return this.Minimum(FindRangeMinimum(astart, mid, qstart, qend, sindex * 2 + 1),
                                FindRangeMinimum(mid + 1, aend, qstart, qend, sindex * 2 + 2));
        }

        private long ConstructSegmentTree(long arrayStart, long arrayEnd, long stArrayIndex)
        {
            if (arrayStart == arrayEnd)
            {
                starray[stArrayIndex] = array[arrayStart];
                return starray[stArrayIndex];
            }

            long mid = (arrayStart + arrayEnd) / 2;

            starray[stArrayIndex] = this.Minimum(ConstructSegmentTree(arrayStart, mid, stArrayIndex * 2 + 1),
                                                    ConstructSegmentTree(mid + 1, arrayEnd, stArrayIndex * 2 + 2));

            return starray[stArrayIndex];
        }

        private long Minimum(long v1, long v2)
        {
            return v1 < v2 ? v1 : v2;
        }

        internal void UpdateArray(long aindex, long avalue)
        {
            UpdateStArray(0, array.Length - 1, aindex - 1, avalue, 0);
        }

        private void UpdateStArray(long astart, long aend, long aindex, long avalue, long sindex)
        {
            if (aindex < astart || aindex > aend)
            {
                return;
            }

            starray[sindex] = Minimum(starray[sindex], avalue);
            if (astart == aend)
            {
                return;
            }

            long mid = (astart + aend) / 2;
            UpdateStArray(astart, mid, aindex, avalue, sindex * 2 + 1);
            UpdateStArray(mid + 1, aend, aindex, avalue, sindex * 2 + 2);
        }

        internal void Add(string[] tokens)
        {
            long i = 0;
            foreach (string s in tokens)
            {
                array[i++] = long.Parse(s);
            }
        }

        internal void ConstructSegmentTree(long n)
        {
            long sn = n * 2 - 1;
            this.ConstructSegmentTree(0, n - 1, 0);
        }
    }
}
