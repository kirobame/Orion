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
        T Read();
    }

    public interface IWritable
    {
        void Write(object value);
    }
    public interface IWritable<in T> : IWritable
    {
        void Write(T value);
    }
    
    public interface IProxy : IReadable, IWritable { }
    public interface IProxy<T> : IReadable<T>, IWritable<T> { }
}