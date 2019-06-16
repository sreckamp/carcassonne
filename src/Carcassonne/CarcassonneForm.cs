using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;
using Carcassonne.Model;
using System.Drawing;

namespace Carcassonne
{
    public partial class CarcassonneForm : Form
    {
        private readonly Game m_game = new Game();
        private readonly GameTile m_previewTile = new GameTile();
        private GameTile m_activeTile;
        private readonly GameTile[,] m_gameTiles;
        private readonly Dictionary<RegionType, Color> m_EdgeColor = new Dictionary<RegionType, Color>();

        public CarcassonneForm()
        {
            InitializeComponent();
            m_game.StartGame();
            //foreach (var t in m_game.Deck)
            //{
            //    lstTiles.Items.Add(t);
            //}
            updateCount();
            pnlTile.Controls.Add(m_previewTile);
            m_previewTile.Dock = DockStyle.Fill;
            lstTiles.SelectedIndex = 0;
            m_gameTiles = new GameTile[m_game.Board.MaxX - m_game.Board.MinX,
                m_game.Board.MaxY - m_game.Board.MinY];
            tlpGameGrid.ColumnCount = m_gameTiles.GetLength(0);
            tlpGameGrid.ColumnStyles.Clear();
            for (int x = 0; x < tlpGameGrid.ColumnCount; x++)
            {
                tlpGameGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            }
            tlpGameGrid.RowCount = m_gameTiles.GetLength(1);
            tlpGameGrid.RowStyles.Clear();
            for (int y = 0; y < tlpGameGrid.RowCount; y++)
            {
                this.tlpGameGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            }
            tlpGameGrid.Size = new Size(tlpGameGrid.ColumnCount * 150, tlpGameGrid.RowCount * 150);
            for (int y = 0; y < m_gameTiles.GetLength(1); y++)
            {
                for (int x = 0; x < m_gameTiles.GetLength(0); x++)
                {
                    var gt = new GameTile();
                    gt.Location = new Point(x, y);
                    gt.Click += new EventHandler(gameTileClick);
                    gt.Dock = DockStyle.Fill;
                    m_gameTiles[x, y] = gt;
                    tlpGameGrid.Controls.Add(gt);
                    if (m_game.Board[new System.Windows.Point(x,y)] != null)
                    {
                        gt.Tile = m_game.Board[new System.Windows.Point(x, y)];
                    }
                }
            }
        }

        private void updateCount()
        {
            lblCount.Text = string.Format("{0} Tiles", lstTiles.Items.Count);
        }

        private void gameTileClick(object sender, EventArgs e)
        {
            var gt = sender as GameTile;
            if (m_activeTile != null)
            {
                m_activeTile.Active = false;
            }
            m_activeTile = gt;
            m_activeTile.Active = true;
        }

        private void lstTiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tile = lstTiles.SelectedItem as Tile;
            if (tile != null)
            {
                m_previewTile.Tile = tile;
                lblNorth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.North).ToString();
                lblEast.Text = m_previewTile.Tile.GetEdge(EdgeDirection.East).ToString();
                lblSouth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.South).ToString();
                lblWest.Text = m_previewTile.Tile.GetEdge(EdgeDirection.West).ToString();
            }
        }

        private void btnCCW_Click(object sender, EventArgs e)
        {
            if (m_previewTile != null && m_previewTile.Tile != null)
            {
                m_previewTile.Tile.RotateCCW();
                m_previewTile.UpdateRegions();
                lblNorth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.North).ToString();
                lblEast.Text = m_previewTile.Tile.GetEdge(EdgeDirection.East).ToString();
                lblSouth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.South).ToString();
                lblWest.Text = m_previewTile.Tile.GetEdge(EdgeDirection.West).ToString();
            }
        }

        private void btnCW_Click(object sender, EventArgs e)
        {
            if (m_previewTile != null && m_previewTile.Tile != null)
            {
                m_previewTile.Tile.RotateCW();
                m_previewTile.UpdateRegions();
                lblNorth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.North).ToString();
                lblEast.Text = m_previewTile.Tile.GetEdge(EdgeDirection.East).ToString();
                lblSouth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.South).ToString();
                lblWest.Text = m_previewTile.Tile.GetEdge(EdgeDirection.West).ToString();
            }
        }

        private void btnPlace_Click(object sender, EventArgs e)
        {
            if (m_activeTile != null && m_activeTile.Tile == null && m_previewTile.Tile != null)
            {
                m_activeTile.Tile = m_previewTile.Tile;
                m_previewTile.Tile = null;
                int idx = lstTiles.SelectedIndex;
                lstTiles.SelectedIndex = -1;
                lstTiles.Items.Remove(m_activeTile.Tile);
                updateCount();
                if (idx >= lstTiles.Items.Count)
                {
                    idx = 0;
                }
                if (lstTiles.Items.Count > 0)
                {
                    lstTiles.SelectedIndex = idx;
                }
                btnCW.Enabled = btnCCW.Enabled = btnPlace.Enabled = (m_previewTile.Tile != null);
            }
        }
    }
}
