namespace Bug.SnakeGame.Game
{
	public static class CollisionManager
	{
		public static bool CheckSelfCollision(IEnumerable<Point> positions)
		{
			var positionsList = positions.ToList();

			if(positionsList.Count == 0)
				return false;

			for (var indexA = 0; indexA < positionsList.Count; indexA++)
			{
				var positionA = positionsList[indexA];

				for (var indexB = indexA + 1; indexB < positionsList.Count; indexB++)
				{
					var positionB = positionsList[indexB];

					if (positionA == positionB && indexA != indexB)
						return true;
				}
			}

			return false;
		}
		public static bool CheckCollisionWithPosition(IEnumerable<Point> positions, Point otherPosition)
		{
			var positionsList = positions.ToList();

			if (positionsList.Count == 0)
				return false;

			for (var index = 0; index < positionsList.Count; index++)
			{
				var position = positionsList[index];

				if (position == otherPosition)
					return true;
			}

			return false;
		}
	}
}
