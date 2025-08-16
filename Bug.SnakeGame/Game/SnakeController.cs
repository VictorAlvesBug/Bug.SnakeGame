using Bug.SnakeGame.Commands;
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
		}

		public GameState Update(Point fluitPosition)
		{
			_commands.Enqueue(_inputHandler.Command);

			var tailPositionBeforeMovement = _snake.GetSegment(-1).Position;

			_snake.Move(_commandInvoker, _commands);

			if (_snake.CheckCollisionWithTile(fluitPosition))
			{

				_snake.AddSegment(tailPositionBeforeMovement);

				return GameState.FruitEaten;
			}

			if (_snake.CheckCollisionWithItself())
			{
				return GameState.GameOver;
			}

			while (_commands.Count > _snake.Length)
				_commands.Dequeue();

			return GameState.Running;
		}

		public void ProcessInput(Keys keyCode) => _inputHandler.ProcessInput(keyCode);

		public List<Point> GetSnakePositions() => _snake.GetPositions();

		public void Render(Graphics g, int tileSize) => _snake.Render(g, tileSize);
	}
}
