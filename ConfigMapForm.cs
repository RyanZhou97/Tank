//地图编辑
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Xml;
namespace My90Tank
{
    public partial class ConfigMapForm : Form
    {
        Graphics g;
        bool isLoad = false;
        Point xy;
        string liveTypeName = "";
        List<Module> things = new List<Module>();
        public XElement map = new XElement("map", new XElement("modules"));
        string path;
        public ConfigMapForm()
        {
            InitializeComponent();
        }

        private void ConfigMapForm_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            panel1.Size = new Size(ParamSetting.Map_Width + 15, ParamSetting.Map_Height + 40);              //pannel大小
            panel1.Paint += new PaintEventHandler(panel1_Paint);            //pannel画格子
            this.Size = new Size(ParamSetting.Map_Width + 70 + panel1.Location.X, ParamSetting.Map_Height + 100);   //窗口大小
            this.BackColor = Color.Black;   //背景黑色
            XElement types = XElement.Parse(Properties.Resources.Config);//调用Linkq
            ImageList imageList = new ImageList();      
            treeView1.ImageList = imageList;
            treeView1.ImageList.ImageSize = new Size(60, 60);
            //把图片加入树中
            foreach (XElement type in types.Element("types").Elements())
            {
                if (type.Attribute("belong").Value == "")
                {
                    string name = type.Attribute("name").Value;
                    Image image = new Bitmap(60, 60);
                    Graphics g = Graphics.FromImage(image);
                    Module t = Factory.getSomething(1, 1, name);
                    t.Draw(g);
                    imageList.Images.Add(name, image);
                }
            }
            // //把文字加入树中
            foreach (XElement type in types.Element("types").Elements())
            {
                if (type.Attribute("belong").Value == "")
                {
                    TreeNode tn = new TreeNode(type.Attribute("name").Value);
                    tn.ImageKey = type.Attribute("name").Value;
                    tn.SelectedImageKey = type.Attribute("name").Value;
                    treeView1.Nodes.Add(tn);
                    //listBox1.Items.Add(tn);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!isLoad)
                path = "maps/" + textBox1.Text;
            else
                path = textBox1.Text;
            save(path);

            MessageBox.Show("保存成功!");
        }
        private void save(string path)
        {
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            XmlTextWriter xtw = new XmlTextWriter(path, encoding);
            xtw.Formatting = Formatting.Indented;
            map.WriteTo(xtw);
            xtw.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "maps\\";
            openFileDialog.Filter = "地图文件|*.map|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string mapPath = openFileDialog.FileName;
                path = mapPath;
                textBox1.Text = mapPath;
                //mapPath
                XElement map = XElement.Load(mapPath);
                this.map = map;
                things.Clear();
                build(map);
                this.panel1.Invalidate();

            }
            isLoad = true;
        }
        void build(XElement map)
        {

            foreach (XElement birrar in map.Element("modules").Elements("module"))
            {
                int x = int.Parse(birrar.Attribute("x").Value);
                int y = int.Parse(birrar.Attribute("y").Value);
                string type = birrar.Attribute("type").Value;

                Module bc = (Module)Factory.getSomething(x, y, type);
                things.Add(bc);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            liveTypeName = treeView1.SelectedNode.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
      map = new XElement("map", new XElement("modules"));
            things = new List<Module>();
            this.panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            foreach (Module t in things)
            {
                t.Draw(g);
            }
            for (int i = 0; i < ParamSetting.Map_Height; i++)
            {
                g.DrawLine(Pens.White, new Point(0, i * 60), new Point(ParamSetting.Map_Width, i * 60));
            }

            for (int j = 0; j < ParamSetting.Map_Width; j++)
            {
                g.DrawLine(Pens.White, new Point(j * 60, 0), new Point(j * 60, ParamSetting.Map_Height));
            }

        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            xy = (new Point(e.X / 60 + 1, e.Y / 60 + 1));
            xy.X = (xy.X - 1) * 60; xy.Y = (xy.Y - 1) * 60;
            map.Element("modules").Add(new XElement("module",
            new XAttribute("x", xy.X),
            new XAttribute("y", xy.Y),
            new XAttribute("type", liveTypeName)));
            Module t = Factory.getSomething(xy.X, xy.Y, liveTypeName);
            things.Add(t);
            panel1.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }


    }
}
