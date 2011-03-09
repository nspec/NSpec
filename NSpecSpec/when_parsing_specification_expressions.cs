using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;
using NSpec;

namespace NSpecSpec
{
    public class when_parsing_specification_expressions :spec
    {
        public void given_an_expression()
        {
            var someCounter=0;
            new Dictionary<Expression<Action>, string>
                {
                    {() => someCounter.should_be(0), "someCounter should_be 0"},
                    {() => 1.Primes().should_be(0), "1 should_be {}"},
                }
            .Do((exp, sentence) => xit(exp.ToSentence()));
        }
    }
    public static class Extensions
    {
        public static IEnumerable<int> Primes(this int num)
        {
            return new int[] {};
        }

        public static string ToSentence(this Expression<Action> exp)
        {
            var body = exp.Body.ToString();

            Console.WriteLine(body);

            var cut = body.IndexOf(").");

            return body.Substring(cut + 1, body.Length - cut - 1).Replace(")", " ").Replace(".", " ").Replace("(", "").Replace("  ", " ").Trim();
        }
    }
} 