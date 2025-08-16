using Bug.SnakeGame.Commands;
using Bug.SnakeGame.Entities;
using Bug.SnakeGame.Game;
using Bug.SnakeGame.Rendering;

namespace Bug.SnakeGame.Core
{
	public class GameManager
	{
		private InputHandler _inputHandler;
		private SnakeController _snakeController;
		private Fruit _fruit;

		private int _columns;
		private int _rows;
		private int _tileSize;

		public Subject<GameManager> Subject { get; private set; }

		public GameManager(int screenWidth, int screenHeight, int tileSize)
		{
			_columns = screenWidth / tileSize;
			_rows = screenHeight / tileSize;
			_tileSize = tileSize;

			_snakeController = new SnakeController(new Snake.Options
			{
				Columns = _columns,
				Rows = _rows,
				SegmentSize = _tileSize,
				InitialLength = 5,
				InitialPotition = new Point(2, 2),
				InitialCommand = new MoveRightCommand()
			});

			Subject = new(this);

			_fruit = Fruit.Generate(new Fruit.Options
			{
				Columns = _columns,
				Rows = _rows,
				BlockedPositions = _snakeController.GetSnakePositions()
			});
		}

		public void ProcessInput(Keys keyCode) => _snakeController.ProcessInput(keyCode);

		public void Update()
		{
			var newState = _snakeController.Update(_fruit.Position);

			switch (newState)
			{
				case GameState.AddScore:
					_fruit = Fruit.Generate(new Fruit.Options
					{
						Columns = _columns,
						Rows = _rows,
						BlockedPositions = _snakeController.GetSnakePositions()
					});
					break;
				case GameState.GameOver:
					Subject.Notify();
					break;
			}
		}

		public void Render(Graphics g)
		{
			g.Clear(Color.White);

			Background.Render(g, _columns, _rows, _tileSize);

			_fruit.Render(g, _tileSize);

			_snakeController.Render(g, _tileSize);
		}
	}
}
