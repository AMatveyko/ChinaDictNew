using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaDict
{
    internal class TreeNode
    {
        public Dictionary<char, TreeNode> Childs { get; set; }
        public String Value { get; set; }

        public Boolean HasValue
        {
            get { return Value != null; }
        }

        public TreeNode()
        {
            Childs = new Dictionary<char, TreeNode>();
        }
    }

    class PrefixTree
    {
        TreeNode Root = new TreeNode();

        public void Add(string key, string value)
        {
            TreeNode current = Root;

            foreach (char c in key)
            {
                if (!current.Childs.ContainsKey(c))
                    current.Childs.Add(c, new TreeNode());
                current = current.Childs[c];
            }

            current.Value = value;
        }

        public string ValueForTheKey(string key, out int remainingPart)
        {
            TreeNode current = Root;
            var value = key[0].ToString();
            int deep = 1;
            int wrdLng = 0;

            foreach(var keyChr in key)
            {
                if (current.Childs.ContainsKey(keyChr))
                {
                    wrdLng++;
                    current = current.Childs[keyChr];
                    if (current.HasValue)
                    {
                        deep = wrdLng;
                        value = current.Value;
                    }
                }
                else
                    break;
            }
            remainingPart = deep;
            return value;
        }

        public string Get(string key)
        {
            TreeNode current = Root;

            foreach (var c in key)
            {
                if (!current.Childs.ContainsKey(c))
                {
                    throw new ArgumentException();
                }
                current = current.Childs[c];
            }

            if (current.HasValue)
            {
                return current.Value;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
