using LitJson;

namespace Wundee.Stories
{
    public class EffectDefinition : StoryElementDefinition<Effect>
	{
		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
			base.ParseDefinition(definitionKey, jsonData);
		}
		
		public static Definition<Effect>[] ParseDefinitions(JsonData effectData, string definitionKey)
		{
			return ContentHelper.GetDefinitions<EffectDefinition, Effect>
				(effectData, definitionKey, KEYS.EFFECT);

		}
		
	}
}