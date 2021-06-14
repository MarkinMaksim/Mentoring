using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        Thread workThread;
        const string ServerQueueName = @".\Private$\EntryService";
        const string _dirName = @".\Data";
        const string _format = ".pdf";
        const int _maxMessageSize = 4096;
        ManualResetEvent stopWorkEvent;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!MessageQueue.Exists(ServerQueueName))
                MessageQueue.Create(ServerQueueName);

            stopWorkEvent = new ManualResetEvent(false);
            
            workThread = new Thread(Client);
            workThread.Start();
        }

        private void Client(object obj)
        {
            using (var serverQueue = new MessageQueue(ServerQueueName, QueueAccessMode.Send))
            {
                serverQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(FileMessage) });
                serverQueue.MessageReadPropertyFilter.CorrelationId = true;
                string[] previosFiles = new string[0];

                while (!stopWorkEvent.WaitOne(TimeSpan.Zero))
                {
                    var files = Directory.GetFiles(_dirName);

                    var filesToSend = files.Where(x => !previosFiles.Contains(x) && x.Contains(_format)).ToArray();

                    if (previosFiles.Length == 0)
                    {
                        filesToSend = files.Where(x => x.Contains(_format)).ToArray();
                    }

                    if (filesToSend.Length > 0)
                    {
                        SendMessage(filesToSend, serverQueue, 0);
                    }

                    previosFiles = files;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopWorkEvent.Set();
            workThread.Join(1000);
        }

        private void SendMessage(string[] files, MessageQueue serverQueue, int repeatCount)
        {
            foreach (var file in files)
            {
                try
                {
                    using (FileStream fstream = File.OpenRead(file))
                    {
                        byte[] data;
                        var fileLength = new FileInfo(file).Length;

                        while (fstream.Position < fileLength)
                        {
                            var dataSize = fileLength - fstream.Position;

                            if (dataSize >= _maxMessageSize)
                            {
                                data = new byte[_maxMessageSize];
                            }
                            else
                            {
                                data = new byte[dataSize];
                            }

                            fstream.Read(data, 0, data.Length);
                            var message = new FileMessage()
                            {
                                Data = data,
                                FileName = file,
                            };

                            try
                            {
                                serverQueue.Send(message);
                                OutputInfo.Invoke(new Action(() => this.OutputInfo.AppendText($"File with name {message.FileName} was sended. Position: {fstream.Position} \r\n")));
                            }
                            catch (Exception ex)
                            {
                                OutputInfo.Invoke(new Action(() => this.OutputInfo.AppendText(ex.Message)));
                            }
                        }
                    }
                }
                catch(IOException ex)
                {
                    if(repeatCount < 5)
                    {
                        Thread.Sleep(1000);
                        SendMessage(new[] { file }, serverQueue, repeatCount + 1);
                    }
                    else
                    {
                        OutputInfo.Invoke(new Action(() => OutputInfo.AppendText(ex.Message)));
                    }
                    
                }
               
            }
        }
    }
}
