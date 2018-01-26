
using LitJson;


namespace Wundee.Stories
{
	public class ConditionDefinition : StoryElementDefinition<Condition>
	{
		public static Definition<Condition>[] ParseDefinitions(JsonData conditionData, string definitionKey)
		{
			return ContentHelper.GetDefinitions<ConditionDefinition, Condition>
				(conditionData, definitionKey, KEYS.CONDITION);
		}
	}
}