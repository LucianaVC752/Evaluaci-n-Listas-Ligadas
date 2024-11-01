using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        bool paint = false;
        int index = 1;
        int x, y, sX, sY, cX, cY;
        Color colorP;
        Point pointX, pointY;
        Bitmap bitmapN;
        Graphics graphics;
        Pen pen = new Pen(Color.Black, 2);  
        Pen eraser = new Pen(Color.White, 4);
        ColorDialog colorDialog = new ColorDialog();
        private ImageLinkedList imageList = new ImageLinkedList();
        private bool isModified = false;


        static Point SetPoint(PictureBox pictureBox, Point point)
        {
            float pX = 1f * pictureBox.Image.Width / pictureBox.Width;
            float pY = 1f * pictureBox.Image.Height / pictureBox.Height;

            return new Point((int)(point.X* pX),(int)(point.Y * pY));
        }
        private void Validate(Bitmap bitmap,Stack <Point> pointStack, int x,int y,Color ColorNew, Color colorOld)
        {
            Color cx = bitmap.GetPixel(x, y);
            if(cx == colorOld)
            {
                pointStack.Push(new Point(x, y));
                bitmap.SetPixel(x, y,ColorNew);
            }
        }

        public void FillUp(Bitmap bitmap, int x, int y, Color newColor)
        {
            Color oldColor = bitmap.GetPixel(x, y);
            Stack <Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bitmap.SetPixel(x,y,newColor);
            if (oldColor == newColor) return;

            while (pixel.Count>0)
            {
                Point point = (Point)pixel.Pop();
                if(point.X>0 && point.Y>0 && point.X < bitmap.Width-1&& point.Y < bitmap.Height -1)
                {
                    Validate(bitmap, pixel, point.X-1,point.Y,  newColor,oldColor);
                    Validate(bitmap, pixel, point.X,point.Y-1, newColor, oldColor);
                    Validate(bitmap, pixel, point.X+1, point.Y, newColor, oldColor);
                    Validate(bitmap, pixel, point.X,point.Y+1, newColor, oldColor);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            bitmapN = new Bitmap(Pic.Width,Pic.Height);
            graphics = Graphics.FromImage(bitmapN);
            graphics.Clear(Color.White);
            Pic.Image = bitmapN;
            BtnPencil.BackColor = BtnAncho1.BackColor = Color.Red;
        }

      

        private void BtnPencilAnchor_Click(object sender, EventArgs e)
        {
            foreach (var btn in Panel1.Controls.OfType<ArtanButton>())
            {
                btn.BackColor = Color.WhiteSmoke;
                ArtanButton artanButton = (ArtanButton)sender;
                artanButton.BackColor = Color.Red;
                pen.Width = eraser.Width =Convert.ToInt32(artanButton.Tag);
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            foreach (var btn in tableLayoutPanel1.Controls.OfType<ArtanButton>())
            {
                btn.BackColor = Color.WhiteSmoke;
                ArtanButton artanButton = (ArtanButton)sender;
                artanButton.BackColor = Color.Red;
                index= Convert.ToInt32(artanButton.Tag);
            }
        }

        private void Pic_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            pointY = e.Location;
            cX = e.X;
            cY = e.Y;
        }

        private void Pic_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            sX = x - cX;
            sY = y - cY;

            if (index == 5)
                graphics.DrawLine(pen, cX, cY, x, y);
            if (index == 6)
                graphics.DrawRectangle(pen, cX, cY,sX,sY);
            if (index == 7)
                graphics.DrawEllipse(pen, cX, cY, sX, sY);

        }

        private void Pic_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = SetPoint(Pic, e.Location);
            if (index == 3)
                FillUp(bitmapN,point.X,point.Y,colorP);
            if (index == 4)
              colorP = pen.Color = PenColor.BackColor =((Bitmap) Pic.Image).GetPixel(point.X,point.Y);
        }

        private void BtnCircle_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Pic_Paint(object sender, PaintEventArgs e)
        {
           Graphics graphicsPaint = e.Graphics;
            if (paint)
            {
                if (index == 5)
                    graphicsPaint.DrawLine(pen, cX, cY, x, y);
                if (index == 6)
                    graphicsPaint.DrawRectangle(pen, cX, cY, sX, sY);
                if (index == 7)
                    graphicsPaint.DrawEllipse(pen, cX, cY, sX, sY);              
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var saveFile = new SaveFileDialog();
            saveFile.Filter = "Image(*.jpg)|*.jpg|(*.*)|*.*";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                //Bitmap btm = bitmapN.Clone(new Rectangle(0,0,Pic.Width,Pic.Height),bitmapN.PixelFormat);
                bitmapN.Save(saveFile.FileName,ImageFormat.Jpeg);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {

            graphics.Clear(Color.White);
            Pic.Image = bitmapN;
            foreach (var btn in tableLayoutPanel1.Controls.OfType<ArtanButton>())
                btn.BackColor = Color.WhiteSmoke;
            foreach (var btn in tableLayoutPanel1.Controls.OfType<ArtanButton>())
                btn.BackColor = Color.WhiteSmoke;
            BtnPencil.BackColor = BtnAncho1.BackColor = Color.LightGreen;
            pen.Width = eraser.Width = 2;
            index = 1;

        }

        private void BtnAbrir_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                Bitmap openedImage = new Bitmap(openFile.FileName);
                imageList.AddImage(openedImage);

                Pic.Image = imageList.GetLatestImage();
                bitmapN = new Bitmap(Pic.Image);
                graphics = Graphics.FromImage(bitmapN);
            }
        }

        private void Pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                if(index == 1)
                {
                    pointX = e.Location;
                    graphics.DrawLine(pen,pointX,pointY);
                    pointY = pointX;
                }
                if (index == 2)
                {
                    pointX = e.Location;
                    graphics.DrawLine(eraser, pointX, pointY);
                    pointY = pointX;
                }
                
            }
            Pic.Refresh();
            x = e.X;
            y = e.Y;
            sX = e.X-cX;
            sY = e.Y-cY;

        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            ArtanPictureBox artanPictureBox = (ArtanPictureBox)sender;
            PenColor.BackColor= pen.Color = colorP = artanPictureBox.BackColor;
        }

        private void BtnColorSet_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            colorP = PenColor.BackColor = pen.Color = colorDialog.Color;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else 
                this.WindowState = FormWindowState.Normal;
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Minimized;
        }
    }
}
