using Bug.SnakeGame.Interfaces;

namespace Bug.SnakeGame.Components
{
	public class BodySegment(UnitRange xRange, UnitRange yRange) : IMovable
	{
		private UnitRange _xRange = xRange;

		private UnitRange _yRange = yRange;
		public Point Position => new(_xRange.Value, _yRange.Value);

		public void MoveUp() => _yRange = _yRange.Decrement();
		public void MoveDown() => _yRange = _yRange.Increment();
		public void MoveLeft() => _xRange = _xRange.Decrement();
		public void MoveRight() => _xRange = _xRange.Increment();
	}
}
