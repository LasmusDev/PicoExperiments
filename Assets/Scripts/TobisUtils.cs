using System.IO;
using UnityEngine;

namespace TobisUtils
{
    public static class Math {
        public const float TAU = 6.283185307179586f;
        public static Vector2 AngToDir2D( float angRad ) => new Vector2 ( Mathf.Cos ( angRad ), Mathf.Sin( angRad ));
        public static float DirToAng2D( Vector2 v ) => Mathf.Atan2(v.y, v.x);
    }
    public static class LoadData
    {
        public static Texture2D ImageAsset(string path)
        {
            Texture2D tex = new Texture2D(2, 2);
            // A small 64x64 Unity logo encoded into a PNG.
            byte[] pngBytes = File.ReadAllBytes(path);

            tex.LoadImage(pngBytes);

            return tex;
        }
    }
}