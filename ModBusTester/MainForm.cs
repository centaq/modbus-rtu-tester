using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace ModBusTester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataBitsComboBox.Items.Add(5);
            DataBitsComboBox.Items.Add(6);
            DataBitsComboBox.Items.Add(7);
            DataBitsComboBox.Items.Add(8);
            DataBitsComboBox.SelectedIndex = 3;

            BaudRateComboBox.Items.Add(300);
            BaudRateComboBox.Items.Add(600);
            BaudRateComboBox.Items.Add(1200);
            BaudRateComboBox.Items.Add(2400);
            BaudRateComboBox.Items.Add(9600);
            BaudRateComboBox.Items.Add(14400);
            BaudRateComboBox.Items.Add(19200);
            BaudRateComboBox.Items.Add(38400);
            BaudRateComboBox.Items.Add(57600);
            BaudRateComboBox.Items.Add(115200);
            BaudRateComboBox.Items.ToString();
			
            BaudRateComboBox.SelectedIndex=8;

            HandShakeComboBox.Items.Add("None");
            HandShakeComboBox.Items.Add("XOnXOff");
            HandShakeComboBox.Items.Add("RequestToSend");
            HandShakeComboBox.Items.Add("RequestToSendXOnXOff");

            DataBitsComboBox.SelectedIndex = 3;
            HandShakeComboBox.SelectedIndex = 0;
            StopBitsComboBox.SelectedIndex = 1;
            ParityComboBox.SelectedIndex = 0;

            ToTextBox_TextChanged(null, null);
        }

        private void GetCOMButton_Click(object sender, EventArgs e)
        {
            foreach (string port in SerialPort.GetPortNames())
            {
                COMComboBox.Items.Add(port);
            }
            if (COMComboBox.Items.Count > 0)
                COMComboBox.SelectedIndex = 0;
        }

        private void ToTextBox_TextChanged(object sender, EventArgs e)
        {
            ToSendTextBox.Text = ToTextBox.Text + " " + CmdTextBox.Text + " " + ParamTextBox.Text;
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            try
            {
                ComPort.PortName = Convert.ToString(COMComboBox.Text);
                ComPort.BaudRate = Convert.ToInt32(BaudRateComboBox.Text);
                ComPort.DataBits = Convert.ToInt16(DataBitsComboBox.Text);
                ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBitsComboBox.Text);
                ComPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), HandShakeComboBox.Text);
                ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), ParityComboBox.Text);
                ComPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            byte[] buffor = new byte[11];
            buffor[0] = Convert.ToByte(ToTextBox.Text, 16);
            buffor[1] = Convert.ToByte(CmdTextBox.Text, 16);
            for (int i = 0; i < ParamTextBox.Text.Length/2; i++)
                buffor[2+i] = Convert.ToByte(ParamTextBox.Text.Substring((i)*2, 2), 16);
            int len = 0;
            if (buffor[1] == 3)
                len = 2;
            else if (buffor[1] == 6)
                len = 6;
            else if (buffor[1] == 16)
                len = 2 + ParamTextBox.Text.Length/2;
            else
                len = 0;
            ComPort.Write(buffor, 0, len);
            label9.Text = DateTime.Now.ToString("o");
            label10.Text = "";
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ComPort.Close();
        }

        private void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            byte[] bytes = new byte[2000];
            ComPort.Read(bytes, 0, ComPort.BytesToRead);
            string text = string.Join(" ", bytes.Take(40).Select(b => Convert.ToString(b, 16).PadLeft(2,'0')).ToArray());

            SetText(text);
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                label10.Text = DateTime.Now.ToString("o");
                this.richTextBox1.Text =richTextBox1.Text + Environment.NewLine + text;
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
