using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary
{
    public class Evaluate
    {
        List<info> myInfo = new List<info>();
        List<Answers> myAnswers = new List<Answers>();
        string listPath;
        string answersPath;
        string[] answerKeys;
        string[] results;
        public List<Answers> AnswersList(string ansPa)
        {
            answersPath = ansPa;
            answerKeys = File.ReadAllLines(answersPath, Encoding.GetEncoding("iso-8859-9"));
            foreach (string answersString in answerKeys)
            {
                Answers a = new Answers();
                a.Type = answersString.Substring(0, 1);
                a.Number = answersString.Length - 1;
                a.True = answersString.Substring(1, a.Number);
                myAnswers.Add(a);
            }
            return myAnswers;
        }

        public List<info> StudentList(string liPa)
        {
            listPath = liPa;
            results = File.ReadAllLines(listPath, Encoding.GetEncoding("iso-8859-9"));
            foreach (string listString in results)
            {
                info i = new info();
                i.Name = listString.Substring(0, 12);
                i.Surname = listString.Substring(12, 12);
                i.No = listString.Substring(24, 9);
                i.Group = listString.Substring(33, 1);
                foreach (Answers ans in myAnswers)
                {
                    if (ans.Type == i.Group)
                    {
                        i.Answers = listString.Substring(34, ans.Number);
                        break;
                    }
                }
                myInfo.Add(i);
            }
            return myInfo;
        }
        public List<info> RightAnswers()
        {
            int correct;
            int counter;
            foreach (info i in myInfo)
            {
                correct = 0;
                counter = 0;
                foreach (Answers ans in myAnswers)
                {
                    if (i.Group == ans.Type)
                    {
                        while (counter != ans.Number)
                        {
                            if (i.Answers[counter].ToString() == ans.True.Substring(counter, 1))
                            {
                                correct++;
                            }
                            counter++;
                        }
                        break;
                    }
                }
                i.CorrectNumber = correct;
            }
            return myInfo;
        }
    }
}
