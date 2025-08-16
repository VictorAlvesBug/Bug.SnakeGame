namespace Bug.SnakeGame.Core
{
	public class Subject<T> : ISubject
	{
		public T Entity { get; set; }

		private List<IObserver> _observers = [];

		public Subject(T entity)
		{
			Entity = entity;
		}

		public void Attach(IObserver observer)
		{
			_observers.Add(observer);
		}

		public void Detach(IObserver observer)
		{
			_observers.Remove(observer);
		}

		public void Notify()
		{
			foreach (var observer in _observers)
			{
				observer.Update(this);
			}
		}

		public void SomeBusinessLogic()
		{
			this.Notify();
		}
	}
}
