using Bug.SnakeGame.Game;

namespace Bug.SnakeGame.Rendering
{
	public class Tileset
	{
		private List<Bitmap> _tiles = [];

		public Tileset(Image tilesetImage)
		{
			int tilesetCols = tilesetImage.Width / GameConfig.SourceTileSize;
			int tilesetRows = tilesetImage.Height / GameConfig.SourceTileSize;

			for (int y = 0; y < tilesetRows; y++)
			{
				for (int x = 0; x < tilesetCols; x++)
				{
					Rectangle cropRect = new Rectangle(x * GameConfig.SourceTileSize, y * GameConfig.SourceTileSize, GameConfig.SourceTileSize, GameConfig.SourceTileSize);

					Bitmap tile = new(GameConfig.SourceTileSize, GameConfig.SourceTileSize);

					using Graphics g = Graphics.FromImage(tile);
					g.DrawImage(tilesetImage, new Rectangle(0, 0, GameConfig.SourceTileSize, GameConfig.SourceTileSize), cropRect, GraphicsUnit.Pixel);

					_tiles.Add(tile);
				}
			}
		}

		public Bitmap GetTile(TileType type)
		{
			int index = (int)type;

			if (index < 0 || index >= _tiles.Count)
				throw new ArgumentOutOfRangeException(nameof(type), $"Tile {index} ({type}) não disponível no tileset.");

			return _tiles[index];
		}
	}
}
