using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisSystemFinal
{
    class SizeHelper
    {
        public static int GetNewDimension(int oldContainer, int newContainer, int oldSize)
        {
            double diff = newContainer - oldContainer;
            double percent = diff / oldContainer;
            double scaledval = (1 + percent) * oldSize;
            int intVal = Convert.ToInt32(scaledval);

            return intVal;
        }
    }
}
