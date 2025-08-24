using Bug.SnakeGame.Interfaces;

namespace Bug.SnakeGame.Infrastructure
{
	public class InMemoryEventBus : IEventBus
	{
		private readonly Dictionary<Type, List<Delegate>> _handlers = [];

		public void Publish<T>(T @event) where T : IEvent
		{
			if (_handlers.TryGetValue(typeof(T), out var list))
			{
				foreach (var handler in list)
				{
					var func = (Action<T>)handler;
					func(@event);
				}
			}
		}

		public IDisposable Subscribe<T>(Action<T> handler) where T : IEvent
		{
			var type = typeof(T);

			if (!_handlers.TryGetValue(type, out var list))
			{
				_handlers[type] = list = [];
			}

			list.Add(handler);

			return new Subscription(onDispose: () => list.Remove(handler));
		}

		private sealed class Subscription : IDisposable
		{
			private readonly Action _onDispose;
			private bool _done;

			public Subscription(Action onDispose) => _onDispose = onDispose;

			public void Dispose()
			{
				if (_done)
					return;

				_done = true;
				_onDispose();
			}
		}
	}
}
