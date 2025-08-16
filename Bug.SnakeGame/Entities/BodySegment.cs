using Bug.SnakeGame.Core;

namespace Bug.SnakeGame.Entities
{
	public class BodySegment : IMovable
	{
		private ValueRange _xRange;

		private ValueRange _yRange;
		public Point Position => new(_xRange.Value, _yRange.Value);

		public BodySegment(ValueRange xRange, ValueRange yRange)
		{
			_xRange = xRange;
			_yRange = yRange;
		}

		public void MoveUp() => _yRange = _yRange.Decrement();
		public void MoveDown() => _yRange = _yRange.Increment();
		public void MoveLeft() => _xRange = _xRange.Decrement();
		public void MoveRight() => _xRange = _xRange.Increment();
	}
}
