using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace TestSystem
{
    class Test
    {

        public string testName;
        public string input;
        public string answer;
        public string trueAnswer;

        public Test(string testName_, string input_, string answer_)
        {
            testName = testName_;
            input = input_;
            answer = answer_;
        }

        public Test(string[] fullTest)
        {
            testName = fullTest[0];
            input = fullTest[1];
            answer = fullTest[2];
            trueAnswer = fullTest[3];
        }

        public override string ToString()
        {
            return testName;
        }
    }

    class TestSystem
    {

        private Queue<Test> Tests;
        public int size;
        private string codePath;
        private string exeName;
        private string testPath;


        public TestSystem(string testsPath_, string codePath_)
        {
            Tests = new Queue<Test>();
            codePath = codePath_;
            testPath = testsPath_;
            /*парсер*/
            string[] filelines = File.ReadAllLines(testsPath_);
            for (int i = 0; i < filelines.Length;)
            {
                string currentTestName = "";
                string currentInput = "";
                string currentAnswer = "";
                if (filelines[i] == "[TESTNAME]")
                {
                    i++;
                    currentTestName = filelines[i];
                    i++;
                }
                if (filelines[i] == "[DATA]")
                {
                    i++;
                    while (filelines[i] != "[ANSWER]")
                    {
                        currentInput += '"' + filelines[i] + '"';
                        i++;
                        if (filelines[i] != "[ANSWER]")
                        {
                            currentInput += ' ';
                        }
                    }
                }
                if (filelines[i] == "[ANSWER]")
                {
                    i++;
                    while (filelines[i] != "[TESTNAME]")
                    {
                        currentAnswer += filelines[i];
                        i++;
                        if (i >= filelines.Length)
                        {
                            break;
                        }
                        if (filelines[i] != "[TESTNAME]")
                        {
                            currentAnswer += '\n';
                        }
                    }
                }

                Tests.Enqueue(new Test(currentTestName, currentInput, currentAnswer));
            }
            size = Tests.Count;
        }

        public Queue<KeyValuePair<Test, bool>> start()
        {

            Queue<KeyValuePair<Test, bool>> result = new Queue<KeyValuePair<Test, bool>>();

            /*пробег тестов*/

            Process process = new Process();

            //process.StartInfo.WorkingDirectory = codePath;
            process.StartInfo.FileName = codePath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();
            while (Tests.Count > 0)
            {

                process.StartInfo.Arguments = Tests.Peek().input;

                process.Start();

                StreamReader reader = process.StandardOutput;
                string answer = reader.ReadToEnd();

                string[] fullInfo = new string[4];
                fullInfo[0] = Tests.Peek().testName;
                fullInfo[1] = Tests.Peek().input;
                fullInfo[2] = Tests.Peek().answer;
                fullInfo[3] = answer;

                bool compare = (fullInfo[2] == fullInfo[3]);

                KeyValuePair<Test, bool> pushToResult = new KeyValuePair<Test, bool>(new Test(fullInfo), compare);

                result.Enqueue(pushToResult);
                Tests.Dequeue();
            }

            return result;
        }

    }
}
