using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminHunter
{
    public partial class HuntForm : Form
    {
        public HuntForm()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private  Task<List<Result>> MainRun(string Url)
        {
            if (!File.Exists(Application.StartupPath + "dic.txt"))
            {
                MessageBox.Show("Can't Load Dictionary . . .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return null;
            }
            List<string> peyloads = File.ReadAllLines(Application.StartupPath + "dic.txt").ToList();

            return Task.Run(()=>
            {
                List<Result> results = new List<Result>();
                int offset = 1;
                MessageBox.Show(Url);
                foreach (string peyload in peyloads)
                {
                    try
                    {
                        HttpWebRequest request = WebRequest.Create(Url + peyload) as HttpWebRequest;
                        request.Method = "HEAD";
                        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        if (response.StatusCode == HttpStatusCode.Forbidden ||
                            response.StatusCode == HttpStatusCode.Accepted ||
                            response.StatusCode == HttpStatusCode.OK ||
                            response.StatusCode == HttpStatusCode.Found)
                        {
                            results.Add(new Result() {HuntUrl = Url + peyload, PeyLoad = peyload, Target = Url});
                        }
                        this.Invoke(new Action(() =>
                        {
                            progressBar1.Maximum = peyloads.Count;
                            progressBar1.Value++;
                        }));
                        response.Close();

                    }
                    catch (WebException w)
                    {
                        this.Invoke(new Action(() =>
                        {
                            progressBar1.Maximum = peyloads.Count;
                            progressBar1.Value++;
                        }));
                        continue;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                        this.Close();
                    }
                    offset++;
                }
                return results;
            });
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void HuntForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Please Select a Method . . .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dataGridView1.AutoGenerateColumns = false;
            List<Result> Finaly = await MainRun(comboBox1.Text + textBox1.Text + "/");
            dataGridView1.DataSource = Finaly;
            if (Finaly.Count < 600)
            {
                MessageBox.Show("This Site Is a Redirect Url Site . . .", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    public class Result
    {
        public string Target { get; set; }
        public string HuntUrl { get; set; }
        public string PeyLoad { get; set; }
    }
}
