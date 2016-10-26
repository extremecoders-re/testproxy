using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace testproxy
{
	public partial class MainForm : Form
	{
		private string result;
		public MainForm()
		{
			InitializeComponent();
		}
		
		private void Button1Click(object sender, EventArgs e)
		{	
			button1.Enabled = false;
			backgroundWorker1.RunWorkerAsync();			
		}
		

		void BackgroundWorker1DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			string IP = textBox1.Text;
			string port = textBox2.Text;
			string URI = textBox4.Text;
			string useragent = textBox5.Text;
			int port_num;
			
			if (IP.Length == 0 || port.Length == 0 || URI.Length == 0) return;
			if (!Int32.TryParse(port, out port_num)) return;	
			
			try
            {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);		
				request.Proxy = new WebProxy(IP, port_num);
            	request.UserAgent = useragent;
            
                WebResponse response = request.GetResponse();
                int cl = (response.ContentLength > 32767) ? 32767: (int)response.ContentLength;
                byte[] buffer = new byte[cl];
                response.GetResponseStream().Read(buffer, 0, buffer.Length);                
                result = Encoding.UTF8.GetString(buffer);
                response.Close();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }	
			
		}
		
		void BackgroundWorker1RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			textBox3.Text = result;
			button1.Enabled = true;			
		}
	}		
		

}
