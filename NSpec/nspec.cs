using System;
using System.Linq.Expressions;
using NSpec.Domain;

namespace NSpec
{
    /// <summary>
    /// Inherit from this class to create your own specifications.  NSpecRunner will look through your project for 
    /// classes that derive from this class (inheritance chain is taken into consideration).
    /// </summary>
    public class nspec
    {
        public nspec()
        {
            context = new ActionRegister(AddContext);
            xcontext = new ActionRegister(AddIgnoredContext);
            describe = new ActionRegister(AddContext);
            xdescribe = new ActionRegister(AddIgnoredContext);

            it = new ActionRegister((name, tags, action) => AddExample(new Example(name, tags, action, pending: action == todo)));
            xit = new ActionRegister((name, tags, action) => AddExample(new Example(name, tags, action, pending: true)));
        }

        /// <summary>
        /// Create a specification/example using a single line lambda with an assertion(should). 
        /// The name of the specification will be parsed from the Expression
        /// <para>For Example:</para>
        /// <para>specify = () => _controller.should_be(false);</para>
        /// </summary>
        public virtual Expression<Action> specify
        {
            set { AddExample(new Example(value)); }
        }

        /// <summary>
        /// Mark a spec as pending 
        /// <para>For Example:</para>
        /// <para>xspecify = () => _controller.should_be(false);</para>
        /// <para>(the example will be marked as pending any lambda provided will not be executed)</para>
        /// </summary>
        public virtual Expression<Action> xspecify
        {
            set { AddExample(new Example(value, pending:true)); }
        }

        /// <summary>
        /// This Action gets executed before each example is run.
        /// <para>For Example:</para>
        /// <para>before.each = () => someList = new List&lt;int&gt;();</para>
        /// <para>The before.each can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Action before
        {
            get { return Context.Before; }
            set { Context.Before = value; }
        }

        /// <summary>
        /// This Action is an alias of before. This Action get executed before each example is run.
        /// <para>For Example:</para>
        /// <para>before.each = () => someList = new List&lt;int&gt;();</para>
        /// <para>The before.each can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Action beforeEach
        {
            get { return Context.Before; }
            set { Context.Before = value; }
        }

        /// <summary>
        /// This Action gets executed before all examples in a context.
        /// <para>For Example:</para>
        /// <para>beforeAll = () => someList = new List&lt;int&gt;();</para>
        /// <para>The beforeAll can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Action beforeAll
        {
            get { return Context.BeforeAll; }
            set { Context.BeforeAll = value; }
        }

        /// <summary>
        /// In development.
        /// </summary>
        public virtual Action after
        {
            get { return Context.After; }
            set { Context.After = value; }
        }

        /// <summary>
        /// This Action is an alias of after. This Action get executed after each example is run.
        /// <para>For Example:</para>
        /// <para>afterEach = () => someList = new List&lt;int&gt;();</para>
        /// <para>The afterEach can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Action afterEach
        {
            get { return Context.After; }
            set { Context.After = value; }
        }


        /// <summary>
        /// This Action gets executed after all examples in a context.
        /// <para>For Example:</para>
        /// <para>afterAll = () => someList = new List&lt;int&gt;();</para>
        /// <para>The afterAll can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Action afterAll
        {
            get { return Context.AfterAll; }
            set { Context.AfterAll = value; }
        }

        /// <summary>
        /// Assign this member within your context.  The Action assigned will get executed
        /// with every example in scope.  Befores will run first, then acts, then your examples.  It's a way for you to define once a common Act in Arrange-Act-Assert for all subcontexts.  For more information visit http://www.nspec.org
        /// </summary>
        public virtual Action act
        {
            get { return Context.Act; }
            set { Context.Act = value; }
        }

        /// <summary>
        /// Create a subcontext.
        /// <para>For Examples see http://www.nspec.org</para>
        /// </summary>
        public ActionRegister context;

        /// <summary>
        /// Mark a subcontext as pending (add all child contexts as pending)
        /// </summary>
        public ActionRegister xcontext;

        /// <summary>
        /// This is an alias for creating a subcontext.  Use this to create sub contexts within your methods.
        /// <para>For Examples see http://www.nspec.org</para>
        /// </summary>
        public ActionRegister describe;

        /// <summary>
        /// This is an alias for creating a xcontext.
        /// <para>For Examples see http://www.nspec.org</para>
        /// </summary>
        public ActionRegister xdescribe;

        /// <summary>
        /// Create a specification/example using a name and a lambda with an assertion(should).
        /// <para>For Example:</para>
        /// <para>it["should return false"] = () => _controller.should_be(false);</para>
        /// </summary>
        public ActionRegister it;

        /// <summary>
        /// Mark a spec as pending 
        /// <para>For Example:</para>
        /// <para>xit["should return false"] = () => _controller.should_be(false);</para>
        /// <para>(the example will be marked as pending any lambda provided will not be executed)</para>
        /// </summary>
        public ActionRegister xit;

        /// <summary>
        /// Set up a pending spec.
        /// <para>For Example:</para>
        /// <para>it["a test i haven't flushed out yet, but need to"] = todo;</para>
        /// </summary>
        public readonly Action todo = () => { };

        /// <summary>
        /// Set up an expectation for a particular exception type to be thrown.
        /// <para>For Example:</para>
        /// <para>it["should throw exception"] = expect&lt;InvalidOperationException&gt;();</para>
        /// </summary>
        public virtual Action expect<T>() where T : Exception
        {
            return expect<T>(expectedMessage: null);
        }

        /// <summary>
        /// Set up an expectation for a particular exception type to be thrown.
        /// <para>For Example:</para>
        /// <para>it["should throw exception"] = expect&lt;InvalidOperationException&gt;();</para>
        /// </summary>
        public virtual Action expect<T>(string expectedMessage) where T : Exception
        {
            var specContext = Context;

            return () =>
            {
                if (specContext.Exception == null || specContext.Exception.GetType() != typeof(T))
                    throw new ExceptionNotThrown(IncorrectType<T>());

                if (expectedMessage != null && expectedMessage != specContext.Exception.Message)
                {
                    throw new ExceptionNotThrown(
                        IncorrectMessage(
                            expectedMessage,
                            specContext.Exception.Message));
                }

                if (specContext.Exception.GetType() == typeof(T))
                {
                    specContext.Exception = null;
                }
            };
        }

        /// <summary>
        /// Set up an expectation for a particular exception type to be thrown.
        /// <para>For Example:</para>
        /// <para>it["should throw exception"] = expect&lt;InvalidOperationException&gt;(() => SomeMethodThatThrowsException());</para>
        /// </summary>
        public virtual Action expect<T>(Action action) where T : Exception
        {
            return expect<T>(null, action);
        }

        /// <summary>
        /// Set up an expectation for a particular exception type to be thrown with an expected message.
        /// <para>For Example:</para>
        /// <para>it["should throw exception with message Error"] = expect&lt;InvalidOperationException&gt;("Error", () => SomeMethodThatThrowsException());</para>
        /// </summary>
        public virtual Action expect<T>(string expectedMessage, Action action) where T : Exception
        {
            return () =>
            {
                var closureType = typeof(T);

                try
                {
                    action();
                    throw new ExceptionNotThrown(IncorrectType<T>());
                }
                catch (ExceptionNotThrown)
                {
                    throw;
                }

                catch (Exception ex)
                {
                    if (ex.GetType() != closureType)
                    {
                        throw new ExceptionNotThrown(IncorrectType<T>());
                    }

                    if (expectedMessage != null && expectedMessage != ex.Message)
                    {
                        throw new ExceptionNotThrown(IncorrectMessage(expectedMessage, ex.Message));
                    }
                }
            };
        }

        /// <summary>
        /// Override this method to alter the stack trace that NSpec prints.  This is useful to override
        /// if you want to provide additional information (eg. information from a log that is generated out of proc).
        /// </summary>
        /// <param name="flattenedStackTrace">A clean stack trace that excludes NSpec specfic namespaces</param>
        /// <returns></returns>
        public virtual string StackTraceToPrint(string flattenedStackTrace)
        {
            return flattenedStackTrace;
        }

        /// <summary>
        /// Override this method to return another exception in the event of a failure of a test.  This is useful to override
        /// when catching for specific exceptions and returning a more meaningful exception to the developer.
        /// </summary>
        /// <param name="originalException">Original exception that was thrown.</param>
        /// <returns></returns>
        public virtual Exception ExceptionToReturn(Exception originalException)
        {
            return originalException;
        }

        string IncorrectType<T>() where T : Exception
        {
            return "Exception of type " + typeof(T).Name + " was not thrown.";
        }

        string IncorrectMessage(string expected, string actual)
        {
            return String.Format("Expected message: \"{0}\" But was: \"{1}\"", expected, actual);
        }

        void AddExample(Example example)
        {
            Context.AddExample(example);
        }

        void AddContext(string name, string tags, Action action)
        {
            var childContext = new Context(name, tags);

            RunContext(childContext, action);
        }

        void AddIgnoredContext(string name, string tags, Action action)
        {
            var ignored = new Context(name, tags, isPending: true);

            RunContext(ignored, action);
        }

        void RunContext(Context context, Action action)
        {
            Context.AddContext(context);

            var beforeContext = Context;

            Context = context;

            action();

            Context = beforeContext;
        }

        public virtual string OnError(string flattenedStackTrace)
        {
            return flattenedStackTrace;
        }

        internal Context Context { get; set; }

        /// <summary>Tags required to be present or not present in context or example</summary>
        /// <remarks>
        /// Currently, multiple tags indicates any of the tags must be present to be included/excluded.  In other words, they are OR'd, not AND'd.
        /// NOTE: Cucumber's tags wiki offers ideas for handling tags: https://github.com/cucumber/cucumber/wiki/tags
        /// </remarks>
        internal Tags tagsFilter = new Tags();
    }
}