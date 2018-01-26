namespace Wundee
{
	// High level simulation of settlement


	// Contains the current standing of the Settlement and potential Trends for a 
	// ActiveSettlement to represent
	public class WorldSettlement
	{
		private Person _person;

		public WorldSettlement(Person p_Person)
		{
			this._person = p_Person;
		}

		public void Tick(double deltaTime)
		{
		}
	}
}