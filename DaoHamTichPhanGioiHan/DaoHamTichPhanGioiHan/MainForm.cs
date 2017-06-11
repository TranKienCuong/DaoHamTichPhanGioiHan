﻿using System;
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
        string document;

        public MainForm()
        {
            InitializeComponent();
        }

        public void DisplayText(string text)
        {
            document += text;
            document += @"  }
                        \end{document}
                        ";
            File.WriteAllText("input.tex", document);

            Process process = new Process();
            process.StartInfo.FileName = "batch.bat";
            process.StartInfo.Arguments = "input";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.WaitForExit();

            resultPictureBox.ImageLocation = "output.png";

            document = @"\documentclass[12pt,a4paper]{slides}
                    \begin{document}
                    {\huge
                    ";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            document = @"\documentclass[12pt,a4paper]{slides}
                    \begin{document}
                    {\huge
                    ";
            infinityButton.Text = INFINITY_SYMBOL;
            directionComboBox.SelectedIndex = 0;
        }

        private void solveButton_Click(object sender, EventArgs e)
        {
            waitingLabel.Visible = true;

            switch (tabControl.SelectedIndex)
            {
                case 0: // differential
                    DisplayText(@"\[ \frac{\partial Q}{\partial t} = \frac{\partial s}{\partial t} \]");
                    break;
                case 1: // integral
                    DisplayText(@"\[ \int_{0}^{\pi} \ (sin x + cos x )\, dx = 2. \]");
                    break;
                case 2: // limit
                    DisplayText(@"\[ \lim_{x \to a} \frac{f(x) - f(a)}{x - a}. \]");
                    break;
            }

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

        private void infinityButton_Click(object sender, EventArgs e)
        {
            limTextBox2.Text += INFINITY_SYMBOL;
        }
    }
}