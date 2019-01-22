using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace CompareMSSQL.Services
{
    class MaskLayerForm : Form
    {
        private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
        private double _opacity = 0.8;

        public MaskLayerForm() : this(new Size(500, 500), new Point(0, 0) , 0.8, true, "正在加载...")
        {

        }

        public MaskLayerForm(Size size, Point pt, string msg) : this(size, pt, 0.8, true, msg)
        {

        }

        public MaskLayerForm(Size size, Point pt,  double opacity, bool isShowLoadingImage, string msg)
        {
            this._opacity = opacity;
            this.Opacity = opacity;
            //this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Size = size;
            this.ResumeLayout(false);
            this.Location = pt;
            this.TopLevel = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            PictureBox pictureBox_Loading = new PictureBox();
            //if (isShowLoadingImage)
            //{
                
            //    pictureBox_Loading.BackColor = System.Drawing.Color.White;
            //    pictureBox_Loading.Image = CompareMSSQL.Properties.Resources.loading;
            //    pictureBox_Loading.Name = "picLoading";
            //    pictureBox_Loading.Size = new System.Drawing.Size(48, 48);
            //    pictureBox_Loading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            //    Point Location = new Point(this.Location.X + (this.Width - pictureBox_Loading.Width) / 2, this.Location.Y + (this.Height - pictureBox_Loading.Height) / 2);//居中
            //    pictureBox_Loading.Location = Location;
            //    pictureBox_Loading.Anchor = AnchorStyles.None;
            //    this.Controls.Add(pictureBox_Loading);
            //}

            //if (msg != null && msg != "")
            //{
            //    Label lblMask = new Label();
            //    lblMask.Text = msg;
            //    //lblMask.Size = new System.Drawing.Size();
            //    lblMask.Font = new Font("宋体", 10);
            //    Point Location = new Point(this.Location.X + (this.Width - lblMask.Width) / 2 , this.Location.Y + (this.Height - lblMask.Height) / 2 + pictureBox_Loading.Height / 2 + lblMask.Height / 2);
            //    lblMask.Location = Location;
            //    lblMask.Anchor = AnchorStyles.None;
            //    this.Controls.Add(lblMask);
            //}
        }

        /// <summary>
        /// 自定义绘制窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!((components == null)))
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

    }
}
