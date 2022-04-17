// IEqualityComparer which can be created using a lambda function

using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Types
{
    public class KeyEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, object> _keyExtractor;

        public KeyEqualityComparer(Func<T, object> keyExtractor) : this(keyExtractor, null) { }
        public KeyEqualityComparer(Func<T, T, bool> comparer) : this(null, comparer) { }

        public KeyEqualityComparer(Func<T, object> keyExtractor, Func<T, T, bool> comparer)
        {
            _keyExtractor = keyExtractor;
            _comparer = comparer;
        }

        public bool Equals(T x, T y)
        {
            if (_comparer != null)
                return _comparer(x, y);
            else
            {
                var valX = _keyExtractor(x);
                if (valX is IEnumerable<object> listResult) // The special case where we pass a list of keys
                    return listResult.SequenceEqual((IEnumerable<object>)_keyExtractor(y));

                return valX.Equals(_keyExtractor(y));
            }
        }

        public int GetHashCode(T obj)
        {
            if (_keyExtractor == null)
                return obj.ToString().ToLower().GetHashCode();
            else
            {
                var val = _keyExtractor(obj);
                if (val is IEnumerable<object> listResult) // The special case where we pass a list of keys
                    return (int)listResult.Aggregate((x, y) => x.GetHashCode() ^ y.GetHashCode());

                return val.GetHashCode();
            }
        }
    }
}
