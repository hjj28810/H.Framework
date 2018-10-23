namespace H.Framework.Core.Utilities
{
    // <summary>
    /// 字节序转换扩展
    /// </summary>
    public static class EndianExtensions
    {
        public static short SwapInt16(this short n)
        {
            return (short)(((n & 0xff) << 8) | ((n >> 8) & 0xff));
        }

        public static ushort SwapUInt16(this ushort n)
        {
            return (ushort)(((n & 0xff) << 8) | ((n >> 8) & 0xff));
        }

        public static int SwapInt32(this int n)
        {
            return ((SwapInt16((short)n) & 0xffff) << 0x10) |
                          (SwapInt16((short)(n >> 0x10)) & 0xffff);
        }

        public static uint SwapUInt32(this uint n)
        {
            return (uint)(((SwapUInt16((ushort)n) & 0xffff) << 0x10) |
                           (SwapUInt16((ushort)(n >> 0x10)) & 0xffff));
        }

        public static long SwapInt64(this long n)
        {
            return ((SwapInt32((int)n) & 0xffffffffL) << 0x20) |
                           (SwapInt32((int)(n >> 0x20)) & 0xffffffffL);
        }

        public static ulong SwapUInt64(this ulong n)
        {
            return (ulong)(((SwapUInt32((uint)n) & 0xffffffffL) << 0x20) |
                            (SwapUInt32((uint)(n >> 0x20)) & 0xffffffffL));
        }
    }
}