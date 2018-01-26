
using System;
using System.Collections.Generic;

namespace Wundee
{

	public enum Operator
	{
		Equals,
		NotEquals,
		GreaterEquals,
		Greater,
		LesserEquals,
		Lesser
	}

	public static class OperatorExtensions
	{
		public static Dictionary<string, Operator> stringToOperator
		{
			get
			{
				if (_stringToOperator == null)
				{
					_stringToOperator = new Dictionary<string, Operator>();

					var enumNames = Enum.GetNames(typeof(Operator));
					var enumValues = Enum.GetValues(typeof(Operator));

					for (int i = 0; i < enumNames.Length; i++)
					{
						_stringToOperator[enumNames[i]] = (Wundee.Operator)enumValues.GetValue(i);
					}
				}

				return _stringToOperator;
			}
		}

		private static Dictionary<string, Operator> _stringToOperator;

		public static bool CheckCondition(this Operator op, double lhv, double rhv)
		{
			switch (op)
			{
				case Operator.Equals:
					return lhv == rhv;
				case Operator.NotEquals:
					return lhv != rhv;
				case Operator.GreaterEquals:
					return lhv >= rhv;
				case Operator.Greater:
					return lhv > rhv;
				case Operator.LesserEquals:
					return lhv <= rhv;
				case Operator.Lesser:
					return lhv < rhv;
				default:
					throw new ArgumentOutOfRangeException("op", op, "Invalid operator " + op);
			}
		}
	}
}
