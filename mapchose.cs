using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace My90Tank
{
    public partial class mapchose : Form
    {
        public mapchose()
        {
            InitializeComponent();
        }

        private void mapchose_Load(object sender, EventArgs e)
        {
            
            DirectoryInfo directory = new DirectoryInfo("maps");
       
            listBox1.Items.Clear();
            foreach (FileInfo file in directory.GetFiles())
            {
                Scene.Instance.maps.Add(file.Name);
                listBox1.Items.Add(file.Name);
                listBox1.Text = file.Name;
                
            }
        //    listBox1.SelectedIndex = 0;
            if (Scene.Instance.selectedMap == null)
            {
                listBox1.SelectedIndex = 0;
                Scene.Instance.selectedMap = Scene.Instance.maps[listBox1.SelectedIndex];
            }  
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // MessageBox.Show(listBox1.SelectedIndex.ToString());
           Scene.Instance.selectedMap = Scene.Instance.maps[listBox1.SelectedIndex];
        }
    }
}
