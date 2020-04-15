using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDataScraper
{
    class AutoPliusScraper : Scraper
    {
        public AutoPliusScraper(string initLink, string folder) : base(initLink, folder)
        {            
        }

        public override int CurrentPage()
        {
            return int.Parse(Regex.Match(m_CurrentLink, @"page_nr=(\d+)").Groups[1].Value);
        }

        public override int GetLastPage(string html)
        {
            var pattern = "<span>(\\d+)</span>";
            MatchCollection match = Regex.Matches(html, pattern, RegexOptions.Singleline);
            int lastPage = 1;

            foreach (Match m in match)
            {
                lastPage = Math.Max(lastPage, int.Parse(m.Groups[1].Value));
            }

            return lastPage;
        }

        public override string PageUrl(int pageNr)
        {
            return m_CurrentLink.Substring(0, m_CurrentLink.LastIndexOf("page_nr")) + "page_nr=" + pageNr;
        }

        public override string[] GetCarList(string html)
        {
            var pattern = "<a class=\"announcement-item( car-history-approved)?( is-highlighted)?( is-sold)?\" href=\"(.+?)\" target=\"_blank\">";
            MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.Singleline);

            var carPages = new List<string>();

            foreach (Match m in matches)
            {
                carPages.Add(m.Groups[4].Value);
            }

            return carPages.ToArray();
        }

        public override string ParseMake(string html)
        {
            var pattern = "<a href=\"https://autoplius.lt/skelbimai/naudoti-automobiliai/";
            int index = html.IndexOf(pattern, html.IndexOf(pattern) + 1);
            string substr = html.Substring(index + pattern.Length, 200);

            char[] delimiterChars = { ' ', '/', '"' };
            string[] args = substr.Split(delimiterChars);

            return args[0].ToLower();
        }

        public override string ParseModel(string html)
        {
            var pattern = "<a href=\"https://autoplius.lt/skelbimai/naudoti-automobiliai/";
            int index = html.IndexOf(pattern, html.IndexOf(pattern) + 1);
            string substr = html.Substring(index + pattern.Length, 200);

            char[] delimiterChars = { ' ', '/', '"' };
            string[] args = substr.Split(delimiterChars);

            return args[1].ToLower();
        }

        public override int ParsePrice(string html)
        {
            var pattern = "<div class=\"price\">\\s+(\\d+ \\d+)";
            return ParseInt(pattern, html);
        }

        public override int ParseYear(string html)
        {
            var pattern = "Pagaminimo data\\s+</div>\\s+<div class=\"parameter-value\">\\s+(\\d+)";
            return ParseInt(pattern, html);
        }

        public override int ParseMileage(string html)
        {
            var pattern = "Rida\\s+</div>\\s+<div class=\"parameter-value\">\\s+(\\d+\\s+\\d+)";
            return ParseInt(pattern, html);
        }

        public override int ParseKW(string html)
        {
            var pattern = "\\d+\\s+AG\\s+.(\\d+)";
            return ParseInt(pattern, html);
        }
        
        public override BodyStyle ParseBody(string html)
        {
            if (html.Contains("Kėbulo tipas Kabrioletas"))
            {
                return BodyStyle.convertible;
            }
            else if (html.Contains("Kėbulo tipas Kupė"))
            {
                return BodyStyle.coupe;
            }
            else if (html.Contains("Kėbulo tipas Hečbekas"))
            {
                return BodyStyle.hatchback;
            }
            else if (html.Contains("Kėbulo tipas Sedanas"))
            {
                return BodyStyle.sedan;
            }
            else if (html.Contains("Kėbulo tipas Visureigis"))
            {
                return BodyStyle.SUV;
            }
            else if (html.Contains("Kėbulo tipas Komercinis"))
            {
                return BodyStyle.transporter;
            }
            else if (html.Contains("Kėbulo tipas Vienatūris"))
            {
                return BodyStyle.van;
            }
            else if (html.Contains("Kėbulo tipas Universalas"))
            {
                return BodyStyle.wagon;
            }

            return BodyStyle.unknown;
        }

        public override Fuel ParseFuel(string html)
        {
            if (html.Contains("Kuro tipas Dyzelinas"))
            {
                return Fuel.diesel;
            }
            else if (html.Contains("Kuro tipas Benzinas"))
            {
                return Fuel.gas;
            }
            else if (html.Contains("Kuro tipas Elektra"))
            {
                return Fuel.electric;
            }

            return Fuel.unknown;
        }

        public override Gear ParseGear(string html)
        {
            if (html.Contains("Pavarų dėžė Mechaninė"))
            {
                return Gear.manual;
            }
            else if (html.Contains("Pavarų dėžė Automatinė"))
            {
                return Gear.automatic;
            }

            return Gear.unknown;
        }

        public override Transmition ParseTransmition(string html)
        {
            if (html.Contains("Varantieji ratai Visi varantys"))
            {
                return Transmition.awd;
            }
            else if (html.Contains("Varantieji ratai Priekiniai"))
            {
                return Transmition.fwd;
            }
            else if (html.Contains("Varantieji ratai Galiniai"))
            {
                return Transmition.rwd;
            }

            return Transmition.unknown;
        }

        public override Condition ParseCondition(string html)
        {
            if(html.Contains("Defektai Be defektų"))
            {
                return Condition.fine;
            }
            if (html.Contains("Defektai Daužtas"))
            {
                return Condition.defected;
            }

            return Condition.fine;
        }
        
        public override List<string> ParsePhotos(string html)
        {
            var urls = new List<string>();

            var pattern = "\"type\":\"photo\",\"url\":([\"\'])((?:\\\\\\1|.)*?)\\1";

            var matches = Regex.Matches(html, pattern);

            foreach(Match m in matches)
            {
                var url = m.Groups[2].Value.Replace("\\/", "/");
                urls.Add(url);
            }

            return urls;
        }        
    }
}
