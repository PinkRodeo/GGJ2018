using UnityEngine;

namespace WundeeUnity
{
    [RequireComponent(typeof(Rigidbody))]
	public abstract class Unit : MonoBehaviour
	{
		[HideInInspector]
		public Rigidbody _rigidBody;
		[HideInInspector]
		public Transform _transform;
		public Collider _characterCollider;
		public UnitVisualizer _unitVisualizer;


		public double health;

		//private float _distanceToGround;


		[Header("Physics")]
		[SerializeField]
		protected Vector2 baseDrag = new Vector2(3f, 0f);
		[SerializeField]
		protected Vector2 runningDrag = new Vector2(2f, 0f);
		[SerializeField, ReadOnly]
		protected Vector2 currentDrag = new Vector2(0f, 0f);

		protected bool _isRunning;

		[HideInInspector]
		protected Vector3 previousInput;
		protected Vector3 previousHeading;

		protected Vector3 currentMovementVelocity;

		public virtual Vector3 GetCurrentHeading()
		{
			return previousHeading;
		}

		public virtual bool IsRunning()
		{
			return _isRunning;
		}

		[Header("Weapons")]
		[SerializeField]
		protected Weapon[] _weapons;

		// Use this for early referencing
		protected virtual void Awake()
		{
			// TODO: STEB: Fix dis
			//GameGlobals.units.Add(this);

			_rigidBody = GetComponent<Rigidbody>();

			//_distanceToGround = _characterCollider.bounds.extents.y;
			_transform = GetComponent<Transform>();
		}

		public bool IsGrounded()
		{

			
			return Physics.Raycast(_transform.position + Vector3.up*0.1f, -Vector3.up, _characterCollider.contactOffset + 0.1f);
		}
	}
}
