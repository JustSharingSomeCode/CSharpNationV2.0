using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNation.Diagnostics
{
    public class PerformanceLog
    {
        public PerformanceLog(string name)
        {
            Name = name;
            Values = new List<long>();
        }

        public string Name { get; private set; }
        public long MinMillis { get; private set; } = long.MaxValue;
        public long MaxMillis { get; private set; } = long.MinValue;

        public List<long> Values { get; private set; }

        public void AddValue(long value)
        {
            Values.Add(value);
        }

        public void CalculatePerformanceData(int skip = 0)
        {
            if(skip >= Values.Count)
            {
                MinMillis = 0;
                MaxMillis = 0;

                return;
            }

            for (int i = skip; i < Values.Count; i++)
            {
                if (Values[i] < MinMillis)
                {
                    MinMillis = Values[i];
                }
                else
                {
                    if (Values[i] > MaxMillis)
                    {
                        MaxMillis = Values[i];
                    }
                }
            }
        }

        public override string ToString()
        {
            return Name + "( MinMillis: " + MinMillis.ToString() + ", MaxMillis: " + MaxMillis.ToString() + ")";
        }
    }
}
