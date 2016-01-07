using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using My90Tank;
using My90Tank.Properties;
using System.Media;
namespace My90Tank
{
    public class Scene          //描述场景
    {
        public static int MAX_ENEMY_NUMBER = 20;
        private P1Player play1 = new P1Player(240, 540, 3, 6, 1, DIRECTION.UP);
       //private P2Player play2 = new P2Player(480, 540, 3, 6, 1, DIRECTION.UP);
        private P2Player play2 = new P2Player(480, 540, 3, 6, 1, DIRECTION.UP);
        private Symbol symbol = new Symbol(360, 560);
        private static int numflag=0;               //单双人模式
        private int bossflag = 0;                   //BOSS
        public int killnum =0;
        public int pvp=0;
        public Boss boss;
        private List<Water> waterList = new List<Water>();//水
        private List<Wall> wallList = new List<Wall>();   //墙
        private List<Grass> grassList = new List<Grass>();//草
        private List<Steel> steelList = new List<Steel>();//钢
        private List<Enemy> enemyList = new List<Enemy>();//敌人
        private List<Missile> missileList = new List<Missile>();//子弹
        private List<Missile> deadMissiles = new List<Missile>();
        private List<Star> starList = new List<Star>(); 
        public int NumFlag     
        {
            get
            {
                return numflag;
            }
            set
            {
                numflag = value;
            }
        }
        public P1Player P1Play//对象play1的属性
        {
            get
            {
                return play1;
            }
            set
            {
                play1 = value;
            }
        }
        //加入的play2的属性
        //胜利标志
        public int win = 0;
        public P2Player P2Play
        {
            get
            {
                return play2;
            }
            set
            {
                play2 = value;
            }
        }
        public List<Star> STAR
        {
            get
            {
                return starList;
            }
        }

        private static Scene instance = null;

        public static Scene Instance    //返回一个Scene对象
        {
            get
            {
                if (instance == null)
                {
                    instance = new Scene();                    
                }
                return instance;
            }
        }

        private Scene()
        {
            //if(pvp==0)
            //this.CreateAnEnemyAndAdd();
            if (pvp == 0)
            this.CreateAnStarAndAdd();
        }

        public void CreateAnEnemyAndAdd()   //加入敌人
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            int t = random.Next(3);
            EnemyType type = (EnemyType)t;
            int p = random.Next(3);
            int birthX = 0;
            int birthY = 0;
            if (p == 0)
            {
                birthX = 0;
            }
            else if (p == 1)
            {
                birthX = 60 * 6;
            }
            else
                birthX = 12 * 60;

            Enemy enemy = new Enemy(type, birthX, birthY, DIRECTION.DOWN);
            this.enemyList.Add(enemy);
        }

        public void AddElement(Element ele)
        {
            if (ele is P1Player)
            {
                this.play1 = ele as P1Player;
            }
            else if( ele is Wall)
            {
                this.wallList.Add(ele as Wall);
            }
            else if (ele is Grass)
            {
                this.grassList.Add(ele as Grass);
            }
            else if( ele is Steel)
            {
                this.steelList.Add(ele as Steel);
            }
            else if (ele is Water)
            {
                this.waterList.Add(ele as Water);
            }
            else if ( ele is Missile)
            {
                this.missileList.Add(ele as Missile);
            }
            

        }

        public void RemoveElement(Element e)
        {
            if (e is Wall)
            {
                this.wallList.Remove( e as Wall);
            }
            else if (e is Enemy)
            {
                //this..Remove(e as Enemy);
            }
            else if (e is Steel)
            {
                this.steelList.Remove(e as Steel);
            }          

        }

        public void Draw(Graphics g)
        {
            if (play1 != null)
            {
                this.P1Play.Draw(g);
            }
            if (play2 != null && numflag == 1)
            {
                this.P2Play.Draw(g);
            }
            foreach (Grass grass in grassList)
                grass.Draw(g);
            foreach (Wall wall in wallList)
                wall.Draw(g);
            foreach (Water water in waterList)
                water.Draw(g);
            foreach (Steel steel in steelList)
                steel.Draw(g);
            foreach (Missile missile in missileList)
                missile.Draw(g);
            foreach (Enemy enemy in enemyList)
                enemy.Draw(g);
            foreach (Star star in starList)
                star.Draw(g);
            if (this.symbol != null&&pvp==0)
                symbol.Draw(g);
            if (this.boss != null)
                boss.Draw(g);

            
        }
        public void CreateAnStarAndAdd()
        {
            //道具出现的声音。
            SoundPlayer addsound = new SoundPlayer(Resources.add);
            addsound.Play();
            Random random = new Random((int)DateTime.Now.Ticks);//系统自动选取当前时前作随机种子
            int t = random.Next(3);
            int X = 0;
            int Y = 0;  
            switch (t)
            {
                case 0: X = 240; Y = 400;
                    break;
                case 1: X = 480; Y = 200;
                    break;
                case 2: X = 0; Y = 540;
                    break;
            }
            Star star = new Star(X, Y);
            this.starList.Add(star);
        }
        public void CheckState()
        {
         //   List<Missile> deadMissile = new List<Missile>();
            if (play1 != null)
            {
                //play1.Move();
                CheckBlock(P1Play);
            }
            if (play2 != null)
            {
                //play2.Move();
                CheckBlock(P2Play);
            }
            foreach (Enemy enemy in enemyList)            
                CheckBlock(enemy);
            foreach (Missile missile in missileList)
                CheckDeadAndRemove(missile);
            if(boss!=null)
            CheckBlock(boss);
            for (int i = 0; i < deadMissiles.Count; i++)
            {
                missileList.Remove(deadMissiles[i]);
            }
         
            //P1复活

            //P2复活

        }

        public void CreateABoss()
        {
            //血条

            Random random = new Random((int)DateTime.Now.Ticks);//系统自动选取当前时前作随机种子
            int t = random.Next(3);
            int X = 0;
            int Y = 0;
            switch (t)
            {
                case 0: X = 240; Y = 400;
                    break;
                case 1: X = 480; Y = 200;
                    break;
                case 2: X = 0; Y = 540;
                    break;
            }
            
            boss = new Boss(X, Y,20,10,5,DIRECTION.DOWN);
        }
        //确定某个坦克是否处于堵塞状态（可以是敌人或者自己的坦克）
        public void CheckBlock(Member m)
        {

            int i = 0;
            Rectangle rect = m.GetRectangle();
            for (i = 0; i < wallList.Count; i++)
            {
                if (rect.IntersectsWith(wallList[i].GetRectangle()))
                {
                    m.IsBlocked = true;
                    return;
                }
            }
            for (i = 0; i < waterList.Count; i++)
            {
                if (rect.IntersectsWith(waterList[i].GetRectangle()))
                {
                    m.IsBlocked = true;
                    return;
                }
            }
            for (i = 0; i < steelList.Count; i++)
            {
                if (rect.IntersectsWith(steelList[i].GetRectangle()))
                {
                    m.IsBlocked = true;
                    return;
                }
            }
            if (m is P1Player)
            {
                for (i = 0; i < starList.Count; i++)
                {
                    if (rect.IntersectsWith(starList[i].GetRectangle()))
                    {
                        starList.RemoveAt(i);
                        m.power +=1;
                    }
                }
                for (i = 0; i < enemyList.Count; i++)
                {
                    if (rect.IntersectsWith(enemyList[i].GetRectangle()))
                    {
                        m.IsBlocked = true;
                        return;
                    }
                }

            }
            if (m is P2Player)
            {
                 for (i = 0; i < starList.Count; i++)
                {
                    if (rect.IntersectsWith(starList[i].GetRectangle()))
                    {
                        starList.RemoveAt(i);
                        m.power +=1;
                    }
                }
                for (i = 0; i < enemyList.Count; i++)
                {
                    if (rect.IntersectsWith(enemyList[i].GetRectangle()))
                    {
                        m.IsBlocked = true;
                        return;
                    }
                }
            }
            if (m is Enemy)
            {
                //修复BUG 判断P1Play 是否为NULL
                if (P1Play != null && rect.IntersectsWith(P1Play.GetRectangle()))
                {
                    m.IsBlocked = true;
                    return;
                }
                if (P2Play != null && rect.IntersectsWith(P2Play.GetRectangle()))
                {
                    m.IsBlocked = true;
                    return;
                }

         }
         /* BOSS太智能 消除他和P1 P2体积碰撞
            if (m is Boss)
            {
                //修复BUG 判断P1Play 是否为NULL
                if (P1Play!=null&&rect.IntersectsWith(P1Play.GetRectangle()))
                {
                    m.IsBlocked = true;
                    return;
                }
                if (P2Play != null && rect.IntersectsWith(P2Play.GetRectangle()))
                {
                    m.IsBlocked = true;
                    return;
                }
            }
            */
            if (rect.Right >= ParamSetting.Map_Width || rect.Left < 0 || rect.Bottom > ParamSetting.Map_Height || rect.Top < 0)
            {
                m.IsBlocked = true;
                return;
            }
            m.IsBlocked = false;
        }

        //用missile对象确定场景中目标的生命值小于等于0，并删除掉该对象
        public void CheckDeadAndRemove(Missile m)
        {
           
            int i = 0;
            //打中普通墙壁
            for (i = 0; i < wallList.Count; i++)
            {
                if (m.GetRectangle().IntersectsWith(wallList[i].GetRectangle()))    //墙壁与子弹是否相交
                {
                    deadMissiles.Add(m);
                    wallList.RemoveAt(i);
                    return;
                }
            }
            //打中钢墙
            for (i = 0; i < steelList.Count; i++)
            {
                if (m.GetRectangle().IntersectsWith(steelList[i].GetRectangle()))
                {
                    deadMissiles.Add(m);
                    if (m.power > 2)
                    {
                        steelList.RemoveAt(i);
                        i--;
                    }
                    return;
                }
            }
            
            //子弹打中P1Play
            if(P1Play!=null)
            if (m.GetRectangle().IntersectsWith(P1Play.GetRectangle()))
            {
                //打击声
                SoundPlayer hitsound = new SoundPlayer(Resources.hit);
                hitsound.Play();
                //子弹移除
                deadMissiles.Add(m);
                //被击中扣血复活，当前版本只有一滴血直接死亡 复活
                P1Play.life--;
                if (P1Play.life >= 0)   //还有命就复活，不然狗带吧
                {
                    P1Play.bornTime = 0;
                    P1Play.X=240;
                    P1Play.Y = 540;
                 }
                else                    //狗带
                {
                    P1Play = null;
                }

                //  P1Play.
                //MessageBox.Show("Game Over!");
                return;
            }
             //子弹打中P2Play
            if(P2Play!=null)
            if (m.GetRectangle().IntersectsWith(P2Play.GetRectangle()))
            {
                //打击声
                SoundPlayer hitsound = new SoundPlayer(Resources.hit);
                hitsound.Play();
                //子弹移除
                deadMissiles.Add(m);
                //被击中扣血复活，当前版本只有一滴血直接死亡 复活
                P2Play.life--;
                if (P2Play.life >= 0)   //还有命就复活，不然狗带吧
                {
                    P2Play.bornTime = 0;
                    P2Play.X = 480;
                    P2Play.Y = 540;
                }
                else                    //狗带
                {
                    P2Play = null;
                }
              //  P2Play.
                //MessageBox.Show("Game Over!");
                return;
            }
            //消灭敌人
            for (i = 0; i < enemyList.Count; i++)
            {
                if (m.GetRectangle().IntersectsWith(enemyList[i].GetRectangle()))
                {
                    //打击声
                    SoundPlayer hitsound = new SoundPlayer(Resources.hit);
                    hitsound.Play();
                    //移除敌人，当前小兵默认一点生命值，直接移除
                    enemyList.RemoveAt(i);
                    //死亡音效
                    SoundPlayer deadsound = new SoundPlayer(Resources.blast);
                    deadsound.Play();
                    
                    i--;
                    killnum++;
                    deadMissiles.Add(m);
                    return;
                }
            }
            //打BOSS
            if (boss != null)
            {
                if (m.GetRectangle().IntersectsWith(boss.GetRectangle()))
                {
                    //打击声
                    SoundPlayer hitsound = new SoundPlayer(Resources.hit);
                    hitsound.Play();
                    boss.life-=m.power;//减少子弹的伤害
                    if (boss.life <= 0) //BOSS死亡
                    {
                        //死亡音效
                        SoundPlayer deadsound1 = new SoundPlayer(Resources.blast);
                        deadsound1.Play();
                        boss = null;
                        //杀死BOSS即胜利
                        win = 1;
                    }    
                    deadMissiles.Add(m);
                    return;
                }
            }
            //子弹进入边缘
            if (m.X == 0 || m.Y == 0 || m.X >= ParamSetting.Map_Width || m.Y >= ParamSetting.Map_Height)
                deadMissiles.Add(m);
            //满足条件召唤BOSS
            if (boss == null && pvp==0)
            CreateABoss();
         //   if (enemyList.Count == 0)
        //    {
       //         MessageBox.Show("You Win!");
                
          //  }
            //m.IsBlocked = false;
           
            return ;
        }
    }
}
