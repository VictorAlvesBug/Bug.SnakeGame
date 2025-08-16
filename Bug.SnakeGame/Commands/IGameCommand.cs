using Bug.SnakeGame.Interfaces;

namespace Bug.SnakeGame.Commands
{
	public interface IGameCommand
	{
		void Execute(IMovable movable);
		bool CanExecute(IGameCommand lastCommand);
	}
}
