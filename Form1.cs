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
    public partial class Form1 : Form
    {
        private Bitmap canvas = new Bitmap(ParamSetting.Map_Width, ParamSetting.Map_Height);
        private Graphics gra ;
        private int num = 0;
        //下面这个变量没必要，直接用EnemyList.Count就好
        // public int sumenemy = 0;//敌人总数 ，用来判定是否产生BOSS 或 在外面显示
        public Form1()
        {            
            InitializeComponent();
            this.Width = ParamSetting.Map_Width+6;//两边边框的宽度
            this.Height = ParamSetting.Map_Height+24;//24是窗体中标题头的高度加上边框的高度
            InitGame();
            this.timer1.Start();//窗体时间
            gra = Graphics.FromImage(canvas);//获得图形对象引用之后，即可绘制对象、给对象着色并显示对象。
            this.pictureBox1.Image = canvas;//获得一个位图，覆盖整个windows窗口
            
            
        }

        private void InitGame()   //初始化游戏事件
        {
            //Scene.Instance.AddElement(new P1Player(240, 540, 5, 10,1, DIRECTION.UP));
            InitMap();           //调用初始化地图事件

        }

        private void InitMap() //初始化地图
        {
             // Start();  //
            #region 在panel中初始化地图

            Scene.Instance.AddElement(new Wall(60, 60));
            Scene.Instance.AddElement(new Wall(420, 60));
            Scene.Instance.AddElement(new Wall(540, 60));
            Scene.Instance.AddElement(new Wall(660, 60));
            Scene.Instance.AddElement(new Wall(360, 120));
            Scene.Instance.AddElement(new Wall(420, 120));
            Scene.Instance.AddElement(new Wall(660, 120));
            Scene.Instance.AddElement(new Wall(180, 180));
            Scene.Instance.AddElement(new Wall(180, 240));
            Scene.Instance.AddElement(new Wall(300, 300));
            Scene.Instance.AddElement(new Wall(600, 300));
            Scene.Instance.AddElement(new Wall(720, 300));
            Scene.Instance.AddElement(new Wall(60, 360));
            Scene.Instance.AddElement(new Wall(120, 360));
            Scene.Instance.AddElement(new Wall(180, 360));
            Scene.Instance.AddElement(new Wall(600, 360));
            Scene.Instance.AddElement(new Wall(720, 360));
            Scene.Instance.AddElement(new Wall(300, 420));
            Scene.Instance.AddElement(new Wall(420, 420));
            Scene.Instance.AddElement(new Wall(540, 420));
            Scene.Instance.AddElement(new Wall(600, 420));
            Scene.Instance.AddElement(new Wall(660, 420));
            Scene.Instance.AddElement(new Wall(60, 480));
            Scene.Instance.AddElement(new Wall(300, 480));
            Scene.Instance.AddElement(new Wall(420, 480));
            Scene.Instance.AddElement(new Wall(660, 480));
            Scene.Instance.AddElement(new Wall(60, 540));
            Scene.Instance.AddElement(new Wall(180, 540));
            Scene.Instance.AddElement(new Wall(300, 540));
            Scene.Instance.AddElement(new Wall(420, 540));
            Scene.Instance.AddElement(new Wall(540, 540));
            Scene.Instance.AddElement(new Wall(660, 540));
            Scene.Instance.AddElement(new Wall(360, 500));
            //Scene.Instance.AddElement(new Wall(60, 600));
            //Scene.Instance.AddElement(new Wall(180, 600));
            //Scene.Instance.AddElement(new Wall(660, 600));
            //Scene.Instance.AddElement(new Wall(60, 660));
            //Scene.Instance.AddElement(new Wall(300, 660));
            //Scene.Instance.AddElement(new Wall(360, 660));
            //Scene.Instance.AddElement(new Wall(420, 660));
            //Scene.Instance.AddElement(new Wall(540, 660));
            //Scene.Instance.AddElement(new Wall(660, 660));
            //Scene.Instance.AddElement(new Wall(60, 720));
            //Scene.Instance.AddElement(new Wall(180, 720));
            //Scene.Instance.AddElement(new Wall(300, 720));
            //Scene.Instance.AddElement(new Wall(420, 720));
            //Scene.Instance.AddElement(new Wall(540, 720));
            //Scene.Instance.AddElement(new Wall(600, 720));
            //Scene.Instance.AddElement(new Wall(660, 720));


            // 实例化水
            Scene.Instance.AddElement(new Water(600, 300));
            Scene.Instance.AddElement(new Water(660, 300));
            Scene.Instance.AddElement(new Water(720, 300));



            // 实例化草地
            Scene.Instance.AddElement(new Grass(0, 240));
            Scene.Instance.AddElement(new Grass(0, 300));
            Scene.Instance.AddElement(new Grass(60, 300));
            Scene.Instance.AddElement(new Grass(240, 360));
            Scene.Instance.AddElement(new Grass(300, 360));
            Scene.Instance.AddElement(new Grass(360, 360));



            //实例化钢块
            Scene.Instance.AddElement(new Steel(180, 0));
            Scene.Instance.AddElement(new Steel(420, 0));
            Scene.Instance.AddElement(new Steel(180, 60));
            Scene.Instance.AddElement(new Steel(600, 120));
            Scene.Instance.AddElement(new Steel(540, 180));
            Scene.Instance.AddElement(new Steel(360, 240));
            Scene.Instance.AddElement(new Steel(480, 300));
            Scene.Instance.AddElement(new Steel(420, 360));
            Scene.Instance.AddElement(new Steel(660, 360));
            Scene.Instance.AddElement(new Steel(180, 420));
            //Scene.Instance.AddElement(new Steel(480, 480));
            //Scene.Instance.AddElement(new Steel(0, 480));
            //Scene.Instance.AddElement(new Steel(180, 480));
            //Scene.Instance.AddElement(new Steel(360, 540));
            //Scene.Instance.AddElement(new Steel(420, 360));

            ////实例化大本营
            //Scene.Instance.AddElement(new Symbol(360, 540));
            #endregion
                
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
         
      //      MessageBox.Show(Scene.Instance.NumFlag.ToString());
            if (Scene.Instance.NumFlag == 0)
            {
                Scene.Instance.P2Play = null;//把P2取消
            }
            Scene.Instance.CheckState();        //进行杀敌，阻塞，子弹判定
            if (num % 50 == 0&&Scene.Instance.pvp==0)                  //每50s加一个敌人
                Scene.Instance.CreateAnEnemyAndAdd();
            if (num % 500 == 0 && Scene.Instance.STAR.Count <= 1)
            {
                //打击声
                Scene.Instance.CreateAnStarAndAdd();

            }
            num++;
            //更新标签
            if (Scene.Instance.P1Play != null)
                label2.Text = Scene.Instance.P1Play.life.ToString();
            else label2.Text = "已死亡";
            if (Scene.Instance.P2Play != null)
                label3.Text = Scene.Instance.P2Play.life.ToString();
            else label3.Text = "已死亡";
            label6.Text = Scene.Instance.killnum.ToString();
          //更新生命条
            if(Scene.Instance.boss!=null)
            progressBar1.Value =Scene.Instance.boss.life;
            
            //游戏结束判定
            //BOSS死亡游戏结束
            if(Scene.Instance.win==1)
            {

                timer1.Stop();
                MessageBox.Show("You are winner");
                this.Close();
            }
            //都死亡游戏结束
            if (Scene.Instance.P1Play == null && Scene.Instance.P2Play == null)
            {
                
                timer1.Stop();
                MessageBox.Show("You are loser");
                this.Close();
            }



            this.pictureBox1.Invalidate();
           

            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Scene.Instance.P1Play != null)
            {
                Scene.Instance.P1Play.KeyDown(e);
            }
            if (Scene.Instance.P2Play != null)
            {
                Scene.Instance.P2Play.KeyDown(e);
            }
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (Scene.Instance.P1Play != null)
            {
                Scene.Instance.P1Play.KeyUp(e);
            }
            if (Scene.Instance.P2Play != null)
            {
                Scene.Instance.P2Play.KeyUp(e);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            this.gra.Clear(Color.Black);
            Scene.Instance.Draw(gra);
            //e.Graphics.DrawImage(canvas,0,0);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
           
        }

   
    }
}
