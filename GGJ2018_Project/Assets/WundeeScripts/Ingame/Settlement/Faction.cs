using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wundee
{
    public class Faction
    {
        public FactionDefinition definition;

        public int reputation
        {
            get
            {
                return _reputation;
            }
            set
            {
                var previousValue = _reputation;
                _reputation = value;

                OnReputationChanged(previousValue, _reputation, this);
            }
        }

        private int _reputation;

        /// <summary>
        /// previousAmount, newAmount, faction
        /// </summary>
        public System.Action<int, int, Faction> OnReputationChanged = delegate { };


        public Faction(FactionDefinition p_Definition)
        {
            this.definition = p_Definition;

            _reputation = definition.startingReputation;

        }
    }

}
