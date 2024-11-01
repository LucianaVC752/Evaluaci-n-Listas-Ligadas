using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Forms;

namespace Paint
{
    public class ArtanPictureBox : PictureBox
    {
        // Fields
        private int borderSize = 2;
        private int borderRadius = 40;
        private float gradientAngle = 90F;
        private Color borderGradientTop = Color.DodgerBlue;
        private Color borderGradientBottom = Color.CadetBlue;

        // Constructor
        public ArtanPictureBox()
        {
            SizeMode = PictureBoxSizeMode.StretchImage;
            Size = new Size(120, 120);
        }

        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; Invalidate(); }
        }

        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; Invalidate(); }
        }

        public float GradientAngle
        {
            get { return gradientAngle; }
            set { gradientAngle = value; Invalidate(); }
        }

        public Color BorderGradientTop
        {
            get { return borderGradientTop; }
            set { borderGradientTop = value; Invalidate(); }
        }

        public Color BorderGradientBottom
        {
            get { return borderGradientBottom; }
            set { borderGradientBottom = value; Invalidate(); }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            var graphics = pe.Graphics;
            var rectangleSmooth = Rectangle.Inflate(ClientRectangle, -1, -1);
            var rectangleBorder = Rectangle.Inflate(rectangleSmooth, -borderSize, -borderSize);
            int smoothSize = borderSize > 0 ? borderSize * 3 : 1;

            using (var borderGradientBrush = new LinearGradientBrush(rectangleBorder, borderGradientTop, borderGradientBottom, gradientAngle))
            using (var pathRegion = new GraphicsPath())
            using (var penSmooth = new Pen(Parent.BackColor, smoothSize))
            using (var penBorder = new Pen(borderGradientBrush, borderSize))
            {
                // Dibujar el área de recorte redondeada
                pathRegion.AddEllipse(rectangleSmooth);
                Region = new Region(pathRegion);

                // Suavizado para evitar bordes duros
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.DrawEllipse(penSmooth, rectangleSmooth);

                // Dibujar borde solo si `borderSize` es mayor a 0
                if (borderSize > 0)
                    graphics.DrawEllipse(penBorder, rectangleBorder);
            }
        }
    }
}
