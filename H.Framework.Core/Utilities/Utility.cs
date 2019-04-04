using System;

namespace H.Framework.Core.Utilities
{
    public class Utility
    {
        public static string ObjectID => Guid.NewGuid().ToString("N");
    }
}