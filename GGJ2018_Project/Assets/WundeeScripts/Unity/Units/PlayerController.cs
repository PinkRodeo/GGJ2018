using UnityEngine;

namespace WundeeUnity
{
    public class PlayerController : Unit
	{
		[Header("Movement/Input")]

		[Range(0.5f, 20f), SerializeField]
		private float globalForceMultiplier = 1f;

		[Range(0.5f, 200f), SerializeField]
		private float directionalForceMultiplier = 1f;

		[Range(0.5f, 60f), SerializeField]
		private float jumpForce = 1f;

		private CapsuleCollider _characterCapsuleCollider;

		public bool enablePredictiveMovement = true;

		// Use this for early referencing
		protected override void Awake()
		{
			base.Awake();

			_rigidBody.drag = 0f;

			currentDrag = baseDrag;

			previousHeading = _transform.forward;

			_characterCapsuleCollider = _characterCollider as CapsuleCollider;
		}

		// Use this for initialization
		private void Start () 
		{
		
		}

		private void FixedUpdate()
		{


			var dt = Time.fixedDeltaTime;

			/*
			{
				var rigidBodyVelocity = _rigidBody.velocity*_rigidBody.mass;

				if (rigidBodyVelocity.sqrMagnitude < currentMovementVelocity.sqrMagnitude)
				{
					currentMovementVelocity = rigidBodyVelocity;
				}
			}
			*/

	//		_rigidBody.velocity = currentMovementVelocity / _rigidBody.mass;


			// Polling input
			var horizontalInput = Input.GetAxis("Horizontal");
			var verticalInput = Input.GetAxis("Vertical");
			
			var directionalInput = new Vector3(horizontalInput, 0, verticalInput);

			previousInput = directionalInput;

			var runInput = Input.GetAxis("Run");
			var jumpInput = Input.GetButtonDown("Jump");

			if (Input.GetButtonDown("Fire1"))
			{
				
	//			Debug.Log("Attacking");

				if (_weapons[0].IsReadyForAttack())
					_weapons[0].DoAttack();
				else
					_weapons[1].DoAttack();

			}

			if (jumpInput && IsGrounded())
			{
				_rigidBody.AddForce(Vector3.up * jumpForce * globalForceMultiplier * dt, ForceMode.VelocityChange);
			}




			if (directionalInput.magnitude < 0.2f)
			{
				const float maxDrag = 5f;

				currentDrag.x = Mathf.MoveTowards(currentDrag.x, maxDrag, Time.deltaTime * (maxDrag - baseDrag.x )  *5f);
			}
			else
			{
				if (runInput > 0.8f)
				{
					currentDrag.x = runningDrag.x;
					_isRunning = true;
				}
				else
				{
					currentDrag.x = baseDrag.x;
					_isRunning = false;
				}
			}


			if (enablePredictiveMovement)
			{
				if (IsRunning())
					directionalInput = AdjustDirectionForObstacles(directionalInput, 80f);
				else
					directionalInput = AdjustDirectionForObstacles(directionalInput, MathS.Lerp(0f, 45f, currentMovementVelocity.magnitude/200f - 0.3f));
			}

			#region capsuleAvoidance
			/*
			var capsuleStart = cap.center + _transform.position;
			capsuleStart.y -= (cap.height / 2f);

			const float distanceAhead = 1.2f;


			var capStartForward = capsuleStart + directionalInput.normalized * 0.3f;

			var radius = cap.radius * 0.9f;

			DebugExtension.DebugCapsule(capStartForward,
				capStartForward + Vector3.up * (cap.height),
				Color.white, radius, dt, false);

			if (Physics.CheckCapsule(capStartForward,
				capStartForward + Vector3.up*(cap.height),
				radius, 1 << HitCollider.layerMask))
			{
				const float angle = Mathf.Deg2Rad * 45f;

				var rotatedLeft = new Vector3
				{
					x = directionalInput.x*Mathf.Cos(angle) - directionalInput.z*Mathf.Sin(angle),
					z = directionalInput.z*Mathf.Cos(angle) + directionalInput.x*Mathf.Sin(angle)
				};


				capStartForward = capsuleStart + rotatedLeft.normalized * distanceAhead;
				
				DebugExtension.DebugCapsule(capStartForward,
					capStartForward + Vector3.up * (cap.height),
					Color.red, radius, dt, false);

				if (!Physics.CheckCapsule(capStartForward,
					capStartForward + Vector3.up*(cap.height),
					radius, 1 << HitCollider.layerMask))
				{
					directionalInput = rotatedLeft;
					//currentMovementVelocity = currentMovementVelocity.RotateBy(-Vector3.Angle(directionalInput, currentMovementVelocity) * Mathf.Deg2Rad);

				}
				else
				{
					var rotatedRight = new Vector3
					{
						x = directionalInput.x * Mathf.Cos(-angle) - directionalInput.z * Mathf.Sin(-angle),
						z = directionalInput.z * Mathf.Cos(-angle) + directionalInput.x * Mathf.Sin(-angle)
					};


					capStartForward = capsuleStart + rotatedRight.normalized * distanceAhead;

					DebugExtension.DebugCapsule(capStartForward,
						capStartForward + Vector3.up * (cap.height),
						Color.blue, radius, dt, false);

					if (!Physics.CheckCapsule(capStartForward,
						capStartForward + Vector3.up*(cap.height),
						radius, 1 << HitCollider.layerMask))
					{
						directionalInput = rotatedRight;

						//currentMovementVelocity = currentMovementVelocity.RotateBy(Vector3.Angle(directionalInput, currentMovementVelocity)*Mathf.Deg2Rad);

						//currentMovementVelocity = currentMovementVelocity.RotateBy(-angle);
					}
				}
				


			}
			*/
			#endregion


			if (directionalInput.magnitude > 0.1f)
			{
				currentMovementVelocity = Vector3.Project(currentMovementVelocity, directionalInput);

				previousHeading = directionalInput;

			}
		


			//DebugExtension.DebugArrow(_transform.position, directionalInput.normalized * 5f, Color.gray, dt, false);

			DebugExtension.DebugArrow(_transform.position, currentMovementVelocity * dt, Color.blue, dt * 10f, false);


			/*
			_rigidBody.AddForce(directionalInput * directionalForceMultiplier * globalForceMultiplier * dt, ForceMode.Acceleration);

			// Applying drag
			var curVelocity = _rigidBody.velocity;


			curVelocity.x = curVelocity.x * (1f - dt * currentDrag.x);
			curVelocity.z = curVelocity.z * (1f - dt * currentDrag.x);
			// Vertical drag
			curVelocity.y = curVelocity.y * (1f - dt * currentDrag.y);

			_rigidBody.velocity = curVelocity;
			//*/

			currentMovementVelocity += directionalInput*directionalForceMultiplier*globalForceMultiplier*dt;

			currentMovementVelocity.x = currentMovementVelocity.x * (1f - dt * currentDrag.x);
			currentMovementVelocity.z = currentMovementVelocity.z * (1f - dt * currentDrag.x);

			if (IsGrounded() == false)
			{
				_rigidBody.velocity = currentMovementVelocity/_rigidBody.mass + Vector3.down*2f;

			}
			else
			{
				_rigidBody.velocity = currentMovementVelocity / _rigidBody.mass;
			}

		}

		protected Vector3 AdjustDirectionForObstacles(Vector3 initialDirection, float maxRotation = 65f)
		{
			const float rotationSteps = 5f;
			
			float rotationAmount = 0f;

			Vector3 adjustedDirection = new Vector3(initialDirection.x, initialDirection.y, initialDirection.z);

			int numberOfLoops = 0;

			while (Mathf.Abs(rotationAmount) <= maxRotation && numberOfLoops < 100)
			{
				numberOfLoops++;

				float middleSpace, leftSpace, rightSpace;

				TestProng(adjustedDirection.normalized, out middleSpace, out leftSpace, out rightSpace);

				const float margin = 0.1f;

				// rotate left
				if (Mathf.Approximately(leftSpace, rightSpace))
				{
					return adjustedDirection;
				}
				else if (leftSpace > rightSpace)
				{
					// try steer right
					if (middleSpace < leftSpace - margin || rightSpace < middleSpace - margin)
					{
						adjustedDirection = adjustedDirection.RotateBy(rotationSteps*Mathf.Deg2Rad);
						rotationAmount += rotationSteps;
					}
					// try steer left
					else if (middleSpace < rightSpace - margin || leftSpace < middleSpace - margin)
					{
						adjustedDirection = adjustedDirection.RotateBy(-rotationSteps*Mathf.Deg2Rad);
						rotationAmount -= rotationSteps;
					}
					else
					{
						return adjustedDirection;
					}
				}
				else
				{
					// try steer left
					if (middleSpace < rightSpace - margin || leftSpace < middleSpace - margin)
					{
						adjustedDirection = adjustedDirection.RotateBy(-rotationSteps * Mathf.Deg2Rad);
						rotationAmount -= rotationSteps;
					}
					// try steer right
					else if (middleSpace < leftSpace - margin || rightSpace < middleSpace - margin)
					{
						adjustedDirection = adjustedDirection.RotateBy(rotationSteps * Mathf.Deg2Rad);
						rotationAmount += rotationSteps;
					}
					else
					{
						return adjustedDirection;
					}
				}

			}
			
			//if (numberOfLoops > 80)
				//Debug.Log(numberOfLoops);

			return adjustedDirection;
		}

		private RaycastHit[] rayCastBuffer = new RaycastHit[20];

		protected void TestProng(Vector3 direction, out float middleSpace, out float leftSpace, out float rightSpace)
		{

			var cap = _characterCapsuleCollider;

			var center = cap.center;

			var sideWaysDirection = direction.RotateBy(90f * Mathf.Deg2Rad);
			
			var middleRay = new Ray(_transform.position + center, direction);
			var leftRay = new Ray(_transform.position + center + sideWaysDirection*cap.radius, direction);
			var rightRay = new Ray(_transform.position + center - sideWaysDirection * cap.radius, direction);

			middleSpace = GetSpaceAhead(middleRay);
			leftSpace = GetSpaceAhead(leftRay);
			rightSpace = GetSpaceAhead(rightRay);


			var dt = Time.fixedDeltaTime;

			DebugExtension.DebugPoint(_transform.position + center, Color.yellow, 1f, dt, false);
			DebugExtension.DebugRay(_transform.position + center, direction * middleSpace, Color.blue, dt, false);
			
			DebugExtension.DebugPoint(_transform.position + center + sideWaysDirection * cap.radius, new Color(1, 0, 0, 0.5f), 1f, dt);
			DebugExtension.DebugRay(_transform.position + center + sideWaysDirection * cap.radius, direction * leftSpace, Color.blue, dt, false);

			DebugExtension.DebugPoint(_transform.position + center - sideWaysDirection * cap.radius, new Color(1, 0, 0, 0.5f), 1f, dt);
			DebugExtension.DebugRay(_transform.position + center - sideWaysDirection * cap.radius, direction * rightSpace, Color.blue, dt, false);
		}

		private float GetSpaceAhead(Ray ray)
		{
			const float lookAheadDist = 3f;
			
			var numHits = Physics.RaycastNonAlloc(ray, rayCastBuffer, lookAheadDist, 1 << HitCollider.layerMask);
			var shortestDistance = lookAheadDist;

			for (int i = 0; i < numHits; i++)
			{
				if (rayCastBuffer[i].distance < shortestDistance)
					shortestDistance = rayCastBuffer[i].distance;
			}

			return shortestDistance;
		}

		// Update is called once per frame
		private void Update ()
		{
			_unitVisualizer.C_Update(this);


		}
	}
}
