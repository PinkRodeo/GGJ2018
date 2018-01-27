using UnityEngine;
using Wundee;

namespace WundeeUnity
{
    public class GameEntry : MonoBehaviour
	{
		public Game game;
		public string gameParamsKey = "default";

        public static Game gameInstance;

		protected virtual void Awake()
		{
			this.game = new Game(gameParamsKey, this);
            gameInstance = this.game;

            game.Initialize();
        }

		protected void Update()
		{
			game.Update(UnityEngine.Time.deltaTime);
		}
	}
}
