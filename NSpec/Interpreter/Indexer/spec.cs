using NSpec.Domain;
using NSpec.Interpreter;
using NSpec.Interpreter.Indexer;

namespace NSpec
{
    /// <summary>
    /// Inherit from this class to create your own specifications.  NSpecRunner will look through your project for 
    /// classes that derive from this class (inheritance chain is taken into consideration).
    /// </summary>
    public class spec : SpecInterpreterBase
    {
        /// <summary>
        /// Assign this member within your context.  The Action that is assigned to this member variable will get executed
        /// before an example is run.
        /// <para>For Example:</para>
        /// <para>before.each = () => someList = new List&lt;int&gt;();</para>
        /// <para>The before.each can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspecdriven.net</para>
        /// </summary>
        protected ActionRegister before;

        /// <summary>
        /// In development.
        /// </summary>
        protected ActionRegister after;

        /// <summary>
        /// Assign this member within your context.  The Action that is assigned to this member variable will get executed
        /// with ever example in scope.  Befores will run first, then Acts will run, then your examples will be executed.  It's a way for you to specify a common Act in Arrange-Act-Assert.  For more information visit http://www.nspecdriven.net
        /// </summary>
        protected ActionRegister act;

        /// <summary>
        /// This is your context registry.  Use this to create sub contexts within your methods.
        /// <para>For Examples see http://www.nspecdriven.net</para>
        /// </summary>
        protected ActionRegister context;

        /// <summary>
        /// This is an alias for the context registry.  Use this to create sub contexts within your methods.
        /// <para>For Examples see http://www.nspecdriven.net</para>
        /// </summary>
        protected ActionRegister describe;

        /// <summary>
        /// In development.
        /// </summary>
        protected ActionRegister specify;

        /// <summary>
        /// This is your specification/examples registry.  Within your contexts, specify assersions via this member.  
        /// <para>For Example:</para>
        /// <para>it["should return false"] = () => _controller.should_be(false);</para>
        /// <para>(Extension methods are in the NSpec.Extensions namespace)</para>
        /// </summary>
        protected ActionRegister it;

        public spec()
        {
            before = new ActionRegister( (f,b) =>
            {
                Context.BeforeFrequency = f;
                Context.Before = b;
            });

            after = new ActionRegister( (f, a) =>
            {
                Context.AfterFrequency = f;
                Context.After = a;
            });

            act = new ActionRegister((f, a) =>
            {
                Context.Act = a;
                Context.ActFrequency = f;
            });

            context = new ActionRegister(AddContext);
            describe = new ActionRegister(AddContext);

            specify = new ActionRegister((name,action)=> Exercise(new Example(name),action));
            it = new ActionRegister((name,action)=> Exercise(new Example(name),action));
        }
    }
}