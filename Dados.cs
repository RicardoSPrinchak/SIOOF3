using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using GemBox.Spreadsheet;
using System.Diagnostics;

namespace SIOOF3
{
    
    public partial class Dados : Form
    {

        


        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "u8t2EbIOxgPzi0FJKNQmiRUj4kUUSpoepGzRVxJK",
            BasePath = "https://sioof-dd93e-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        public Dados()
        {
            InitializeComponent();
        }
        private async void Dados_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            
            


            if (Form1.nomeS != "")
            {
                tbNome.Text = Form1.nomeS;
                FirebaseResponse response = client.Get("SIOOF3/" + tbNome.Text);
                Data obj = response.ResultAs<Data>();

                tbCod.Text = obj.cod;
                tbSerie.Text = obj.serie;
                tbTurma.Text = obj.turma;
                tbTurno.Text = obj.turno;
                tbMae.Text = obj.mae;
                tbPai.Text = obj.pai;
                tbTel1.Text = obj.tel1;
                tbTel2.Text = obj.tel2;
                tbObs.Text = obj.obs;
                tbOco.Text = obj.oco;
                    

            }
            

        }

        private async void btSalvar_Click(object sender, EventArgs e)
        {
            if(Form1.nomeS != "")
            {
                client.Delete("SIOOF3/" + Form1.nomeS);
            }

            if (tbNome.Text == "") 
            {
                MessageBox.Show("Insira o nome do aluno.");
            }
            else if(tbOco.Text == "")
            {
                MessageBox.Show("Insira uma ocorrência.");
            }
                        
            else
            {


                var data = new Data
                {
                    nome = tbNome.Text,
                    cod = tbCod.Text,
                    serie = tbSerie.Text,
                    turma = tbTurma.Text,
                    turno = tbTurno.Text,
                    mae = tbMae.Text,
                    pai = tbPai.Text,
                    tel1 = tbTel1.Text,
                    tel2 = tbTel2.Text,
                    obs = tbObs.Text,
                    oco = tbOco.Text

                };
                SetResponse response = await client.SetAsync("SIOOF3/" + tbNome.Text, data);
                Data result = response.ResultAs<Data>();
                MessageBox.Show("Os dados do(a) aluno(a) " + result.nome + " foram salvos");

                var frm = (Form1)this.Owner;
                
                this.Close();


            }
        }

        private void Dados_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 frm = new Form1();
            frm.Form1_Load(sender, e);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            ExcelFile workbook = ExcelFile.Load("SIOOFTEMPLATE.xlsx");
            var worksheet = workbook.Worksheets[0];

            worksheet.Cells["B5"].Value = tbCod.Text;
            worksheet.Cells["B6"].Value = tbNome.Text;
            worksheet.Cells["B7"].Value = tbSerie.Text;
            worksheet.Cells["E7"].Value = tbTurma.Text;
            worksheet.Cells["H7"].Value = tbTurno.Text;
            worksheet.Cells["A10"].Value = tbOco.Text;

            /*  PrintOptions printOptions = new PrintOptions();
              printOptions.SelectionType = SelectionType.EntireFile;
              string printerName = null;
              workbook.Print(printerName, printOptions);

            */
            if (!Directory.Exists("C:\\SIOOF"))
            {
                Directory.CreateDirectory("C:\\SIOOF");
            }
            workbook.Save("C:\\SIOOF\\" + tbNome.Text + ".pdf");
            MessageBox.Show("Documento gerado.");
           

            var p = new Process(); 
            p.StartInfo = new ProcessStartInfo("C:\\SIOOF\\" + tbNome.Text + ".pdf") 
            {
                UseShellExecute = true
            };
            p.Start();
        }
        

        
    }
}
