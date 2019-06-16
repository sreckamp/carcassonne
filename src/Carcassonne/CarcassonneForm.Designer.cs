namespace Carcassonne
{
    partial class CarcassonneForm
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
            this.lstTiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTiles.FormattingEnabled = true;
            this.lstTiles.ItemHeight = 16;
            this.lstTiles.Location = new System.Drawing.Point(310, 14);
            this.lstTiles.Name = "lstTiles";
            this.lstTiles.Size = new System.Drawing.Size(476, 228);
            this.lstTiles.TabIndex = 0;
            this.lstTiles.SelectedIndexChanged += new System.EventHandler(this.lstTiles_SelectedIndexChanged);
            // 
            // btnCCW
            // 
            this.btnCCW.Location = new System.Drawing.Point(84, 205);
            this.btnCCW.Name = "btnCCW";
            this.btnCCW.Size = new System.Drawing.Size(41, 23);
            this.btnCCW.TabIndex = 1;
            this.btnCCW.Text = "<<";
            this.btnCCW.UseVisualStyleBackColor = true;
            this.btnCCW.Click += new System.EventHandler(this.btnCCW_Click);
            // 
            // btnCW
            // 
            this.btnCW.Location = new System.Drawing.Point(194, 205);
            this.btnCW.Name = "btnCW";
            this.btnCW.Size = new System.Drawing.Size(40, 23);
            this.btnCW.TabIndex = 2;
            this.btnCW.Text = ">>";
            this.btnCW.UseVisualStyleBackColor = true;
            this.btnCW.Click += new System.EventHandler(this.btnCW_Click);
            // 
            // pnlTile
            // 
            this.pnlTile.BackColor = System.Drawing.Color.Transparent;
            this.pnlTile.Location = new System.Drawing.Point(84, 32);
            this.pnlTile.Name = "pnlTile";
            this.pnlTile.Size = new System.Drawing.Size(150, 150);
            this.pnlTile.TabIndex = 3;
            // 
            // lblWest
            // 
            this.lblWest.AutoSize = true;
            this.lblWest.Location = new System.Drawing.Point(24, 96);
            this.lblWest.Name = "lblWest";
            this.lblWest.Size = new System.Drawing.Size(54, 17);
            this.lblWest.TabIndex = 4;
            this.lblWest.Text = "lblWest";
            // 
            // lblNorth
            // 
            this.lblNorth.AutoSize = true;
            this.lblNorth.Location = new System.Drawing.Point(131, 12);
            this.lblNorth.Name = "lblNorth";
            this.lblNorth.Size = new System.Drawing.Size(57, 17);
            this.lblNorth.TabIndex = 5;
            this.lblNorth.Text = "lblNorth";
            // 
            // lblEast
            // 
            this.lblEast.AutoSize = true;
            this.lblEast.Location = new System.Drawing.Point(240, 96);
            this.lblEast.Name = "lblEast";
            this.lblEast.Size = new System.Drawing.Size(50, 17);
            this.lblEast.TabIndex = 6;
            this.lblEast.Text = "lblEast";
            // 
            // lblSouth
            // 
            this.lblSouth.AutoSize = true;
            this.lblSouth.Location = new System.Drawing.Point(129, 185);
            this.lblSouth.Name = "lblSouth";
            this.lblSouth.Size = new System.Drawing.Size(59, 17);
            this.lblSouth.TabIndex = 7;
            this.lblSouth.Text = "lblSouth";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(245, 225);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(59, 17);
            this.lblCount.TabIndex = 8;
            this.lblCount.Text = "lblCount";
            // 
            // tlpGameGrid
            // 
            this.tlpGameGrid.ColumnCount = 1;
            this.tlpGameGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpGameGrid.Location = new System.Drawing.Point(0, 0);
            this.tlpGameGrid.Name = "tlpGameGrid";
            this.tlpGameGrid.RowCount = 1;
            this.tlpGameGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpGameGrid.Size = new System.Drawing.Size(150, 150);
            this.tlpGameGrid.TabIndex = 9;
            // 
            // pnlGameView
            // 
            this.pnlGameView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGameView.AutoScroll = true;
            this.pnlGameView.Controls.Add(this.tlpGameGrid);
            this.pnlGameView.Location = new System.Drawing.Point(12, 255);
            this.pnlGameView.Name = "pnlGameView";
            this.pnlGameView.Size = new System.Drawing.Size(774, 341);
            this.pnlGameView.TabIndex = 10;
            // 
            // btnPlace
            // 
            this.btnPlace.Location = new System.Drawing.Point(134, 204);
            this.btnPlace.Name = "btnPlace";
            this.btnPlace.Size = new System.Drawing.Size(53, 24);
            this.btnPlace.TabIndex = 11;
            this.btnPlace.Text = "Place";
            this.btnPlace.UseVisualStyleBackColor = true;
            this.btnPlace.Click += new System.EventHandler(this.btnPlace_Click);
            // 
            // CarcassonneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 608);
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
            this.Name = "CarcassonneForm";
            this.Text = "Form1";
            this.pnlGameView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstTiles;
        private System.Windows.Forms.Button btnCCW;
        private System.Windows.Forms.Button btnCW;
        private System.Windows.Forms.Panel pnlTile;
        private System.Windows.Forms.Label lblWest;
        private System.Windows.Forms.Label lblNorth;
        private System.Windows.Forms.Label lblEast;
        private System.Windows.Forms.Label lblSouth;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.TableLayoutPanel tlpGameGrid;
        private System.Windows.Forms.Panel pnlGameView;
        private System.Windows.Forms.Button btnPlace;

    }
}

