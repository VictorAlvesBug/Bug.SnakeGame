namespace Bug.SnakeGame.Rendering
{
	public static class Renderer
	{
		public static void Render(this IEnumerable<Tile> tiles, Graphics g)
		{
			foreach (var tile in tiles)
			{
				var x = tile.Position.X * tile.Size;
				var y = tile.Position.Y * tile.Size;
				g.DrawImage(tile.Sprite, x, y, tile.Size, tile.Size);
			}
		}
	}
}
