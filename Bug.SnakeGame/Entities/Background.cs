using Bug.SnakeGame.Game;

namespace Bug.SnakeGame.Entities
{
	public static class Background
	{
		public static void Render(Graphics g)
		{
			for (int y = 0; y < GameConfig.GridRows; y++)
			{
				for (int x = 0; x < GameConfig.GridColumns; x++)
				{
					var brush = x % 2 == y % 2 ? Brushes.Honeydew : Brushes.Lavender;
					var tileSize = GameConfig.GridTileSize;
					var xPos = x * tileSize;
					var yPos = y * tileSize;
					g.FillRectangle(brush, xPos, yPos, tileSize, tileSize);
				}
			}
		}
	}
}
