namespace StarshipGame;

public class Vector
{
    private int[] data;

    public Vector(int[] data)
    {
        this.data = data;
    }

    public static Vector operator +(Vector a, Vector b)
    {
        if (a.data.Length != b.data.Length)
        {
            throw new ArgumentException("Vectors must be of the same length.");
        }

        int[] resultData = new int[a.data.Length];
        for (int i = 0; i < a.data.Length; i++)
        {
            resultData[i] = a.data[i] + b.data[i];
        }
        return new Vector(resultData);
    }

    public static bool operator ==(Vector a, Vector b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
        return a.data.SequenceEqual(b.data);
    }

    public static bool operator !=(Vector a, Vector b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Vector other)
        {
            return data.SequenceEqual(other.data);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return data.Aggregate(17, (current, item) => current * 31 + item);
    }
}