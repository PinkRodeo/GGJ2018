using UnityEngine;

namespace WundeeUnity
{
    public class Fist : Weapon 
	{
		[HideInInspector]
		public SphereCollider _sphereCollider;

		private float timeOfPreviousAttack;
		
		[SerializeField]
		private float attackDelay = 1f;

		[SerializeField]
		private float attackDuration = 0.5f;

		[SerializeField]
		private AnimationCurve _movementCurve;

		private Vector3 _originalFistPosition;
		private Vector3 _currentFistPosition;


		protected override void Awake()
		{
			base.Awake();

			_originalFistPosition = _transform.localPosition;
			_currentFistPosition = new Vector3(	_originalFistPosition.x,
												_originalFistPosition.y,
												_originalFistPosition.z);

			_sphereCollider = GetComponent<SphereCollider>();
		}

		public override void DoAttack()
		{
			
			if (IsReadyForAttack())
			{
				timeOfPreviousAttack = Time.time;
				_collidersHit.Clear();
				_isAttacking = true;
			}
		}

		private Collider[] colliderBuffer = new Collider[20];

		protected void FixedUpdate()
		{
			if (_isAttacking)
			{
				var currentSample = _movementCurve.Evaluate((Time.time - timeOfPreviousAttack)/attackDuration);

				_currentFistPosition.z = currentSample/100f + _originalFistPosition.z;

				_transform.localPosition = _currentFistPosition;

				var count = Physics.OverlapSphereNonAlloc(_transform.position, 1f * currentSample, colliderBuffer, 1 << HitCollider.layerMask);

				DebugExtension.DebugWireSphere(_transform.position, Color.red, 1f * currentSample, Time.fixedDeltaTime, true);

				if (count >= 20)
					Debug.LogWarning("Increase collider buffer size!!!");

				for (int i = 0; i < count; i++)
				{
					AttackCollider(colliderBuffer[i].GetComponent<HitCollider>());
				}

				var scale = Mathf.Max(1f, currentSample / 2f + 1f);
				_transform.localScale = new Vector3(scale, scale, scale);

				if (currentSample == Mathf.Epsilon)
					_isAttacking = false;

			}
		}

		public override bool IsReadyForAttack()
		{
			return Time.time > (timeOfPreviousAttack + attackDelay);
		}
	}
}
