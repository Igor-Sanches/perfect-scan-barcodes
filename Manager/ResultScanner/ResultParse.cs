using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Perfect_Scan.Manager.ResultScanner
{
    public class ResultParse
    {

        protected static string MatchSinglePrefixedField(String prefix, String rawText, char endChar, bool trim)
        {
            String[] matches = MatchPrefixedField(prefix, rawText, endChar, trim);
            if (matches == null)
            {
                return null;
            }
            return matches[0];
        }

        protected static string[] MatchPrefixedField(String prefix, String rawText, char endChar, bool trim)
        {
            List<String> matches = null;
            int i = 0;
            int max = rawText.Length; 
            while (i < max)
            {
                int i2 = rawText.IndexOf(prefix, i);
                if (i2 < 0)
                {
                    break;
                }
                i = i2 + prefix.Length;
                int start = i;
                bool more = true;
                while (more)
                {
                    int i3 = rawText.IndexOf(endChar, i); 
                    if (i3 < 0)
                    {
                        i = rawText.Length;
                        more = false;
                    }
                    else
                    {
                        int i4 = 0;
                        int i5 = i3 - 1;
                        while (i5 >= 0 && rawText[i5] == '\\')
                        {
                            i4++;
                            i5--;
                        }
                        if (i4 % 2 != 0)
                        {
                            i = i3 + 1;
                        }
                        else
                        {
                            if (matches == null)
                            {
                                matches = new List<string>(3);
                            } 
                            String element = rawText.Substring(start, i3 - start);
                            if (trim)
                            {
                                element = element.Trim();
                            }
                            if (!element.Equals(""))
                            {
                                matches.Add(element);
                            }
                            i = i3 + 1;
                            more = false;
                        }
                    }
                }
            }
            if (matches == null || matches.Equals(""))
            {
                return null;
            }
            return (String[])matches.ToArray();
        }


        protected static String UnescapeBackslash(String escaped)
        {
            int backslash = escaped.IndexOf("92");
            if (backslash < 0)
            {
                return escaped;
            }
            int max = escaped.Length;
            StringBuilder unescaped = new StringBuilder(max - 1);
            unescaped.Append(escaped.ToCharArray(), 0, backslash);
            bool nextIsEscaped = false;
            for (int i = backslash; i < max; i++)
            {
                char c = escaped[i];
                if (nextIsEscaped || c != '\\')
                {
                    unescaped.Append(c);
                    nextIsEscaped = false;
                }
                else
                {
                    nextIsEscaped = true;
                }
            }
            return unescaped.ToString();
        }

    }
}
