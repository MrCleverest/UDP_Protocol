using System;
using SimpleUdp;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientUDP
{
    public partial class UDPClient : Form
    {
        private UdpEndpoint udp;

        public UDPClient()
        {
            InitializeComponent();
        }

        private void UDPClient_Load(object sender, EventArgs e)
        {
            udp = new UdpEndpoint("127.0.0.1", 8000);
            udp.DatagramReceived += Client_DataReceived;
            udp.Start();
        }

        private void Client_DataReceived(object sender, Datagram dg)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += "[" + dg.Ip + ":" + dg.Port + "]: " + Encoding.UTF8.GetString(dg.Data);
            });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string serverIP = txtHost.Text;
            int serverPort;
            if (int.TryParse(txtPort.Text, out serverPort))
            {
                // Зібрання всіх змінних у рядок
                string message = $"{txtA.Text}#{txtY.Text}#{txtX.Text}#{txtB.Text}#";
                udp.Send(serverIP, serverPort, message);
            }
            else
            {
                MessageBox.Show("Неправильний номер порту.");
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            MessageBox.Show("UDP не потребує підключення до серверу. Просто відправте дані.");
        }
    }
}
