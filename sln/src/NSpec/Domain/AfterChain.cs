namespace NSpec.Domain
{
    public class AfterChain : TraversingHookChain
    {
        protected override bool CanRun(nspec instance)
        {
            return !context.BeforeAllChain.AnyThrew();
        }

        public AfterChain(Context context, Conventions conventions)
            : base(context, "after", "afterAsync", "after_each", reversed: true)
        {
            methodSelector = conventions.GetMethodLevelAfter;
            asyncMethodSelector = conventions.GetAsyncMethodLevelAfter;
            chainSelector = c => c.AfterChain;
        }
    }
}
