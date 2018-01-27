using UnityEngine;
using Wundee;

namespace WundeeUnity
{
    public class GameEntry : MonoBehaviour
	{
		public Game game;
		public string gameParamsKey = "default";

        public static Game gameInstance;

		//[Header("Temp Debug")] public GameObject dockObject;

		protected virtual void Awake()
		{
			this.game = new Game(gameParamsKey, this);
            gameInstance = this.game;

            game.Initialize();
        }

		// Update is called once per frame
		protected void Update()
		{
			game.Update(UnityEngine.Time.deltaTime);
		}
	}
}
