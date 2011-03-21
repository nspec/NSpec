using System.Linq.Expressions;
using NSpec.Domain;
using System;

namespace NSpec
{
    /// <summary>
    /// Inherit from this class to create your own specifications.  NSpecRunner will look through your project for 
    /// classes that derive from this class (inheritance chain is taken into consideration).
    /// </summary>
    public class nspec : SpecInterpreterBase
    {
        public nspec()
        {
            context = new ActionRegister(AddContext);
            describe = new ActionRegister(AddContext);

            it = new ActionRegister((name, action) => Exercise(new Example(name, action)));
            xit = new ActionRegister((name, action) => Pending(new Example(name, action, pending: true)));
        }

        protected Expression<Action> specify
        {
            set{ Exercise(new Example(value));}
        }

        /// <summary>
        /// Assign this member within your context.  The Action that is assigned to this member variable will get executed
        /// before an example is run.
        /// <para>For Example:</para>
        /// <para>before.each = () => someList = new List&lt;int&gt;();</para>
        /// <para>The before.each can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspecdriven.net</para>
        /// </summary>
        protected Action before
        {
            get { return Context.Before; }
            set { Context.Before = value; }
        }

        /// <summary>
        /// In development.
        /// </summary>
        protected Action after
        {
            get { return Context.After; }
            set { Context.After = value; }
        }

        /// <summary>
        /// Assign this member within your context.  The Action that is assigned to this member variable will get executed
        /// with ever example in scope.  Befores will run first, then Acts will run, then your examples will be executed.  It's a way for you to specify a common Act in Arrange-Act-Assert.  For more information visit http://www.nspecdriven.net
        /// </summary>
        protected Action act
        {
            get { return Context.Act; }
            set { Context.Act = value; }
        }

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
        /// This is your specification/examples registry.  Within your contexts, specify assersions via this member.  
        /// <para>For Example:</para>
        /// <para>it["should return false"] = () => _controller.should_be(false);</para>
        /// <para>(Extension methods are in the NSpec.Extensions namespace)</para>
        /// </summary>
        protected ActionRegister it;

        /// <summary>
        /// This is your pending examples registry.  If you have an example already specified that you want to ignore,
        /// do so by putting the letter x infront of it.
        /// <para>For Example:</para>
        /// <para>xit["should return false"] = () => _controller.should_be(false);</para>
        /// <para>(the example will be marked as pending any lambda provided will not be executed)</para>
        /// </summary>
        protected ActionRegister xit;

        /// <summary>
        /// Set a registry entry to this lambda if you want to mark a test as todo.
        /// <para>For Example:</para>
        /// <para>it["a test i haven't flushed out yet, but need to"] = todo;</para>
        /// </summary>
        protected readonly Action todo = () => { throw new PendingExampleException(); };
    }
}