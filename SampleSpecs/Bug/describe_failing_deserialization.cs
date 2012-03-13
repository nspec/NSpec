using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using NSpec;

namespace SampleSpecs.Bug
{
    class describe_failing_deserialization : nspec
    {
        void when_serializing_objects()
        {
            MemoryStream stream = null;
            BinaryFormatter bf = null;

            before = () =>
            {
                stream = new MemoryStream();
                bf = new BinaryFormatter();
            };

            act = () => bf.Serialize(stream, _object);

            context["that are not in the search path"] = () =>
            {
                before = () => _object = new Action(() => { }).Method;

                it["should deserialize them again"] = () => // fails
                {
                    stream.Position = 0;
                    bf.Deserialize(stream).should_cast_to<MethodInfo>();
                };
            };

            context["that are in the search path"] = () =>
            {
                before = () => _object = new object();

                it["should deserialize them again"] = () =>
                {
                    stream.Position = 0;
                    bf.Deserialize(stream).should_not_be_null();
                };
            };
        }

        object _object;
    }
}