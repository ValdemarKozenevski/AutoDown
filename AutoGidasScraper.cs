using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDataScraper
{
    class AutoGidasScraper : Scraper
    {
        public AutoGidasScraper(string initLink, string folder) : base(initLink, folder)
        {            
        }

        public override int CurrentPage()
        {
            return int.Parse(Regex.Match(m_CurrentLink, @"page=(\d+)").Groups[1].Value);
        }

        public override int GetLastPage(string html)
        {
            var pattern = "<div class=\"page\">(\\d+)<\\/div>";

            var matches = Regex.Matches(html, pattern);
            if(matches.Count > 1)
            {

                try
                {
                    return int.Parse(matches[matches.Count - 1].Groups[1].Value);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return ParseInt(pattern, html);
            }            

            return 0;
        }

        public override string PageUrl(int pageNr)
        {
            return m_CurrentLink.Substring(0, m_CurrentLink.LastIndexOf("page=")) + "page=" + pageNr;
        }

        public override string[] GetCarList(string html)
        {
            var pattern = "<meta itemprop=\"url\" content=\"(.*?)\"";
            MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.Singleline);

            var carPages = new List<string>();

            foreach (Match m in matches)
            {
                carPages.Add(m.Groups[1].Value);
            }

            return carPages.ToArray();
        }
        
        public override string ParseMake(string html)
        {
            var pattern = "Markė<\\/div>\\s*<div class=\"right\">(.*?)\\s*<\\/div>";
            var match = Regex.Match(html, pattern, RegexOptions.Singleline);

            return match.Groups[1].Value.Trim();
        }
        
        public override string ParseModel(string html)
        {
            var pattern = "Modelis<\\/div>\\s*<div class=\"right\">(.*?)\\s*<\\/div>";
            var match = Regex.Match(html, pattern, RegexOptions.Singleline);
            return match.Groups[1].Value.Trim();
        }
        
        public override int ParsePrice(string html)
        {
            var pattern = "<meta itemprop=\"price\"\\s*content=\"(\\d*)\"";
            return ParseInt(pattern, html);
        }
        
        public override int ParseYear(string html)
        {
            var pattern = "Metai<\\/div>\\s*<div class=\"right\">(\\d*)";
            return ParseInt(pattern, html);
        }
        
        public override int ParseMileage(string html)
        {
            var pattern = "Rida, km<\\/div>\\s*<div class=\"right\">(\\d*)";
            return ParseInt(pattern, html);
        }
        
        public override int ParseKW(string html)
        {
            var pattern = "Variklis<\\/div>\\s*<div class=\"right\">.*\\s(\\d*)kW";
            return ParseInt(pattern, html);
        }
        
        public override BodyStyle ParseBody(string html)
        {
            var pattern = "Kėbulo tipas<\\/div>\\s*<div class=\"right\">([\\w\\s]*)";
            var body = Regex.Match(html, pattern, RegexOptions.Singleline).Groups[1].Value.Trim();

            if (body.Contains("Kabrioletas"))
            {
                return BodyStyle.convertible;
            }
            else if (body.Contains("Coupe"))
            {
                return BodyStyle.coupe;
            }
            else if (body.Contains("Hečbekas"))
            {
                return BodyStyle.hatchback;
            }
            else if (body.Contains("Sedanas"))
            {
                return BodyStyle.sedan;
            }
            else if (body.Contains("Visureigis"))
            {
                return BodyStyle.SUV;
            }
            else if (body.Contains("mikroautobusas"))
            {
                return BodyStyle.transporter;
            }
            else if (body.Contains("Vienatūris"))
            {
                return BodyStyle.van;
            }
            else if (body.Contains("Universalas"))
            {
                return BodyStyle.wagon;
            }

            return BodyStyle.unknown;
        }
        
        public override Fuel ParseFuel(string html)
        {
            var pattern = "Kuro tipas<\\/div>\\s*<div class=\"right\">([\\w\\s]*)";
            var fuel = Regex.Match(html, pattern, RegexOptions.Singleline).Groups[1].Value.Trim();

            if (fuel.Contains("Dyzelinas"))
            {
                return Fuel.diesel;
            }
            else if (fuel.Contains("Benzinas"))
            {
                return Fuel.gas;
            }
            else if (fuel.Contains("Elektra"))
            {
                return Fuel.electric;
            }

            return Fuel.unknown;
        }
        
        public override Gear ParseGear(string html)
        {
            var pattern = "Pavarų dėžė<\\/div>\\s*<div class=\"right\">([\\w\\s]*)";
            var gear = Regex.Match(html, pattern, RegexOptions.Singleline).Groups[1].Value.Trim();

            if (gear.Contains("Mechaninė"))
            {
                return Gear.manual;
            }
            else if (gear.Contains("Automatinė"))
            {
                return Gear.automatic;
            }

            return Gear.unknown;
        }
        
        public override Transmition ParseTransmition(string html)
        {
            var pattern = "Varomieji ratai<\\/div>\\s*<div class=\"right\">([\\w\\s]*)";
            var transmition = Regex.Match(html, pattern, RegexOptions.Singleline).Groups[1].Value.Trim();

            if (transmition.Contains("Visi varantys ratai"))
            {
                return Transmition.awd;
            }
            else if (transmition.Contains("Priekiniai ratai Priekiniai"))
            {
                return Transmition.fwd;
            }
            else if (transmition.Contains("Galiniai ratai Galiniai"))
            {
                return Transmition.rwd;
            }

            return Transmition.unknown;
        }
        
        public override Condition ParseCondition(string html)
        {
            var pattern = "Defektai<\\/div>\\s*<div class=\"right\">([\\w\\s]*)";
            var defects = Regex.Match(html, pattern, RegexOptions.Singleline).Groups[1].Value.Trim();
            
            if (defects.Contains("Daužtas"))
            {
                return Condition.defected;
            }

            return Condition.fine;
        }
        
        public override List<string> ParsePhotos(string html)
        {
            var urls = new List<string>();

            var pattern = "data-src-very-big=\"(.*)\">";

            var matches = Regex.Matches(html, pattern);

            foreach (Match m in matches)
            {
                var url = m.Groups[1].Value.Trim();
                urls.Add(url);
            }

            pattern = "big-photo\">\\s*<img src=\"(.*)\"";
            var match = Regex.Match(html, pattern).Groups[1].Value.Trim();
            if (!string.IsNullOrEmpty(match))
            {
                urls.Add(match);
            }            

            return urls;
        }
    }
}
