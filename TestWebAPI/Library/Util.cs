namespace TestWebAPI.Library
{
    using System;
    using System.Linq;

    using Newtonsoft.Json;

    /// <summary>
    /// The utility class. Test.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// The copy values.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="overrideTarget">
        /// The override target.
        /// </param>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <exception cref="InvalidOperationException">
        /// The exception.
        /// </exception>
        public static void CopyValues<T>(T source, T target, bool overrideTarget = false)
        {
            if (source == null || target == null)
            {
                throw new InvalidOperationException("Source or Target param cannot be null");
            }

            Type t = typeof(T);

            System.Collections.Generic.IEnumerable<System.Reflection.PropertyInfo> properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                var sourcePropValue = prop.GetValue(source, null);

                if (sourcePropValue != null)
                {
                    var targetPropValue = prop.GetValue(target, null);

                    if (overrideTarget || targetPropValue == null)
                    {
                        prop.SetValue(target, sourcePropValue, null);
                    }
                }
            }
        }

        /// <summary>
        /// The try parse JSON.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <typeparam name="T">
        /// The type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryParseJson<T>(this string obj, out T result)
        {
            if (obj is null)
            {
                result = default(T);
                return false;
            }

            try
            {
                var settings = new JsonSerializerSettings
                                   {
                                       NullValueHandling = NullValueHandling.Ignore,
                                       MissingMemberHandling = MissingMemberHandling.Error
                                   };

                result = JsonConvert.DeserializeObject<T>(obj, settings);
                return true;
            }
            catch (JsonSerializationException ex)
            {
                result = default(T);
                return false;
            }
        }
    }
}
