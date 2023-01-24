using System.Collections.Generic;

namespace Core.Client.Utilities;

public static class ArrayExtensions
{
    public static IEnumerable<IEnumerable<T>> ToJagged<T>(this T[,] square)
    {
        for (int i = 0; i < square.GetLength(0); i++)
        {
            var row = new List<T>(square.GetLength(1));
            for (int j = 0; j < square.GetLength(1); j++)
            {
                row.Add(square[i, j]);
            }

            yield return row;
        }
    }
}
