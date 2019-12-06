using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Carcassonne.Model;

namespace Carcassonne
{
    public partial class CarcassonneForm : Form
    {
        private readonly Game _game = new Game();
        private readonly GameTile _previewTile = new GameTile();
        private GameTile _activeTile;
        private readonly GameTile[,] _gameTiles;
        private readonly Dictionary<RegionType, Color> _edgeColor = new Dictionary<RegionType, Color>();

        public CarcassonneForm()
        {
            InitializeComponent();
            _game.Play();
            //foreach (var t in m_game.Deck)
            //{
            //    lstTiles.Items.Add(t);
            //}
            UpdateCount();
            pnlTile.Controls.Add(_previewTile);
            _previewTile.Dock = DockStyle.Fill;
            lstTiles.SelectedIndex = 0;
            _gameTiles = new GameTile[_game.Board.MaxX - _game.Board.MinX,
                _game.Board.MaxY - _game.Board.MinY];
            tlpGameGrid.ColumnCount = _gameTiles.GetLength(0);
            tlpGameGrid.ColumnStyles.Clear();
            for (var x = 0; x < tlpGameGrid.ColumnCount; x++)
            {
                tlpGameGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            }
            tlpGameGrid.RowCount = _gameTiles.GetLength(1);
            tlpGameGrid.RowStyles.Clear();
            for (var y = 0; y < tlpGameGrid.RowCount; y++)
            {
                this.tlpGameGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            }
            tlpGameGrid.Size = new Size(tlpGameGrid.ColumnCount * 150, tlpGameGrid.RowCount * 150);
            for (var y = 0; y < _gameTiles.GetLength(1); y++)
            {
                for (var x = 0; x < _gameTiles.GetLength(0); x++)
                {
                    var gt = new GameTile {Location = new Point(x, y)};
                    gt.Click += GameTileClick;
                    gt.Dock = DockStyle.Fill;
                    _gameTiles[x, y] = gt;
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
            if (_activeTile != null)
            {
                _activeTile.Active = false;
            }
            var gt = sender as GameTile;
            _activeTile = gt;
            if(gt != null)
                _activeTile.Active = true;
        }

        private void lstTiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(lstTiles.SelectedItem is Tile tile)) return;
            _previewTile.Tile = tile;
            lblNorth.Text = _previewTile.Tile.GetEdge(EdgeDirection.North).ToString();
            lblEast.Text = _previewTile.Tile.GetEdge(EdgeDirection.East).ToString();
            lblSouth.Text = _previewTile.Tile.GetEdge(EdgeDirection.South).ToString();
            lblWest.Text = _previewTile.Tile.GetEdge(EdgeDirection.West).ToString();
        }

        private void btnCCW_Click(object sender, EventArgs e)
        {
            if (_previewTile?.Tile == null) return;
//          m_previewTile.Tile.RotateCCW();
            _previewTile.UpdateRegions();
            lblNorth.Text = _previewTile.Tile.GetEdge(EdgeDirection.North).ToString();
            lblEast.Text = _previewTile.Tile.GetEdge(EdgeDirection.East).ToString();
            lblSouth.Text = _previewTile.Tile.GetEdge(EdgeDirection.South).ToString();
            lblWest.Text = _previewTile.Tile.GetEdge(EdgeDirection.West).ToString();
        }

        private void btnCW_Click(object sender, EventArgs e)
        {
            if (_previewTile != null && _previewTile.Tile != null)
            {
//                m_previewTile.Tile.RotateCW();
                _previewTile.UpdateRegions();
                lblNorth.Text = _previewTile.Tile.GetEdge(EdgeDirection.North).ToString();
                lblEast.Text = _previewTile.Tile.GetEdge(EdgeDirection.East).ToString();
                lblSouth.Text = _previewTile.Tile.GetEdge(EdgeDirection.South).ToString();
                lblWest.Text = _previewTile.Tile.GetEdge(EdgeDirection.West).ToString();
            }
        }

        private void btnPlace_Click(object sender, EventArgs e)
        {
            if (_activeTile != null && _activeTile.Tile == null && _previewTile.Tile != null)
            {
                _activeTile.Tile = _previewTile.Tile;
                _previewTile.Tile = null;
                var idx = lstTiles.SelectedIndex;
                lstTiles.SelectedIndex = -1;
                lstTiles.Items.Remove(_activeTile.Tile);
                UpdateCount();
                if (idx >= lstTiles.Items.Count)
                {
                    idx = 0;
                }
                if (lstTiles.Items.Count > 0)
                {
                    lstTiles.SelectedIndex = idx;
                }
                btnCW.Enabled = btnCCW.Enabled = btnPlace.Enabled = (_previewTile.Tile != null);
            }
        }
    }
}
