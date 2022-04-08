using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Utils
{
    public class Counter
    {
        private SortedDictionary<int, int> totalItemsDefault;
        private SortedDictionary<int, int> totalItemsNoDefault;

        public Counter()
        {
            totalItemsDefault = new SortedDictionary<int, int>();
            totalItemsNoDefault = new SortedDictionary<int, int>();
        }
        public bool Count(bool result, int parent)
        {
            if (!result) {
                try
                {
                    totalItemsNoDefault[parent]++;
                }
                catch (KeyNotFoundException)
                {
                    totalItemsNoDefault.Add(parent, 0);
                }
                return result;
            } 

            try
            {
                totalItemsDefault[parent]++;
            }
            catch(KeyNotFoundException)
            {
                totalItemsDefault.Add(parent, 0);
            }
            return result;
        }
        public int GetCountDefault(int parent)
        {
            try
            {
                return totalItemsDefault[parent];
            }
            catch (KeyNotFoundException)
            {
                return 0;
            }
            
        }
        public int GetCountNoDefault(int parent)
        {
            try
            {
                return totalItemsNoDefault[parent];
            }
            catch (KeyNotFoundException)
            {
                return 0;
            }

        }
    }
}
