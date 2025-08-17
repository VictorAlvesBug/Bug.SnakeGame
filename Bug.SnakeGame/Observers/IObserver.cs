namespace Bug.SnakeGame.Core
{
	public interface IObserver
	{
		void OnNotify(ISubject subject);
	}
}
