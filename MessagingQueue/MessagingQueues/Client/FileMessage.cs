using System;

namespace Client
{
    [Serializable]
    public class FileMessage
    {
        public byte[] Data { get; set; }

        public string FileName { get; set; }

        public bool IsSequence { get; set; }

        public bool IsEndOfFile { get; set; }
    }
}
