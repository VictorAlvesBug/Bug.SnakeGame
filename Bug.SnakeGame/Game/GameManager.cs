using Bug.SnakeGame.Commands;
using Bug.SnakeGame.Entities;
using Bug.SnakeGame.Game;
using Bug.SnakeGame.Rendering;

namespace Bug.SnakeGame.Core
{
	public class GameManager
	{
		private InputHandler _inputHandler;
		private CommandInvoker _commandInvoker;
		private Queue<IGameCommand> _commands;
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
			_inputHandler = new InputHandler(new MoveRightCommand());

			_columns = screenWidth / tileSize;
			_rows = screenHeight / tileSize;
			_tileSize = tileSize;

			Subject = new(this);
			State = GameState.Running;

			var initialSnakeLength = 3;

			for (int i = 0; i < initialSnakeLength; i++)
			{
				_commands.Enqueue(_inputHandler.Command);
			}

			_snake = new Snake(new Snake.Options{
				Columns = _columns,
				Rows = _rows,
				SegmentSize = tileSize,
				InitialLength = initialSnakeLength,
				InitialPotition = new Point(2, 2)
			});

			_snake.SetupInitialMove(_commandInvoker, _inputHandler.Command);

			_fruit = Fruit.Generate(new Fruit.Options
			{
				Columns = _columns,
				Rows = _rows,
				BlockedPositions = _snake.GetPositions()
			});
		}

		public void ProcessInput(Keys keyCode)
		{
			_inputHandler.ProcessInput(keyCode);
		}

		public void Update()
		{
			_commands.Enqueue(_inputHandler.Command);

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
