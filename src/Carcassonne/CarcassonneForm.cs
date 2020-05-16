using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Carcassonne.Model;

namespace Carcassonne
{
    public partial class CarcassonneForm : Form
    {
        private readonly Game m_game = new Game();
        private readonly GameTile m_previewTile = new GameTile();
        private GameTile m_activeTile;
        private readonly Dictionary<RegionType, Color> m_edgeColor = new Dictionary<RegionType, Color>();

        public CarcassonneForm()
        {
            InitializeComponent();
            m_game.Play();
            //foreach (var t in m_game.Deck)
            //{
            //    lstTiles.Items.Add(t);
            //}
            UpdateCount();
            pnlTile.Controls.Add(m_previewTile);
            m_previewTile.Dock = DockStyle.Fill;
            lstTiles.SelectedIndex = 0;
            var gameTiles = new GameTile[m_game.Board.MaxX - m_game.Board.MinX,
                m_game.Board.MaxY - m_game.Board.MinY];
            tlpGameGrid.ColumnCount = gameTiles.GetLength(0);
            tlpGameGrid.ColumnStyles.Clear();
            for (var x = 0; x < tlpGameGrid.ColumnCount; x++)
            {
                tlpGameGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            }
            tlpGameGrid.RowCount = gameTiles.GetLength(1);
            tlpGameGrid.RowStyles.Clear();
            for (var y = 0; y < tlpGameGrid.RowCount; y++)
            {
                tlpGameGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            }
            tlpGameGrid.Size = new Size(tlpGameGrid.ColumnCount * 150, tlpGameGrid.RowCount * 150);
            for (var y = 0; y < gameTiles.GetLength(1); y++)
            {
                for (var x = 0; x < gameTiles.GetLength(0); x++)
                {
                    var gt = new GameTile {Location = new Point(x, y)};
                    gt.Click += GameTileClick;
                    gt.Dock = DockStyle.Fill;
                    gameTiles[x, y] = gt;
                    tlpGameGrid.Controls.Add(gt);
//                    if (m_game.Board[new System.Windows.Point(x,y)] != null)
//                    {
//                        gt.Tile = m_game.Board[new System.Windows.Point(x, y)];
//                    }
                }
            }
        }

        private void UpdateCount()
        {
            lblCount.Text = $@"{lstTiles.Items.Count} Tiles";
        }

        private void GameTileClick(object sender, EventArgs e)
        {
            if (m_activeTile != null)
            {
                m_activeTile.Active = false;
            }
            var gt = sender as GameTile;
            m_activeTile = gt;
            if(gt != null)
                m_activeTile.Active = true;
        }

        private void lstTiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(lstTiles.SelectedItem is Tile tile)) return;
            m_previewTile.Tile = tile;
            lblNorth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.North).ToString();
            lblEast.Text = m_previewTile.Tile.GetEdge(EdgeDirection.East).ToString();
            lblSouth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.South).ToString();
            lblWest.Text = m_previewTile.Tile.GetEdge(EdgeDirection.West).ToString();
        }

        private void btnCCW_Click(object sender, EventArgs e)
        {
            if (m_previewTile?.Tile == null) return;
//          m_previewTile.Tile.RotateCCW();
            m_previewTile.UpdateRegions();
            lblNorth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.North).ToString();
            lblEast.Text = m_previewTile.Tile.GetEdge(EdgeDirection.East).ToString();
            lblSouth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.South).ToString();
            lblWest.Text = m_previewTile.Tile.GetEdge(EdgeDirection.West).ToString();
        }

        private void btnCW_Click(object sender, EventArgs e)
        {
            if (m_previewTile == null || m_previewTile.Tile == Tile.None) return;
            //                m_previewTile.Tile.RotateCW();
            m_previewTile.UpdateRegions();
            lblNorth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.North).ToString();
            lblEast.Text = m_previewTile.Tile.GetEdge(EdgeDirection.East).ToString();
            lblSouth.Text = m_previewTile.Tile.GetEdge(EdgeDirection.South).ToString();
            lblWest.Text = m_previewTile.Tile.GetEdge(EdgeDirection.West).ToString();
        }

        private void btnPlace_Click(object sender, EventArgs e)
        {
            if (m_activeTile == null || m_activeTile.Tile != Tile.None || m_previewTile.Tile == Tile.None) return;
            m_activeTile.Tile = m_previewTile.Tile;
            m_previewTile.Tile = Tile.None;
            var idx = lstTiles.SelectedIndex;
            lstTiles.SelectedIndex = -1;
            lstTiles.Items.Remove(m_activeTile.Tile);
            UpdateCount();
            if (idx >= lstTiles.Items.Count)
            {
                idx = 0;
            }
            if (lstTiles.Items.Count > 0)
            {
                lstTiles.SelectedIndex = idx;
            }
            btnCW.Enabled = btnCCW.Enabled = btnPlace.Enabled = m_previewTile.Tile != Tile.None;
        }
    }
}
