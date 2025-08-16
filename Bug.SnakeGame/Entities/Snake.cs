using Bug.SnakeGame.Commands;
using Bug.SnakeGame.Components;

namespace Bug.SnakeGame.Entities
{
	public class Snake
	{
		private Queue<BodySegment> _body;
		private int _columns;
		private int _rows;

		public int Length => _body.Count;

		public Snake(Options options)
		{
			if (options.Columns < 8 || options.Rows < 8)
				throw new ArgumentException("Grade deve ser no mínimo 8x8");

			if (options.SegmentSize < 5)
				throw new ArgumentException("Segmento deve ter ao menos 5 pixels", nameof(options.SegmentSize));

			if (options.InitialLength < 1)
				throw new ArgumentException("Snake inicial deve ter ao menos 1 segmento", nameof(options.InitialLength));

			_columns = options.Columns;
			_rows = options.Rows;

			_body = new Queue<BodySegment>();

			for (int bodyIndex = 0; bodyIndex < options.InitialLength; bodyIndex++)
			{
				var segment = new BodySegment(
					xRange: new UnitRange(new UnitRange.Options
					{
						InitialValue = options.InitialPotition?.X ?? 1,
						Max = _columns - 1,
						CanOverflow = true,
					}),
					yRange: new UnitRange(new UnitRange.Options
					{
						InitialValue = options.InitialPotition?.Y ?? (int) Math.Floor(_rows/2.0),
						Max = _rows - 1,
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
					Max = _columns - 1,
					CanOverflow = true,
				}),
				yRange: new UnitRange(new UnitRange.Options
				{
					InitialValue = point.Y,
					Max = _rows - 1,
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

		public bool CheckCollisionWithTile(Point position) => _body.Any(segment => segment.Position == position);

		public bool CheckCollisionWithItself()
		{
			if (_body.Count == 0)
				throw new InvalidOperationException("Snake não possui nenhum segmento");

			BodySegment head = _body.Peek();

			return _body.Any(segment => segment != head && segment.Position == head.Position);
		}

		public List<Point> GetPositions() => _body.Select(segment => segment.Position).ToList();

		public class Options
		{
			public int Columns { get; set; }
			public int Rows { get; set; }
			public int SegmentSize { get; set; }
			public int? InitialLength { get; set; }
			public Point? InitialPotition { get; set; }
			public IGameCommand InitialCommand { get; set; }
		}
	}
}
