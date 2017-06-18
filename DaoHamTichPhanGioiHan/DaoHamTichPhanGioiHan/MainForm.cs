using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace DaoHamTichPhanGioiHan
{
    public partial class MainForm : Form
    {
        const string INFINITY_SYMBOL = "\u221E";
        const string SQRT_SYMBOL = "\u221A";
        const string CBRT_SYMBOL = "\u221B";
        const string DIFF_SYMBOL = "\u2211";
        const string PI_SYMBOL = "\u220F";

        private TextBox focusedTextBox;

        string document;
        string beginDoc = @"
            \documentclass[12pt,a4paper]{report}
            \usepackage{amssymb}
            \usepackage{amsmath}
            \usepackage[utf8]{inputenc}
            \usepackage[vietnamese]{babel}
            \usepackage[margin=0.5cm]{geometry}
            \begin{document}
            \pagestyle{empty}
            \fussy
            {\Huge";
        string endDoc = @"}\end{document}";

        StreamReader reader;
        StreamWriter writer;

        public MainForm()
        {
            InitializeComponent();
        }

        public string Solve(string input)
        {
            writer = File.CreateText("input.mpl");
            writer.WriteLine("packageDir:= cat(currentdir(), kernelopts(dirsep) , \"DoAn.mla\"):");
            writer.WriteLine("march('open', packageDir):");

            writer.WriteLine("A:=" + input);
            writer.WriteLine("S:= GiaiChiTiet(A);");
            writer.WriteLine("XuatLoiGiai(A,S);");

            writer.Close();

            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = "solve.bat";
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;

            Process process = Process.Start(processInfo);
            process.WaitForExit();

            reader = File.OpenText("input.tex");
            string text = reader.ReadToEnd();
            reader.Close();

            return text;
        }

        public void DisplayText(string text)
        {
            try
            {
                document += text;
                document += endDoc;
                File.WriteAllText("input.tex", document);

                Process process = new Process();
                process.StartInfo.FileName = "display.bat";
                process.StartInfo.Arguments = "input";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit(20000);

                resultPictureBox.ImageLocation = "output.png";

                document = beginDoc;
            }
            catch
            {
                resultPictureBox.Image = Properties.Resources.error;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            document = beginDoc;
            infinityButton.Text = INFINITY_SYMBOL;
            directionComboBox.SelectedIndex = 0;
        }

        private void solveButton_Click(object sender, EventArgs e)
        {
            waitingLabel.Visible = true;

            string input = "";
            switch (mainTabControl.SelectedIndex)
            {
                case 0: // differential
                    if (diffTextBox2.Text == "")
                        input = "lhs(DaoHam(" + diffTextBox1.Text + ", 0));";
                    else
                        input = "Diff(" + diffTextBox1.Text + ", " + diffTextBox2.Text + ");";
                    break;
                case 1: // integral
                    input = "Int(";
                    switch (intTabControl.SelectedIndex)
                    {
                        case 0: // single integral
                            input += (intTextBox3.Text + ", " + intTextBox4.Text);
                            if (intTextBox1.Text != "" && intTextBox2.Text != "")
                                input += ("=" + intTextBox2.Text + ".." + intTextBox1.Text);
                            input += ");";
                                break;
                        case 1: // double integral
                            input += ("Int(" + int2TextBox5.Text + ", " + int2TextBox6.Text);
                            if (int2TextBox3.Text != "" && int2TextBox4.Text != "")
                                input += ("=" + int2TextBox4.Text + ".." + int2TextBox3.Text);
                            input += ("), " + int2TextBox7.Text);
                            if (int2TextBox1.Text != "" && int2TextBox2.Text != "")
                                input += ("=" + int2TextBox2.Text + ".." + int2TextBox1.Text);
                            input += ");";
                            break;
                        case 2: // triple integral
                            input += ("Int(Int(" + int3TextBox7.Text + ", " + int3TextBox8.Text);
                            if (int3TextBox5.Text != "" && int3TextBox6.Text != "")
                                input += ("=" + int3TextBox6.Text + ".." + int3TextBox5.Text);
                            input += ("), " + int3TextBox9.Text);
                            if (int3TextBox3.Text != "" && int3TextBox4.Text != "")
                                input += ("=" + int3TextBox4.Text + ".." + int3TextBox3.Text);
                            input += ("), " + int3TextBox10.Text);
                            if (int3TextBox1.Text != "" && int3TextBox2.Text != "")
                                input += ("=" + int3TextBox2.Text + ".." + int3TextBox1.Text);
                            input += ");";
                            break;
                    }
                    break;
                case 2: // limit
                    input = "Limit(" + limTextBox3.Text + ", " + limTextBox1.Text + " = " + limTextBox2.Text;
                    switch (directionComboBox.SelectedIndex)
                    {
                        case 0: // no direction
                            input += ");";
                            break;
                        case 1: // left direction
                            input += ", left);";
                            break;
                        case 2: // right direction
                            input += ", right);";
                            break;
                    }
                    break;
                case 3: // test
                    input = richTextBox1.Text;
                    break;
            }
            string text = Solve(input);
            DisplayText(text);

            waitingLabel.Visible = false;
        }

        private void directionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (directionComboBox.SelectedIndex)
            {
                case 0:
                    signLabel.Text = "";
                    break;
                case 1:
                    signLabel.Text = "-";
                    break;
                case 2:
                    signLabel.Text = "+";
                    break;
            }
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            focusedTextBox = (TextBox)sender;
        }

        private void infinityButton_Click(object sender, EventArgs e)
        {
            limTextBox2.Text += INFINITY_SYMBOL;
        }

        private void sqrtButton_Click(object sender, EventArgs e)
        {
            if (focusedTextBox != null)
            {
                focusedTextBox.Text += SQRT_SYMBOL;
            }
        }

        private void cbrtButton_Click(object sender, EventArgs e)
        {
            if (focusedTextBox != null)
            {
                focusedTextBox.Text += CBRT_SYMBOL;
            }
        }     

        private void piButton_Click(object sender, EventArgs e)
        {
            if (focusedTextBox != null)
            {
                focusedTextBox.Text += PI_SYMBOL;
            }
        }

        private void sigmaButton_Click(object sender, EventArgs e)
        {
            noteLabel.Visible = true;
            noteLabel.Text = "Cú pháp là :" + DIFF_SYMBOL + " [i= , ]( nội dung)";
            if (focusedTextBox != null)
            {
                focusedTextBox.Text += DIFF_SYMBOL;
            }
        }
    }
}
