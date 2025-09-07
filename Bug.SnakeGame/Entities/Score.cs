using Bug.SnakeGame.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug.SnakeGame.Entities
{
	public class Score
	{
		public int Current { get; private set; } = 0;
		public int Record { get; private set; } = 0;

		public static readonly int TopOffset = 50;

		public void AddOne() => Current++;

		public void ResetCurrent()
		{
			if (Current > Record)
				Record = Current;

			Current = 0;
		}

		public void Render(Graphics g)
		{
			var font = new Font(FontFamily.GenericMonospace, 16, FontStyle.Bold);

			var currentScore = $"SCORE: {Current}";
			var record = $"RECORD: {Record}";
			g.DrawString(currentScore, font, Brushes.Blue, 10, 10);
			g.DrawString(record, font, Brushes.Green, GameConfig.ScreenWidth - 180, 10);
		}
	}
}
