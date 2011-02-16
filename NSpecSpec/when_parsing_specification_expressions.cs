using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpecSpec
{
    public class when_parsing_specification_expressions :spec
    {
        public void given_an_expression()
        {
            var someCounter=0;
            new Dictionary<Expression<Action>, string>
                {
                    {() => someCounter.ShouldBe(0), "someCounter ShouldBe 0"},
                    {() => 1.Primes().ShouldBe(0), "1 ShouldBe {}"},
                }
            .Do((exp, sentence) => xspecify( ()=>exp.ToSentence().ShouldBe(sentence)));
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