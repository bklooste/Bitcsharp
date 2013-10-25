
namespace System
{
    public class GC
    {
        private static extern void __llmvsharp_gc_collect();
        public static void Collect()
        {
            __llmvsharp_gc_collect();
        }
    }
}
