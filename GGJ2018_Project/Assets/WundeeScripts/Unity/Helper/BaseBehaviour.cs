using UnityEngine;

namespace WundeeUnity
{
    public class BaseBehaviour : MonoBehaviour
	{
		[HideInInspector]
		public Transform _transform;

		// Use this for early referencing
		protected virtual void Awake()
		{
			_transform = GetComponent<Transform>();
		}

	}
}

