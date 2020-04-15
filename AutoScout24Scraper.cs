using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDataScraper
{
    class AutoScout24Scraper : Scraper
    {
        public AutoScout24Scraper(string initLink, string folder) : base(initLink, folder)
        {
        }

        public override int CurrentPage()
        {
            return int.Parse(Regex.Match(m_CurrentLink, @"page=(\d+)").Groups[1].Value);
        }
        
        
        public override int GetLastPage(string html)
        {            
            return 20;
        }

        public override string PageUrl(int pageNr)
        {
            var pattern = @"pageNumber=(\d+)";
            return Regex.Replace(m_CurrentLink, pattern, pageNr.ToString());
        }

        public override string[] GetCarList(string html)
        {
            var pattern = "<a data-item-name=\"detail-page-link\" href=\"([^\"]*)";
            MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.Singleline);

            var carPages = new List<string>();

            foreach (Match m in matches)
            {
                var url = @"https://www.autoscout24.com" + m.Groups[1].Value.Trim();
                carPages.Add(url);
            }

            return carPages.ToArray();
        }
        
        public override string ParseMake(string html)
        {
            var pattern = "\"stmak\" : \"([^\"]*)";
            var match = Regex.Match(html, pattern, RegexOptions.Singleline);
            return match.Groups[1].Value.Trim().ToLower();
        }

        
        public override string ParseModel(string html)
        {
            var pattern = "\"stmod\" : \"([^\"]*)";
            var match = Regex.Match(html, pattern, RegexOptions.Singleline);
            return match.Groups[1].Value.Trim().ToLower();
        }
        
        public override int ParsePrice(string html)
        {
            /*
            var pattern = "<div class=\"cldt-price\">\\s*<h2>\\s*€ (\\d*),?(\\d*)?[^\\.]*";
            MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.Singleline);

            if (string.IsNullOrEmpty(matches[0].Groups[2].Value))
            {
                return int.Parse(matches[0].Groups[1].Value);
            }
            else
            {
                return (int.Parse(matches[0].Groups[1].Value) * 1000) + int.Parse(matches[1].Groups[2].Value);
            }
            */
            var pattern = "\"cost\" : ([^,]*)";
            return ParseInt(pattern, html);
        }
        
        public override int ParseYear(string html)
        {
            var pattern = "\"styea\" : \"([^\"]*)";
            return ParseInt(pattern, html);
        }
        
        public override int ParseMileage(string html)
        {
            /*
            var pattern = "<span class=\"sc-font-l cldt-stage-primary-keyfact\">(\\d*),?(\\d*)? km<\\/span>";
            MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.Singleline);

            if (string.IsNullOrEmpty(matches[0].Groups[2].Value))
            {
                return int.Parse(matches[0].Groups[1].Value);
            }
            else
            {
                return (int.Parse(matches[0].Groups[1].Value) * 1000) + int.Parse(matches[1].Groups[2].Value);
            }
            */
            var pattern = "\"stmil\" : ([^,]*)";
            return ParseInt(pattern, html);
        }
       
        public override int ParseKW(string html)
        {
            var pattern = "\"stkw\" : ([^,]*)";
            return ParseInt(pattern, html);
        }

        
        public override BodyStyle ParseBody(string html)
        {
            var pattern = "<dt class=\"sc-ellipsis\">Body<\\/dt>\\s*<dd>\\s*<a[^>]*>([^>]*)<\\/a>";
            var match = Regex.Match(html, pattern, RegexOptions.Singleline);
            var body = match.Groups[1].Value.Trim();

            if (body.Contains("Convertible"))
            {
                return BodyStyle.convertible;
            }
            else if (body.Contains("Coupe"))
            {
                return BodyStyle.coupe;
            }
            else if (body.Contains("Compact"))
            {
                return BodyStyle.hatchback;
            }
            else if (body.Contains("Sedans"))
            {
                return BodyStyle.sedan;
            }
            else if (body.Contains("Off-Road"))
            {
                return BodyStyle.SUV;
            }
            else if (body.Contains("Transporter"))
            {
                return BodyStyle.transporter;
            }
            else if (body.Contains("Van"))
            {
                return BodyStyle.van;
            }
            else if (body.Contains("wagon"))
            {
                return BodyStyle.wagon;
            }

            return BodyStyle.unknown;
        }

        
        public override Fuel ParseFuel(string html)
        {
            var pattern = "\"fuel\" : \\[\"(.)\"\\],";
            var match = Regex.Match(html, pattern, RegexOptions.Singleline);
            var fuel = match.Groups[1].Value.Trim();

            if (fuel.Contains("D"))
            {
                return Fuel.diesel;
            }
            else if (fuel.Contains("B"))
            {
                return Fuel.gas;
            }
            else if (fuel.Contains("E"))
            {
                return Fuel.electric;
            }

            return Fuel.unknown;
        }

        
        public override Gear ParseGear(string html)
        {
            var pattern = "\"gear\" : \\[\"(.)\"\\],";
            var match = Regex.Match(html, pattern, RegexOptions.Singleline);
            var gear = match.Groups[1].Value.Trim();

            if (gear.Contains("M"))
            {
                return Gear.manual;
            }
            else if (gear.Contains("A"))
            {
                return Gear.automatic;
            }

            return Gear.unknown;
        }

        
        public override Transmition ParseTransmition(string html)
        {
            return Transmition.unknown;
        }
        
        
        public override Condition ParseCondition(string html)
        {
            return Condition.fine;
        }
        
        public override List<string> ParsePhotos(string html)
        {
            var urls = new List<string>();

            var pattern = "data-fullscreen-src=\"([^\"]*)";

            var matches = Regex.Matches(html, pattern);

            foreach (Match m in matches)
            {
                var url = m.Groups[1].Value.Trim();
                urls.Add(url);
            }            

            return urls;
        }
    }
}
