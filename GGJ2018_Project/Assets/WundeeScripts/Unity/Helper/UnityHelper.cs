using System.Linq;
using UnityEngine;

using System.Collections.Generic;


namespace WundeeUnity
{


    public static class UnityHelper
	{


		public static GameObject FindInChildren(this GameObject go, string name)
		{
			return (from x in go.GetComponentsInChildren<Transform>()
					where x.gameObject.name == name
					select x.gameObject).First();
		}

		public static Transform FindInChildren(this Transform go, string name)
		{
			return (from x in go.gameObject.GetComponentsInChildren<Transform>()
					where x.gameObject.name == name
					select x.gameObject).First().transform;
		}

		public static Vector3 RotateBy(this Vector3 vec, float angle)
		{
			return new Vector3
			{
				x = vec.x * Mathf.Cos(-angle) - vec.z * Mathf.Sin(-angle),
				z = vec.z * Mathf.Cos(-angle) + vec.x * Mathf.Sin(-angle)
			};
		}
	}


	public struct Vector2i
	{
		public int x;
		public int y;

		public Vector2i(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	[System.Serializable]
	public struct Vector2s : IEqualityComparer<Vector2s>
	{
		public short x;
		public short y;

		private readonly int hash;

		public Vector2s(short x)
		{
			this.x = x;
			this.y = 0;

			hash = MathS.PerfectlyHashThem(x, y);
		}

		public Vector2s(short x, short y)
		{
			this.x = x;
			this.y = y;

			hash = MathS.PerfectlyHashThem(x, y);
		}

		public override string ToString()
		{
			return ("x: " + x + ", y: " + y);
		}

		public bool Equals(Vector2s a, Vector2s b)
		{
			if (a.x == b.x && a.y == b.y)
				return true;

			return false;
		}
	
		public int GetHashCode(Vector2s obj)
		{
			return obj.hash;
		}
	}

	public struct Vector3i
	{
		public int x;
		public int y;
		public int z;
	}

	public struct Vector4i
	{
		public int x;
		public int y;
		public int z;
		public int w;
	}

	public class MathS
	{
		//http://stackoverflow.com/a/13871379/5946559
		public static long PerfectlyHashThem(int a, int b)
		{
			var A = (ulong)(a >= 0 ? 2 * (long)a : -2 * (long)a - 1);
			var B = (ulong)(b >= 0 ? 2 * (long)b : -2 * (long)b - 1);
			var C = (long)((A >= B ? A * A + A + B : A + B * B) / 2);
			return a < 0 && b < 0 || a >= 0 && b >= 0 ? C : -C - 1;
		}
		//http://stackoverflow.com/a/13871379/5946559
		public static int PerfectlyHashThem(short a, short b)
		{
			var A = (uint)(a >= 0 ? 2 * a : -2 * a - 1);
			var B = (uint)(b >= 0 ? 2 * b : -2 * b - 1);
			var C = (int)((A >= B ? A * A + A + B : A + B * B) / 2);
			return a < 0 && b < 0 || a >= 0 && b >= 0 ? C : -C - 1;
		}

		public static int FloorToInt(float value)
		{
			return (int)System.Math.Floor(value);
		}

		public static float Lerp(float from, float to, float value)
		{
			if (value < 0.0f)
				return from;
			else if (value > 1.0f)
				return to;
			return (to - from) * value + from;
		}

		public static float LerpUnclamped(float from, float to, float value)
		{
			return (1.0f - value) * from + value * to;
		}

		public static float InverseLerp(float from, float to, float value)
		{
			if (from < to)
			{
				if (value < from)
					return 0.0f;
				else if (value > to)
					return 1.0f;
			}
			else {
				if (value < to)
					return 1.0f;
				else if (value > from)
					return 0.0f;
			}
			return (value - from) / (to - from);
		}

		public static float InverseLerpUnclamped(float from, float to, float value)
		{
			return (value - from) / (to - from);
		}

		public static float SmoothStep(float from, float to, float value)
		{
			if (value < 0.0f)
				return from;
			else if (value > 1.0f)
				return to;
			value = value * value * (3.0f - 2.0f * value);
			return (1.0f - value) * from + value * to;
		}

		public static float SmoothStepUnclamped(float from, float to, float value)
		{
			value = value * value * (3.0f - 2.0f * value);
			return (1.0f - value) * from + value * to;
		}

		public static float SuperLerp(float from, float to, float from2, float to2, float value)
		{
			if (from2 < to2)
			{
				if (value < from2)
					value = from2;
				else if (value > to2)
					value = to2;
			}
			else {
				if (value < to2)
					value = to2;
				else if (value > from2)
					value = from2;
			}
			return (to - from) * ((value - from2) / (to2 - from2)) + from;
		}

		public static float SuperLerpUnclamped(float from, float to, float from2, float to2, float value)
		{
			return (to - from) * ((value - from2) / (to2 - from2)) + from;
		}

		public static Color ColorLerp(Color c1, Color c2, float value)
		{
			if (value > 1.0f)
				return c2;
			else if (value < 0.0f)
				return c1;
			return new Color(c1.r + (c2.r - c1.r) * value,
								c1.g + (c2.g - c1.g) * value,
								c1.b + (c2.b - c1.b) * value,
								c1.a + (c2.a - c1.a) * value);
		}

		public static Vector2 Vector2Lerp(Vector2 v1, Vector2 v2, float value)
		{
			if (value > 1.0f)
				return v2;
			else if (value < 0.0f)
				return v1;
			return new Vector2(v1.x + (v2.x - v1.x) * value,
								v1.y + (v2.y - v1.y) * value);
		}

		public static Vector3 Vector3Lerp(Vector3 v1, Vector3 v2, float value)
		{
			if (value > 1.0f)
				return v2;
			else if (value < 0.0f)
				return v1;
			return new Vector3(v1.x + (v2.x - v1.x) * value,
								v1.y + (v2.y - v1.y) * value,
								v1.z + (v2.z - v1.z) * value);
		}

		public static Vector3 Vector3LerpUnclamped(Vector3 v1, Vector3 v2, float value)
		{
			return new Vector3(v1.x + (v2.x - v1.x) * value,
								v1.y + (v2.y - v1.y) * value,
								v1.z + (v2.z - v1.z) * value);
		}

		public static Vector4 Vector4Lerp(Vector4 v1, Vector4 v2, float value)
		{
			if (value > 1.0f)
				return v2;
			else if (value < 0.0f)
				return v1;
			return new Vector4(v1.x + (v2.x - v1.x) * value,
								v1.y + (v2.y - v1.y) * value,
								v1.z + (v2.z - v1.z) * value,
								v1.w + (v2.w - v1.w) * value);
		}
	
		public static float Vector2DistanceSquared(Vector2 pointA, Vector2 pointB)
		{
			return (float) System.Math.Sqrt(Vector2.Distance(pointA, pointB));
		}

		public static float Vector3DistanceSquared(Vector3 pointA, Vector3 pointB)
		{
			return (float)System.Math.Sqrt(Vector3.Distance(pointA, pointB));
		}


		// source: http://theinstructionlimit.com/squaring-the-thumbsticks
		public static Vector2 CircleToSquare(Vector2 point, float innerRoundness = 0f)
		{
			const float Pi = Mathf.PI;
			const float PiOverFour = Mathf.PI/4f;

			// Determine the theta angle
			var angle = Mathf.Atan2(point.y, point.x) + Pi;

			Vector2 squared;

			// Scale according to which wall we're clamping to
			// X+ wall
			if (angle <= PiOverFour || angle > 7f*PiOverFour)
				squared = point*(1f/Mathf.Cos(angle));
			// Y+ wall
			else if (angle > PiOverFour && angle <= 3f*PiOverFour)
				squared = point*(1f/Mathf.Sin(angle));
			// X- wall
			else if (angle > 3f*PiOverFour && angle <= 5f*PiOverFour)
				squared = point*(-1f/Mathf.Cos(angle));
			// Y- wall
			else if (angle > 5f*PiOverFour && angle <= 7f*PiOverFour)
				squared = point*(-1f/Mathf.Sin(angle));
			else throw new System.InvalidOperationException("Invalid angle...?");

			// Early-out for a perfect square output
			if (innerRoundness == 0f)
				return squared;

			// Find the inner-roundness scaling factor and LERP
			var length = point.magnitude;
			var factor = Mathf.Pow(length, innerRoundness);
			return Vector2Lerp(point, squared, factor);
		}
	}
}