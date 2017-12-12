using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NameGen
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        IEnumerable<CheckBox> GetOptionsCheckBox()
        {
            yield return checkBox1;
            yield return checkBox2;
            yield return checkBox3;
            yield return checkBox4;
            yield return checkBox5;
            yield return checkBox6;
            yield return checkBox7;
            yield return checkBox8;
            yield return checkBox9;
            yield return checkBox10;
            yield return checkBox11;
            yield return checkBox14;
            yield return checkBox15;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //save
            try
            {
                TextSource src = new TextSource();

                string[] prefix = tbPrefix.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                src.Prefix.AddRange(prefix.ToList());

                string[] words = tbWords.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                src.Words.AddRange(words.ToList());

                string[] middle = tbMiddle.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                src.Middle.AddRange(middle.ToList());

                string[] sufix = tbSufix.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                src.Sufix.AddRange(sufix.ToList());

                foreach (var cb in GetOptionsCheckBox())
                {
                    if (cb.Checked)
                        src.Options.Add(cb.Tag.ToString());
                }
                new TextSourceProvider().Save(src);



                listView1.Items.Clear();
                int max = Int32.Parse(cbMax.Text);
                WordGenerator generator = new WordGenerator(src);
                var result = generator.Generate();
                //string txt = string.Empty;
                ListViewGroup grp = null;
                foreach (var item in result)
                {
                    var name=item.Replace(" | ", "");
                    if (item.StartsWith(">>>"))
                    {
                        grp = new ListViewGroup(item, item);
                        listView1.Groups.Add(grp);
                    }
                    else if (name.Length > max)
                    { 
                        continue;
                    }
                    else if (!name.ToUpper().Contains(tbFilter.Text.ToUpper()) && !string.IsNullOrEmpty(tbFilter.Text))
                    {
                        continue;
                    }
                    else
                    {
                        var lvi = new ListViewItem(item) { Group = grp };
                        lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                        lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                        listView1.Items.Add(lvi);
                    }
                    //txt += item + Environment.NewLine;

                }
                //tbResult.Text = txt;
                label1.Text = "Count: " + listView1.Items.Count;


                //this.flowLayoutPanel1.SuspendLayout();
                //this.flowLayoutPanel1.Controls.Clear();
                //var items=txt.Split(new string[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                //for (int i = 0; i < items.Length; i++)
                //{
                //    var name=items[i].Trim();
                //    if (name.StartsWith("████████████████"))
                //        continue;
                //    AddItem(name.Replace(" | ",""), i);
                //}
                //this.flowLayoutPanel1.ResumeLayout(false);
            }
            catch (Exception)
            {
                throw;
            }

        }

        //void AddItem(string name,int index)
        //{
        //    var item = new CheckControl();
        //    item.CtrlNameText = name;
        //    //this.checkControl1.Location = new System.Drawing.Point(3, 3);
        //    item.Name = "ctrlCheckControl" + index;
        //    item.Size = new System.Drawing.Size(400, 26);
        //    this.checkControl1.Location = new System.Drawing.Point(3, 3+(index*32));
        //    this.checkControl1.TabIndex = index;
        //    this.flowLayoutPanel1.Controls.Add(item);
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var src = new TextSourceProvider().Load();
                foreach (var item in src.Prefix)
                {
                    tbPrefix.Text += item + Environment.NewLine;
                }
                foreach (var item in src.Words)
                {
                    tbWords.Text += item + Environment.NewLine;
                }
                foreach (var item in src.Middle)
                {
                    tbMiddle.Text += item + Environment.NewLine;
                }
                foreach (var item in src.Sufix)
                {
                    tbSufix.Text += item + Environment.NewLine;
                }
            }
            catch (FileNotFoundException ex)
            {
                //throw;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var item = listView1.SelectedItems[0];
                var text = item.Text;
                item.SubItems[1].Text = "loading...";
                item.SubItems[2].Text = "loading...";

                Thread piThread = new Thread(new ParameterizedThreadStart(loop));
                piThread.Start(item);
            }
        }



        private void loop(Object state)
        {
            Application.DoEvents();
            Thread.Sleep(100);
            Application.DoEvents();
            var item = state as ListViewItem;
            var name = item.Text;
            name = name.Replace(" | ", "");
            Application.DoEvents();
            var exist_de = CheckHost(name + ".de");
            Application.DoEvents();
            var exist_com = CheckHost(name + ".com");
            Application.DoEvents();
            this.UIThread(() =>
            {
                item.BackColor = (!exist_de && !exist_com) ? Color.LightGreen : Color.MistyRose;
                item.SubItems[1].Text = exist_de ? "Exist" : "Empty";
                item.SubItems[2].Text = exist_com ? "Exist" : "Empty";
                Application.DoEvents();
            });
            Application.DoEvents();
        }

        public static bool CheckHost(string nameOrAddress)
        {
            IPAddress[] ips;
            try
            {
                ips = Dns.GetHostAddresses(nameOrAddress);
                Application.DoEvents();
                //Console.WriteLine("GetHostAddresses({0}) returns:", nameOrAddress);
                foreach (IPAddress ip in ips)
                {
                    //Console.WriteLine("    {0}", ip);
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
            /*
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            return pingable;
             */
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            this.progressBar1.Visible = true;
            var max=listView1.Items.Count;
            int i = 0;
            Application.DoEvents();
            foreach (ListViewItem item in listView1.Items)
            {
                i++;
                var text = item.Text;
                item.SubItems[1].Text = "loading...";
                item.SubItems[2].Text = "loading...";
                Application.DoEvents();
                Thread piThread = new Thread(new ParameterizedThreadStart(loop));
                piThread.Start(item);
                this.progressBar1.Value = (int)(i*100/max);
            }
            this.progressBar1.Value = 0;
            this.progressBar1.Visible = false;
        }
    }
}
