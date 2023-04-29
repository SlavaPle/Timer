using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Timer
{
    public partial class Form1 : Form
    {
        readonly List<string> Dane = new List<string> { "Second", "Minute", "Hour", "Day" };
        Dictionary<string, uint> TimeToSec = new Dictionary<string, uint>() { { "Second", 1 }, { "Minute", 60 }, { "Hour", 3600 }, { "Day", 86400 } };
        List<MyTimer> LT = new List<MyTimer>();
        uint TimeSec = 0;

        public Form1()
        {
            InitializeComponent();
            label1.Text = "";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (TimeSec > 0)
            {
                MyTimer T = new MyTimer();
                T.Time = TimeSec;

                LT.Add(T);

                T.Start();

                int i = 0;
                LT = LT.AsEnumerable().OrderBy(x => x.Time).ToList();
                foreach (MyTimer t in LT)
                {
                    t.L.Location = new Point(0, i * 20+5);
                    t.B.Location = new Point(150, i * 20);
                    panel1.Controls.Add(t.L);
                    panel1.Controls.Add(t.B);
                    i++;
                }
            }
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
            var ts = TimeSpan.FromSeconds(TimeSec);
            label1.Text = String.Format("{0} d{1,5} h{2,5} m{3,5} s", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            //label1.Text = TimeSec.ToString() + " sec";
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


    class MyTimer : ITimer
    {
        private uint time;

        public bool Ring { get; private set; }

        public System.Windows.Forms.Timer T = new System.Windows.Forms.Timer { Interval = 100 };
        public Label L = new Label() { Height = 15, Width = 150 };
        public Button B = new Button() { Height = 20, Width = 50, Text = "Reset" };

        public uint Time
        {
            get => time; set
            {
                {
                    time = value;
                    var ts = TimeSpan.FromSeconds(Time);
                    L.Text = String.Format("{0} d{1,5} h{2,5} m{3,5} s", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
                }
            }
        }

        public void Start()
        {
            T.Tick += new EventHandler(Timer_Tick);
            B.Click += new EventHandler(Button_Click);
            T.Start();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Time = 0;
        }

        private void Timer_Tick(object source, EventArgs e)
        {
            if (Time > 0)
            { Time--; }
            else
            { T.Stop(); }
        }
    }

    interface ITimer
    {
        void Start();
    }
}

