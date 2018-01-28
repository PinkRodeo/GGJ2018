
using System;
using Kingdom;

namespace Wundee
{
	public class Game
	{
		#region Private Fields

		private bool _isPlaying = true;

		private WundeeUnity.GameEntry _GameEntry;
        public WundeeUnity.GameEntry gameEntry
        {
            get
            {
                return _GameEntry;
            }
        }

		public DataLoader definitions; 

		#endregion

		#region Public Fields

		public GameParams @params;
		public World world;

		#endregion

		#region Public Properties

		public static Game instance;

		public bool isPlaying
		{
			get { return _isPlaying; }
		}


        public ConversationUI conversationUI
        {
            get
            {
                return m_ConversationUI;
            }
        }
        private ConversationUI m_ConversationUI;


		public bool reloadGame = false;

		#endregion

		public Game(string gameParamsKey, WundeeUnity.GameEntry mainMonoBehaviour)
		{
			Game.instance = this;
			
			this.definitions = new DataLoader();

			var gameParams = new GameParams();
			this.@params = gameParams;

			var gameParamsData = definitions.GetJsonDataFromYamlFile(DataLoader.GetContentFilePath() + "GameParams.yaml");
			ContentHelper.VerifyKey(gameParamsData, gameParamsKey, "PARAMS_READER");
			gameParams.InitializeFromData(gameParamsData[gameParamsKey]);

			this._GameEntry = mainMonoBehaviour;

			Time.gameTime = 0d;
			Time.realTime = 0d;

			Time.multiplier = gameParams.timeMultiplier;

			this.world = new World(this);

		}

		public void Initialize()
		{
			if (@params.parseDefinitions)
			{
				definitions.ParseDefinitions();
			}

            world.Initialize();
		}

        public void SetConversationUI(ConversationUI conversationUI)
        {
            m_ConversationUI = conversationUI;
        }

        public void Update(float dt)
		{
			Time.dt = (float)(dt * Time.multiplier);

			if (_isPlaying)
			{
				Time.gameTime += Time.dt;
			}

			Time.realTime += Time.dt;
		}

		public void FixedUpdate(float fixedDT)
		{
			if (reloadGame == true)
			{
				reloadGame = false;


				ReloadGame();
			}

			Time.fixedDT = (float)(fixedDT * Time.multiplier);

			if (_isPlaying)
			{
				Time.fixedGameTime += Time.fixedDT;
			}

			Time.fixedRealTime += Time.fixedDT;
		}

		private static void ReloadGame()
		{
			R.Reset();

			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, UnityEngine.SceneManagement.LoadSceneMode.Single);

		}
	}
}

