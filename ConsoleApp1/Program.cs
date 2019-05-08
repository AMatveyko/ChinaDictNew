using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaDict
{
    class Program
    {

        private List<string> keys = new List<string>();
        private List<string> values = new List<string>();
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();
        private List<string> text = new List<string>();
        //private List<string> text = new List<string>()
        //{ "oyqqoabqwpazomikdaqiobitfwwwegipqyxbjxzcrotvxmqumlecudqdnbfiocmfwqzeyaqlkut zdshqvrasdrgyo xiddoakxz" };
        private List<string> modText = new List<string>();
        private PrefixTree prefixTree;
        private string chrRange = "qwertyuiopasdfghjklzxcvbnm ";
        private string fileName = "data.txt";

        static void Main(string[] args)
        {
            Program program = new Program();
            //program.GenNewDict(100000);
            program.FillingDictionary();
            program.FillingPrefixTree();
            //program.TestSort();
            program.GenNewText(100, 100);
            //program.FindAndReplace();
            program.FindAndReplacePrefixTree();
            //program.PrintModText();
            Console.ReadLine();
        }

        private void PrintModText()
        {
            Console.WriteLine("Print mod text:");
            foreach (var str in modText)
                Console.WriteLine(str);
        }

        private void FindAndReplace()
        {
            Console.WriteLine(DateTime.Now);
            int maxWrdLng = 8;
            foreach(string str in text) 
            {
                string tmpStr = str;
                string modStr = String.Empty;
                var tmpDict = dictionary.Where(x => tmpStr.Contains(x.Key)).ToDictionary(x=>x.Key,x=>x.Value);

                while (tmpStr.Length > 0)
                {
                    string chars = String.Empty;
                    int wrdLng;
                    string word;
                    int curWrdLng;
                    string key = String.Empty;
                    bool IsFind = false;
                    wrdLng = (tmpStr.Length >= maxWrdLng) ? maxWrdLng : tmpStr.Length;
                    word = tmpStr.Substring(0, wrdLng);
                    tmpStr = tmpStr.Remove(0, wrdLng);
                    curWrdLng = word.Length;
                    for(int i = (curWrdLng - 1 ); i >= 0; i--)
                    {
                        key = tmpDict.Keys.Where(x => x.Length == word.Length).FirstOrDefault(x => x == word);
                        if (key != null)
                        {
                            modStr += dictionary[key];

                            IsFind = true;
                            break;
                        }
                        chars = word[i] + chars;
                        word = word.Remove(i);
                    }
                    if(IsFind)
                        tmpStr = chars + tmpStr;
                    else
                    {
                        //modStr = chars[0] + modStr;
                        modStr += chars[0];
                        tmpStr = chars.Remove(0,1) + tmpStr;
                    }
                }
                //modText.Add(modStr);
                //Console.WriteLine($"|{modStr}|");
            }
            Console.WriteLine(DateTime.Now);
        }

        private void FindAndReplacePrefixTree()
        {
            Console.WriteLine(DateTime.Now);
            var maxWrdLng = 8;
            var modStr = new StringBuilder(116);
            var newStr = new StringBuilder();
            //var tmpWrd = new StringBuilder();
            var tmpWrd = String.Empty;
            foreach (var str in text)
            {
                newStr.Append(str);
                int remaining;
                //Console.WriteLine($"newStr = |{newStr}|");
                while(newStr.Length > 0)
                {
                    int length = (newStr.Length >= maxWrdLng) ? maxWrdLng : newStr.Length;
                    tmpWrd = newStr.ToString(0, length);
                    modStr.Append(prefixTree.ValueForTheKey(tmpWrd.ToString(), out remaining));
                    newStr.Remove(0, remaining);
                }
                //Console.WriteLine($"|{modStr}|");
                //test

                tmpWrd.Remove(0, tmpWrd.Length);
                modStr.Remove(0, modStr.Length);
                newStr.Remove(0, newStr.Length);
            }
            Console.WriteLine(DateTime.Now);
        }

        private void GenNewText(int strCnt, int strLng)
        {
            //Console.WriteLine("Gen new text:");
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < strCnt; i++)
            {
                string newStr = String.Empty;
                for(int j = 0; j < strLng; j++)
                {
                    int index = rnd.Next(chrRange.Length);
                    newStr += chrRange[index];
                }
                text.Add(newStr);
                //Console.WriteLine(newStr);
            }
        }

        private void FillingPrefixTree()
        {
            string textFromFile;
            prefixTree = new PrefixTree();
            textFromFile = File.ReadAllText(fileName);
            string[] textFromFileArray = textFromFile.TrimEnd('|').Split('|');
            foreach (var pair in textFromFileArray)
            {
                string[] tmpArr = pair.Split('=');
                prefixTree.Add(tmpArr[0], tmpArr[1]);
            }
        }

        private void FillingDictionary()
        {
            string textFromFile;
            textFromFile = File.ReadAllText(fileName);
            string[] textFromFileArray = textFromFile.TrimEnd('|').Split('|');
            foreach (var pair in textFromFileArray)
            {
                string[] tmpArr = pair.Split('=');
                dictionary.Add(tmpArr[0], tmpArr[1]);
            }
            //Console.WriteLine(dictionary.Count.ToString());

        }

        private void GenNewDict(int maxCount)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            int i = 0;
            while( i < maxCount )
            {
                string newKey = String.Empty;
                string newValue = String.Empty;
                for (int j = 0; j <= rnd.Next(1,9); j++)
                {
                    int index = rnd.Next(chrRange.Length);
                    newKey += chrRange[index];
                }
                for (int j = 0; j <= rnd.Next(1, 21); j++)
                {
                    int index = rnd.Next(chrRange.Length);
                    newValue += chrRange[index];
                }
                if ((!keys.Contains(newKey)) && (!values.Contains(newValue)))
                {
                    keys.Add(newKey);
                    values.Add(newValue);
                    i++;
                    using (FileStream fstream = new FileStream(fileName, FileMode.Append))
                    {
                        byte[] array = System.Text.Encoding.Default.GetBytes($"{newKey}={newValue}|");
                        fstream.Write(array, 0, array.Length);
                    }
                }
            }
            Console.WriteLine("end");
        }
    }
}