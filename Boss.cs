using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using My90Tank.Properties;

namespace My90Tank
{
    public class Boss : Member
    {
        private const int BORNTIME = 10;
        private new int bornTime = 0;
        public static Image born = Resources.born1;
        public Boss(int x, int y, int life, int speed, int power, DIRECTION direct)
            : base(x, y, life, speed, power, direct)
        {
            this.Width = 60;//坦克图的大小
            this.Height = 60;

        }
        private static Image[] imgBoss = new Image[]  //加载一号敌人图片
        {
          Resources.BossD,
          Resources.BossU,
          Resources.BossL,
          Resources.BossR
        };

        public override void Move()
        {
            if (IsBlocked)//撞墙
            {
                ChangeDirection();
            }
            else
            {
                base.ChangePosition();
            }
        }
        public  void Move(DIRECTION a)
        {
                this.direct = a;
                base.ChangePosition();
        }
        public void ChangeDirection()//随机改变方向
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int t = rand.Next(4);
            this.direct = (DIRECTION)t;
        }
        public override void Draw(Graphics g)
        {
            if (bornTime < BORNTIME)
            { //&&坦克是空的
                g.DrawImage(born, this.X, this.Y);
                bornTime++;
                return;
            }

            //当P1，P2在BOSS的四周时,BOSS，会追杀他们，因为我偷懒，所以优先追杀1P
            //修复一个BUG，忘记判断P1PLAY是否为空
            if (Scene.Instance.P1Play!=null&&System.Math.Abs(this.X - Scene.Instance.P1Play.X)<10)
            {
                if (this.Y > Scene.Instance.P1Play.Y)
                    Move(DIRECTION.UP);
                else Move(DIRECTION.DOWN);
            }
            else if (Scene.Instance.P1Play != null && System.Math.Abs(this.Y - Scene.Instance.P1Play.Y) < 10)
            {
                if (this.X > Scene.Instance.P1Play.X)
                    Move(DIRECTION.LEFT);
                else Move(DIRECTION.RIGHT);
            }
                //追杀P2
            else if (Scene.Instance.P2Play != null && System.Math.Abs(this.X - Scene.Instance.P2Play.X) < 10)
            {
                if (this.Y > Scene.Instance.P2Play.Y)
                    Move(DIRECTION.UP);
                else Move(DIRECTION.DOWN);
            }
            else if (Scene.Instance.P2Play != null && System.Math.Abs(this.Y - Scene.Instance.P2Play.Y) < 10)
            {
                if (this.X > Scene.Instance.P2Play.X)
                    Move(DIRECTION.LEFT);
                else Move(DIRECTION.RIGHT);
            }
            else
            {
                Move();
            }
            Fire();
            switch (this.direct)
            {
                case DIRECTION.DOWN: g.DrawImage(imgBoss[0], this.X, this.Y); break;
                case DIRECTION.UP: g.DrawImage(imgBoss[1], this.X, this.Y); break;
                case DIRECTION.LEFT: g.DrawImage(imgBoss[2], this.X, this.Y); break;
                case DIRECTION.RIGHT: g.DrawImage(imgBoss[3], this.X, this.Y); break;
                default: break;
            }
        }
        public void Fire()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            int a = r.Next(50);
            if (a < 40)
                return;
            int xx = this.X, yy = this.Y;
            switch (this.direct)
            {
                case DIRECTION.UP:
                    yy -= this.speed;
                    xx += this.Width / 2;
                    break;
                case DIRECTION.DOWN:
                    yy += this.speed + this.Height;
                    xx += this.Width / 2;
                    break;
                case DIRECTION.LEFT:
                    xx -= this.speed;
                    yy += this.Width / 2;
                    break;
                case DIRECTION.RIGHT:
                    xx += this.speed + this.Width;
                    yy += this.Width / 2;
                    break;
                default:
                    break;
            }
            
            Scene.Instance.AddElement(new Missile(xx, yy, 1, 50, 5, this.direct));
        }

    }
}
