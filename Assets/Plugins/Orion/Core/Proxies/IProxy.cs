using System.Collections;
using System.Collections.Generic;

namespace Orion
{
    public interface IReadable
    {
        object Read();
    }
    public interface IReadable<out T> : IReadable
    {
        new T Read();
    }

    public interface IWritable
    {
        void Write(object value);
    }
    public interface IWritable<in T>
    {
        void Write(T value);
    }

    public interface IProxy : IReadable, IWritable { }
    public interface IProxy<T> : IProxy, IReadable<T>, IWritable<T> { }
}