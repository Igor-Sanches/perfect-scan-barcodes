using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.Manager
{
    public abstract class ContatoEncoder
    {
        public abstract string[] Encode(List<String> names, List<String> addresses, List<String> phones, List<String> emails, String note);

         string Trim(String s)
        {
            if (s == null)
            {
                return null;
            }
            string result = s.Trim();
            if (result.Equals(""))
            {
                return null;
            }
            return result;
        }

        public void Append(StringBuilder newContents, StringBuilder newDisplayContents, string prefix, String value, Formatter fieldFormatter, char terminator)
        {
            string trimmed = Trim(value);
            if (trimmed != null)
            {
                newContents.Append(prefix).Append(fieldFormatter.Format(trimmed, 0)).Append(terminator);
                newDisplayContents.Append(trimmed).Append(10);
            }
        }


        public void AppendUpToUnique(StringBuilder newContents, StringBuilder newDisplayContents, String prefix, List<String> values, int max, Formatter displayFormatter, Formatter fieldFormatter, char terminator)
        {
            if (values != null)
            {
                int count = 0;
                Collection<String> uniques = new Collection<string>();
                int i = 0;
                while (i < values.Count())
                {
                    String trimmed = Trim((String)values[i]);
                    if (trimmed != null && !trimmed.Equals("") && !uniques.Contains(trimmed))
                    {
                        newContents.Append(prefix).Append(fieldFormatter.Format(trimmed, i)).Append(terminator); 
                        newDisplayContents.Append(displayFormatter == null ? trimmed : displayFormatter.Format(trimmed, i)).Append(10);
                        count++;
                        if (count != max)
                        {
                            uniques.Add(trimmed);
                        }
                        else
                        {
                            return;
                        }
                    }
                    i++;
                }
            }
        }
    }
}
