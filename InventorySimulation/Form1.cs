using InventoryModels;
using InventoryTesting;
using System;
using System.Windows.Forms;

namespace InventorySimulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
         }

        private void Form1_Load(object sender, EventArgs e)
        {
            SimulationSystem s=InputReader.Load();
            s.generateCases();
            string testCase = TestingManager.Test(s,Constants.FileNames.TestCase1);
            dataGridView1.DataSource = s.SimulationCases.ToArray();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBox1.Text = s.PerformanceMeasures.EndingInventoryAverage.ToString();
            textBox2.Text = s.PerformanceMeasures.ShortageQuantityAverage.ToString();
            MessageBox.Show(testCase);
        }
    }
}
