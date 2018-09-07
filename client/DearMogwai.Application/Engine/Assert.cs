using System;

namespace DearMogwai.Application.Engine
{
    public static class Assert
    {
        public static void IsTrue(bool cond,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!cond)
            {
                string msg = $"{nameof(IsTrue)} failed in {memberName}({sourceFilePath}:{sourceLineNumber})";
#if DEBUG
                Console.WriteLine(msg);
#endif
                throw new ArgumentException(msg, nameof(cond));
            }
        }

        public static void IsNullOrWhitespace(string input,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                string msg = $"{nameof(IsNullOrWhitespace)} failed in {memberName}({sourceFilePath}:{sourceLineNumber})";
#if DEBUG
                Console.WriteLine(msg);
#endif
                throw new ArgumentException(msg, nameof(input));
            }
        }
    }
}
