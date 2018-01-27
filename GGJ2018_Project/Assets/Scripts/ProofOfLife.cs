using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Wundee;

namespace Kingdom
{
    public class ProofOfLife : WundeeUnity.GameEntry
    {
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            var person = new Location(game.definitions.personDefinitions["PERSON_DEFAULT_01"]);

        }
    }
}
