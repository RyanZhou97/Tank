using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using My90Tank.Properties;
namespace My90Tank
{
    public class  Star :Module
    {
        public static Image img = My90Tank.Properties.Resources.star;
        public static Image[] bornImages = new Image[] { Resources.born1, Resources.born2, Resources.born3, Resources.born4 };
        private int Borntime = 16;
        private int borntime = 0;
        public Star(int x,int y):base(x,y,img) {
            this.Width = img.Width;
            this.Height = img.Height;
        }
        public override void Draw(Graphics g)
        {
            if (borntime < Borntime) {
                switch (borntime % 8)
                {
                    case 0:
                    case 1:
                        g.DrawImage(bornImages[0], this.X, this.Y);
                        break;
                    case 2:
                    case 3:
                        g.DrawImage(bornImages[1], this.X, this.Y);
                        break;
                    case 4:
                    case 5:
                        g.DrawImage(bornImages[2], this.X, this.Y);
                        break;
                    case 6:
                    case 7:
                        g.DrawImage(bornImages[3], this.X, this.Y);
                        break;
                }
                borntime++;
                return;
            }
            g.DrawImage(img, this.X, this.Y);
        }
    }
}
