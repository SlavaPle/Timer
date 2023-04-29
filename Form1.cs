using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timer
{
    public partial class Form1 : Form
    {
        readonly List<string> Dane = new List<string> { "Second", "Minute", "Hour", "Day" };
        Dictionary<string, uint> TimeToSec = new Dictionary<string, uint>() { { "Second", 1 }, { "Minute", 60 }, { "Hour", 3600 }, { "Day", 86400 } };
        List<Timer> LT = new List<Timer>();
        uint TimeSec = 0;

        public Form1()
        {
            InitializeComponent();
            label1.Text = "";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Timer T = new Timer();
            T.Time = TimeSec;
            T.Ticking();
            LT.Add(T);
        }

        private void ComboBox1_TextChanged(object sender, EventArgs e)
        {
            TimeSec = 0;
            foreach (Control C in Controls)
            {
                if (C.GetType().ToString() is "System.Windows.Forms.ComboBox" && ((ComboBox)C).SelectedIndex > -1)
                {
                    string Index = ((ComboBox)C).Name.Substring(((ComboBox)C).Name.Length - 1);
                    ((NumericUpDown)Controls["NumericUpDown" + Index]).Enabled = true;
                    TimeSec += (uint)((NumericUpDown)Controls["NumericUpDown" + Index]).Value * TimeToSec[((ComboBox)C).Text];
                }
            }
            label1.Text = TimeSec.ToString() + " sec";
        }

        private void ComboBox1_DropDown(object sender, EventArgs e)
        {
            List<string> UsedDane = new List<string>(Dane);
            foreach (Control C in Controls)
            {
                if (C.GetType().ToString() is "System.Windows.Forms.ComboBox" && ((ComboBox)C).SelectedIndex > -1 && C != sender)
                {
                    UsedDane.Remove(((ComboBox)C).Text);
                }
            }
            ((ComboBox)sender).Items.Clear();
            ((ComboBox)sender).Items.AddRange(UsedDane.ToArray());
        }
    }

    class Timer
    {
        public bool Ring { get; private set; }
        public uint Time { get; set; }

        public void Ticking()
        {
            Time = Time--;
        }
    }
}
