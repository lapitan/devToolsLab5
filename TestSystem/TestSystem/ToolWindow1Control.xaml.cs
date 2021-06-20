using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System;
using System.Collections.Generic;

namespace TestSystem
{
    /// <summary>
    /// Interaction logic for ToolWindow1Control.
    /// </summary>
    public partial class ToolWindow1Control : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow1Control"/> class.
        /// </summary>
        public ToolWindow1Control()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
           
            TestSystem tests = new TestSystem(filePath.Text, exePath.Text);
            Queue<KeyValuePair<Test, bool>> result = tests.start();
            string[] output = new string[tests.size];
            string shortinf = "";
            int i = 0;
            while (result.Count > 0)
            {
                Test testResult = result.Peek().Key;
                string testName = testResult.testName;
                string input = testResult.input;
                string answer = testResult.answer;
                string trueAnswer = testResult.trueAnswer;
                bool compare = result.Peek().Value;
                string comp;
                if (compare)
                {
                    comp = "done";
                }
                else
                {
                    comp = "WA";
                }
                string pushToOut = testName + " " + input + " " + answer + " " + trueAnswer + " " + comp;
                shortinf += testName + " " + comp + "\n";
                output[i] = pushToOut;
                i++;                
            }
            File.WriteAllLines(@"C:\Users\Xiaomi\Desktop\testsss.txt", output);
            resultBox.Text = shortinf;
        }

        private void TestComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TestInfo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}