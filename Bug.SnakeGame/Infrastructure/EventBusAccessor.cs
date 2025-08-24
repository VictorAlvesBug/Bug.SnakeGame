using Bug.SnakeGame.Interfaces;

namespace Bug.SnakeGame.Infrastructure
{
	public abstract class EventBusAccessor
	{
		private static IEventBus _bus;

		protected static IEventBus Bus => _bus ?? throw new ArgumentNullException(nameof(_bus));

		internal static void SetEventBus(IEventBus bus)
			=> _bus = bus ?? throw new ArgumentNullException(nameof(bus));
	}
}
