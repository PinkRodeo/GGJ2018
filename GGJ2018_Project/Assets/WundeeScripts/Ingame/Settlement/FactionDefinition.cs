using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace Wundee
{
    public class FactionDefinition : Definition<Faction>
    {
        public int startingReputation;

        public string name;

        public override void ParseDefinition(string definitionKey, JsonData jsonData)
        {
            this.definitionKey = definitionKey;

            var keys = jsonData.Keys;

            this.name = ContentHelper.ParseString(jsonData, D.NAME, definitionKey);
            this.startingReputation = ContentHelper.ParseInt(jsonData, D.STARTING_REPUTATION, 50);
        }

        public override Faction GetConcreteType(object parent = null)
        {
            var newFaction = new Faction(this);

            return newFaction;
        }
    }
}
