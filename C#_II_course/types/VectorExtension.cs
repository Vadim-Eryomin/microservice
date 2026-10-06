using System;
using System.Linq;
using System.Reflection;

namespace StarshipGame
{
    public static class VectorExtensions
    {
        public static Vector Scale(this Vector vector, int scale)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));

            var data = vector.GetInternalData();
            return new Vector(data.Select(x => x * scale).ToArray());
        }

        public static int GetCoordinate(this Vector vector, int index)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));

            var data = vector.GetInternalData();

            if (index < 0 || index >= data.Length)
                throw new IndexOutOfRangeException($"Индекс {index} выходит за границы вектора");

            return data[index];
        }

        private static int[] GetInternalData(this Vector vector)
        {
            var fieldInfo = typeof(Vector).GetField(
                "data",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            return (int[])fieldInfo.GetValue(vector);
        }
    }
}
