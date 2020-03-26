namespace miniJ.Elements
{
     struct ProjectTarget
    {
        public enum Architecture
        {
            X86,
            X86_64,
            LLVM
        }

        public enum ExecutableFormat
        {
            /// <summary>
            /// Windows
            /// </summary>
            PE,

            /// <summary>
            /// Linux
            /// </summary>
            OUT
        }

        public enum OutputType
        {
            Object,
            StaticLibrary,
            Executable
        }

        public Architecture TargetArchitecture;
        public ExecutableFormat TargetExecutableFormat;
        public OutputType TargetOutputType;
    }
}