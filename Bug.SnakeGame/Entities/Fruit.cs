using Bug.SnakeGame.Rendering;
using System;

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
				position = new Point(random.Next(options.Columns), random.Next(options.Rows));
			}
			while (options.BlockedPositions.Contains(position.Value));

			return new Fruit(position.Value);
		}

		public void Render(Graphics g, Tileset tileset, int fruitSize)
		{
			var tiles = new List<Tile>
			{
				new() {
					Sprite = tileset.GetTile(TileType.Fruit),
					Position = Position,
					Size = fruitSize
				}
			};

			tiles.Render(g);
		}

		public class Options
		{
			public int Columns { get; set; }
			public int Rows { get; set; }
			public List<Point> BlockedPositions { get; set; } = [];
		}
	}
}
