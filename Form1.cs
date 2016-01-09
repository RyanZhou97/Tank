using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
using My90Tank.Properties;
namespace My90Tank
{
    public partial class Form1 : Form
    {
        private Bitmap canvas = new Bitmap(ParamSetting.Map_Width, ParamSetting.Map_Height);    //获取一个窗口的位图
        private Graphics gra ;                                                                  
        private int num = 0;       //计时器
        public Form1()
        {            
            InitializeComponent();
            this.Width = ParamSetting.Map_Width+6;//两边边框的宽度
            this.Height = ParamSetting.Map_Height+75;//24是窗体中标题头的高度加上边框的高度
            InitGame();
         

            
            
        }
        private void InitGame()   //初始化游戏事件
        {
            //Scene.Instance.AddElement(new P1Player(240, 540, 5, 10,1, DIRECTION.UP));

           InitMap();           //调用初始化地图事件
             this.timer1.Start();//窗体时间
             gra = Graphics.FromImage(canvas);//获得图形对象引用之后，即可绘制对象、给对象着色并显示对象。
             this.pictureBox1.Image = canvas;//获得一个位图，覆盖整个windows窗口

        }
        private void InitMap() //初始化地图
        {
            //用xml
            string mapPath = "maps/" + Scene.Instance.selectedMap;
            XElement map = XElement.Load(mapPath);
            foreach (XElement birrar in map.Element("modules").Elements("module"))
            {
                int x = int.Parse(birrar.Attribute("x").Value);
                int y = int.Parse(birrar.Attribute("y").Value);
                string type = birrar.Attribute("type").Value;

                Module bc = (Module)Factory.getSomething(x, y, type);
                Scene.Instance.AddElement(bc);
            }
            
            
          //  #region 在panel中初始化地图 已被注释
           
          //  Scene.Instance.AddElement(new Symbol(360, 540));
            // #endregion
            
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            //      MessageBox.Show(Scene.Instance.NumFlag.ToString());
            #region 所有模式都需加载
            if (Scene.Instance.NumFlag == 0)
            {
                Scene.Instance.P2Play = null;//把P2取消
            }
            Scene.Instance.CheckState();        //进行杀敌，阻塞，子弹判定
            //更新标签
            if (Scene.Instance.P1Play != null)
                toolStripStatusLabel1.Text = "P1生命数：" + Scene.Instance.P1Play.life.ToString();
            else toolStripStatusLabel1.Text = "P1:已死亡";
            if (Scene.Instance.P2Play != null)
                toolStripStatusLabel2.Text = "P2生命数：" + Scene.Instance.P2Play.life.ToString();
            else toolStripStatusLabel2.Text = "P2:已死亡";
            toolStripStatusLabel3.Text ="已消灭敌人数量: "+Scene.Instance.killnum.ToString();
            //更新生命条
            if (Scene.Instance.boss != null)
                toolStripProgressBar1.Value = Scene.Instance.boss.life;
            if (num % 500 == 0 && Scene.Instance.STAR.Count <= 1)
            {
                //打击声
                Scene.Instance.CreateAnStarAndAdd();

            }
            num++;
            #endregion
            #region 正常模式(单人或双人)需要加载
            if (Scene.Instance.Normalmode == 1)
            {
                if (num % 50 == 0)                  //每50s加一个敌人
                    Scene.Instance.CreateAnEnemyAndAdd();
                if (Scene.Instance.boss == null && Scene.Instance.killnum>=5)       //BOSS的生成
                    Scene.Instance.CreateABoss();
                NomalCheckEnding();
            }
            #endregion
            #region PVP模式需要加载
            if (Scene.Instance.pvp == 1)
            {
                PvPCheckEnding();
            }
            #endregion
            this.pictureBox1.Invalidate();
           

            
        }
        //PVP模式游戏结束判定
        private void PvPCheckEnding()
    {
        this.pictureBox1.Invalidate();
        if (Scene.Instance.P1Play == null)
        {
            timer1.Stop();
            MessageBox.Show("P2 WIN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            this.Close();
        }
        else if (Scene.Instance.P2Play == null)
        {
            timer1.Stop();
            MessageBox.Show("P1 WIN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            this.Close();
        }
    }
        //正常模式游戏结束判定
        private void NomalCheckEnding()
        {
            this.pictureBox1.Invalidate();
            //BOSS死亡游戏结束
            if (Scene.Instance.win == 1)
            {

                timer1.Stop();
                nomalwin a = new nomalwin();
                a.Show();
            }
            //都死亡游戏结束
            if (Scene.Instance.P1Play == null && Scene.Instance.P2Play == null)
            {

                timer1.Stop();
                nomallose a = new nomallose();
                a.Show();
                // MessageBox.Show("You are loser");
               // this.Close();
            }
            //老鹰没了 游戏结束
            if (Scene.Instance.Symbol == null)
            {               
                timer1.Stop();
                nomallose a = new nomallose();
                a.Show();
            //    MessageBox.Show("You are loser");
             //   this.Close();
            
            }
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

        #region 垃圾堆
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }


        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }



    }
}
