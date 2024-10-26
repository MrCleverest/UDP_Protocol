using System;
using SimpleUdp;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace RSZI_2
{
    public partial class UDPServer : Form
    {
        private UdpEndpoint udp;

        public UDPServer()
        {
            InitializeComponent();
        }

        private void EndpointDetected(object sender, EndpointMetadata md)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += "Виявлено кінцеву точку: " + md.Ip + ":" + md.Port + ". ";
            });
        }

        private void Server_DataReceived(object sender, Datagram dg)
        {
            string clientMessage = Encoding.UTF8.GetString(dg.Data);
            // Розділити рядок на окремі числа
            string[] parts = clientMessage.Split('#');

            // Перевірити, чи отримано рівно 4 числа
            double[] array = new double[4];
            // Спробувати зчитати числа з рядка
            for (int i = 0; i < 4; i++)
            {
                if (double.TryParse(parts[i], out double num))
                {
                    array[i] = num;
                }
                else
                {
                    MessageBox.Show($"Неправильний формат числа: {parts[i]}");
                    return; // Вийти з методу, якщо формат числа неправильний
                }
            }

            // Обрахунок
            double result = 1;
            for (int i = 0; i < 10; i++)
            {
                double numerator = Math.Pow(-1, i) * (Math.Log(array[2]) + array[3] * array[1] + array[0] * Math.Pow(array[2], 2));
                double denominator = Factorial(i) * Math.E;
                result *= numerator / denominator;
            }

            // Повернення результату клієнту
            udp.Send(dg.Ip, dg.Port, "Результат: " + result.ToString());
        }

        private int Factorial(int n)
        {
            if (n <= 1)
                return 1;
            else
                return n * Factorial(n - 1);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            udp = new UdpEndpoint("127.0.0.1", 8001);
            udp.EndpointDetected += EndpointDetected;
            udp.DatagramReceived += Server_DataReceived;
            udp.Start();
            txtStatus.Text += "Сервер запущено.";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            udp.Stop();
            txtStatus.Text += "Сервер зупинений.";
        }
    }
}
