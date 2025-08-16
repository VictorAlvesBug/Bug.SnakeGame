using Bug.SnakeGame.Entities;
using Bug.SnakeGame.Rendering;

namespace Bug.SnakeGame.Rendering
{
	public static class Renderer
	{
		private static Image tilesetImage = Resource1.snakeTileset.ConvertToImage();
		private static Tileset tileset = new Tileset(tilesetImage, 64, 64);

		public static void Render(this Snake snake, Graphics g, int tileSize)
		{
			for (int index = 0; index < snake.Length; index++)
			{
				var segment = snake.GetSegment(index);
				var tileType = snake.GetSnakeTileType(index);

				var x = segment.Position.X * tileSize;
				var y = segment.Position.Y * tileSize;
				g.DrawImage(tileset.GetTile(tileType), x, y, tileSize, tileSize);
			}
		}

		public static void Render(this Fruit fruit, Graphics g, int tileSize)
		{
			var x = fruit.Position.X * tileSize;
			var y = fruit.Position.Y * tileSize;
			g.DrawImage(tileset.GetTile(TileType.Fruit), x, y, tileSize, tileSize);
		}

		private static Image ConvertToImage(this byte[] bytes)
		{
			using MemoryStream ms = new MemoryStream(bytes);

			return Image.FromStream(ms);
		}

		private static TileType GetSnakeTileType(this Snake snake, int index)
		{
			if (index < 0 || index >= snake.Length)
				throw new ArgumentOutOfRangeException(nameof(index), $"Índice {index} não encontrado. Snake possui {snake.Length} segmentos");

			Point currSegmentPos = snake.GetSegment(index).Position;

			Point? prevSegmentPos = index > 0
				? snake.GetSegment(index - 1).Position
				: null;

			Point? nextSegmentPos = index < snake.Length - 1
				? snake.GetSegment(index + 1).Position
				: null;


			// Head
			if (index == 0)
				return GetHeadTileType(currSegmentPos, nextSegmentPos);

			// Tail
			if (index == snake.Length - 1)
				return GetTailTileType(currSegmentPos, prevSegmentPos);

			// Body
			return GetBodyTileType(currSegmentPos, prevSegmentPos, nextSegmentPos);
		}

		private static TileType GetHeadTileType(Point currSegmentPos, Point? nextSegmentPos)
		{
			if (!nextSegmentPos.HasValue)
				throw new ArgumentOutOfRangeException(nameof(nextSegmentPos), $"Segmento {nextSegmentPos} não encontrado");

			if (currSegmentPos.X < nextSegmentPos.Value.X)
				return TileType.SnakeHeadLeft;

			if (currSegmentPos.X > nextSegmentPos.Value.X)
				return TileType.SnakeHeadRight;

			if (currSegmentPos.Y < nextSegmentPos.Value.Y)
				return TileType.SnakeHeadUp;

			if (currSegmentPos.Y > nextSegmentPos.Value.Y)
				return TileType.SnakeHeadDown;

			throw new InvalidOperationException("Impossível determinar o tipo de cabeça");
		}

		private static TileType GetTailTileType(Point currSegmentPos, Point? prevSegmentPos)
		{
			if (!prevSegmentPos.HasValue)
				throw new ArgumentOutOfRangeException(nameof(prevSegmentPos), $"Segmento {prevSegmentPos} não encontrado");

			if (currSegmentPos.X < prevSegmentPos.Value.X)
				return TileType.SnakeTailLeft;

			if (currSegmentPos.X > prevSegmentPos.Value.X)
				return TileType.SnakeTailRight;

			if (currSegmentPos.Y < prevSegmentPos.Value.Y)
				return TileType.SnakeTailUp;

			if (currSegmentPos.Y > prevSegmentPos.Value.Y)
				return TileType.SnakeTailDown;

			throw new InvalidOperationException("Impossível determinar o tipo de cauda");
		}

		private static TileType GetBodyTileType(Point currSegmentPos, Point? prevSegmentPos, Point? nextSegmentPos)
		{

			if (!prevSegmentPos.HasValue || !nextSegmentPos.HasValue)
				throw new ArgumentOutOfRangeException(nameof(nextSegmentPos), $"Segmento anterior ou posterior não encontrado");

			if (currSegmentPos.X == prevSegmentPos.Value.X && currSegmentPos.X == nextSegmentPos.Value.X)
				return TileType.SnakeBodyVertical;

			if (currSegmentPos.Y == prevSegmentPos.Value.Y && currSegmentPos.Y == nextSegmentPos.Value.Y)
				return TileType.SnakeBodyHorizontal;

			if (currSegmentPos.X < prevSegmentPos.Value.X
				&& currSegmentPos.Y < nextSegmentPos.Value.Y)
			{
				return TileType.SnakeBodyDownRight;
			}

			if (currSegmentPos.X < nextSegmentPos.Value.X
				&& currSegmentPos.Y < prevSegmentPos.Value.Y)
			{
				return TileType.SnakeBodyDownRight;
			}

			if (currSegmentPos.X > prevSegmentPos.Value.X
				&& currSegmentPos.Y < nextSegmentPos.Value.Y)
			{
				return TileType.SnakeBodyDownLeft;
			}

			if (currSegmentPos.X > nextSegmentPos.Value.X
				&& currSegmentPos.Y < prevSegmentPos.Value.Y)
			{
				return TileType.SnakeBodyDownLeft;
			}

			if (currSegmentPos.X < prevSegmentPos.Value.X
				&& currSegmentPos.Y > nextSegmentPos.Value.Y)
			{
				return TileType.SnakeBodyUpRight;
			}

			if (currSegmentPos.X < nextSegmentPos.Value.X
				&& currSegmentPos.Y > prevSegmentPos.Value.Y)
			{
				return TileType.SnakeBodyUpRight;
			}

			if (currSegmentPos.X > prevSegmentPos.Value.X
				&& currSegmentPos.Y > nextSegmentPos.Value.Y)
			{
				return TileType.SnakeBodyUpLeft;
			}

			if (currSegmentPos.X > nextSegmentPos.Value.X
				&& currSegmentPos.Y > prevSegmentPos.Value.Y)
			{
				return TileType.SnakeBodyUpLeft;
			}

			throw new InvalidOperationException("Impossível definir um Tile para o segmento");
		}
	}
}
