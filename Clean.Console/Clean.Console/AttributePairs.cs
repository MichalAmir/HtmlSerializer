using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Consle
{
    internal class AttributePairs
    {
        public string Key { get;}
        public string Value { get;}

        public AttributePairs(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
