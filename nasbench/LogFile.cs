using System;
using System.Collections.Generic;
using System.Text;

namespace nasbench
{
    public class LogFile
    {
        public enum Tag
        {
            GENERATE_INIT_DONE, // Entry
            GENERATE_FILE_DONE, // GenerateFileEventArgs
            COPY_FILE_START,    // CopyFileEntry
            COPY_FILE_STREAM,   // CopyFileEntry
            COPY_FILE_CHUNK,    // CopyFileEntry
            COPY_FILE_END,      // CopyFileEntry
            COPY_GROUP_START,   // CopyGroupEntry
            COPY_GROUP_END,     // CopyGroupEntry
            VERIFY_INIT_DONE,   // Entry
            VERIFY_OKAY,        // VerifyFileEntry
            VERIFY_FAIL         // VerifyFailEntry
        }

        public class Entry
        {
            public Tag Tag;
            public ulong Timestamp;
        }

        public class GenerateFileEntry : Entry
        {
            public uint GroupIndex;
            public uint FileIndex;
            public ulong FileSize;
            public string Path;
        }

        public class CopyGroupEntry : Entry
        {
            public uint GroupIndex;
            public uint FileCount;
            public ulong FileSize;
        }

        public class CopyFileEntry : Entry
        {
            public uint GroupIndex;
            public uint FileIndex;
            public ulong FileSize;
            public string SourcePath;
            public string TargetPath;
            public ulong BytesTransferred;
            public ulong TimeSinceStart;
        }

        public class VerifyFileEntry : Entry
        {
            public uint GroupIndex;
            public uint FileIndex;
            public ulong FileSize;
            public string Path;
        }

        public class VerifyFailEntry : VerifyFileEntry
        {
            public string FailMessage;
        }

        public static IEnumerable<Entry> ParseFile(string path)
        {
            return ParseFile(ReadLines(path));
        }

        private static IEnumerable<string> ReadLines(string path)
        {
            System.IO.StreamReader reader = null;
            try
            {
                reader = new System.IO.StreamReader(path, Encoding.UTF8);
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
            finally
            {
                if(reader != null) reader.Close();
            }
        }

        public static IEnumerable<Entry> ParseFile(IEnumerable<string> lines)
        {
            foreach(string line in lines)
            {
                if(!String.IsNullOrEmpty(line))
                {
                    yield return ParseEntry(line);
                }
            }
        }

        public static Entry ParseEntry(string line)
        {
            try
            {
                string[] args = line.Split('\t');

                Tag tag = (Tag)Enum.Parse(typeof(Tag), args[1]);
                Entry entry;

                switch (tag)
                {
                    case Tag.GENERATE_INIT_DONE:
                    case Tag.VERIFY_INIT_DONE:
                        entry = new Entry();
                        break;
                    case Tag.GENERATE_FILE_DONE:
                        entry = new GenerateFileEntry()
                        {
                            GroupIndex = uint.Parse(args[2]),
                            FileIndex = uint.Parse(args[3]),
                            FileSize = ulong.Parse(args[4]),
                            Path = args[5]
                        };
                        break;
                    case Tag.VERIFY_OKAY:
                        entry = new VerifyFileEntry()
                        {
                            GroupIndex = uint.Parse(args[2]),
                            FileIndex = uint.Parse(args[3]),
                            FileSize = ulong.Parse(args[4]),
                            Path = args[5]
                        };
                        break;
                    case Tag.VERIFY_FAIL:
                        entry = new VerifyFailEntry()
                        {
                            GroupIndex = uint.Parse(args[2]),
                            FileIndex = uint.Parse(args[3]),
                            FileSize = ulong.Parse(args[4]),
                            Path = args[5],
                            FailMessage = args[6]
                        };
                        break;
                    case Tag.COPY_GROUP_START:
                    case Tag.COPY_GROUP_END:
                        entry = new CopyGroupEntry()
                        {
                            GroupIndex = uint.Parse(args[2]),
                            FileCount = uint.Parse(args[3]),
                            FileSize = ulong.Parse(args[4])
                        };
                        break;
                    case Tag.COPY_FILE_START:
                    case Tag.COPY_FILE_STREAM:
                    case Tag.COPY_FILE_CHUNK:
                    case Tag.COPY_FILE_END:
                        entry = new CopyFileEntry()
                        {
                            GroupIndex = uint.Parse(args[2]),
                            FileIndex = uint.Parse(args[3]),
                            FileSize = ulong.Parse(args[4]),
                            SourcePath = args[5],
                            TargetPath = args[6],
                            BytesTransferred = ulong.Parse(args[7]),
                            TimeSinceStart = ulong.Parse(args[8])
                        };
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "Invalid log tag: " + line);
                        return null;
                }

                entry.Tag = tag;
                entry.Timestamp = ulong.Parse(args[0].Trim());

                return entry;
            }
            catch(Exception e)
            {
                throw new Exception("Invalid log line: " + e.Message + ": " + line, e);
            }
        }
    }
}
