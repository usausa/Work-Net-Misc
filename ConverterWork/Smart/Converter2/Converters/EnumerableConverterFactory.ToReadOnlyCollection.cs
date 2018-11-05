﻿namespace Smart.Converter2.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public sealed partial class EnumerableConverterFactory
    {
        private sealed class SameTypeReadOnlyCollectionBuilderProvider : IConverterBuilderProvider
        {
            public static IConverterBuilderProvider Default { get; } = new SameTypeReadOnlyCollectionBuilderProvider();

            public Type GetBuilderType(SourceEnumerableType sourceEnumerableType)
            {
                switch (sourceEnumerableType)
                {
                    case SourceEnumerableType.Array:
                        return typeof(SameTypeReadOnlyCollectionFromArrayBuilder<>);
                    case SourceEnumerableType.List:
                        return typeof(SameTypeReadOnlyCollectionFromListBuilder<>);
                    default:
                        return typeof(SameTypeReadOnlyCollectionFromEnumerableBuilder<>);
                }
            }
        }

        private sealed class OtherTypeReadOnlyCollectionBuilderProvider : IConverterBuilderProvider
        {
            public static IConverterBuilderProvider Default { get; } = new OtherTypeReadOnlyCollectionBuilderProvider();

            public Type GetBuilderType(SourceEnumerableType sourceEnumerableType)
            {
                switch (sourceEnumerableType)
                {
                    case SourceEnumerableType.Array:
                        return typeof(OtherTypeReadOnlyCollectionFromArrayBuilder<,>);
                    case SourceEnumerableType.List:
                        return typeof(OtherTypeReadOnlyCollectionFromListBuilder<,>);
                    default:
                        return typeof(OtherTypeReadOnlyCollectionFromEnumerableBuilder<,>);
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Same type
        //--------------------------------------------------------------------------------

        private sealed class SameTypeReadOnlyCollectionFromArrayBuilder<TDestination> : IConverterBuilder
        {
            public object Create(object source)
            {
                return new ReadOnlyCollection<TDestination>((TDestination[])source);
            }
        }

        private sealed class SameTypeReadOnlyCollectionFromListBuilder<TDestination> : IConverterBuilder
        {
            public object Create(object source)
            {
                return new ReadOnlyCollection<TDestination>((IList<TDestination>)source);
            }
        }

        private sealed class SameTypeReadOnlyCollectionFromEnumerableBuilder<TDestination> : IConverterBuilder
        {
            public object Create(object source)
            {
                return new ReadOnlyCollection<TDestination>(((IEnumerable<TDestination>)source).ToList());
            }
        }

        //--------------------------------------------------------------------------------
        // Other type
        //--------------------------------------------------------------------------------

        private sealed class OtherTypeReadOnlyCollectionFromArrayBuilder<TSource, TDestination> : IConverterBuilder
        {
            private readonly Func<object, object> converter;

            public OtherTypeReadOnlyCollectionFromArrayBuilder(Func<object, object> converter)
            {
                this.converter = converter;
            }

            public object Create(object source)
            {
                return new ReadOnlyCollection<TDestination>(new ArrayConvertStructList<TSource, TDestination>((TSource[])source, converter));
            }
        }

        private sealed class OtherTypeReadOnlyCollectionFromListBuilder<TSource, TDestination> : IConverterBuilder
        {
            private readonly Func<object, object> converter;

            public OtherTypeReadOnlyCollectionFromListBuilder(Func<object, object> converter)
            {
                this.converter = converter;
            }

            public object Create(object source)
            {
                return new ReadOnlyCollection<TDestination>(new ListConvertStructList<TSource, TDestination>((IList<TSource>)source, converter));
            }
        }

        private sealed class OtherTypeReadOnlyCollectionFromEnumerableBuilder<TSource, TDestination> : IConverterBuilder
        {
            private readonly Func<object, object> converter;

            public OtherTypeReadOnlyCollectionFromEnumerableBuilder(Func<object, object> converter)
            {
                this.converter = converter;
            }

            public object Create(object source)
            {
                var list = new List<TDestination>();
                foreach (var value in (IEnumerable<TSource>)source)
                {
                    list.Add((TDestination)converter(value));
                }

                return new ReadOnlyCollection<TDestination>(list);
            }
        }
    }
}