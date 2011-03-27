using System;
using System.Linq;
using System.Reflection;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class ClassContext : Context
    {
        public ClassContext(Type type) : base(type.Name,0)
        {
            Type = type;

            var before = Enumerable.FirstOrDefault<MethodInfo>(Type.Methods(), m=>m.Name=="before_each");

            if (before != null) BeforeInstance = i => before.Invoke(i, null);
        }
    }
}