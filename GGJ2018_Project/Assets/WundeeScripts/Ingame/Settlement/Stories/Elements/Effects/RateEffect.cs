namespace Wundee.Stories
{

    public interface IRateEffect
	{

	}

	/*
	public class NeedRateEffect : Effect, IRateEffect
	{
		private double _rate;
		private int _needIndex;
		
		public override void ParseParams(JsonData parameters)
		{
			_rate = ContentHelper.ParseDouble(parameters, D.RATE, 0d);
			_needIndex = ContentHelper.ParseNeedIndex(parameters, D.NEED);
		}

		public override void ExecuteEffect()
		{
			parentStoryNode.parentStory.parentSettlement.needs[_needIndex].amount += _rate * Time.fixedDT;
		}
	}

	public class NeedAmountEffect : Effect, IRateEffect
	{
		private double _amount;
		private int _needIndex;

		public override void ParseParams(JsonData parameters)
		{
			_amount = ContentHelper.ParseDouble(parameters, D.AMOUNT, 0d);
			_needIndex = ContentHelper.ParseNeedIndex(parameters, D.NEED);
		}

		public override void ExecuteEffect()
		{
			parentStoryNode.parentStory.parentSettlement.needs[_needIndex].amount += _amount;
		}
	}
	*/
}

