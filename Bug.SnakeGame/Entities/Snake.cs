using Bug.SnakeGame.Commands;
using Bug.SnakeGame.Components;
using Bug.SnakeGame.Core;
using Bug.SnakeGame.Game;

namespace Bug.SnakeGame.Entities
{
	public class Snake
	{
		private Queue<BodySegment> _body;

		public int Length => _body.Count;

		public Snake(Options options)
		{
			if (options.InitialLength < 1)
				throw new ArgumentException("Snake inicial deve ter ao menos 1 segmento", nameof(options.InitialLength));

			_body = new Queue<BodySegment>();

			for (int bodyIndex = 0; bodyIndex < options.InitialLength; bodyIndex++)
			{
				var segment = new BodySegment(
					xRange: new UnitRange(new UnitRange.Options
					{
						InitialValue = options.InitialPotition?.X ?? 1,
						Max = GameConfig.GridColumns - 1,
						CanOverflow = true,
					}),
					yRange: new UnitRange(new UnitRange.Options
					{
						InitialValue = options.InitialPotition?.Y ?? (int) Math.Floor(GameConfig.GridRows / 2.0),
						Max = GameConfig.GridRows - 1,
						CanOverflow = true,
					})
				);

				_body.Enqueue(segment);
			}
		}

		public void SetupInitialMove(CommandInvoker commandInvoker, IGameCommand initialCommands)
		{
			for (int segmentIndex = 0; segmentIndex < _body.Count; segmentIndex++)
			{
				for (int i = 0; i < _body.Count - segmentIndex; i++)
				{
					commandInvoker.ExecuteCommand(initialCommands, _body.ElementAt(segmentIndex));
				}
			}
		}

		public BodySegment GetSegment(int index)
		{
			if (_body.Count == 0)
				throw new InvalidOperationException("Snake não possui nenhum segmento");

			if (index < 0)
				index = _body.Count + index;

			if (index >= _body.Count)
				throw new ArgumentOutOfRangeException(nameof(index), $"Índice {index} não encontrado. Snake possui {_body.Count} segmentos");

			return _body.ElementAt(index);
		}

		public void AddSegment(Point point)
		{
			var newBodySegment = new BodySegment(
				xRange: new UnitRange(new UnitRange.Options
				{
					InitialValue = point.X,
					Max = GameConfig.GridColumns - 1,
					CanOverflow = true,
				}),
				yRange: new UnitRange(new UnitRange.Options
				{
					InitialValue = point.Y,
					Max = GameConfig.GridRows - 1,
					CanOverflow = true,
				})
			);

			_body.Enqueue(newBodySegment);
		}

		public void Move(CommandInvoker commandInvoker, Queue<IGameCommand> commands)
		{
			for (int segmentIndex = 0; segmentIndex < _body.Count; segmentIndex++)
			{
				var commandIndex = commands.Count - segmentIndex - 1;
				commandInvoker.ExecuteCommand(commands.ElementAt(commandIndex), _body.ElementAt(segmentIndex));
			}
		}

		public List<Point> GetPositions() => _body.Select(segment => segment.Position).ToList();

		public class Options
		{
			public int? InitialLength { get; set; }
			public Point? InitialPotition { get; set; }
			public IGameCommand InitialCommand { get; set; }
		}
	}
}
