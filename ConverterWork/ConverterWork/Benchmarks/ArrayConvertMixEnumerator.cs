﻿namespace Smart.Benchmarks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class ArrayConvertMixEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] source;

        private readonly Func<object, object> converter;

        private int index;

        public ArrayConvertMixEnumerator(T[] source, Func<object, object> converter)
        {
            this.source = source;
            this.converter = converter;
            index = -1;
        }

        public bool MoveNext()
        {
            index++;
            return index < source.Length;
        }

        public void Reset()
        {
            index = -1;
        }

        public T Current => (T)converter(source[index]);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }

    public readonly struct ArrayConvertMixEnumerable<T> : IEnumerable<T>
    {
        private readonly T[] array;

        private readonly Func<object, object> converter;

        public ArrayConvertMixEnumerable(T[] array, Func<object, object> converter)
        {
            this.array = array;
            this.converter = converter;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArrayConvertClassEnumerator<T>(array, converter);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class ArrayConvertMixCollection<T> : ICollection<T>
    {
        private readonly T[] source;

        private readonly Func<object, object> converter;

        public ArrayConvertMixCollection(T[] source, Func<object, object> converter)
        {
            this.source = source;
            this.converter = converter;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArrayConvertClassEnumerator<T>(source, converter);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        public bool Contains(T item) => throw new NotSupportedException();

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (var i = 0; i < source.Length; i++)
            {
                array[arrayIndex + i] = (T)converter(source[i]);
            }
        }

        public bool Remove(T item) => throw new NotSupportedException();

        public int Count => source.Length;

        public bool IsReadOnly => true;
    }

    public readonly struct ArrayConvertMixList<T> : IList<T>
    {
        private readonly T[] source;

        private readonly Func<object, object> converter;

        public ArrayConvertMixList(T[] source, Func<object, object> converter)
        {
            this.source = source;
            this.converter = converter;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArrayConvertClassEnumerator<T>(source, converter);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        public bool Contains(T item) => throw new NotSupportedException();

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (var i = 0; i < source.Length; i++)
            {
                array[arrayIndex + i] = (T)converter(source[i]);
            }
        }

        public bool Remove(T item) => throw new NotSupportedException();

        public int Count => source.Length;

        public bool IsReadOnly => true;

        public int IndexOf(T item) => throw new NotSupportedException();

        public void Insert(int index, T item) => throw new NotSupportedException();

        public void RemoveAt(int index) => throw new NotSupportedException();

        public T this[int index]
        {
            get => source[index];
            set => source[index] = value;
        }
    }
}