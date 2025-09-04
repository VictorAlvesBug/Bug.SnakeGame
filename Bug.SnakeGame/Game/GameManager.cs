using Bug.SnakeGame.Commands;
using Bug.SnakeGame.DomainEvents;
using Bug.SnakeGame.Entities;
using Bug.SnakeGame.Game;
using Bug.SnakeGame.Infrastructure;
using Bug.SnakeGame.Rendering;

namespace Bug.SnakeGame.Core
{
	public class GameManager : EventBusAccessor, IDisposable
	{
		private bool _disposed;
		private readonly IDisposable _subscriptionFruitEaten;

		private static Image tilesetImage = ConvertToImage(Resource1.snakeTileset);
		private Tileset tileset = new Tileset(tilesetImage);

		private SnakeController _snakeController;
		private Fruit _fruit;
		private static Score _score = new Score();

		public GameManager()
		{
			_subscriptionFruitEaten = Bus.Subscribe<FruitEaten>(OnFruitEaten);

			_snakeController = new SnakeController(new Snake.Options
			{
				InitialLength = 5,
				InitialPotition = new Point(2, 2),
				InitialCommand = new MoveRightCommand()
			});

			GenerateNewFruit();
		}

		public void ProcessInput(Keys keyCode) => _snakeController.ProcessInput(keyCode);

		public void Update()
		{
			_snakeController.Update(_fruit.Position);
		}

		public void OnFruitEaten(FruitEaten e = null)
		{
			GenerateNewFruit();
			_score.AddOne();
		}

		private void GenerateNewFruit()
		{
			_fruit = Fruit.Generate(new Fruit.Options
			{
				BlockedPositions = _snakeController.GetSnakePositions()
			});
		}

		public void Render(Graphics g)
		{
			g.Clear(Color.White);

			_score.Render(g);
			g.TranslateTransform(0, Score.TopOffset);

			Background.Render(g);

			_fruit.Render(g, tileset);

			_snakeController.Render(g, tileset);
			g.ResetTransform();
		}

		private static Image ConvertToImage(byte[] bytes)
		{
			using MemoryStream ms = new(bytes);

			return Image.FromStream(ms);
		}

		public void Dispose()
		{
			if (_disposed)
				return;

			_disposed = true;

			_subscriptionFruitEaten.Dispose();
			_score.ResetCurrent();
		}
	}
}
