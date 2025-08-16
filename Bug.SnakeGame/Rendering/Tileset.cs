using Bug.SnakeGame.Rendering;

namespace Bug.SnakeGame.Rendering
{
	public class Tileset
	{
		private List<Bitmap> _tiles = [];

		public Tileset(Image tilesetImage, int tileWidth, int tileHeight)
		{
			int cols = tilesetImage.Width / tileWidth;
			int rows = tilesetImage.Height / tileHeight;

			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < cols; x++)
				{
					Rectangle cropRect = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);

					Bitmap tile = new Bitmap(tileWidth, tileHeight);

					using Graphics g = Graphics.FromImage(tile);
					g.DrawImage(tilesetImage, new Rectangle(0, 0, tileWidth, tileHeight), cropRect, GraphicsUnit.Pixel);

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
