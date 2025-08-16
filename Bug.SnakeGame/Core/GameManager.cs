using Bug.SnakeGame.Commands;
using Bug.SnakeGame.Entities;
using Bug.SnakeGame.Rendering;

namespace Bug.SnakeGame.Core
{
	public class GameManager
	{
		private CommandInvoker _commandInvoker;
		private Queue<IGameCommand> _commands;
		private IGameCommand _lastPressedCommand;
		private Snake _snake;
		private Fruit _fruit;

		private int _columns;
		private int _rows;
		private int _tileSize;

		public Subject<GameManager> Subject { get; private set; }
		public GameState State { get; private set; }

		public GameManager(int screenWidth, int screenHeight, int tileSize)
		{
			_commandInvoker = new CommandInvoker();
			_commands = new Queue<IGameCommand>();
			_lastPressedCommand = new MoveRightCommand();

			_columns = screenWidth / tileSize;
			_rows = screenHeight / tileSize;
			_tileSize = tileSize;

			Subject = new(this);
			//subject.Attach
			State = GameState.Running;

			var initialSnakeLength = 3;

			for (int i = 0; i < initialSnakeLength; i++)
			{
				_commands.Enqueue(_lastPressedCommand);
			}

			_snake = new Snake(new Snake.Options{
				Columns = _columns,
				Rows = _rows,
				SegmentSize = tileSize,
				InitialLength = initialSnakeLength,
				InitialPotition = new Point(2, 2)
			});

			_snake.InitialMove(_commandInvoker, _lastPressedCommand);

			_fruit = Fruit.Generate(new Fruit.Options
			{
				Columns = _columns,
				Rows = _rows,
				BlockedPositions = _snake.GetPositions()
			});
		}

		public void ProcessInput(Keys keyCode)
		{
			IGameCommand? newCommand = keyCode switch
			{
				// Arrows
				Keys.Up => new MoveUpCommand(),
				Keys.Down => new MoveDownCommand(),
				Keys.Left => new MoveLeftCommand(),
				Keys.Right => new MoveRightCommand(),

				// WASD
				Keys.W => new MoveUpCommand(),
				Keys.S => new MoveDownCommand(),
				Keys.A => new MoveLeftCommand(),
				Keys.D => new MoveRightCommand(),
				_ => null
			};

			if (newCommand is null)
				return;

			if (newCommand != null && _commands.Count > 0 && newCommand.CanExecute(_commands.Last()))
			{
				_lastPressedCommand = newCommand;
			}
		}

		public void Update()
		{
			_commands.Enqueue(_lastPressedCommand);

			var lastBodySegmentPositionBeforeMove = _snake.GetSegment(-1).Position;

			_snake.Move(_commandInvoker, _commands);

			if (_snake.CheckCollisionWithTile(_fruit.Position))
			{

				_snake.AddSegment(lastBodySegmentPositionBeforeMove);
				
				_fruit = Fruit.Generate(new Fruit.Options
				{
					Columns = _columns,
					Rows = _rows,
					BlockedPositions = _snake.GetPositions()
				});
			}

			if (_snake.CheckCollisionWithItself())
			{
				State = GameState.GameOver;
				Subject.Notify();
			}

			while (_commands.Count > _snake.Length)
				_commands.Dequeue();
		}

		public void Render(Graphics g)
		{
			g.Clear(Color.White);

			Background.Render(g, _columns, _rows, _tileSize);

			_fruit.Render(g, _tileSize);

			_snake.Render(g, _tileSize);
		}
	}
}
