using UnityEngine;

namespace Fairwood.Math {
	/// <summary>
	/// Vector functions. 各种向量、Color的常用操作方法
	/// </summary>
	public static class VectorExtension
	{
		public static Vector3 GetXModV3(Vector3 v3, float x)
		{
			return new Vector3(x, v3.y, v3.z);
		}
		public static Vector3 GetYModV3(Vector3 v3, float y)
		{
			return new Vector3(v3.x, y, v3.z);
		}
		public static Vector3 GetZModV3(Vector3 v3, float z)
		{
			return new Vector3(v3.x, v3.y, z);
		}
		public static Color GetAModClr(Color clr, float a)
		{
			return new Color(clr.r, clr.g, clr.b, a);
		}
		public static Vector3 GetCpntModV3(Vector3 v3, int ind, float c)
		{
			v3[ind] = c;
			return v3;
		}
		public static Color GetCpntModClr(Color clr, int ind, float c)
		{
			clr[ind] = c;
			return clr;
		}
		
		public static Vector3 GetXAddV3(Vector3 v3, float deltaX)
		{
			return new Vector3(v3.x + deltaX, v3.y, v3.z);
		}
		public static Vector3 GetYAddV3(Vector3 v3, float deltaY)
		{
			return new Vector3(v3.x, v3.y + deltaY, v3.z);
		}
		public static Vector3 GetZAddV3(Vector3 v3, float deltaZ)
		{
			return new Vector3(v3.x, v3.y, v3.z + deltaZ);
		}
        public static Vector3 SetV3X(this Vector3 v3, float x)
		{
			return new Vector3(x, v3.y, v3.z);
		}
        public static Vector3 SetV3Y(this Vector3 v3, float y)
		{
            return new Vector3(v3.x, y, v3.z);
		}
        public static Vector3 SetV3Z(this Vector3 v3, float z)
		{
            return new Vector3(v3.x, v3.y, z);
		}
        public static Color SetAlpha(this Color clr, float a)
		{
            return new Color(clr.r, clr.g, clr.b, a);
		}
        public static Vector3 SetV3Cpnt(this Vector3 v3, int ind, float c)
		{
			var tmpV3 = v3;
			tmpV3[ind] = c;
			return tmpV3;
		}

        public static Vector3 AddV3X(this Vector3 v3, float deltaX)
		{
            return new Vector3(v3.x + deltaX, v3.y, v3.z);
		}
        public static Vector3 AddV3Y(this Vector3 v3, float deltaY)
		{
            return new Vector3(v3.x, v3.y + deltaY, v3.z);
		}
        public static Vector3 AddV3Z(this Vector3 v3, float deltaZ)
		{
            return new Vector3(v3.x, v3.y, v3.z + deltaZ);
		}
        public static Color AddAlpha(this Color clr, float deltaA)
		{
            return new Color(clr.r, clr.g, clr.b, clr.a + deltaA);
		}
        public static Vector3 AddV3Cpnt(this Vector3 v3, int ind, float deltaC)
		{
			Vector3 tmpV3 = v3;
			tmpV3[ind] = tmpV3[ind] + deltaC;
            return tmpV3;
		}

        public static Vector2 ToVector2(this Vector3 me)
        {
            return new Vector2(me.x, me.y);
        }
        public static Vector3 ToVector3(this Vector2 me, float z = 0)
        {
            return new Vector3(me.x, me.y, z);
        }
    }

    public struct IntVector2
    {
        public bool Equals(IntVector2 other)
        {
            return i == other.i && j == other.j;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IntVector2 && Equals((IntVector2)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (i * 397) ^ j;
            }
        }

        public int i, j;
        public IntVector2(int i, int j)
        {
            this.i = i; this.j = j;
        }

        public readonly static IntVector2 zero = new IntVector2(0, 0);
        public readonly static IntVector2 up = new IntVector2(0, 1);
        public readonly static IntVector2 right = new IntVector2(1, 0);

        public static bool operator ==(IntVector2 a, IntVector2 b)
        {
            return a.i == b.i && a.j == b.j;
        }
        public static bool operator !=(IntVector2 a, IntVector2 b)
        {
            return !(a == b);
        }
        public static IntVector2 operator -(IntVector2 a)
        {
            return new IntVector2(-a.i, -a.j);
        }
        public override string ToString()
        {
            return new System.Text.StringBuilder().Append('(').Append(i).Append(',').Append(j).Append(')').ToString();
        }
        public static implicit operator Vector2(IntVector2 a)
        {
            return new Vector2(a.i, a.j);
        }
    }
    public struct IntVector3
    {
        public int i, j, k;
        public IntVector3(int i, int j, int k)
        {
            this.i = i; this.j = j; this.k = k;
        }

        public static implicit operator Vector3(IntVector3 a)
        {
            return new Vector3(a.i, a.j, a.k);
        }
    }
}