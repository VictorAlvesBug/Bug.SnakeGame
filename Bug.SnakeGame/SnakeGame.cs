using Bug.SnakeGame.Core;
using Bug.SnakeGame.DomainEvents;
using Bug.SnakeGame.Entities;
using Bug.SnakeGame.Game;
using Bug.SnakeGame.Infrastructure;

namespace Bug.SnakeGame
{
	public partial class SnakeGame : Form
	{
		private GameManager _game;
		private Bitmap _bitmap;
		private Graphics _graphics;

		public SnakeGame()
		{
			var bus = new InMemoryEventBus();
			bus.Subscribe<SnakeDied>(OnSnakeDied);
			EventBusAccessor.SetEventBus(bus);

			InitializeComponent();
			Setup();
			KeyDown += ProcessInput;

			Width = GameConfig.ScreenWidth + 16;
			Height = GameConfig.ScreenHeight + 45 + Score.TopOffset;

			_bitmap = new Bitmap(GameConfig.ScreenWidth, GameConfig.ScreenHeight + Score.TopOffset);
			_graphics = Graphics.FromImage(_bitmap);
		}

		private void Setup()
		{
			clock.Start();

			_game?.Dispose();
			_game = new GameManager();
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

		public void OnSnakeDied(SnakeDied e)
		{
			clock.Stop();
			MessageBox.Show(e.Reason, "Game Over");
			Setup();
		}
	}
}
