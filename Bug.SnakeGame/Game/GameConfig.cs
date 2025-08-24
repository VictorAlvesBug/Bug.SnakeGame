namespace Bug.SnakeGame.Game
{
	public static class GameConfig
	{
		public const int ScreenWidth = 500;
		public const int ScreenHeight = 500;
		public const int GridTileSize = 20;
		public const int SourceTileSize = 64;

		public const int GridColumns = ScreenWidth / GridTileSize;
		public const int GridRows = ScreenHeight / GridTileSize;
	}
}
