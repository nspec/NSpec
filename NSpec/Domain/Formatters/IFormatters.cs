using System;
using System.Collections.Generic;

namespace NSpec.Domain.Formatters
{
    public interface IFormatter
    {
        void Write(ContextCollection contexts);

        IDictionary<string, string> Options { get; set; }
    }

    public interface ILiveFormatter
    {
        void Write(Context context);
        void Write(ExampleBase example, int level);
    }
}