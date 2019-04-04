namespace Tetris_Game
{
    partial class FormTetrisGame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTetrisGame));
            this.pnlGameBoard = new Tetris_Game.pnlGameboardDoubleBuffer();
            this.lblLevel = new System.Windows.Forms.Label();
            this.picLevel = new System.Windows.Forms.PictureBox();
            this.picLines = new System.Windows.Forms.PictureBox();
            this.picScore = new System.Windows.Forms.PictureBox();
            this.picHighScore = new System.Windows.Forms.PictureBox();
            this.pnlHold = new Tetris_Game.pnlGameboardDoubleBuffer();
            this.pboxHoldPiece = new System.Windows.Forms.PictureBox();
            this.pnlNext = new System.Windows.Forms.Panel();
            this.pboxBlock3 = new System.Windows.Forms.PictureBox();
            this.pboxBlock2 = new System.Windows.Forms.PictureBox();
            this.pboxBlock1 = new System.Windows.Forms.PictureBox();
            this.lblLines = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblHighScore = new System.Windows.Forms.Label();
            this.picCornerLogo = new System.Windows.Forms.PictureBox();
            this.pnlGameBoard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHighScore)).BeginInit();
            this.pnlHold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxHoldPiece)).BeginInit();
            this.pnlNext.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxBlock3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxBlock2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxBlock1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCornerLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlGameBoard
            // 
            this.pnlGameBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlGameBoard.Location = new System.Drawing.Point(12, 21);
            this.pnlGameBoard.Name = "pnlGameBoard";
            this.pnlGameBoard.Size = new System.Drawing.Size(240, 480);
            this.pnlGameBoard.TabIndex = 0;
            this.pnlGameBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlGameBoard_Paint);
            // 
            // lblLevel
            // 
            this.lblLevel.BackColor = System.Drawing.Color.Transparent;
            this.lblLevel.Font = new System.Drawing.Font("Franklin Gothic Medium", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel.Location = new System.Drawing.Point(439, 218);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblLevel.Size = new System.Drawing.Size(95, 20);
            this.lblLevel.TabIndex = 9;
            this.lblLevel.Text = "999999";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picLevel
            // 
            this.picLevel.BackgroundImage = global::Tetris_Game.Properties.Resources.level;
            this.picLevel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picLevel.ImageLocation = "";
            this.picLevel.Location = new System.Drawing.Point(425, 199);
            this.picLevel.Name = "picLevel";
            this.picLevel.Size = new System.Drawing.Size(120, 53);
            this.picLevel.TabIndex = 8;
            this.picLevel.TabStop = false;
            // 
            // picLines
            // 
            this.picLines.BackgroundImage = global::Tetris_Game.Properties.Resources.lines;
            this.picLines.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picLines.ImageLocation = "";
            this.picLines.Location = new System.Drawing.Point(425, 140);
            this.picLines.Name = "picLines";
            this.picLines.Size = new System.Drawing.Size(120, 53);
            this.picLines.TabIndex = 6;
            this.picLines.TabStop = false;
            // 
            // picScore
            // 
            this.picScore.BackgroundImage = global::Tetris_Game.Properties.Resources.score;
            this.picScore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picScore.ImageLocation = "";
            this.picScore.Location = new System.Drawing.Point(425, 80);
            this.picScore.Name = "picScore";
            this.picScore.Size = new System.Drawing.Size(120, 53);
            this.picScore.TabIndex = 4;
            this.picScore.TabStop = false;
            // 
            // picHighScore
            // 
            this.picHighScore.BackgroundImage = global::Tetris_Game.Properties.Resources.highscore;
            this.picHighScore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picHighScore.ImageLocation = "";
            this.picHighScore.Location = new System.Drawing.Point(425, 21);
            this.picHighScore.Name = "picHighScore";
            this.picHighScore.Size = new System.Drawing.Size(120, 53);
            this.picHighScore.TabIndex = 2;
            this.picHighScore.TabStop = false;
            // 
            // pnlHold
            // 
            this.pnlHold.BackColor = System.Drawing.Color.White;
            this.pnlHold.BackgroundImage = global::Tetris_Game.Properties.Resources.hold;
            this.pnlHold.Controls.Add(this.pboxHoldPiece);
            this.pnlHold.Location = new System.Drawing.Point(279, 372);
            this.pnlHold.Name = "pnlHold";
            this.pnlHold.Size = new System.Drawing.Size(140, 130);
            this.pnlHold.TabIndex = 1;
            // 
            // pboxHoldPiece
            // 
            this.pboxHoldPiece.Location = new System.Drawing.Point(33, 49);
            this.pboxHoldPiece.Name = "pboxHoldPiece";
            this.pboxHoldPiece.Size = new System.Drawing.Size(72, 72);
            this.pboxHoldPiece.TabIndex = 3;
            this.pboxHoldPiece.TabStop = false;
            // 
            // pnlNext
            // 
            this.pnlNext.BackColor = System.Drawing.Color.White;
            this.pnlNext.BackgroundImage = global::Tetris_Game.Properties.Resources.next;
            this.pnlNext.Controls.Add(this.pboxBlock3);
            this.pnlNext.Controls.Add(this.pboxBlock2);
            this.pnlNext.Controls.Add(this.pboxBlock1);
            this.pnlNext.Location = new System.Drawing.Point(279, 21);
            this.pnlNext.Name = "pnlNext";
            this.pnlNext.Size = new System.Drawing.Size(140, 350);
            this.pnlNext.TabIndex = 1;
            // 
            // pboxBlock3
            // 
            this.pboxBlock3.Location = new System.Drawing.Point(35, 256);
            this.pboxBlock3.Name = "pboxBlock3";
            this.pboxBlock3.Size = new System.Drawing.Size(72, 72);
            this.pboxBlock3.TabIndex = 2;
            this.pboxBlock3.TabStop = false;
            // 
            // pboxBlock2
            // 
            this.pboxBlock2.Location = new System.Drawing.Point(35, 178);
            this.pboxBlock2.Name = "pboxBlock2";
            this.pboxBlock2.Size = new System.Drawing.Size(72, 72);
            this.pboxBlock2.TabIndex = 1;
            this.pboxBlock2.TabStop = false;
            // 
            // pboxBlock1
            // 
            this.pboxBlock1.Location = new System.Drawing.Point(35, 100);
            this.pboxBlock1.Name = "pboxBlock1";
            this.pboxBlock1.Size = new System.Drawing.Size(72, 72);
            this.pboxBlock1.TabIndex = 0;
            this.pboxBlock1.TabStop = false;
            // 
            // lblLines
            // 
            this.lblLines.BackColor = System.Drawing.Color.Transparent;
            this.lblLines.Font = new System.Drawing.Font("Franklin Gothic Medium", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLines.Location = new System.Drawing.Point(439, 159);
            this.lblLines.Name = "lblLines";
            this.lblLines.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblLines.Size = new System.Drawing.Size(95, 20);
            this.lblLines.TabIndex = 10;
            this.lblLines.Text = "999999";
            this.lblLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScore
            // 
            this.lblScore.BackColor = System.Drawing.Color.Transparent;
            this.lblScore.Font = new System.Drawing.Font("Franklin Gothic Medium", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScore.Location = new System.Drawing.Point(439, 99);
            this.lblScore.Name = "lblScore";
            this.lblScore.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblScore.Size = new System.Drawing.Size(95, 20);
            this.lblScore.TabIndex = 11;
            this.lblScore.Text = "999999";
            this.lblScore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHighScore
            // 
            this.lblHighScore.BackColor = System.Drawing.Color.Transparent;
            this.lblHighScore.Font = new System.Drawing.Font("Franklin Gothic Medium", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHighScore.Location = new System.Drawing.Point(439, 40);
            this.lblHighScore.Name = "lblHighScore";
            this.lblHighScore.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblHighScore.Size = new System.Drawing.Size(95, 20);
            this.lblHighScore.TabIndex = 12;
            this.lblHighScore.Text = "999999";
            this.lblHighScore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picCornerLogo
            // 
            this.picCornerLogo.BackgroundImage = global::Tetris_Game.Properties.Resources.cornerlogo;
            this.picCornerLogo.Location = new System.Drawing.Point(425, 258);
            this.picCornerLogo.Name = "picCornerLogo";
            this.picCornerLogo.Size = new System.Drawing.Size(120, 244);
            this.picCornerLogo.TabIndex = 13;
            this.picCornerLogo.TabStop = false;
            // 
            // FormTetrisGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(554, 512);
            this.Controls.Add(this.picCornerLogo);
            this.Controls.Add(this.lblHighScore);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.lblLines);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.picLevel);
            this.Controls.Add(this.picLines);
            this.Controls.Add(this.picScore);
            this.Controls.Add(this.picHighScore);
            this.Controls.Add(this.pnlHold);
            this.Controls.Add(this.pnlNext);
            this.Controls.Add(this.pnlGameBoard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormTetrisGame";
            this.Text = "Tetris";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTetrisGame_FormClosed);
            this.Load += new System.EventHandler(this.FormTetrisGame_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormTetrisGame_KeyUp);
            this.pnlGameBoard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHighScore)).EndInit();
            this.pnlHold.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pboxHoldPiece)).EndInit();
            this.pnlNext.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pboxBlock3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxBlock2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxBlock1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCornerLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlNext;
        private System.Windows.Forms.PictureBox pboxBlock3;
        private System.Windows.Forms.PictureBox pboxBlock2;
        private System.Windows.Forms.PictureBox pboxBlock1;
        private pnlGameboardDoubleBuffer pnlGameBoard;
        private System.Windows.Forms.PictureBox pboxHoldPiece;
        private pnlGameboardDoubleBuffer pnlHold;
        private System.Windows.Forms.PictureBox picHighScore;
        private System.Windows.Forms.PictureBox picScore;
        private System.Windows.Forms.PictureBox picLines;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.PictureBox picLevel;
        private System.Windows.Forms.Label lblLines;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblHighScore;
        private System.Windows.Forms.PictureBox picCornerLogo;
    }
}