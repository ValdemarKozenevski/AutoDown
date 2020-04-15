using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDataScraper
{
    public partial class Form1 : Form
    {
        Distributor m_Distributor;

        public Form1()
        {
            InitializeComponent();
            dataFolderBox.Text = @"G:\AutoData";
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(linksTextBox.Text))
            {
                MessageBox.Show("No URL IS GIVEN", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(dataFolderBox.Text))
            {
                MessageBox.Show("No FOLDER IS GIVEN", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (m_Distributor == null)
            {
                m_Distributor = new Distributor(linksTextBox.Text.Split('\n'), dataFolderBox.Text, parsingStatusBox, statusListBox);
            }

            m_Distributor.StartScraping();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            m_Distributor?.PauseScraping();
        }

        private void StatusUpdateButton_Click(object sender, EventArgs e)
        {
            m_Distributor?.StatusUpdate();
        }
    }
}
