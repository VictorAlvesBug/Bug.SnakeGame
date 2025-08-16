using Bug.SnakeGame.Core;

namespace Bug.SnakeGame.Commands
{
	public class CommandInvoker
	{
		public void ExecuteCommand(IGameCommand command, IMovable movable)
		{
			command.Execute(movable);
		}
	}
}
