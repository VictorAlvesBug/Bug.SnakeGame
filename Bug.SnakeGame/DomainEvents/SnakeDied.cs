using Bug.SnakeGame.Interfaces;

namespace Bug.SnakeGame.DomainEvents
{
	public class SnakeDied(string reason) : IEvent
	{
		public string Reason { get; } = reason;
	}
}
