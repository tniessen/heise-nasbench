using System.Collections.Generic;
using System.Linq;

namespace nasbench
{
    public class Performance
    {
        public class Group
        {
            public readonly List<File> Files;

            public LogFile.CopyGroupEntry StartEntry;
            public LogFile.CopyGroupEntry EndEntry;

            public Group()
            {
                Files = new List<File>();
            }

            public ulong FileCount
            {
                get { return StartEntry.FileCount; }
            }

            public ulong FileSize
            {
                get { return StartEntry.FileSize; }
            }

            public ulong TotalSize
            {
                get { return FileCount * FileSize; }
            }

            public ulong ApproxTotalTime
            {
                get { return EndEntry.Timestamp - StartEntry.Timestamp; }
            }

            public ulong Time(bool incBeforeStream, bool incAfterStream)
            {
                ulong time = 0;
                foreach (File file in Files)
                {
                        time += file.Time(incBeforeStream, incAfterStream);
                }
                return time;
            }

            public double TransmissionRate(bool incBeforeStream, bool incAfterStream)
            {
                ulong size = FileSize * FileCount;
                return 1000000 * size / (double)Time(incBeforeStream, incAfterStream);
            }
        }

        public class File
        {
            public readonly Group Group;
            public readonly List<Chunk> Chunks;

            public LogFile.CopyFileEntry StartEntry;
            public LogFile.CopyFileEntry StreamStartEntry;
            public LogFile.CopyFileEntry EndEntry;

            public File(Group group)
            {
                this.Group = group;
                this.Chunks = new List<Chunk>();
            }

            public ulong FileSize
            {
                get { return StartEntry.FileSize; }
            }

            public ulong StreamStartTime
            {
                get { return StreamStartEntry.TimeSinceStart - StartEntry.TimeSinceStart; }
            }

            public ulong StreamEndTime
            {
                get { return Chunks.Last().TimeSinceFileStart - StartEntry.TimeSinceStart; }
            }

            public ulong EndTime
            {
                get { return EndEntry.TimeSinceStart - StartEntry.TimeSinceStart; }
            }

            public ulong Time(bool incBeforeStream, bool incAfterStream)
            {
                ulong end = incAfterStream ? EndTime : StreamEndTime;
                ulong start = incBeforeStream ? 0 : StreamStartTime;
                return end - start;
            }

            public double TransmissionRate(bool incBeforeStream, bool incAfterStream)
            {
                return 1000000 * FileSize / Time(incBeforeStream, incAfterStream);
            }
        }

        public class Chunk
        {
            public readonly File File;

            public LogFile.CopyFileEntry VirtualStartEntry;
            public LogFile.CopyFileEntry EndEntry;

            public Chunk(File file)
            {
                this.File = file;
            }

            public ulong TimeSinceFileStart
            {
                get { return EndEntry.TimeSinceStart - File.StartEntry.TimeSinceStart; }
            }

            public ulong Time
            {
                get { return EndEntry.TimeSinceStart - VirtualStartEntry.TimeSinceStart; }
            }

            public ulong BytesTransferred
            {
                get { return EndEntry.BytesTransferred; }
            }

            public ulong Size
            {
                get { return EndEntry.BytesTransferred - VirtualStartEntry.BytesTransferred; }
            }

            public double TransmissionRate
            {
                get { return 1000000 * Size / (double)Time; }
            }
        }

        public static IEnumerable<Group> ParseLog(IEnumerable<LogFile.Entry> entries)
        {
            Group currentGroup = null;
            File currentFile = null;

            foreach(LogFile.Entry entry in entries)
            {
                switch(entry.Tag)
                {
                    case LogFile.Tag.COPY_GROUP_START:
                        currentGroup = new Group() { StartEntry = (LogFile.CopyGroupEntry)entry };
                        break;
                    case LogFile.Tag.COPY_GROUP_END:
                        currentGroup.EndEntry = (LogFile.CopyGroupEntry)entry;
                        yield return currentGroup;
                        break;
                    case LogFile.Tag.COPY_FILE_START:
                        currentFile = new File(currentGroup) { StartEntry = (LogFile.CopyFileEntry)entry };
                        currentGroup.Files.Add(currentFile);
                        break;
                    case LogFile.Tag.COPY_FILE_END:
                        currentFile.EndEntry = (LogFile.CopyFileEntry)entry;
                        break;
                    case LogFile.Tag.COPY_FILE_STREAM:
                        currentFile.StreamStartEntry = (LogFile.CopyFileEntry)entry;
                        break;
                    case LogFile.Tag.COPY_FILE_CHUNK:
                        Chunk chunk = new Chunk(currentFile);
                        if(currentFile.Chunks.Count > 0)
                        {
                            chunk.VirtualStartEntry = currentFile.Chunks.Last().EndEntry;
                        }
                        else
                        {
                            chunk.VirtualStartEntry = currentFile.StreamStartEntry;
                        }
                        currentFile.Chunks.Add(chunk);
                        chunk.EndEntry = (LogFile.CopyFileEntry)entry;
                        break;
                }
            }
        }
    }
}
