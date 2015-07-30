using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeApp.Utils
{
    public static class UIHelper
    {

        public static string GetTxtBtwn(string input, string start, string end, int startIndex)
        {
            return GetTxtBtwn(input, start, end, startIndex, false);
        }


        public static string GetLastTxtBtwn(string input, string start, string end, int startIndex)
        {
            return GetTxtBtwn(input, start, end, startIndex, true);
        }


        private static string GetTxtBtwn(string input, string start, string end, int startIndex, bool UseLastIndexOf)
        {
            int index1 = UseLastIndexOf ? input.LastIndexOf(start, startIndex) :
                                          input.IndexOf(start, startIndex);
            if (index1 == -1) return "";
            index1 += start.Length;
            int index2 = input.IndexOf(end, index1);
            if (index2 == -1) return input.Substring(index1);
            return input.Substring(index1, index2 - index1);
        }
    }
}
