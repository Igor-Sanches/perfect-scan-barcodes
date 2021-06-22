using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Globalization.PhoneNumberFormatting;

namespace Perfect_Scan.Manager
{
    public class VCardContactEncoder : ContatoEncoder
    {
        private static char TERMINATOR = '\n';

        public override string[] Encode(List<string> names, List<string> addresses, List<string> phones, List<string> emails, string note)
        {
            StringBuilder newContents = new StringBuilder(100);
            newContents.Append("BEGIN:VCARD\n");
            newContents.Append("VERSION:3.0\n");
            StringBuilder newDisplayContents = new StringBuilder(100);
            VCardFieldFormatter vCardFieldFormatter = new VCardFieldFormatter();
            AppendUpToUnique(newContents, newDisplayContents, "N", names, 1, null, vCardFieldFormatter, TERMINATOR); 
            AppendUpToUnique(newContents, newDisplayContents, "ADR", addresses, 1, null, vCardFieldFormatter, TERMINATOR); 
            AppendUpToUnique(newContents, newDisplayContents, "TEL", phones, Int32.MaxValue, new VCardTelDisplayFormatter(), new VCardFieldFormatter(), TERMINATOR);
            AppendUpToUnique(newContents, newDisplayContents, "EMAIL", emails, Int32.MaxValue, null, vCardFieldFormatter, TERMINATOR); 
            Append(newContents, newDisplayContents, "NOTE", note, vCardFieldFormatter, TERMINATOR);
            newContents.Append("END:VCARD\n");
            return new String[] { newContents.ToString(), newDisplayContents.ToString() };
        }
    }

    internal class VCardTelDisplayFormatter : Formatter
    {
        private static Regex NOT_DIGITS_OR_PLUS = new Regex("[^0-9+]+");
       
        public VCardTelDisplayFormatter()
        {
        }
         

        public string Format(string value, int i)
        {
            PhoneNumberFormatter phone = new PhoneNumberFormatter();
            return NOT_DIGITS_OR_PLUS.Replace(phone.FormatString(value), "");
        }
    }

    internal class VCardFieldFormatter : Manager.Formatter
    {
        public string Format(string value, int i)
        {
            return ":" + Regex.Replace(value, "(?<!\r)\n", "\r\n");//, "\\\\$1").Replace(value, ""), "");
        }
    }
}
