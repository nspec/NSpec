using NSpec.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NSpec.Domain
{
    public class ConventionSpecification
    {
        public void SetBefore(string startsWith)
        {
            SetBefore(new Regex("^" + startsWith));
        }

        public void SetBefore(Regex regex)
        {
            Before = regex;
        }

        public void SetAct(string startsWith)
        {
            SetAct(new Regex("^" + startsWith));
        }

        public void SetAct(Regex regex)
        {
            Act = regex;
        }

        public void SetAfter(string startsWith)
        {
            SetAfter(new Regex("^" + startsWith));
        }

        public void SetAfter(Regex regex)
        {
            After = regex;
        }

        public void SetAfterAll(string startsWith)
        {
            SetAfterAll(new Regex("^" + startsWith));
        }

        public void SetAfterAll(Regex regex)
        {
            AfterAll = regex;
        }

        public void SetExample(string startsWith)
        {
            SetExample(new Regex("^" + startsWith));
        }

        public void SetExample(Regex regex)
        {
            Example = regex;
        }

        public void SetContext(string startsWith)
        {
            SetContext(new Regex("^" + startsWith));
        }

        public void SetContext(Regex regex)
        {
            Context = regex;
        }

        public void SetBeforeAll(Regex regex)
        {
            BeforeAll = regex;
        }

        public Regex Before, BeforeAll, Act, After, AfterAll, Example, Context;
    }

    public abstract class Conventions
    {
        public Conventions Initialize()
        {
            specification = new ConventionSpecification();
            SpecifyConventions(specification);
            return this;
        }

        public abstract void SpecifyConventions(ConventionSpecification specification);

        public MethodInfo GetMethodLevelBeforeAll(Type type)
        {
            return GetMethodMatchingRegex(type, specification.BeforeAll);
        }

        public MethodInfo GetAsyncMethodLevelBeforeAll(Type type)
        {
            return GetAsyncMethodMatchingRegex(type, specification.BeforeAll);
        }

        public MethodInfo GetMethodLevelBefore(Type type)
        {
            return GetMethodMatchingRegex(type, specification.Before);
        }

        public MethodInfo GetAsyncMethodLevelBefore(Type type)
        {
            return GetAsyncMethodMatchingRegex(type, specification.Before);
        }

        public MethodInfo GetMethodLevelAct(Type type)
        {
            return GetMethodMatchingRegex(type, specification.Act);
        }

        public MethodInfo GetMethodLevelAfter(Type type)
        {
            return GetMethodMatchingRegex(type, specification.After);
        }

        public MethodInfo GetAsyncMethodLevelAfter(Type type)
        {
            return GetAsyncMethodMatchingRegex(type, specification.After);
        }

        public MethodInfo GetMethodLevelAfterAll(Type type)
        {
            return GetMethodMatchingRegex(type, specification.AfterAll);
        }

        public MethodInfo GetAsyncMethodLevelAfterAll(Type type)
        {
            return GetAsyncMethodMatchingRegex(type, specification.AfterAll);
        }

        public bool IsMethodLevelExample(string name)
        {
            return specification.Example.IsMatch(name);
        }

        public bool IsMethodLevelBefore(string name)
        {
            return specification.Before.IsMatch(name) || specification.BeforeAll.IsMatch(name);
        }

        public bool IsMethodLevelAct(string name)
        {
            return specification.Act.IsMatch(name);
        }

        public bool IsMethodLevelAfter(string name)
        {
            return specification.After.IsMatch(name) || specification.AfterAll.IsMatch(name);
        }

        public bool IsMethodLevelContext(string name)
        {
            if (IsMethodLevelBefore(name)) return false;

            if (IsMethodLevelAct(name)) return false;

            if (IsMethodLevelExample(name)) return false;

            if (IsMethodLevelAfter(name)) return false;

            return specification.Context.IsMatch(name);
        }

        static MethodInfo GetMethodMatchingRegex(Type type, Regex regex)
        {
            return FindMatching(type.Methods(), type, regex);
        }

        static MethodInfo GetAsyncMethodMatchingRegex(Type type, Regex regex)
        {
            return FindMatching(type.AsyncMethods(), type, regex);
        }

        static MethodInfo FindMatching(IEnumerable<MethodInfo> methods, Type type, Regex regex)
        {
            return methods
                .Where(mi => mi.DeclaringType == type)
                .FirstOrDefault(mi => regex.IsMatch(mi.Name));
        }

        ConventionSpecification specification;
    }
}