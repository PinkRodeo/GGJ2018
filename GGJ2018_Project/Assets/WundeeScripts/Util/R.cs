

using System;

namespace Wundee
{
	public static class R
	{
		//public static System.Random generator = new System.Random(System.DateTime.Now.Millisecond);
		public static Random Content = new System.Random(System.DateTime.Now.Millisecond);

		public static Random Generation = new System.Random(System.DateTime.Now.Millisecond);

		public static Random WorldEffects = new Random(System.DateTime.Now.Millisecond);

		public static void Reset()
		{
			Content = new Random(0);
			Generation = new Random(0);
			WorldEffects = new Random(0);
		}

	}


}
