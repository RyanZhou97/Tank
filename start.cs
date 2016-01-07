using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using My90Tank.Properties;
namespace My90Tank
{
    
    public partial class start : Form
    {
        SoundPlayer player = new SoundPlayer();
        public start()
        {
            InitializeComponent();
        }

        private void start_Load(object sender, EventArgs e)
        {
            player.Stream=Resources.start; 
            player.PlayLooping();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Scene.Instance.NumFlag = 1;
            player.Stop();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Scene.Instance.NumFlag = 0;
            player.Stop();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Scene.Instance.NumFlag = 1;
            Scene.Instance.pvp = 1;
            player.Stop();
            this.Close();
        }
    }
}
