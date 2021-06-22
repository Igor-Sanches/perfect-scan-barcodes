using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Perfect_Scan.Manager.ResultScanner
{
    public class ContatoResult : ResultParse
    {
        private string codigo; 

        public string FormattedName { get; set; }

        public string Surname { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public string Title { get; set; }

        public DateTime Birthday { get; set; }

        public DateTime Rev { get; set; }

        public string Org { get; set; }

        public string Note { get; set; }

        public Address[] Andress { get; set; }

        public Phone[] Phones { get; set; }

        public Email[] Emails { get; set; }


        public ContatoResult(string codigo)
        {
            this.codigo = codigo;
        }

        public bool IsValidate { get; internal set; }

        public void DecodificarContato()
        { 
            if (codigo.StartsWith("MECARD:"))
            {
                String replace = codigo.Replace("MECARD:", "");
                String[] split = replace.Split(';');
                String codigoReplace = "";
                foreach(var r in split)
                {
                    if (!r.Equals(""))
                    {
                        codigoReplace += $"\n{r};";
                    }
                } 
                codigo = codigoReplace;
            }
            Decompiler();
           
        }
         
        private void Decompiler()
        {
            string s = codigo;
 
 RegexOptions options = RegexOptions.IgnoreCase |
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace;

            Regex regex;
            Match m;
            MatchCollection mc;

            regex = new Regex(@"(?<strElement>(FN))   (:(?<strFN>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
                FormattedName = m.Groups["strFN"].Value;

            regex = new Regex(@"(\n(?<strElement>(N)))   
                    (:(?<strSurname>([^;]*))) (;(?<strGivenName>([^;]*)))  
                    (;(?<strMidName>([^;]*))) (;(?<strPrefix>([^;]*))) 
                    (;(?<strSuffix>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
            {
                Surname = m.Groups["strSurname"].Value;
                GivenName = m.Groups["strGivenName"].Value;
                MiddleName = m.Groups["strMidName"].Value;
                Prefix = m.Groups["strPrefix"].Value;
                Suffix = m.Groups["strSuffix"].Value;
            }

            ///Title
            regex = new Regex(@"(?<strElement>(TITLE))   
                    (:(?<strTITLE>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
                Title = m.Groups["strTITLE"].Value;

            ///ORG
            regex = new Regex(@"(?<strElement>(ORG))   
                    (:(?<strORG>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
                Org = m.Groups["strORG"].Value;

            ///Note
            regex = new Regex(@"((?<strElement>(NOTE)) 
                    (;*(?<strAttr>(ENCODING=QUOTED-PRINTABLE)))*  
                    ([^:]*)*  (:(?<strValue> 
                    (([^\n\r]*=[\n\r]+)*[^\n\r]*[^=][\n\r]*) )))", options);
            m = regex.Match(s);
            if (m.Success)
            {
                Note = m.Groups["strValue"].Value;
                //Remove connections and escape strings. The order is significant.
                Note = Note.Replace("=" + Environment.NewLine, "");
                Note = Note.Replace("=0D=0A", Environment.NewLine);
                Note = Note.Replace("=3D", "=");
            }

            ///Birthday
            regex = new Regex(@"(?<strElement>(BDAY))   
                    (:(?<strBDAY>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
            {
                string[] expectedFormats = { "yyyyMMdd", "yyMMdd", "yyyy-MM-dd" };
                Birthday = DateTime.ParseExact
                           (m.Groups["strBDAY"].Value, expectedFormats, null,
                           System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            }

            ///Rev
            regex = new Regex(@"(?<strElement>(REV))   (:(?<strREV>[^\n\r]*))", options);
            m = regex.Match(s);
            if (m.Success)
            {
                string[] expectedFormats = { "yyyyMMddHHmmss", "yyyyMMddTHHmmssZ" };
                Rev = DateTime.ParseExact
                      (m.Groups["strREV"].Value, expectedFormats, null,
                      System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            }

            ///Emails
            string ss;

            regex = new Regex(@"((?<strElement>(EMAIL)) 
                    (;*(?<strAttr>(HOME|WORK)))*  (;(?<strPref>(PREF)))* 
                    (;[^:]*)*  (:(?<strValue>[^\n\r]*)))", options);
            mc = regex.Matches(s);
            if (mc.Count > 0)
            {
                Emails = new Email[mc.Count];
                for (int i = 0; i < mc.Count; i++)
                {
                    m = mc[i];
                    Emails[i].address = m.Groups["strValue"].Value;
                    ss = m.Groups["strAttr"].Value;
                    if (ss == "HOME")
                        Emails[i].homeWorkType = HomeWorkType.home;
                    else if (ss == "WORK")
                        Emails[i].homeWorkType = HomeWorkType.work;

                    if (m.Groups["strPref"].Value == "PREF")
                        Emails[i].pref = true;
                }
            }

            ///Phones
            regex = new Regex(@"(\n(?<strElement>(TEL)) 
                    (;*(?<strAttr>(HOME|WORK)))* 
                    (;(?<strType>(VOICE|CELL|PAGER|MSG|FAX)))*  
                    (;(?<strPref>(PREF)))* (;[^:]*)*  
                    (:(?<strValue>[^\n\r]*)))", options);
            mc = regex.Matches(s);
            if (mc.Count > 0)
            {
                IsValidate = true;
                Phones = new Phone[mc.Count];
                for (int i = 0; i < mc.Count; i++)
                {
                    m = mc[i];
                    Phones[i].number = m.Groups["strValue"].Value;
                    ss = m.Groups["strAttr"].Value;
                    if (ss == "HOME")
                        Phones[i].homeWorkType = HomeWorkType.home;
                    else if (ss == "WORK")
                        Phones[i].homeWorkType = HomeWorkType.work;

                    if (m.Groups["strPref"].Value == "PREF")
                        Phones[i].pref = true;

                    ss = m.Groups["strType"].Value;
                    if (ss == "VOICE")
                        Phones[i].phoneType = PhoneType.VOICE;
                    else if (ss == "CELL")
                        Phones[i].phoneType = PhoneType.CELL;
                    else if (ss == "PAGER")
                        Phones[i].phoneType = PhoneType.PAGER;
                    else if (ss == "MSG")
                        Phones[i].phoneType = PhoneType.MSG;
                    else if (ss == "FAX")
                        Phones[i].phoneType = PhoneType.FAX;
                }
            }
            ///Addresses
            regex = new Regex(@"(\n(?<strElement>(ADR))) 
                    (;*(?<strAttr>(HOME|WORK)))*  (:(?<strPo>([^;]*)))  
                    (;(?<strBlock>([^;]*)))  (;(?<strStreet>([^;]*)))  
                    (;(?<strCity>([^;]*))) (;(?<strRegion>([^;]*))) 
                    (;(?<strPostcode>([^;]*)))(;(?<strNation>[^\n\r]*)) ", options);
            mc = regex.Matches(s);
            if (mc.Count > 0)
            {
                Andress = new Address[mc.Count];
                for (int i = 0; i < mc.Count; i++)
                {
                    m = mc[i];
                    ss = m.Groups["strAttr"].Value;
                    if (ss == "HOME")
                        Andress[i].homeWorkType = HomeWorkType.home;
                    else if (ss == "WORK")
                        Andress[i].homeWorkType = HomeWorkType.work;

                    Andress[i].po = m.Groups["strPo"].Value;
                    Andress[i].ext = m.Groups["strBlock"].Value;
                    Andress[i].street = m.Groups["strStreet"].Value;
                    Andress[i].locality = m.Groups["strCity"].Value;
                    Andress[i].region = m.Groups["strRegion"].Value;
                    Andress[i].postcode = m.Groups["strPostcode"].Value;
                    Andress[i].country = m.Groups["strNation"].Value;
                }
            }
        }
    }
    public enum HomeWorkType
    {
        home, work
    }

    public enum PhoneType
    {
        VOICE,
        FAX,
        MSG,
        CELL,
        PAGER

    }
    /// <summary>
    /// If you flag the enume types, you may use flags.
    /// </summary>
    public struct Phone
    {
        public string number;
        public HomeWorkType homeWorkType;
        public bool pref;
        public PhoneType phoneType;

    }

    public struct Email
    {
        public string address;
        public HomeWorkType homeWorkType;
        public bool pref;
    }

    public struct Address
    {
        public string po;
        public string ext;
        public string street;
        public string locality;
        public string region;
        public string postcode;
        public string country;

        public HomeWorkType homeWorkType;
    }
    public enum LabelType
    {
        DOM,
        INTL,
        POSTAL,
        PARCEL

    }

    /// <summary>
    /// Not used yet. You may use regular expressions or String.Replace() to replace =0D=0A to line breaks.
    /// </summary>
    public struct Label
    {
        public string address;
        public LabelType labelType;
    }

}
