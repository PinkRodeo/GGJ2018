using LitJson;
using Wundee.Stories;

namespace Wundee
{
    public class LocationDefinition : Definition<Location>
	{
		public Definition<Effect>[] _onStartRewardDefinitions;
		public Definition<Effect>[] _onTuneInRewardDefinitions;

		public string voiceType;
		public float voiceVolume;

		public string portraitKey;



		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
			this.definitionKey = definitionKey;

			var keys = jsonData.Keys;

			portraitKey = ContentHelper.ParseString(jsonData, D.PORTRAITKEY, "Portrait_1");
			voiceVolume = ContentHelper.ParseFloat (jsonData, "voiceVolume", 1.0f);

			if (keys.Contains(D.REWARDS_ON_START))
				this._onStartRewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS_ON_START], definitionKey);
			else
				this._onStartRewardDefinitions = new Definition<Effect>[0];

			this.voiceType = ContentHelper.ParseString (jsonData, "voiceKey", "A");

			if (keys.Contains(D.REWARDS_ON_TUNE))
				this._onTuneInRewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS_ON_TUNE], definitionKey);
			else
				this._onTuneInRewardDefinitions = new Definition<Effect>[0];

		}

		// parent == habitat
		public override Location GetConcreteType(object parent = null)
		{
			var newPerson = new Location(this);
			newPerson.ExecuteEffectFromDefinition(ref _onStartRewardDefinitions);
		

			return newPerson;
		}
	}
}

