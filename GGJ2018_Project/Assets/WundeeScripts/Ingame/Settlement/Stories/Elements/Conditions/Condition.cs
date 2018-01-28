using LitJson;
using Kingdom;


namespace Wundee.Stories
{
    public abstract class Condition : StoryElement<Condition>
	{
		public abstract bool Check();
	}

	public abstract class CollectionCondition : Condition
	{
		protected Definition<Condition>[] _childConditionDefinitions;
		protected Condition[] childConditions;
	}

	public abstract class DecoratorCondition : Condition
	{
		private Definition<Condition> _childConditionDefinition;
		protected Condition childCondition;

		protected void _ParseChildCondition(JsonData parameters)
		{
			ContentHelper.VerifyKey(parameters, D.CONDITIONS, definition.definitionKey);
			var conditions = ConditionDefinition.ParseDefinitions(parameters[D.CONDITIONS], definition.definitionKey);

			ContentHelper.VerifyMaxArrayLength(conditions, 1, definition.definitionKey);
			if (conditions.Length == 1)
				_childConditionDefinition = conditions[0];
		}

		public override Condition GetClone(StoryNode parent)
		{
			var retValue = base.GetClone(parent) as DecoratorCondition;

			if (_childConditionDefinition != null)
				retValue.childCondition = _childConditionDefinition.GetConcreteType(parent);
			else
				retValue.childCondition = new TrueCondition();

			return retValue;
		}
	}

	public class AndCondition : CollectionCondition
	{
		public override void ParseParams(JsonData parameters)
		{
			_childConditionDefinitions = ConditionDefinition.ParseDefinitions(parameters[D.CONDITIONS], definition.definitionKey);
		}

		public override Condition GetClone(StoryNode parent)
		{
			var retValue = base.GetClone(parent) as AndCondition;

			retValue.childConditions = _childConditionDefinitions.GetConcreteTypes(parent);

			return retValue;
		}

		public override bool Check()
		{
			return childConditions.CheckConditions();
		}
	}

	public class OrCondition : CollectionCondition
	{
		public override void ParseParams(JsonData parameters)
		{
			_childConditionDefinitions = ConditionDefinition.ParseDefinitions(parameters[D.CONDITIONS], definition.definitionKey);
		}

		public override Condition GetClone(StoryNode parent)
		{
			var retValue = base.GetClone(parent) as OrCondition;

			retValue.childConditions = _childConditionDefinitions.GetConcreteTypes(parent);

			return retValue;
		}

		public override bool Check()
		{
			return childConditions.CheckOrConditions();
		}
	}

	public class NotCondition : DecoratorCondition
	{
		public override void ParseParams(JsonData parameters)
		{
			_ParseChildCondition(parameters);
		}

		public override bool Check()
		{
			return !childCondition.Check();
		}
	}


	public class TrueCondition : Condition
	{
		public override void ParseParams(JsonData parameters)
		{}

		public override bool Check()
		{
			return true;
		}
	}

	public class KeyHeldCondition : Condition
	{
		public override void ParseParams(JsonData parameters)
		{}

		public override bool Check()
		{
			if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.Q))
				return true;
			return false;
		}
	}

	public class LocationFlagCondition : Condition
	{
		private ushort _locationFlag;
		private Operator @operator;

		public override void ParseParams(JsonData parameters)
		{
			_locationFlag = ContentHelper.ParseLocationFlag(parameters);
			@operator = ContentHelper.ParseOperator(parameters, Operator.Equals);
		}

		public override bool Check()
		{
			var hasFlag = parentStoryNode.parentLocation.HasFlag(_locationFlag);

			switch (@operator)
			{
				case Operator.Equals:
					return hasFlag;
				case Operator.NotEquals:
					return !hasFlag;
				default:
					return hasFlag;
			}
		}
	}

	public class WorldFlagCondition : Condition
	{
		private ushort _worldFlag;
		private Operator @operator;

		public override void ParseParams(JsonData parameters)
		{
			_worldFlag = ContentHelper.ParseLocationFlag(parameters);
			@operator = ContentHelper.ParseOperator(parameters, Operator.Equals);
		}

		public override bool Check()
		{
			var hasFlag = Game.instance.world.HasFlag(_worldFlag);

			switch (@operator)
			{
				case Operator.Equals:
					return hasFlag;
				case Operator.NotEquals:
					return !hasFlag;
				default:
					return hasFlag;
			}
		}
	}

    public class GoldCondition : Condition
    {
        private int _amount;
        private Operator _operator;

        public override void ParseParams(JsonData parameters)
        {
            _amount = ContentHelper.ParseInt(parameters, D.AMOUNT, 0);
            _operator = ContentHelper.ParseOperator(parameters, Operator.Equals);
        }

        public override bool Check()
        {
            return _operator.CheckCondition(Game.instance.world.gold, _amount);
        }
    }

    public class PawnsCondition : Condition
    {
        private int _amount;
        private Operator _operator;

        public override void ParseParams(JsonData parameters)
        {
            _amount = ContentHelper.ParseInt(parameters, D.AMOUNT, 0);
            _operator = ContentHelper.ParseOperator(parameters, Operator.Equals);
        }

        public override bool Check()
        {
            return _operator.CheckCondition(Game.instance.world.pawns, _amount);
        }
    }

    public class ReputationCondition : Condition
    {
        private int _amount;
        private string _targetFaction;
        private Operator _operator;

        public override void ParseParams(JsonData parameters)
        {
            _amount = ContentHelper.ParseInt(parameters, D.AMOUNT, 0);
            _operator = ContentHelper.ParseOperator(parameters, Operator.Equals);
            _targetFaction = ContentHelper.ParseString(parameters, D.FACTION_KEY, parentStoryNode.parentLocation.owningFaction.definition.definitionKey);
        }

        public override bool Check()
        {
            return _operator.CheckCondition(parentStoryNode.parentLocation.world.factions[_targetFaction].reputation, _amount);
        }
    }
}

