using UnityEditor;

namespace Jenga.Core
{
    #if UNITY_EDITOR
    /// <summary>
    /// Helper methods for <see cref="SerializableGUIDJengaVersion"/>
    /// </summary>
    public static class SerializableGUIDExtensions
    {
        /// <summary>
        /// Converts a <see cref="GUID"/> to a <see cref="SerializableGUIDJengaVersion"/>.
        /// </summary>
        /// <param name="guid">The GUID to convert.</param>
        /// <returns>The resulting SerializableGUID.</returns>
        public static unsafe SerializableGUIDJengaVersion ToSerializableGUID(this GUID guid)
        {
            return *(SerializableGUIDJengaVersion*)&guid;
        }

        /// <summary>
        /// Converts a <see cref="SerializableGUIDJengaVersion"/> to a <see cref="GUID"/>.
        /// </summary>
        /// <param name="guid">The SerializableGUID to convert.</param>
        /// <returns>The resulting GUID.</returns>
        public static unsafe GUID ToGUID(this SerializableGUIDJengaVersion guid)
        {
            return *(GUID*)&guid;
        }
        
        public static string ToString(this SerializableGUIDJengaVersion guid)
        {
            return $"{guid.Value0}{guid.Value1}";
        }
    }
    #endif
}

