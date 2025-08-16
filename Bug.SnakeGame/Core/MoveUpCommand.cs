using Bug.SnakeGame.Core;

namespace Bug.SnakeGame.Commands
{
	internal class MoveUpCommand : IGameCommand
	{
		public bool CanExecute(IGameCommand lastCommand)
			=> lastCommand.GetType() != typeof(MoveDownCommand)
			&& lastCommand.GetType() != GetType();

		public void Execute(IMovable movable)
		{
			movable.MoveUp();
		}
	}
}
