using System.Text.Json;

namespace Victorina.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            if (value == null)
            {
                session.Remove(key);
                return;
            }

            var jsonString = JsonSerializer.Serialize(value);
            session.SetString(key, jsonString);
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var jsonString = session.GetString(key);
            return jsonString == null ? default(T) : JsonSerializer.Deserialize<T>(jsonString);
        }

        public static bool TryGetObject<T>(this ISession session, string key, out T value)
        {
            var jsonString = session.GetString(key);
            if (jsonString == null)
            {
                value = default(T);
                return false;
            }

            value = JsonSerializer.Deserialize<T>(jsonString);
            return true;
        }

        public static void SetBool(this ISession session, string key, bool value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static bool? GetBool(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null || data.Length == 0)
                return null;

            return BitConverter.ToBoolean(data, 0);
        }

        public static void SetDateTime(this ISession session, string key, DateTime value)
        {
            session.SetString(key, value.ToString("O")); // ISO 8601 format
        }

        public static DateTime? GetDateTime(this ISession session, string key)
        {
            var value = session.GetString(key);
            return string.IsNullOrEmpty(value) ? null : DateTime.Parse(value);
        }
    }
}
