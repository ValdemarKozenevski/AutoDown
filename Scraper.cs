using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDataScraper
{
    class Scraper
    {
        protected string m_DataFolder;
        public string m_InitLink;
        protected string m_CurrentLink;

        protected string m_Html;

        public bool m_ThreadHalt;
        public bool m_Done;

        List<Thread> m_Threads;
        int m_ThreadIndex;

        public Scraper(string initLink, string folder)
        {
            m_InitLink = initLink;
            m_CurrentLink = initLink;
            m_DataFolder = folder;
            
            m_Html = string.Empty;          

            m_ThreadHalt = false;
            m_Done = false;

            m_Threads = new List<Thread>();
            for (int i = 0; i < 10; i++) m_Threads.Add(null);

            m_ThreadIndex = 0;
        }

        public void ScrapeTheData()
        {            
            while (true)
            {
                if (!m_Done)
                {
                    m_Html = DownloadContent(m_CurrentLink);
                }                
                if (string.IsNullOrEmpty(m_Html))
                {
                    HaltThread("m_Html downloaded empty");
                    continue;
                }

                m_Threads[m_ThreadIndex]?.Join();
                if (!m_Done)
                {
                    m_Threads[m_ThreadIndex] = new Thread(() => ParseCarList(m_Html));
                    m_Threads[m_ThreadIndex].Start();                    
                }
                else
                {
                    m_Threads[m_ThreadIndex] = null;
                }

                m_ThreadIndex++;
                if (m_ThreadIndex >= m_Threads.Count)
                {
                    m_ThreadIndex = 0;
                }

                if (!m_Done)
                {
                    /*
                    bool doneParsing = true;
                    foreach (var t in m_Threads)
                    {
                        if (t != null && t.IsAlive)
                        {
                            doneParsing = false;
                        }
                    }
                    */

                    int currentPage = CurrentPage();
                    if (currentPage < GetLastPage(m_Html))
                    {
                        m_CurrentLink = PageUrl(currentPage + 1);
                    }
                    else
                    {                        
                        MessageBox.Show(string.Format("Done parsing: {0}", m_InitLink), "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        m_Done = true;                        
                    }
                }
            }
        }

        public bool Done()
        {
            return m_Done;
        }

        public string DownloadContent(string url)
        {
            try
            {
                using(var webClient = new WebClient())
                {
                    var htmlData = webClient.DownloadData(url);
                    return Encoding.UTF8.GetString(htmlData);
                }
            }
            catch (Exception e)
            {
                if(e.Message == "The remote server returned an error: (404) Not Found.")
                {
                    return "";
                }

                HaltThread(e.Message);

                //try parsing again
                return DownloadContent(url);
            }
        }

        public void DownloadFile(string url, string path)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(url, path);
                }
            }
            catch (Exception e)
            {
                HaltThread(e.Message);

                //try again
                DownloadFile(url, path);
            }
        }

        private void HaltThread(string message = "")
        {            
            if(m_ThreadHalt == false)
            {
                m_ThreadHalt = true;
                //MessageBox.Show("Scraper halted: "+ message, "HALT", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            while (m_ThreadHalt)
            {
                Thread.Sleep(10);
            }
        }

        public void ParseCarList(string html)
        {
            var carList = GetCarList(html);
            foreach (var carPage in carList)
            {
                var car = ParseCar(carPage);                
                SavePhotos(car);

                if (m_ThreadHalt)
                {
                    HaltThread();
                }
            }
        }

        public virtual string[] GetCarList(string html)
        {
            return null;
        }

        public Car ParseCar(string url)
        {
            var html = DownloadContent(url);
            var car = new Car();

            if (string.IsNullOrEmpty(html))
            {
                return car;
            }
            
            car.m_Make = ParseMake(html);
            car.m_Model = ParseModel(html);
            car.m_Price = ParsePrice(html);
            car.m_Year = ParseYear(html);
            car.m_Mileage = ParseMileage(html);
            car.m_kW = ParseKW(html);
            car.m_BodyStyle = ParseBody(html);
            car.m_Fuel = ParseFuel(html);
            car.m_Gear = ParseGear(html);
            car.m_Transmition = ParseTransmition(html);
            car.m_Condition = ParseCondition(html);
            car.m_PhotoUrls = ParsePhotos(html);

            return car;
        }

        public void SavePhotos(Car car)
        {
            int count = 0;
            foreach (var photoUrl in car.m_PhotoUrls)
            {
                var path = Path.Combine(m_DataFolder, car.m_Make);
                VerifyDataFolder(path);
                path = Path.Combine(path, car.m_Model);
                VerifyDataFolder(path);
                path = Path.Combine(path, car.m_Year.ToString());
                VerifyDataFolder(path);

                var fileName = Path.Combine(path, FileName(car, count));
                count++;

                if (!File.Exists(fileName))
                {
                    DownloadFile(photoUrl, fileName);
                }                
            }
        }

        public string FileName(Car car, int count)
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}({11}).jpg",
                                car.m_Make,
                                car.m_Model,
                                car.m_Year,
                                car.m_kW,
                                car.m_Price,
                                car.m_Mileage,
                                car.m_Fuel,
                                car.m_Gear,
                                car.m_BodyStyle,
                                car.m_Transmition,
                                car.m_Condition,
                                count);
        }

        public void VerifyDataFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public virtual int CurrentPage()
        {
            return -1;
        }

        public virtual string PageUrl(int pageNr)
        {
            return string.Empty;
        }

        public virtual string ParseMake(string html)
        {            
            return string.Empty;
        }

        public virtual string ParseModel(string html)
        {
            return string.Empty;
        }

        public virtual int ParsePrice(string html)
        {
            return int.MinValue;
        }

        public virtual int ParseYear(string html)
        {
            return int.MinValue;
        }

        public virtual int ParseMileage(string html)
        {
            return int.MinValue;
        }

        public virtual int ParseKW(string html)
        {
            return int.MinValue;
        }

        public virtual BodyStyle ParseBody(string html)
        {
            return BodyStyle.unknown;
        }

        public virtual Fuel ParseFuel(string html)
        {
            return Fuel.unknown;
        }

        public virtual Gear ParseGear(string html)
        {
            return Gear.unknown;
        }

        public virtual Transmition ParseTransmition(string html)
        {
            return Transmition.unknown;
        }

        public virtual Condition ParseCondition(string html)
        {
            return Condition.fine;
        }

        public virtual List<string> ParsePhotos(string html)
        {
            return new List<string>();
        }

        public virtual int GetLastPage(string html)
        {
            return -1;
        }

        public int ParseInt(string pattern, string html)
        {
            string match = Regex.Match(html, pattern).Groups[1].Value.Replace(" ", "");

            try
            {
                return int.Parse(match);
            }
            catch
            {
                return 0;
            }
        }
    }
}
