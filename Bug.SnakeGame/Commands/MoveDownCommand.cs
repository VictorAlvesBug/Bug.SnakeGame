using Bug.SnakeGame.Interfaces;
using System.Runtime.CompilerServices;

namespace Bug.SnakeGame.Commands
{
	internal class MoveDownCommand : IGameCommand
	{
		public bool CanExecute(IGameCommand lastCommand)
			=> lastCommand.GetType() != typeof(MoveUpCommand)
			&& lastCommand.GetType() != GetType();

		public void Execute(IMovable movable)
		{
			movable.MoveDown();
		}
	}
}
