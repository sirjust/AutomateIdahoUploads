using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomateWashingtonUploads;

namespace AutomateIdahoUploads
{
    class Program
    {
        static void Main(string[] args)
        {
            // take user input and convert to a string list
            Console.WriteLine("Please input completion data, then press Enter twice: ");
            ConvertDataToObjectList convertDataToObjectList = new ConvertDataToObjectList();
            List<string> convertedList = convertDataToObjectList.ConvertDataToStringList();

            // convert string list to completion list which can be used by the upload task
            Completion completion = new Completion();
            var finishedList = completion.listToCompletionList(convertedList);

            // if you want to see the data before it goes through the uploader uncomment the following code
            //foreach (Completion courseCompletion in finishedList)
            //{
            //    Console.WriteLine("Course: {0} Date: {1} License: {2} Name: {3}", courseCompletion.course, courseCompletion.date, courseCompletion.license, courseCompletion.name);
            //}
            //Console.Read();

            // send sanitized data to uploader, iterate and upload each entry
            UploadTask uploadTask = new UploadTask();
            uploadTask.inputCompletions(finishedList);

            // the log file is located in the bin/debug folder, it is called log.txt
            Console.WriteLine("Your uploads are complete. Please check the log file for any errors.");
            Console.Read();
        }
    }
}
