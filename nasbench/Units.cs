using System;

namespace nasbench
{
    public class Units
    {
        public static string FormatTime(ulong time)
        {
            // Special treatment for exceptionally short times
            if(time != 0 && time < 10 * 1000)
            {
                return String.Format("{0:0.000}ms", time / 1000.0);
            }

            return String.Format("{0:0.000}s", time / 1000000.0);
        }

        public static string FormatSize(double size)
        {
            string unit = "B";

            if (size >= 1024 * 2)
            {
                unit = "KB";
                size /= 1024;
                if (size >= 1024 * 2)
                {
                    unit = "MB";
                    size /= 1024;
                    if (size >= 1024 * 2)
                    {
                        unit = "GB";
                        size /= 1024;
                    }
                }
            }

            return String.Format("{0:0.00} {1}", size, unit);
        }

        public static string FormatRate(double rate)
        {
            return FormatSize(rate) + " pro Sekunde";
        }
    }
}
