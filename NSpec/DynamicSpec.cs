using System.Dynamic;

namespace NSpec
{
    public class DynamicSpec : DynamicObject
    {
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            //_dictionary[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            //return _dictionary.TryGetValue(binder.Name, out result);
            result = null;
            return true;
        }
    }
}