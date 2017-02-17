#if NET451

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentAssertions;
using NSpec;

namespace SampleSpecs.Bug
{
    public class describe_failing_deserialization : nspec
    {
        MemoryStream stream;
        BinaryFormatter formatter;
        object _object;

        void when_serializing_objects()
        {
            before = () =>
            {
                stream = new MemoryStream();
                formatter = new BinaryFormatter();
            };

            act = () => formatter.Serialize(stream, _object);

            context["that are not in the search path"] = () =>
            {
                before = () => _object = new Action(() => { }).Method;

                it["should deserialize them again"] = () => // fails
                {
                    stream.Position = 0;
                    formatter.Deserialize(stream).Should().NotBeNull();
                };
            };

            context["that are in the search path"] = () =>
            {
                before = () => _object = new object();

                it["should deserialize them again"] = () =>
                {
                    stream.Position = 0;
                    formatter.Deserialize(stream).Should().NotBeNull();
                };
            };
        }
    }
}
#endif
