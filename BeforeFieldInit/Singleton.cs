using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace BeforeFieldInit
{
    public class SingletonLazy
    {
        #region Lazy Singleton
        private static readonly Lazy<SingletonLazy> _instace = new Lazy<SingletonLazy>(() => new SingletonLazy(), LazyThreadSafetyMode.ExecutionAndPublication);

        private SingletonLazy()
        {

        }

        public static SingletonLazy Instance
        {
            get { return _instace.Value; }
        } 
        #endregion
    }

    public sealed class SingletonNest
    {
        private SingletonNest()
        { }

        public static SingletonNest Instance
        {
            get { return Nested._instance; }
        }

        private class Nested
        {
            static Nested()
            {

            }

            internal static readonly SingletonNest _instance = new SingletonNest();
        }
    }

    public class SingletonStatic
    {
        private static readonly SingletonStatic _instance = new SingletonStatic();
        public static SingletonStatic Instance
        {
            get { return _instance; }
        }

        private SingletonStatic()
        { }

        static SingletonStatic()
        { }
    }

    public sealed class SingletonLock
    {
        private static volatile SingletonLock instance = null;
        private static object syncRoot = new object();

        private SingletonLock()
        { }

        public static SingletonLock Instance
        {
            get
            {
                if (instance == null)
                {
                    lock(syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SingletonLock();
                        }
                    }
                }

                return instance;
            }
        }
    }

    public sealed class SingletonPrivate
    {
        private static readonly SingletonPrivate instance = new SingletonPrivate();

        private SingletonPrivate()
        { }

        public static SingletonPrivate Instance
        {
            get { return instance; }
        }
    }

    public sealed class Singleton
    {
        private static Singleton instance;
        private Singleton()
        { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }

                return instance;
            }
        }
    }

    public abstract class SingletonGeneric<T>
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(() =>
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (ctors.Count() != 1)
            {
                throw new InvalidOperationException($"Type constructor for {typeof(T)} must be private and take no paraneters.");
            }

            var ctor = ctors.SingleOrDefault(c => c.GetParameters().Count() == 0 && c.IsPrivate);
            if (ctor == null)
                throw new InvalidOperationException($"The constructor for {typeof(T)} must be private and take no parameters.");
            return (T)ctor.Invoke(null);
        });

        public static T Instance
        {
            get { return _instance.Value; }
        }
    }

    class MySingleton : SingletonGeneric<MySingleton>
    {
        int _counter;
        public int Counter
        {
            get { return _counter; }
        }

        private MySingleton()
        {
            _counter = 0;
        }

        public void IncrementCounter()
        {
            Interlocked.Increment(ref _counter);
            //++_counter;
        }
    }
}
