using Bug.SnakeGame.Commands;

namespace Bug.SnakeGame.Game
{
	public class InputHandler
	{
		public IGameCommand Command { get; private set; }

		public InputHandler(IGameCommand initialCommand)
		{
			Command = initialCommand;
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

			if (newCommand != null && newCommand.CanExecuteAfter(Command))
			{
				Command = newCommand;
			}
		}
	}
}
