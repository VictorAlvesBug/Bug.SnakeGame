using Bug.SnakeGame.Core;

namespace Bug.SnakeGame.Commands
{
	public interface IGameCommand
	{
		void Execute(IMovable movable);
		bool CanExecute(IGameCommand lastCommand);
	}
}
