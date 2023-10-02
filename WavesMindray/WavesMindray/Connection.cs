using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodSharp.Sockets;

namespace WavesMindray
{
    public class Connection
    {   

        public Connection()
        {

        }

        public void openServerTCP(string ip)
        {
            ITcpServer server = new TcpServer(5510)
            {
                OnConnected = (c) =>
                {
                    Console.WriteLine($"{c.RemoteEndPoint} connected.");
                    
                },
                OnReceived = (c) =>
                {
                    if(c.RemoteEndPoint.ToString().Split(":")[0] == ip)
                    {
                        string data = System.Text.Encoding.ASCII.GetString(c.Buffers);
                        //data = data.Replace("\v", "");
                        data = data.Replace("\r", "\n");
                        data = data.Replace("\u001c", "");
                        if (data.Contains("WAVEFORM")) {
                            //Console.WriteLine("Mensagem Recebida");
                            //Console.WriteLine(data);
                            translateMessage(data);
                        }
                    }
                },
                OnDisconnected = (c) =>
                   {
                       Console.WriteLine($"{c.RemoteEndPoint} disconnected.");
                   },
                   OnStarted = (c) =>
                   {
                       Console.WriteLine($"{c.LocalEndPoint} started.");
                   },
                   OnStopped = (c) =>
                   {
                       Console.WriteLine($"{c.LocalEndPoint} stopped.");
                   },
                   OnException = (c) =>
                   {
                       Console.WriteLine($"{c.RemoteEndPoint} exception:{c.Exception.StackTrace.ToString()}.");
                   },
                   OnServerException = (c) =>
                   {
                       Console.WriteLine($"{c.LocalEndPoint} exception:{c.Exception.StackTrace.ToString()}.");
                   }
            };
            server.UseKeepAlive(true, 500, 500);
            server.Start();
            while (true)
            {
                Console.ReadKey();
                server.Stop();
                break;
            }
        }

        public void translateMessage(string message)
        {
            //message = message.Replace("\v", "");
            string waveI = "MDC_ECG_ELEC_POTL_AVR";
            string fileNameWaveI = waveI + ".txt";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileNameWaveI);
            string[] lines = message.Split("\n");
            var line = lines.Where(x => x.Contains(waveI)).FirstOrDefault();
            if(line != null)
            {
                Console.WriteLine(line.ToString());
                var wave = line.Split("|")[5];
                wave = wave.Replace("^", "\n");
                //wave = wave.Replace(",", " \n");
                StreamWriter wrStream = new StreamWriter(fileNameWaveI, true, Encoding.ASCII);
                wrStream.Write(wave);
                wrStream.Close();
            }
        }
    }
}
