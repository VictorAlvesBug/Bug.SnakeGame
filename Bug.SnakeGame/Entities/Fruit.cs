using Bug.SnakeGame.Game;
using Bug.SnakeGame.Rendering;

namespace Bug.SnakeGame.Entities
{
	public sealed class Fruit
	{
		public Point Position { get; private set; }

		private Fruit(Point position)
		{
			Position = position;
		}

		public static Fruit Generate(Options options)
		{
			var random = new Random();
			Point? position;

			do
			{
				position = new Point(random.Next(GameConfig.GridColumns), random.Next(GameConfig.GridRows));
			}
			while (options.BlockedPositions.Contains(position.Value));

			return new Fruit(position.Value);
		}

		public void Render(Graphics g, Tileset tileset)
		{
			var tiles = new List<Tile>
			{
				new() {
					Sprite = tileset.GetTile(TileType.Fruit),
					Position = Position,
					Size = GameConfig.GridTileSize
				}
			};

			tiles.Render(g);
		}

		public class Options
		{
			public List<Point> BlockedPositions { get; set; } = [];
		}
	}
}
