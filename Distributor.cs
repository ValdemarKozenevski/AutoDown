using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDataScraper
{
    class Distributor
    {
        string[] m_Links;
        List<Thread> m_Threads;
        List<Scraper> m_Scrapers;
        string m_DataFolder;

        ListBox m_OutputBox;
        ListBox m_StatusListBox;
        Thread m_UIUpdate;

        public Distributor(string[] links, string folder, ListBox outputBox, ListBox statusListBox)
        {
            m_Links = links;
            m_Threads = new List<Thread>();
            m_Scrapers = new List<Scraper>();
            m_DataFolder = folder;
            m_OutputBox = outputBox;
            m_StatusListBox = statusListBox;

            VerifyDataFolder();
            InitThreads();

            if (m_Threads.Count == 0)
            {
                MessageBox.Show("No URL IS GIVEN", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void InitThreads()
        {
            for (int i = 0; i < m_Links.Length; i++)
            {
                Scraper scraper = new Scraper(m_Links[i], m_DataFolder);
                if (m_Links[i].Contains("autoplius"))
                {
                    scraper = new AutoPliusScraper(m_Links[i], m_DataFolder);
                }
                else if (m_Links[i].Contains("autogidas"))
                {
                    scraper = new AutoGidasScraper(m_Links[i], m_DataFolder);
                }
                else if (m_Links[i].Contains("autoscout24"))
                {
                    scraper = new AutoScout24Scraper(m_Links[i], m_DataFolder);
                }
                else if (m_Links[i].Contains("mobile.de"))
                {
                    scraper = new MobileDeScraper(m_Links[i], m_DataFolder);
                }

                m_Scrapers.Add(scraper);

                var thread = new Thread(scraper.ScrapeTheData);
                thread.IsBackground = false;
                m_Threads.Add(thread);
            }
        }

        public void VerifyDataFolder()
        {            
            if (!Directory.Exists(m_DataFolder))
            {
                Directory.CreateDirectory(m_DataFolder);
            }            
        }

        public void StartScraping()
        {
            for(int i=0;i< m_Threads.Count; i++)
            {
                if (!m_Threads[i].IsAlive)
                {
                    m_Threads[i].Start();
                }
                
                m_Scrapers[i].m_ThreadHalt = false;
            }

            StatusUpdate();
        }

        public void PauseScraping()
        {
            for (int i = 0; i < m_Threads.Count; i++)
            {
                m_Scrapers[i].m_ThreadHalt = true;                
            }

            StatusUpdate();
        }

        public void StatusUpdate()
        {
            var dict = Directory.GetDirectories(m_DataFolder).ToDictionary(dir => dir.Replace(Path.GetDirectoryName(dir) + Path.DirectorySeparatorChar, ""), q => 0);
            var totalPhotos = 0;

            foreach (var make in Directory.GetDirectories(m_DataFolder))
            {
                foreach (var models in Directory.GetDirectories(make))
                {
                    foreach (var year in Directory.GetDirectories(models))
                    {
                        string previous = string.Empty;
                        foreach (var car in Directory.GetFiles(year))
                        {
                            var name = Path.GetFileNameWithoutExtension(car).Split('(')[0];

                            if (!string.Equals(name, previous))
                            {
                                dict[make.Replace(Path.GetDirectoryName(make) + Path.DirectorySeparatorChar, "")]++;
                            }
                            totalPhotos++;

                            previous = name;
                        }
                    }
                }
            }

            m_StatusListBox.Items.Clear();

            //m_StatusListBox.Items.Add(m_Scrapers.Any(q => !q.m_ThreadHalt)? "Running..." : "Halted! Press Start!");
            m_StatusListBox.Items.Add(string.Format("Total photos:{0}", totalPhotos));
            m_StatusListBox.Items.Add(string.Format("Total cars:{0}", dict.Sum(q => q.Value)));            
            foreach (var entry in dict.OrderByDescending(q => q.Value))
            {
                m_StatusListBox.Items.Add(string.Format("{0}:{1}", entry.Key, entry.Value));
            }

            m_OutputBox.Items.Clear();
            foreach(var scraper in m_Scrapers)
            {
                if (scraper.Done())
                {
                    m_OutputBox.Items.Add(string.Format("Done with {0} pages!: {1}", scraper.CurrentPage(), scraper.m_InitLink));
                }
                else if (scraper.m_ThreadHalt)
                {
                    m_OutputBox.Items.Add(string.Format("HALTED at{0}: {1}", scraper.CurrentPage(), scraper.m_InitLink));
                }
                else
                {
                    m_OutputBox.Items.Add(string.Format("Page {0}: {1}", scraper.CurrentPage(), scraper.m_InitLink));
                }
            }
        }        
    }
}
