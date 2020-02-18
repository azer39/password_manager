using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;

namespace PW_Manager
{

    public partial class Form1 : Form
    {
        string path = "C:\\PWD Manager\\";

        public Form1()
        {
            InitializeComponent();
            createFiles();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String name = textBox1.Text;
                String username = textBox2.Text;
                String password = textBox3.Text;

                if (name != "" && username != "" && password != "")
                {
                    username = functions.Encrypt(username);
                    password = functions.Encrypt(password);

                    String data = name + "!" + username + "§" + password;

                    bool nameExists = false;

                    using(StreamReader srr = new StreamReader(path + "file.txt"))
                    {
                        string line;
                        while ((line = srr.ReadLine()) != null)
                        {
                            if (line.StartsWith(name))
                            {
                                nameExists = true;
                                MessageBox.Show("Name already exists!");
                            }
                        }
                    }

                    if (!nameExists)
                    {
                        using (FileStream fs = new FileStream(path + "file.txt", FileMode.Append, FileAccess.Write))
                        {
                            using (StreamWriter sr = new StreamWriter(fs))
                            {
                                sr.WriteLine(data);
                                sr.Close();
                            }
                        }
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();

                    }
                    refreshBox();
                }
                else
                {
                    MessageBox.Show("Fields empty!");
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void createFiles()
        {
            try
            {
                bool folderExists = Directory.Exists(path);
                if (!folderExists)
                {
                    Directory.CreateDirectory("C:\\PWD Manager\\");
                }

                if (!File.Exists(path + "file.txt"))
                {
                    File.Create(path + "file.txt");
                }
                else
                {
                    using (StreamReader sw = new StreamReader(path + "file.txt"))
                    {
                        string line;

                        while ((line = sw.ReadLine()) != null)
                        {
                            string[] name = line.Split('!');

                            listBox1.Items.Add(name[0]);
                        }
                        sw.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        //Copy Username
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    string itm = listBox1.SelectedItem.ToString();

                    using (StreamReader sw = new StreamReader(path + "file.txt"))
                    {
                        string line;
                        while ((line = sw.ReadLine()) != null)
                        {

                            string[] name = line.Split('!');
                            if (name[0] == itm)
                            {
                                string usr = name[1];
                                usr = usr.Split('§')[0];
                                usr = functions.Decrypt(usr);

                                Clipboard.SetText(usr);
                            }
                        }
                        sw.Close();
                    }
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        //Copy Password
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    string itm = listBox1.SelectedItem.ToString();

                    using (StreamReader sw = new StreamReader(path + "file.txt"))
                    {
                        string line;

                        while ((line = sw.ReadLine()) != null)
                        {
                            string[] name = line.Split('!');

                            if (name[0] == itm)
                            {
                                string pwd = name[1];
                                pwd = pwd.Split('§')[1];
                                pwd = functions.Decrypt(pwd);

                                Clipboard.SetText(pwd);
                            }
                        }
                        sw.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void refreshBox()
        {
            try
            {
                listBox1.Items.Clear();

                using (StreamReader sw = new StreamReader(path + "file.txt"))
                {
                    string line;

                    while ((line = sw.ReadLine()) != null)
                    {
                        string[] name = line.Split('!');

                        listBox1.Items.Add(name[0]);
                    }
                    sw.Close();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        //Delete Item
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    var confirmDialog = MessageBox.Show("You sure you wanna delete this Item?", "Confirm delete", MessageBoxButtons.YesNoCancel);

                    if (confirmDialog == DialogResult.Yes)
                    {
                        string item = listBox1.SelectedItem.ToString();
                        string tmpfile = Path.GetTempFileName();

                        using (StreamReader sr = new StreamReader(path + "file.txt"))
                        {
                            using (StreamWriter sw = new StreamWriter(tmpfile))
                            {

                                string line;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (!line.StartsWith(item))
                                    {
                                        sw.WriteLine(line);
                                    }
                                }
                                sw.Close();
                                sr.Close();

                            }
                        }
                        File.Delete(path + "file.txt");
                        File.Move(tmpfile, path + "file.txt");
                        refreshBox();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string b = CreatePassword(22);
                textBox3.Text = b;

            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890$%&/()=";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void keyPress(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                try
                {
                    String name = textBox1.Text;
                    String username = textBox2.Text;
                    String password = textBox3.Text;

                    if(name != "" && username != "" && password != "")
                    {
                        username = functions.Encrypt(username);
                        password = functions.Encrypt(password);

                        String data = name + "!" + username + "§" + password;

                        using (FileStream fs = new FileStream(path + "file.txt", FileMode.Append, FileAccess.Write))
                        {
                            using (StreamWriter sr = new StreamWriter(fs))
                            {
                                sr.WriteLine(data);
                                sr.Close();
                            }
                        }
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        refreshBox();
                    }
                    else
                    {
                        MessageBox.Show("Fields empty!");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            try
            {


                listBox1.Items.Clear();
                using (StreamReader sw = new StreamReader(path + "file.txt"))
                {
                    string line;

                    while ((line = sw.ReadLine()) != null)
                    {
                        string[] name = line.Split('!');

                        string v = name[0];

                        if (v.StartsWith(textBox4.Text))
                        {
                            listBox1.Items.Add(name[0]);
                        }
                    }
                    sw.Close();
                }
                textBox4.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }




        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                refreshBox();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textChange(object sender, EventArgs e)
        {
            string d = textBox4.Text.ToLower();

            try
            {
                listBox1.Items.Clear();
                using (StreamReader sw = new StreamReader(path + "file.txt"))
                {
                    string line;

                    while ((line = sw.ReadLine()) != null)
                    {
                        string[] name = line.Split('!');

                        string v = name[0].ToLower();

                        if (v.Contains(d))
                        {
                            listBox1.Items.Add(name[0]);
                        }
                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            try
            {

                using (StreamReader sw = new StreamReader(path + "file.txt"))
                {
                    string line;

                    while ((line = sw.ReadLine()) != null)
                    {
                        string[] name = line.Split('!');
                        if (name[0] == listBox1.SelectedItem.ToString())
                        {
                            string[] data = name[1].Split('§');
                            string usr = functions.Decrypt(data[0]);
                            string pwd = functions.Decrypt(data[1]);
                            //MessageBox.Show(usr);
                            //MessageBox.Show(pwd);

                            MessageBox.Show("soon");
                        }

                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
