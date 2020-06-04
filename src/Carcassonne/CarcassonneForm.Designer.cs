using System.ComponentModel;
using System.Windows.Forms;

namespace Carcassonne
{
    partial class CarcassonneForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.lstTiles = new System.Windows.Forms.ListBox();
            this.btnCCW = new System.Windows.Forms.Button();
            this.btnCW = new System.Windows.Forms.Button();
            this.pnlTile = new System.Windows.Forms.Panel();
            this.lblWest = new System.Windows.Forms.Label();
            this.lblNorth = new System.Windows.Forms.Label();
            this.lblEast = new System.Windows.Forms.Label();
            this.lblSouth = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.tlpGameGrid = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGameView = new System.Windows.Forms.Panel();
            this.btnPlace = new System.Windows.Forms.Button();
            this.pnlGameView.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstTiles
            // 
            this.lstTiles.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTiles.FormattingEnabled = true;
            this.lstTiles.Location = new System.Drawing.Point(232, 11);
            this.lstTiles.Margin = new System.Windows.Forms.Padding(2);
            this.lstTiles.Name = "lstTiles";
            this.lstTiles.Size = new System.Drawing.Size(358, 186);
            this.lstTiles.TabIndex = 0;
            this.lstTiles.SelectedIndexChanged += new System.EventHandler(this.lstTiles_SelectedIndexChanged);
            // 
            // btnCCW
            // 
            this.btnCCW.Location = new System.Drawing.Point(63, 167);
            this.btnCCW.Margin = new System.Windows.Forms.Padding(2);
            this.btnCCW.Name = "btnCCW";
            this.btnCCW.Size = new System.Drawing.Size(31, 19);
            this.btnCCW.TabIndex = 1;
            this.btnCCW.Text = "<<";
            this.btnCCW.UseVisualStyleBackColor = true;
            this.btnCCW.Click += new System.EventHandler(this.btnCCW_Click);
            // 
            // btnCW
            // 
            this.btnCW.Location = new System.Drawing.Point(146, 167);
            this.btnCW.Margin = new System.Windows.Forms.Padding(2);
            this.btnCW.Name = "btnCW";
            this.btnCW.Size = new System.Drawing.Size(30, 19);
            this.btnCW.TabIndex = 2;
            this.btnCW.Text = ">>";
            this.btnCW.UseVisualStyleBackColor = true;
            this.btnCW.Click += new System.EventHandler(this.btnCW_Click);
            // 
            // pnlTile
            // 
            this.pnlTile.BackColor = System.Drawing.Color.Transparent;
            this.pnlTile.Location = new System.Drawing.Point(63, 26);
            this.pnlTile.Margin = new System.Windows.Forms.Padding(2);
            this.pnlTile.Name = "pnlTile";
            this.pnlTile.Size = new System.Drawing.Size(112, 122);
            this.pnlTile.TabIndex = 3;
            // 
            // lblWest
            // 
            this.lblWest.AutoSize = true;
            this.lblWest.Location = new System.Drawing.Point(18, 78);
            this.lblWest.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWest.Name = "lblWest";
            this.lblWest.Size = new System.Drawing.Size(42, 13);
            this.lblWest.TabIndex = 4;
            this.lblWest.Text = "lblWest";
            // 
            // lblNorth
            // 
            this.lblNorth.AutoSize = true;
            this.lblNorth.Location = new System.Drawing.Point(98, 10);
            this.lblNorth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNorth.Name = "lblNorth";
            this.lblNorth.Size = new System.Drawing.Size(43, 13);
            this.lblNorth.TabIndex = 5;
            this.lblNorth.Text = "lblNorth";
            // 
            // lblEast
            // 
            this.lblEast.AutoSize = true;
            this.lblEast.Location = new System.Drawing.Point(180, 78);
            this.lblEast.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEast.Name = "lblEast";
            this.lblEast.Size = new System.Drawing.Size(38, 13);
            this.lblEast.TabIndex = 6;
            this.lblEast.Text = "lblEast";
            // 
            // lblSouth
            // 
            this.lblSouth.AutoSize = true;
            this.lblSouth.Location = new System.Drawing.Point(97, 150);
            this.lblSouth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSouth.Name = "lblSouth";
            this.lblSouth.Size = new System.Drawing.Size(45, 13);
            this.lblSouth.TabIndex = 7;
            this.lblSouth.Text = "lblSouth";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(184, 183);
            this.lblCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(45, 13);
            this.lblCount.TabIndex = 8;
            this.lblCount.Text = "lblCount";
            // 
            // tlpGameGrid
            // 
            this.tlpGameGrid.ColumnCount = 1;
            this.tlpGameGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tlpGameGrid.Location = new System.Drawing.Point(0, 0);
            this.tlpGameGrid.Margin = new System.Windows.Forms.Padding(2);
            this.tlpGameGrid.Name = "tlpGameGrid";
            this.tlpGameGrid.RowCount = 1;
            this.tlpGameGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.tlpGameGrid.Size = new System.Drawing.Size(112, 122);
            this.tlpGameGrid.TabIndex = 9;
            // 
            // pnlGameView
            // 
            this.pnlGameView.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGameView.AutoScroll = true;
            this.pnlGameView.Controls.Add(this.tlpGameGrid);
            this.pnlGameView.Location = new System.Drawing.Point(9, 207);
            this.pnlGameView.Margin = new System.Windows.Forms.Padding(2);
            this.pnlGameView.Name = "pnlGameView";
            this.pnlGameView.Size = new System.Drawing.Size(580, 277);
            this.pnlGameView.TabIndex = 10;
            // 
            // btnPlace
            // 
            this.btnPlace.Location = new System.Drawing.Point(100, 166);
            this.btnPlace.Margin = new System.Windows.Forms.Padding(2);
            this.btnPlace.Name = "btnPlace";
            this.btnPlace.Size = new System.Drawing.Size(40, 20);
            this.btnPlace.TabIndex = 11;
            this.btnPlace.Text = "Place";
            this.btnPlace.UseVisualStyleBackColor = true;
            this.btnPlace.Click += new System.EventHandler(this.btnPlace_Click);
            // 
            // CarcassonneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 494);
            this.Controls.Add(this.btnPlace);
            this.Controls.Add(this.pnlGameView);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblSouth);
            this.Controls.Add(this.lblEast);
            this.Controls.Add(this.lblNorth);
            this.Controls.Add(this.lblWest);
            this.Controls.Add(this.pnlTile);
            this.Controls.Add(this.btnCW);
            this.Controls.Add(this.btnCCW);
            this.Controls.Add(this.lstTiles);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CarcassonneForm";
            this.Text = "Form1";
            this.pnlGameView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnCCW;
        private System.Windows.Forms.Button btnCW;
        private System.Windows.Forms.Button btnPlace;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblEast;
        private System.Windows.Forms.Label lblNorth;
        private System.Windows.Forms.Label lblSouth;
        private System.Windows.Forms.Label lblWest;
        private System.Windows.Forms.ListBox lstTiles;
        private System.Windows.Forms.Panel pnlGameView;
        private System.Windows.Forms.Panel pnlTile;
        private System.Windows.Forms.TableLayoutPanel tlpGameGrid;

        #endregion
    }
}

