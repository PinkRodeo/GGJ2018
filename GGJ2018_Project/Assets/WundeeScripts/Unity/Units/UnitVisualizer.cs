using UnityEngine;

namespace WundeeUnity
{
    public class UnitVisualizer : MonoBehaviour 
	{
		[HideInInspector]
		public Transform _transform;

		public ParticleSystem movementDust;


		public Transform mainBody;
		public Transform head;

		// Use this for early referencing
		protected void Awake()
		{
			_transform = GetComponent<Transform>();
		}

		private static Color part_full_run = new Color(1f,1f,1f, 1f);
		private static Color part_lower_run = new Color(1f, 1f, 1f, 0.5f);


		private static Color part_full_walk = new Color(1f, 1f, 1f, 0.8f);
		private static Color part_lower_walk = new Color(1f, 1f, 1f, 0.3f);

		private static Color part_invisible = new Color(1f, 1f, 1f, 0.0f);




		public void C_Update(Unit unit)
		{
			if (unit.IsGrounded())
			{
				if (unit.IsRunning())
				{
					movementDust.startSpeed = Mathf.Lerp(-0.8f, -10.8f, unit._rigidBody.velocity.magnitude/13f);
					movementDust.startColor = Color.Lerp(part_lower_run, part_full_run, unit._rigidBody.velocity.magnitude/13f);
				}
				else
				{
					movementDust.startSpeed = Mathf.Lerp(-0.8f, -3.8f, unit._rigidBody.velocity.magnitude/13f);
					movementDust.startColor = Color.Lerp(part_lower_walk, part_full_walk, unit._rigidBody.velocity.magnitude/13f);

				}
			}
			else
			{
				movementDust.startColor = part_invisible;
			}



			var dt = Time.deltaTime;

			var curHeading = unit.GetCurrentHeading();

				Debug.DrawRay(_transform.position, curHeading.normalized, Color.black, dt, false);



			mainBody.rotation = Quaternion.Lerp(mainBody.rotation, Quaternion.LookRotation(curHeading.normalized), 0.2f);
			head.rotation = Quaternion.LookRotation(curHeading.normalized);

			/*
				_transform.rotation = Quaternion.RotateTowards(_transform.rotation,
					_transform.rotation * Quaternion.FromToRotation(_transform.forward, curHeading.normalized), 5f);
			*/
			//Quaternion.Euler(Vector3.RotateTowards(_transform.rotation.eulerAngles, curHeading.normalized, 10f, 10f));




		}
	}
}
