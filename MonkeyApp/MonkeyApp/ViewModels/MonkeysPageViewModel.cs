using System.Collections.Generic;

namespace MonkeyApp
{
	public class MonkeysPageViewModel
	{
		public IList<Monkey> Monkeys { get; private set; }

		public MonkeysPageViewModel()
		{
			Monkeys = MonkeyData.Monkeys;
		}
	}
}
