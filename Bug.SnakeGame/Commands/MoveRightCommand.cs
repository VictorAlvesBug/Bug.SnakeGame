using Bug.SnakeGame.Interfaces;

namespace Bug.SnakeGame.Commands
{
	internal class MoveRightCommand : IGameCommand
	{
		public bool CanExecute(IGameCommand lastCommand)
			=> lastCommand.GetType() != typeof(MoveLeftCommand)
			&& lastCommand.GetType() != GetType();

		public void Execute(IMovable movable)
		{
			movable.MoveRight();
		}
	}
}
