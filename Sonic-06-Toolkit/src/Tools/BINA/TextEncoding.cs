﻿using System;
using System.IO;
using Toolkit.Text;
using Toolkit.EnvironmentX;
using System.Windows.Forms;

namespace Toolkit.Tools
{
    public partial class TextEncoding : Form
    {
        private Main mainForm = null;
        private string location = Paths.currentPath;

        public TextEncoding(Form callingForm) {
            mainForm = callingForm as Main;
            InitializeComponent();
        }

        private void TextEditor_Load(object sender, EventArgs e) {
            clb_MSTs.Items.Clear();

            if (Directory.GetFiles(location, "*.mst").Length > 0) {
                combo_Mode.SelectedIndex = 0;
            } else if (Directory.GetFiles(location, "*.xml").Length > 0) {
                combo_Mode.SelectedIndex = 1;
            } else {
                MessageBox.Show(SystemMessages.msg_NoConvertableFiles, SystemMessages.tl_NoFilesAvailable(string.Empty), MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private void Clb_MSTs_SelectedIndexChanged(object sender, EventArgs e) {
            clb_MSTs.ClearSelected();
            btn_Process.Enabled = clb_MSTs.CheckedItems.Count > 0;
        }

        private void Combo_Mode_SelectedIndexChanged(object sender, EventArgs e) {
            if (combo_Mode.SelectedIndex == 0) {
                btn_Process.Text = "Export";

                clb_MSTs.Items.Clear();
                foreach (string MST in Directory.GetFiles(location, "*.mst", SearchOption.TopDirectoryOnly))
                    if (File.Exists(MST) && Verification.VerifyMagicNumberExtended(MST))
                        clb_MSTs.Items.Add(Path.GetFileName(MST));

                if (Directory.GetFiles(location, "*.mst").Length == 0) {
                    MessageBox.Show(SystemMessages.msg_NoConvertableFiles, SystemMessages.tl_NoFilesAvailable("XML"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (Directory.GetFiles(location, "*.xml").Length == 0) Close();
                    else combo_Mode.SelectedIndex = 1;
                }
            } else if (combo_Mode.SelectedIndex == 1) {
                btn_Process.Text = "Import";

                clb_MSTs.Items.Clear();
                foreach (string XML in Directory.GetFiles(location, "*.xml", SearchOption.TopDirectoryOnly))
                    if (File.Exists(XML) && Verification.VerifyXML(XML, "MST"))
                        clb_MSTs.Items.Add(Path.GetFileName(XML));

                if (Directory.GetFiles(location, "*.xml").Length == 0) {
                    MessageBox.Show(SystemMessages.msg_NoConvertableFiles, SystemMessages.tl_NoFilesAvailable("MST"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (Directory.GetFiles(location, "*.mst").Length == 0) Close();
                    else combo_Mode.SelectedIndex = 0;
                }
            }
        }

        private async void Btn_Process_Click(object sender, EventArgs e) {
            if (combo_Mode.SelectedIndex == 0) {
                foreach (string MST in clb_MSTs.CheckedItems) {
                    if (File.Exists(Path.Combine(location, MST)) && Verification.VerifyMagicNumberExtended(Path.Combine(location, MST))) {
                        mainForm.Status = StatusMessages.cmn_Exporting(MST, false);
                        var export = await ProcessAsyncHelper.ExecuteShellCommand(Paths.MSTTool,
                                           $"\"{Path.Combine(location, MST)}\"",
                                           Application.StartupPath,
                                           100000);
                        if (export.Completed)
                            if (export.ExitCode != 0)
                                MessageBox.Show($"{SystemMessages.ex_MSTExportError}\n\n{export.Output}", SystemMessages.tl_FatalError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            } else if (combo_Mode.SelectedIndex == 1) {
                foreach (string XML in clb_MSTs.CheckedItems) {
                    if (File.Exists(Path.Combine(location, XML)) && Verification.VerifyXML(Path.Combine(location, XML), "MST")) {
                        mainForm.Status = StatusMessages.cmn_Importing(XML, false);
                        var export = await ProcessAsyncHelper.ExecuteShellCommand(Paths.MSTTool,
                                           $"\"{Path.Combine(location, XML)}\"",
                                           Application.StartupPath,
                                           100000);
                        if (export.Completed)
                            if (export.ExitCode != 0)
                                MessageBox.Show($"{SystemMessages.ex_XMLImportError}\n\n{export.Output}", SystemMessages.tl_FatalError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Btn_SelectAll_Click(object sender, EventArgs e) {
            for (int i = 0; i < clb_MSTs.Items.Count; i++) clb_MSTs.SetItemChecked(i, true);
            btn_Process.Enabled = true;
        }

        private void Btn_DeselectAll_Click(object sender, EventArgs e) {
            for (int i = 0; i < clb_MSTs.Items.Count; i++) clb_MSTs.SetItemChecked(i, false);
            btn_Process.Enabled = false;
        }
    }
}