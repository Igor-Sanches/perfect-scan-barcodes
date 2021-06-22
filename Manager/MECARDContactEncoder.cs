using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Globalization.PhoneNumberFormatting;

namespace Perfect_Scan.Manager
{
    public class MECARDContactEncoder : ContatoEncoder
    {
        private static char TERMINATOR = ';';

        public MECARDContactEncoder()
        {
        }

        class MECARDFieldFormatter : Formatter
        {
        private static Regex NEWLINE = new Regex("\\n");
        private static Regex RESERVED_MECARD_CHARS = new Regex("([\\\\:;])");
            private byte v;

            public MECARDFieldFormatter(byte v)
            {
                this.v = v;
            }

            public string Format(string value, int i)
            {
                
                return ":" + Regex.Replace(value, "(?<!\r)\n", "\r\n");//, "\\\\$1").Replace(value, ""), "");
            }
        }


        class MECARDNameDisplayFormatter : Formatter
        {

            private static Regex COMMA = new Regex(",");
            private byte v;

            public MECARDNameDisplayFormatter(byte v)
            {
                this.v = v;
            }

            public string Format(string value, int i)
            {
                return Regex.Replace(value, ",", "");
            }
        }

        class MECARDTelDisplayFormatter : Formatter
        {
        private static Regex NOT_DIGITS_OR_PLUS = new Regex("[^0-9+]+");
            private byte v;

            public MECARDTelDisplayFormatter(byte v)
            {
                this.v = v;
            }
              
            public string Format(string value, int i)
            {
                PhoneNumberFormatter phone = new PhoneNumberFormatter();
                return NOT_DIGITS_OR_PLUS.Replace(phone.FormatString(value), "");
            }
        }


        public override string[] Encode(List<String> names, List<String> addresses, List<String> phones, List<String> emails, String note)
        {
            StringBuilder newContents = new StringBuilder(100);
            newContents.Append("MECARD:");
            StringBuilder newDisplayContents = new StringBuilder(100);
            MECARDFieldFormatter mECARDFieldFormatter = new MECARDFieldFormatter((byte)0);
            AppendUpToUnique(newContents, newDisplayContents, "N", names, 1, new MECARDNameDisplayFormatter((byte)0), mECARDFieldFormatter, TERMINATOR); 
            AppendUpToUnique(newContents, newDisplayContents, "ADR", addresses, 1, null, mECARDFieldFormatter, TERMINATOR);
            AppendUpToUnique(newContents, newDisplayContents, "TEL", phones, Int32.MaxValue, new MECARDTelDisplayFormatter((byte)0), mECARDFieldFormatter, TERMINATOR);
            AppendUpToUnique(newContents, newDisplayContents, "EMAIL", emails, Int32.MaxValue, null, mECARDFieldFormatter, TERMINATOR); 
            Append(newContents, newDisplayContents, "NOTE", note, mECARDFieldFormatter, TERMINATOR);
            newContents.Append(TERMINATOR);

             return new string[] { newContents.ToString(), newDisplayContents.ToString()};
        }
 

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
