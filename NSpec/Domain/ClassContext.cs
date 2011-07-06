using System;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class ClassContext : Context
    {
        public void Build()
        {
            BuildMethodLevelBefore();

            BuildMethodLevelAct();
        }

        private void BuildMethodLevelBefore()
        {
            var before = conventions.GetMethodLevelBefore(type);

            if (before != null) BeforeInstance = i => before.Invoke(i, null);
        }

        private void BuildMethodLevelAct()
        {
            var act = conventions.GetMethodLevelAct(type);

            if (act != null) ActInstance = i => act.Invoke(i, null);
        }

        public ClassContext(Type type, Conventions conventions = null)
            : base(type.Name, 0)
        {
            this.type = type;

            this.conventions = conventions ?? new DefaultConventions().Initialize();
        }

        public override void Run(nspec instance = null)
        {
            var nspec = type.Instance<nspec>();

            base.Run(nspec);

            //haven't figured out how to write a failing test but
            //using regular iteration causes Collection was modified
            //exception when running samples (rake samples)
            for (int i = 0; i < Examples.Count; i++)
                Run(Examples[i], nspec);
        }

        public override bool IsSub(Type baseType)
        {
            return baseType == type;
        }

        Conventions conventions;
        Type type;

    }
}