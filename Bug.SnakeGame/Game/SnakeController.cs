using Bug.SnakeGame.Commands;
using Bug.SnakeGame.Core;
using Bug.SnakeGame.Entities;
using Bug.SnakeGame.Rendering;

namespace Bug.SnakeGame.Game
{
	public class SnakeController
	{
		private InputHandler _inputHandler;
		private CommandInvoker _commandInvoker;
		private Queue<IGameCommand> _commands;
		private Snake _snake;
		public GameState State { get; private set; }
		public Subject<SnakeController> Subject { get; private set; }

		public SnakeController(Snake.Options options)
		{
			_inputHandler = new InputHandler(options.InitialCommand);
			_commandInvoker = new CommandInvoker();
			_commands = new Queue<IGameCommand>();

			for (int i = 0; i < options.InitialLength; i++)
			{
				_commands.Enqueue(_inputHandler.Command);
			}

			_snake = new Snake(options);

			_snake.SetupInitialMove(_commandInvoker, _inputHandler.Command);

			Subject = new(this);
			Subject.Attach(options.GameManager);
			Subject.Attach(options.SnakeGame);
		}

		public void Update(Point fruitPosition)
		{
			_commands.Enqueue(_inputHandler.Command);

			var tailPositionBeforeMovement = _snake.GetSegment(-1).Position;

			_snake.Move(_commandInvoker, _commands);

			// Colisão da cobrinha com ela mesma
			if (CollisionManager.CheckCollision(_snake.GetPositions()))
			{
				State = GameState.GameOver;
				Subject.Notify();
				return;
			}

			// Colisão da cobrinha com a maçã
			if (CollisionManager.CheckCollision(_snake.GetPositions().Append(fruitPosition)))
			{
				_snake.AddSegment(tailPositionBeforeMovement);
				State = GameState.AddScore;
				Subject.Notify();
				return;
			}

			while (_commands.Count > _snake.Length)
				_commands.Dequeue();

			State = GameState.Running;
			Subject.Notify();
		}

		public void ProcessInput(Keys keyCode) => _inputHandler.ProcessInput(keyCode);

		public List<Point> GetSnakePositions() => _snake.GetPositions();

		public void Render(Graphics g, Tileset tileset, int segmentSize)
		{
			List<Tile> tiles = [];

			var positions = _snake.GetPositions();

			for (int index = 0; index < positions.Count; index++)
			{
				tiles.Add(new Tile
				{
					Sprite = tileset.GetTile(GetSnakeTileType(index)),
					Position = positions[index],
					Size = segmentSize
				});
			}

			tiles.Render(g);
		}
		private TileType GetSnakeTileType(int index)
		{
			if (index < 0 || index >= _snake.Length)
				throw new ArgumentOutOfRangeException(nameof(index), $"Índice {index} não encontrado. Snake possui {_snake.Length} segmentos");

			Point currSegmentPos = _snake.GetSegment(index).Position;

			Point? prevSegmentPos = index > 0
				? _snake.GetSegment(index - 1).Position
				: null;

			Point? nextSegmentPos = index < _snake.Length - 1
				? _snake.GetSegment(index + 1).Position
				: null;


			// Head
			if (index == 0)
				return GetHeadTileType(currSegmentPos, nextSegmentPos);

			// Tail
			if (index == _snake.Length - 1)
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
