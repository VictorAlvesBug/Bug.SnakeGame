using Bug.SnakeGame.Interfaces;

namespace Bug.SnakeGame.Commands
{
	internal class MoveLeftCommand : IGameCommand
	{
		public bool CanExecute(IGameCommand lastCommand)
			=> lastCommand.GetType() != typeof(MoveRightCommand)
			&& lastCommand.GetType() != GetType();

		public void Execute(IMovable movable)
		{
			movable.MoveLeft();
		}
	}
}
