﻿namespace Smart.Converter2.Converters
{
    using System;

    public sealed class ToStringConverterFactory : IConverterFactory
    {
        private static readonly Type StringType = typeof(string);

        private static readonly Func<object, object> Converter = source => source.ToString();

        public Func<object, object> GetConverter(in TypePair typePair)
        {
            return typePair.TargetType == StringType ? Converter : null;
        }
    }
}
