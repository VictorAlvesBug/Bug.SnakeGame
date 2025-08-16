namespace Bug.SnakeGame.Entities
{
	public static class Background
	{
		public static void Render(Graphics g, int columns, int rows, int tileSize)
		{
			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < columns; x++)
				{
					var brush = x % 2 == y % 2 ? Brushes.Honeydew : Brushes.Lavender;
					g.FillRectangle(brush, x * tileSize, y * tileSize, tileSize, tileSize);
				}
			}
		}
	}
}
