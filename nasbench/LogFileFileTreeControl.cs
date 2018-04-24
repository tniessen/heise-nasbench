using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace nasbench
{
    public partial class LogFileFileTreeControl : UserControl
    {
        public LogFileFileTreeControl()
        {
            InitializeComponent();

            performanceTreeView.BeforeExpand += new TreeViewCancelEventHandler(BeforeTreeViewExpand);
            performanceTreeView.AfterSelect += new TreeViewEventHandler((sender, args) => UpdateInfo());
        }

        public void SelectGroup(Performance.Group group)
        {
            TreeNode node = performanceTreeView.Nodes.OfType<TreeNode>().First(n => n.Tag == group);
            if(node != null)
            {
                performanceTreeView.SelectedNode = node;
            }
        }

        public void RebuildView(IList<Performance.Group> Groups)
        {
            performanceTreeView.BeginUpdate();
            performanceTreeView.Nodes.Clear();

            for (int i = 0; i < Groups.Count; i++)
            {
                TreeNode groupNode = performanceTreeView.Nodes.Add("Gruppe " + (i + 1) + ": " + Units.FormatSize(Groups[i].FileSize));
                groupNode.Tag = Groups[i];
                groupNode.Nodes.Add("Loading...").Tag = "replace me";
            }

            performanceTreeView.EndUpdate();

            performanceGraphView.ClearGraph();
            UpdateInfo();
        }

        private void BeforeTreeViewExpand(object sender, TreeViewCancelEventArgs args)
        {
            performanceTreeView.BeginUpdate();

            TreeNode node = args.Node;
            if(node.Nodes.Count == 0 || "replace me".Equals(node.Nodes[0].Tag))
            {
                node.Nodes.Clear();

                if (node.Tag is Performance.Group)
                {
                    Performance.Group group = (Performance.Group)node.Tag;
                    for (int i = 0; i < group.Files.Count; i++)
                    {
                        TreeNode fileNode = args.Node.Nodes.Add("Datei " + (i + 1) + ": " + Units.FormatTime(group.Files[i].Time(true, true)));
                        fileNode.Tag = group.Files[i];
                        fileNode.Nodes.Add("Loading...").Tag = "replace me";
                    }
                }
                else if (node.Tag is Performance.File)
                {
                    Performance.File file = (Performance.File)node.Tag;
                    for (int i = 0; i < file.Chunks.Count; i++)
                    {
                        TreeNode chunkNode = node.Nodes.Add("Chunk " + (i + 1) + ": " + Units.FormatTime(file.Chunks[i].Time));
                        chunkNode.Tag = file.Chunks[i];
                    }
                }
            }

            performanceTreeView.EndUpdate();
        }

        private void UpdateInfo()
        {
            detailsTextBox.Clear();

            TreeNode selectedNode = performanceTreeView.SelectedNode;
            if (selectedNode == null) return;

            if(selectedNode.Tag is Performance.Group)
            {
                UpdateInfo((Performance.Group)selectedNode.Tag);
            }
            else if(selectedNode.Tag is Performance.File)
            {
                UpdateInfo((Performance.File)selectedNode.Tag);
            }
            else if(selectedNode.Tag is Performance.Chunk)
            {
                UpdateInfo((Performance.Chunk)selectedNode.Tag);
            }
        }

        private void UpdateInfo(Performance.Group group)
        {
            string[] details =
            {
                "Gesamtdauer: " + Units.FormatTime(group.Time(true, true)),
                "Größe: " + Units.FormatSize(group.TotalSize) + " (" + group.FileCount + " Dateien mit je " + Units.FormatSize(group.FileSize) + ")",
                "",
                "Übertragungsrate: " + Units.FormatRate(group.TransmissionRate(true, true)),
                "... ohne Verzögerung vor Stream: " + Units.FormatRate(group.TransmissionRate(false, true)),
                "... ohne Verzögerung nach Stream: " + Units.FormatRate(group.TransmissionRate(true, false)),
                "... ohne Verzögerungen: " + Units.FormatRate(group.TransmissionRate(false, false))
            };
            detailsTextBox.Text = string.Join("\r\n", details);

            performanceGraphView.CreateGraph(group);
        }

        private void UpdateInfo(Performance.File file)
        {
            string[] details =
            {
                "Gesamtdauer: " + Units.FormatTime(file.Time(true, true)),
                "Größe: " + Units.FormatSize(file.FileSize) + " (" + file.Chunks.Count + " Chunks)",
                "",
                "Übertragungsrate: " + Units.FormatRate(file.TransmissionRate(true, true)),
                "... ohne Verzögerung vor Stream: " + Units.FormatRate(file.TransmissionRate(false, true)),
                "... ohne Verzögerung nach Stream: " + Units.FormatRate(file.TransmissionRate(true, false)),
                "... ohne Verzögerungen: " + Units.FormatRate(file.TransmissionRate(false, false)),
                "",
                "Verzögerung vor Stream: " + Units.FormatTime(file.StreamStartTime),
                "Verzögerung nach Stream: " + Units.FormatTime(file.EndTime - file.StreamEndTime),
                "",
                file.StartEntry.SourcePath + " -> " + file.StartEntry.TargetPath
            };
            detailsTextBox.Text = string.Join("\r\n", details);

            performanceGraphView.CreateGraph(file);
        }

        private void UpdateInfo(Performance.Chunk chunk)
        {
            string[] details =
            {
                "Dauer: " + Units.FormatTime(chunk.Time),
                "Größe: " + Units.FormatSize(chunk.Size)
            };
            detailsTextBox.Text = string.Join("\r\n", details);

            performanceGraphView.CreateGraph(chunk.File);
        }
    }
}
