using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        Thread workThread;
        const string ServerQueueName = @".\Private$\EntryService";
        const string _directory = "./Data";
        ManualResetEvent stopWorkEvent;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            stopWorkEvent = new ManualResetEvent(false);

            workThread = new Thread(Server);
            workThread.Start();
        }

        private void Server(object obj)
        {
            using (var serverQueue = new MessageQueue(ServerQueueName))
            {
                serverQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(FileMessage) });

                while (!stopWorkEvent.WaitOne(TimeSpan.Zero))
                {
                    var asyncReceive = serverQueue.BeginReceive();

                    var res = WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, asyncReceive.AsyncWaitHandle });
                    if (res == 0)
                        break;

                    var message = serverQueue.EndReceive(asyncReceive);

                    //Parse
                    FileMessage messageData = null;
                    if (message.BodyStream.Length > 0)
                    {
                        try
                        {
                            messageData = message.Body as FileMessage;
                        }
                        catch (Exception ex)
                        {
                            textBox1.Invoke(new Action(() => this.textBox1.AppendText(ex.Message)));
                        }
                    }

                    if(messageData != null)
                    {
                        SaveFile(messageData);
                    }

                }
            }
                                                
        }

        private void SaveFile(FileMessage messageData)
        {
            if (messageData != null)
            {
                var existingDirectories = Directory.GetDirectories("./");
                if (!existingDirectories.Contains(_directory))
                {
                    Directory.CreateDirectory("./" + _directory);
                }

                string[] files = Directory.GetFiles(_directory).Select(x => x.Replace("/", "\\")).ToArray();

                if (files.Contains(messageData.FileName))
                {
                    using (FileStream fstream = new FileStream(messageData.FileName, FileMode.Append))
                    {
                        fstream.Write(messageData.Data, 0, messageData.Data.Length);
                    }
                }
                else
                {
                    using (FileStream fstream = new FileStream(messageData.FileName, FileMode.OpenOrCreate))
                    {
                        fstream.Write(messageData.Data, 0, messageData.Data.Length);
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopWorkEvent.Set();
            workThread.Join(1000);
        }
    }
}
