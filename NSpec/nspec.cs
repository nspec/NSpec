using System;
using System.Linq.Expressions;
using NSpec.Domain;
using System.Threading.Tasks;

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

            itAsync = new AsyncActionRegister((name, tags, asyncAction) => AddExample(new AsyncExample(name, tags, asyncAction, pending: asyncAction == todoAsync)));
            xitAsync = new AsyncActionRegister((name, tags, asyncAction) => AddExample(new AsyncExample(name, tags, asyncAction, pending: true)));
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

        /* No need for the following, as async lambda expressions cannot be converted to expression trees:

        public virtual Expression<Func<Task>> specifyAsync { ... }
         */

        /// <summary>
        /// Mark a spec as pending
        /// <para>For Example:</para>
        /// <para>xspecify = () => _controller.should_be(false);</para>
        /// <para>(the example will be marked as pending any lambda provided will not be executed)</para>
        /// </summary>
        public virtual Expression<Action> xspecify
        {
            set { AddExample(new Example(value, pending: true)); }
        }

        /* No need for the following, as async lambda expressions cannot be converted to expression trees:

        public virtual Expression<Func<Task>> xspecifyAsync { ... }
         */

        /// <summary>
        /// This Action gets executed before each example is run.
        /// <para>For Example:</para>
        /// <para>before = () => someList = new List&lt;int&gt;();</para>
        /// <para>The before can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Action before
        {
            get { return Context.Before; }
            set { Context.Before = value; }
        }

        /// <summary>
        /// This Function gets executed asynchronously before each example is run.
        /// <para>For Example:</para>
        /// <para>beforeAsync = async () => someList = await GetListAsync();</para>
        /// <para>The beforeAsync can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Func<Task> beforeAsync
        {
            get { return Context.BeforeAsync; }
            set { Context.BeforeAsync = value; }
        }

        /// <summary>
        /// This Action is an alias of before. This Action gets executed before each example is run.
        /// <para>For Example:</para>
        /// <para>beforeEach = () => someList = new List&lt;int&gt;();</para>
        /// <para>The beforeEach can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Action beforeEach
        {
            get { return Context.Before; }
            set { Context.Before = value; }
        }

        /// <summary>
        /// This Function is an alias of beforeAsync. It gets executed asynchronously before each example is run.
        /// <para>For Example:</para>
        /// <para>beforeEachAsync = async () => someList = await GetListAsync();</para>
        /// <para>The beforeEachAsync can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Func<Task> beforeEachAsync
        {
            get { return Context.BeforeAsync; }
            set { Context.BeforeAsync = value; }
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
        /// This Function gets executed asynchronously before all examples in a context.
        /// <para>For Example:</para>
        /// <para>beforeAllAsync = async () => someList = await GetListAsync();</para>
        /// <para>The beforeAllAsync can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Func<Task> beforeAllAsync
        {
            get { return Context.BeforeAllAsync; }
            set { Context.BeforeAllAsync = value; }
        }

        /// <summary>
        /// This Action gets executed after each example is run.
        /// <para>For Example:</para>
        /// <para>after = () => someList = new List&lt;int&gt;();</para>
        /// <para>The after can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Action after
        {
            get { return Context.After; }
            set { Context.After = value; }
        }

        /// <summary>
        /// This Function gets executed asynchronously after each example is run.
        /// <para>For Example:</para>
        /// <para>afterAsync = async () => someList = await GetListAsync();</para>
        /// <para>The after can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Func<Task> afterAsync
        {
            get { return Context.AfterAsync; }
            set { Context.AfterAsync = value; }
        }

        /// <summary>
        /// This Action is an alias of after. This Action gets executed after each example is run.
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
        /// This Action is an alias of afterAsync. This Function gets executed asynchronously after each example is run.
        /// <para>For Example:</para>
        /// <para>afterEachAsync = async () => someList = await GetListAsync();</para>
        /// <para>The after can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Func<Task> afterEachAsync
        {
            get { return Context.AfterAsync; }
            set { Context.AfterAsync = value; }
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
        /// This Function gets executed asynchronously after all examples in a context.
        /// <para>For Example:</para>
        /// <para>afterAllAsync = async () => someList = await GetListAsync();</para>
        /// <para>The afterAllAsync can be a multi-line lambda.  Setting the member multiple times through out sub-contexts will not override the action, but instead will append to your setup (this is a good thing).  For more information visit http://www.nspec.org</para>
        /// </summary>
        public virtual Func<Task> afterAllAsync
        {
            get { return Context.AfterAllAsync; }
            set { Context.AfterAllAsync = value; }
        }

        /// <summary>
        /// Assign this member within your context.  The Action assigned will gets executed
        /// with every example in scope.  Befores will run first, then acts, then your examples.  It's a way for you to define once a common Act in Arrange-Act-Assert for all subcontexts.  For more information visit http://www.nspec.org
        /// </summary>
        public virtual Action act
        {
            get { return Context.Act; }
            set { Context.Act = value; }
        }

        /// <summary>
        /// Assign this member within your context.  The Function assigned will gets executed asynchronously
        /// with every example in scope.  Befores will run first, then acts, then your examples.  It's a way for you to define once a common Act in Arrange-Act-Assert for all subcontexts.  For more information visit http://www.nspec.org
        /// </summary>
        public virtual Func<Task> actAsync
        {
            get { return Context.ActAsync; }
            set { Context.ActAsync = value; }
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
        /// Create an asynchronous specification/example using a name and an async lambda with an assertion(should).
        /// <para>For Example:</para>
        /// <para>itAsync["should return false"] = async () => (await GetResultAsync()).should_be(false);</para>
        /// </summary>
        public AsyncActionRegister itAsync;

        /// <summary>
        /// Mark a spec as pending
        /// <para>For Example:</para>
        /// <para>xit["should return false"] = () => _controller.should_be(false);</para>
        /// <para>(the example will be marked as pending, any lambda provided will not be executed)</para>
        /// </summary>
        public ActionRegister xit;

        /// <summary>
        /// Mark an asynchronous spec as pending
        /// <para>For Example:</para>
        /// <para>xitAsync["should return false"] = async () => (await GetResultAsync()).should_be(false);</para>
        /// <para>(the example will be marked as pending, any lambda provided will not be executed)</para>
        /// </summary>
        public AsyncActionRegister xitAsync;

        /// <summary>
        /// Set up a pending spec.
        /// <para>For Example:</para>
        /// <para>it["a test i haven't flushed out yet, but need to"] = todo;</para>
        /// </summary>
        public readonly Action todo = () => { };

        /// <summary>
        /// Set up a pending asynchronous spec.
        /// <para>For Example:</para>
        /// <para>itAsync["a test i haven't flushed out yet, but need to"] = todoAsync;</para>
        /// </summary>
        public readonly Func<Task> todoAsync = () => Task.Run(() => { });

        /// <summary>
        /// Set up an expectation for a particular exception type to be thrown before expectation.
        /// <para>For Example:</para>
        /// <para>it["should throw exception"] = expect&lt;InvalidOperationException&gt;();</para>
        /// </summary>
        public virtual Action expect<T>() where T : Exception
        {
            return expect<T>(expectedMessage: null);
        }

        /// <summary>
        /// Set up an expectation for a particular exception type to be thrown before expectation, with an expected message.
        /// <para>For Example:</para>
        /// <para>it["should throw exception"] = expect&lt;InvalidOperationException&gt;();</para>
        /// </summary>
        public virtual Action expect<T>(string expectedMessage) where T : Exception
        {
            var specContext = Context;

            return () =>
            {
                if (specContext.Exception == null)
                    throw new ExceptionNotThrown(IncorrectType<T>());

                AssertExpectedException<T>(specContext.Exception, expectedMessage);

                // do not clear exception right now, during first phase, but leave a note for second phase
                specContext.ClearExpectedException = true;
            };
        }

        /// <summary>
        /// Set up an expectation for a particular exception type to be thrown inside passed action.
        /// <para>For Example:</para>
        /// <para>it["should throw exception"] = expect&lt;InvalidOperationException&gt;(() => SomeMethodThatThrowsException());</para>
        /// </summary>
        public virtual Action expect<T>(Action action) where T : Exception
        {
            return expect<T>(null, action);
        }

        /// <summary>
        /// Set up an expectation for a particular exception type to be thrown inside passed action, with an expected message.
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
                    AssertExpectedException<T>(ex, expectedMessage);
                }
            };
        }

        /// <summary>
        /// Set up an asynchronous expectation for a particular exception type to be thrown inside passed asynchronous action.
        /// <para>For Example:</para>
        /// <para>itAsync["should throw exception"] = expectAsync&lt;InvalidOperationException&gt;(async () => await SomeAsyncMethodThatThrowsException());</para>
        /// </summary>
        public virtual Func<Task> expectAsync<T>(Func<Task> asyncAction) where T : Exception
        {
            return expectAsync<T>(null, asyncAction);
        }

        /// <summary>
        /// Set up an asynchronous expectation for a particular exception type to be thrown inside passed asynchronous action, with an expected message.
        /// <para>For Example:</para>
        /// <para>itAsync["should throw exception with message Error"] = expectAsync&lt;InvalidOperationException&gt;("Error", async () => await SomeAsyncMethodThatThrowsException());</para>
        /// </summary>
        public virtual Func<Task> expectAsync<T>(string expectedMessage, Func<Task> asyncAction) where T : Exception
        {
            return async () =>
            {
                var closureType = typeof(T);

                try
                {
                    await asyncAction();

                    throw new ExceptionNotThrown(IncorrectType<T>());
                }
                catch (ExceptionNotThrown)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    AssertExpectedException<T>(ex, expectedMessage);
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

        // TODO if this is intendend to be overridden by client spec classes, add describing comments
        public virtual string OnError(string flattenedStackTrace)
        {
            return flattenedStackTrace;
        }

        static string IncorrectType<T>() where T : Exception
        {
            return "Exception of type " + typeof(T).Name + " was not thrown.";
        }

        static string IncorrectMessage(string expected, string actual)
        {
            return String.Format("Expected message: \"{0}\" But was: \"{1}\"", expected, actual);
        }

        void AddExample(ExampleBase example)
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

            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception executing context: {0}".With(context.FullContext()));

                AddFailingExample(ex);
            }

            Context = beforeContext;
        }

        void AddFailingExample(Exception reportedEx)
        {
            string exampleName = "Context body throws an exception of type {0}".With(reportedEx.GetType().Name);

            it[exampleName] = () => { throw new ContextBareCodeException(reportedEx); };
        }

        void AssertExpectedException<T>(Exception actualException, string expectedMessage) where T : Exception
        {
            var expectedType = typeof(T);
            Exception matchingException = null;

            if (actualException.GetType() == expectedType)
            {
                matchingException = actualException;
            }
            else
            {
                var aggregateException = actualException as AggregateException;
                if (aggregateException != null)
                {
                    foreach (var innerException in aggregateException.InnerExceptions)
                    {
                        if (innerException.GetType() == expectedType)
                        {
                            matchingException = innerException;
                            break;
                        }
                    }
                }
            }

            if (matchingException == null)
            {
                throw new ExceptionNotThrown(IncorrectType<T>());
            }

            if (expectedMessage != null && expectedMessage != matchingException.Message)
            {
                throw new ExceptionNotThrown(IncorrectMessage(expectedMessage, matchingException.Message));
            }
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