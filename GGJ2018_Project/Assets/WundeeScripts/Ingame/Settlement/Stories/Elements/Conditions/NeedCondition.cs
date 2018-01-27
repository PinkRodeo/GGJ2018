namespace Wundee.Stories
{
    /*
        public class NeedCondition : Condition
        {
            private double _amount;
            private int _needIndex;
            private Operator _operator;

            public override void ParseParams(JsonData parameters)
            {
                ContentHelper.VerifyKey(parameters, D.AMOUNT, definition.definitionKey);
                ContentHelper.VerifyDouble(parameters, D.AMOUNT, definition.definitionKey);
                _amount = ContentHelper.ParseDouble(parameters, D.AMOUNT, 0d);

                ContentHelper.VerifyKey(parameters, D.NEED, definition.definitionKey);
                _needIndex = ContentHelper.ParseNeedIndex(parameters, D.NEED);

                ContentHelper.VerifyOperator(parameters, D.OPERATOR, definition.definitionKey);
                _operator = ContentHelper.ParseOperator(parameters);
            }

            public override bool Check()
            {
                return _operator.CheckCondition(parentStoryNode.parentStory.parentSettlement.needs[_needIndex].amount, _amount);
            }
        }
        */


}

