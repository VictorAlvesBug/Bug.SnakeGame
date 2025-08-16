using Bug.SnakeGame.Components;
namespace Bug.SnakeGame.Game
{
	public static class CollisionManager
	{
		public static bool CheckCollision(IEnumerable<Point> positionsToCheck)
		{
			var positions = positionsToCheck.ToList();

			if(positions.Count == 0)
				return false;

			for (var indexA = 0; indexA < positions.Count; indexA++)
			{
				var positionA = positions[indexA];

				for (var indexB = indexA + 1; indexB < positions.Count; indexB++)
				{
					var positionB = positions[indexB];

					if (positionA == positionB && indexA != indexB)
						return true;
				}
			}

			return false;
		}
	}
}
