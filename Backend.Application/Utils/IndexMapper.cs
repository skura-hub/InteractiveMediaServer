using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Utils
{
    public class IndexMapper<OldIndex>
    {
        private SortedDictionary<OldIndex, int> keyValuePairs;
        private int currentIndex;

        public IndexMapper(int startingIndex)
        {
            keyValuePairs = new SortedDictionary<OldIndex, int>();
            currentIndex = startingIndex;
        }

        public int CreateAndGetNewIndex(OldIndex oldIndex)
        {
            keyValuePairs.Add(oldIndex, currentIndex);
            int result = currentIndex;
            currentIndex++;
            return result;
        }

        public int GetIndex(OldIndex oldIndex)
        {
            return keyValuePairs[oldIndex];
        }
    }
}
