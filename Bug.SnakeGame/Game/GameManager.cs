using Bug.SnakeGame.Commands;
using Bug.SnakeGame.Entities;
using Bug.SnakeGame.Game;
using Bug.SnakeGame.Rendering;
using System.Xml.Linq;

namespace Bug.SnakeGame.Core
{
	public class GameManager : IObserver
	{
		private static Image tilesetImage = ConvertToImage(Resource1.snakeTileset);
		private Tileset tileset = new Tileset(tilesetImage, 64, 64);

		private SnakeController _snakeController;
		private Fruit _fruit;

		private int _columns;
		private int _rows;
		private int _tileSize;

		public GameManager(Options options)
		{
			_columns = options.ScreenWidth / options.TileSize;
			_rows = options.ScreenHeight / options.TileSize;
			_tileSize = options.TileSize;

			_snakeController = new SnakeController(new Snake.Options
			{
				Columns = _columns,
				Rows = _rows,
				SegmentSize = _tileSize,
				InitialLength = 5,
				InitialPotition = new Point(2, 2),
				InitialCommand = new MoveRightCommand(),
				GameManager = this,
				SnakeGame = options.SnakeGame
			});

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
			_snakeController.Update(_fruit.Position);
		}

		public void OnNotify(ISubject subject)
		{
			var concreteSubject = subject as Subject<SnakeController>;

			if (concreteSubject is not null && concreteSubject.Entity.State == GameState.AddScore)
			{
				_fruit = Fruit.Generate(new Fruit.Options
				{
					Columns = _columns,
					Rows = _rows,
					BlockedPositions = _snakeController.GetSnakePositions()
				});
			}
		}

		public void Render(Graphics g)
		{
			g.Clear(Color.White);

			Background.Render(g, _columns, _rows, _tileSize);

			_fruit.Render(g, tileset, _tileSize);

			_snakeController.Render(g, tileset, _tileSize);
		}

		private static Image ConvertToImage(byte[] bytes)
		{
			using MemoryStream ms = new(bytes);

			return Image.FromStream(ms);
		}

		public class Options
		{
			public int ScreenWidth { get; set; }
			public int ScreenHeight { get; set; }
			public int TileSize { get; set; }
			public SnakeGame SnakeGame { get; set; }
		}
	}
}
