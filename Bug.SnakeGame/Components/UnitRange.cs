namespace Bug.SnakeGame.Components
{
	public class UnitRange
	{
		public int Value { get; set; } = 0;
		public int Min { get; set; } = 0;
		public int Max { get; set; }
		public int Step { get; set; } = 1;
		public bool CanOverflow { get; set; } = false;

		public UnitRange(Options options)
		{
			Value = options.InitialValue ?? Value;
			Min = options.Min ?? Min;
			Max = options.Max;
			Step = options.Step ?? Step;
			CanOverflow = options.CanOverflow ?? CanOverflow;
		}

		public UnitRange Increment()
		{
			var newValue = Value + Step;

			return new UnitRange(new Options
			{
				InitialValue = newValue <= Max ? newValue : CanOverflow ? Min : throw new InvalidOperationException($"Valor máximo {Max} excedido"),
				Min = Min,
				Max = Max,
				Step = Step,
				CanOverflow = CanOverflow
			});

		}

		public UnitRange Decrement()
		{
			var newValue = Value - Step;

			return new UnitRange(new Options
			{
				InitialValue = newValue >= Min ? newValue : CanOverflow ? Max : throw new InvalidOperationException($"Valor mínimo {Min} excedido"),
				Min = Min,
				Max = Max,
				Step = Step,
				CanOverflow = CanOverflow
			});
		}

		public class Options
		{
			public int? InitialValue { get; set; }
			public int? Min { get; set; }
			public int Max { get; set; }
			public int? Step { get; set; }
			public bool? CanOverflow { get; set; }
		}
	}
}
