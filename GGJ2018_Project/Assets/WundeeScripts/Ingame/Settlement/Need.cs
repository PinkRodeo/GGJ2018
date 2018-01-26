namespace Wundee
{
	[System.Serializable]
	public class Need
	{
		public readonly Person owner;
		public readonly string type;


		public double amount = 50d;

		public Need(Person owner, string type)
		{
			this.owner = owner;
			this.type = type;
		}
	}
}