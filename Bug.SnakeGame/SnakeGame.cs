using Bug.SnakeGame.Commands;
using Bug.SnakeGame.Core;
using Bug.SnakeGame.Entities;
using Microsoft.VisualBasic.Devices;

namespace Bug.SnakeGame
{
	public partial class SnakeGame : Form, IObserver
	{
		private GameManager _game;
		private Bitmap _bitmap;
		private Graphics _graphics;

		private const int ScreenWidth = 400;
		private const int ScreenHeight = 400;
		private const int TileSize = 16;

		public SnakeGame()
		{
			InitializeComponent();
			Setup();
			KeyDown += ProcessInput;
			//_game.OnGameOver(clock.Stop);
			//_game.AfterGameOver(Setup);

			Width = ScreenWidth + 16;
			Height = ScreenHeight + 39;

			_bitmap = new Bitmap(ScreenWidth, ScreenHeight);
			_graphics = Graphics.FromImage(_bitmap);
		}

		private void Setup()
		{
			clock.Start();
			_game = new GameManager(ScreenWidth, ScreenHeight, TileSize);
			_game.Subject.Attach(this);
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

		public void Update(ISubject subject)
		{
			if ((subject as Subject<GameManager>).Entity.State == GameState.GameOver)
			{
				clock.Stop();
				MessageBox.Show("Game Over");
				Setup();
			}
		}
	}
}
