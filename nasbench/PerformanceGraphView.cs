using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace nasbench
{
    public partial class PerformanceGraphView : UserControl
    {
        public class Range
        {
            public readonly float Start, End;

            public Range(float start, float end)
            {
                this.Start = start;
                this.End = end;
            }

            public Range Flip()
            {
                return new Range(End, Start);
            }
        }

        public class Axis
        {
            public string Label;
            public Range Range;
            public IDictionary<float, Color> Markers;
            public Func<float, string> Format;

            public Axis()
            {
                Markers = new Dictionary<float, Color>();
            }
        }

        public class Graph
        {
            public PointF[] Points;
            public Axis AxisX, AxisY;
            public Func<float, float, string> Translation;
        }

        private Graph graph;

        public PerformanceGraphView()
        {
            InitializeComponent();

            Resize += new EventHandler((sender, args) => Refresh());
        }

        public void ClearGraph()
        {
            graph = null;
            Refresh();
        }

        public void CreateGraph(Performance.Group group)
        {
            ulong totalTime = group.Time(true, true);
            ulong totalSize = group.TotalSize;
            ulong currentTime = 0;
            ulong currentSize = 0;

            PointF[] points = new PointF[group.FileCount + 1];
            int pointIndex = 0;
            points[pointIndex++] = new PointF(0, 0);

            foreach (Performance.File file in group.Files)
            {
                currentTime += file.Time(true, true);
                currentSize += file.FileSize;

                points[pointIndex++] = new PointF(currentTime, currentSize);
            }

            Axis axisX = new Axis()
            {
                Range = new Range(0, totalTime),
                Label = "Zeit",
                Format = (f) => "" + Units.FormatTime((ulong)f)
            };

            Axis axisY = new Axis()
            {
                Range = new Range(0, totalSize),
                Label = "Größe",
                Format = (f) => Units.FormatSize(f)
            };

            graph = new Graph() { Points = points, AxisX = axisX, AxisY = axisY };
            Refresh();
        }

        public void CreateGraph(Performance.File file)
        {
            PointF[] points = new PointF[file.Chunks.Count + 3];
            int pointIndex = 0;
            points[pointIndex++] = new PointF(0, 0);
            points[pointIndex++] = new PointF(file.StreamStartTime, 0);

            foreach (Performance.Chunk chunk in file.Chunks)
            {
                points[pointIndex++] = new PointF(chunk.TimeSinceFileStart, chunk.BytesTransferred);
            }

            points[pointIndex] = new PointF(file.EndTime, file.FileSize);

            Axis axisX = new Axis()
            {
                Range = new Range(0, file.Time(true, true)),
                Label = "Zeit",
                Format = (f) => "" + Units.FormatTime((ulong)f)
            };
            axisX.Markers.Add(file.StreamStartTime, Color.Red);
            axisX.Markers.Add(file.StreamEndTime, Color.Purple);

            Axis axisY = new Axis()
            {
                Range = new Range(0, file.FileSize),
                Label = "Größe",
                Format = (f) => Units.FormatSize(f)
            };

            graph = new Graph() { Points = points, AxisX = axisX, AxisY = axisY };
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if(graph != null) PaintGraph(e.Graphics);
        }

        private static float Map(Range iRange, float iVal, Range oRange)
        {
            return Map(iRange.Start, iRange.End, iVal, oRange.Start, oRange.End);
        }

        private static float Map(float iLow, float iHigh, float iVal, float oLow, float oHigh)
        {
            return oLow + (iVal - iLow) * (oHigh - oLow) / (iHigh - iLow);
        }

        private void PaintGraph(Graphics g)
        {
            /*
             *-spaceLeft-      |         -spaceRight-
             *              spaceTop
             *      labelY     |
             *     endY |
             *          |
             *   startY |________________ labelX
             *        startX       endX           | spaceBot
             */

            // Fonts
            Font axisLabelFont = new Font("Arial", 9);
            Font axisValFont = new Font("Arial", 8);

            string yUpperBound = graph.AxisY.Format(graph.AxisY.Range.End);
            string yLowerBound = graph.AxisY.Format(graph.AxisY.Range.Start);
            string xUpperBound = graph.AxisX.Format(graph.AxisX.Range.End);
            string xLowerBound = graph.AxisX.Format(graph.AxisX.Range.Start);

            // Layout constraints
            int spaceLeft, spaceTop, spaceRight, spaceBottom;

            // The X label must fit into spaceRight
            spaceRight = 5 + (int)g.MeasureString(graph.AxisX.Label, axisLabelFont).Width;

            // The Y label must fit into spaceTop
            spaceTop = 5 + (int)g.MeasureString(graph.AxisY.Label, axisLabelFont).Height;

            // Values on the y axis must fit into spaceLeft
            spaceLeft = 5 + Math.Max(
                (int)g.MeasureString(yUpperBound, axisValFont).Width,
                (int)g.MeasureString(yLowerBound, axisValFont).Width
            );

            // Values on the x axis must fit into spaceBottom
            spaceBottom = 5 + Math.Max(
                (int)g.MeasureString(xUpperBound, axisValFont).Height,
                (int)g.MeasureString(xLowerBound, axisValFont).Height
            );

            Pen graphOutlinePen = new Pen(Color.Gray, 1.0f);

            // Graph area in graphics
            Range hAvailable = new Range(spaceLeft, Width - spaceRight);
            Range vAvailable = new Range(spaceTop, Height - spaceBottom);

            // Actual graph polygon
            PointF[] points = new PointF[graph.Points.Length + 2];

            // Bottom-left corner
            points[1] = new PointF(hAvailable.Start, vAvailable.End);

            // Generate upper border of polygon from graph data
            float lastX = 0;
            for (int i = 0; i < graph.Points.Length; i++)
            {
                lastX = Map(graph.AxisX.Range, graph.Points[i].X, hAvailable);
                float y = Map(graph.AxisY.Range, graph.Points[i].Y, vAvailable.Flip());
                points[i + 2] = new PointF(lastX, y);
            }

            // Bottom right corner
            points[0] = new PointF(lastX, vAvailable.End);

            // Draw polygon
            g.FillPolygon(new SolidBrush(Color.LightGreen), points);
            g.DrawPolygon(graphOutlinePen, points);

            // Draw horizontal markers
            foreach (KeyValuePair<float, Color> marker in graph.AxisY.Markers)
            {
                Pen markerPen = new Pen(marker.Value, 1.0f);
                float y = Map(graph.AxisY.Range, marker.Key, vAvailable.Flip());
                g.DrawLine(markerPen, hAvailable.Start, y, hAvailable.End + 5, y);
            }

            // Draw vertical markers
            foreach (KeyValuePair<float, Color> marker in graph.AxisX.Markers)
            {
                Pen markerPen = new Pen(marker.Value, 1.0f);
                float x = Map(graph.AxisX.Range, marker.Key, hAvailable);
                g.DrawLine(markerPen, x, vAvailable.Start - 5, x, vAvailable.End);
            }

            // Draw X and Y axis
            Pen axisPen = new Pen(Color.Black, 1.4f);
            g.DrawLine(axisPen, hAvailable.Start, vAvailable.Start, hAvailable.Start, vAvailable.End);
            g.DrawLine(axisPen, hAvailable.Start, vAvailable.End, hAvailable.End, vAvailable.End);

            Brush axisTextBrush = new SolidBrush(Color.Black);

            // Prepare string formats
            StringFormat leftCenter = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
            StringFormat rightCenter = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
            StringFormat leftTop = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            StringFormat rightTop = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };
            StringFormat rightBottom = new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };
            StringFormat centerBottom = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };

            // Draw labels and values on axes
            Rectangle labelXRect = new Rectangle(Width - spaceRight, 0, spaceRight, Height);
            g.DrawString(graph.AxisX.Label, axisLabelFont, axisTextBrush, labelXRect, centerBottom);

            Rectangle labelYRect = new Rectangle(0, 0, Width, spaceTop);
            g.DrawString(graph.AxisY.Label, axisLabelFont, axisTextBrush, labelYRect, leftCenter);

            Rectangle minYRect = new Rectangle(0, 0, spaceLeft, Height - spaceBottom);
            g.DrawString(yLowerBound, axisValFont, axisTextBrush, minYRect, rightBottom);

            Rectangle maxYRect = new Rectangle(0, spaceTop, spaceLeft, Height - spaceTop);
            g.DrawString(yUpperBound, axisValFont, axisTextBrush, maxYRect, rightTop);

            Rectangle minXRect = new Rectangle(spaceLeft, Height - spaceBottom, Width - spaceLeft, spaceBottom);
            g.DrawString(xLowerBound, axisValFont, axisTextBrush, minXRect, leftTop);

            Rectangle maxXRect = new Rectangle(0, Height - spaceBottom, Width - spaceRight, spaceBottom);
            g.DrawString(xUpperBound, axisValFont, axisTextBrush, maxXRect, rightTop);
        }
    }
}
