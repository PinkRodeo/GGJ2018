using UnityEngine;
using Wundee;

namespace WundeeUnity
{
    public class GameEntry : MonoBehaviour
	{
		public Game game;
		public string gameParamsKey = "default";

		//[Header("Temp Debug")] public GameObject dockObject;

		protected virtual void Awake()
		{
			this.game = new Game(gameParamsKey, this);

            game.Initialize();
        }

		// Update is called once per frame
		protected void Update()
		{
			game.Update(UnityEngine.Time.deltaTime);
		}

		protected void FixedUpdate()
		{
			if (Input.GetKey(KeyCode.LeftBracket))
			{
				Wundee.Time.multiplier -= 0.5d;
				Debug.Log(Wundee.Time.multiplier);
			}

			if (Input.GetKey(KeyCode.RightBracket))
			{
				Wundee.Time.multiplier += 0.5d;
				Debug.Log(Wundee.Time.multiplier);
			}

			game.FixedUpdate(UnityEngine.Time.fixedDeltaTime);

			if (Input.GetKeyUp(KeyCode.R))
			{
				Game.instance.reloadGame = true;
				return;
			}


		}
	}

}
