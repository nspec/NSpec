using System;
using System.Linq.Expressions;

namespace NSpec.Domain
{
    public class ExpressionParser
    {
        public static string Parse(Expression<Action> exp)
        {
            var body = exp.Body.ToString();

            var cut = body.IndexOf(").");

            var sentance = body.Substring(cut + 1, body.Length - cut - 1).Replace(")", " ").Replace(".", " ").Replace("(", " ").Replace("  ", " ").Trim().Replace("_", " ").Replace("\"", " ");

            while (sentance.Contains("  ")) sentance = sentance.Replace("  ", " ");

            return sentance.Trim();
        }
    }
}
