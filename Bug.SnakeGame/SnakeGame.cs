using Bug.SnakeGame.Core;
using Bug.SnakeGame.Entities;
using Bug.SnakeGame.Game;

namespace Bug.SnakeGame
{
	public partial class SnakeGame : Form, IObserver
	{
		private GameManager _game;
		private Bitmap _bitmap;
		private Graphics _graphics;

		private const int ScreenWidth = 800;
		private const int ScreenHeight = 800;
		private const int TileSize = 50;

		public SnakeGame()
		{
			InitializeComponent();
			Setup();
			KeyDown += ProcessInput;

			Width = ScreenWidth + 16;
			Height = ScreenHeight + 39;

			_bitmap = new Bitmap(ScreenWidth, ScreenHeight);
			_graphics = Graphics.FromImage(_bitmap);
		}

		private void Setup()
		{
			clock.Start();
			_game = new GameManager(new GameManager.Options
			{
				ScreenWidth = ScreenWidth,
				ScreenHeight = ScreenHeight,
				TileSize = TileSize,
				SnakeGame = this
			});
		}

		private void GameLoop(object sender, EventArgs e)
		{
			_game.Update();
			_game.Render(_graphics);
			ptbPlayGround.Image = _bitmap;
		}

		private void ProcessInput(object sender, KeyEventArgs e)
		{
			_game.ProcessInput(e.KeyCode);
		}

		public void OnNotify(ISubject subject)
		{
			var concreteSubject = subject as Subject<SnakeController>;

			if (concreteSubject is not null && concreteSubject.Entity.State == GameState.GameOver)
			{
				clock.Stop();
				MessageBox.Show("Game Over");
				Setup();
			}
		}
	}
}
