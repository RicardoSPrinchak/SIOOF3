using FireSharp.Config;
using FireSharp.Extensions;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using GemBox.Spreadsheet;
using System.Windows.Forms;
using GemBox.Spreadsheet.Drawing;
using System.Collections.ObjectModel;
using System.Data;
using ListBox = System.Windows.Forms.ListBox;
using Microsoft.VisualBasic.ApplicationServices;

namespace SIOOF3
{
    
    public partial class Form1 : Form
    {
       
        public static string nomeS;

        List<string> listcollection = new List<string>();

        IFirebaseConfig config = new FirebaseConfig
        {
            //Add your firebase path and token here!
            AuthSecret = "u8t2EbIOxgPzi0FJKNQmiRUj4kUUSpoepGzRVxJK",
            BasePath = "https://sioof-dd93e-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        
        public Form1()
        {
            InitializeComponent();
            
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            
            client = new FireSharp.FirebaseClient(config);
            if(client!=null)
            {
                label7.Text = "Conectado";
                label7.ForeColor = Color.Green;
            }
            else
            {
                label7.Text = "Não conectado";
                label7.ForeColor = Color.Red;
            }
            MyMethod();
          
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nomeS = "";
            Dados settingForm = new Dados();
             settingForm.ShowDialog();
          
            MyMethod();
        }

        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           // filterbox.Text = filterbox.Text.ToUpper();
            listBox2.Items.Clear();
            if (string.IsNullOrEmpty(filterbox.Text) == false)
            {
                foreach(string str in listcollection)
                {
                    if (str.StartsWith(filterbox.Text))
                    {
                        listBox2.Items.Add(str);
                    }
                }
            }
          else if (filterbox.Text == "")
            {
                MyMethod();
            }



         
        }

        public async void button4_Click(object sender, EventArgs e)
        {
            await Task.Delay(1000);
                        
            MyMethod();
        }
   

        private void button2_Click(object sender, EventArgs e)
        {       
            if (listBox2.SelectedItems.Count > 0)
            {
                nomeS = listBox2.GetItemText(listBox2.SelectedItem);
                Dados settingForm = new Dados();
                settingForm.ShowDialog();
                MyMethod();
                
            }
            else
            {
                MessageBox.Show("Selecione um aluno para acessar");
            }
            
            
        }
      
            private async void button3_Click(object sender, EventArgs e)
        {
           if (listBox2.SelectedItems.Count > 0)
          {
                FirebaseResponse response = client.Delete("SIOOF3/" + listBox2.GetItemText(listBox2.SelectedItem));
                listBox2.Items.Remove(listBox2.SelectedItem);

                MyMethod();
      }
          else
          {
              MessageBox.Show("Selecione um aluno para deletar");
          }
        
        }


    public void MyMethod()
        {
            FirebaseResponse res = client.Get(@"SIOOF3");
            Dictionary<string, Data> data = res.ResultAs<Dictionary<string, Data>>();
            listBox2.Items.Clear();
                  if (data != null)
                 {
                     foreach (var get in data)
                        {

                            listBox2.Items.Add(get.Key);
                        };
                 }
            listcollection.Clear();
            foreach(string str in listBox2.Items)
            {
                listcollection.Add(str);
            }

        }
       
       
    }
}
