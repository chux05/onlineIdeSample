using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using onlineIdeSample.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineIdeSample.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //Post
        public IActionResult Edit()
        {
            return View();
        }

        string fileName = $"{Environment.CurrentDirectory}\\sample.py";
        public IActionResult Post([Bind("Id, Language, Code")] Test test)
        {
            if(test.Code != null && test.Code != "")
            {
                CreateFile(test.Code);
                string result = executeScript(fileName);
                bool output = TestReverse(result.Trim());
                test.Language = result.Trim();
                test.Result = output;
            }
            else
            {
                test.Code = @"print('That')";
            }

            return View(test);
        }

        public void CreateFile(string code)
        {                              

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(code);
            }

        }

        private bool TestReverse(string output)
        {

            return output.Equals("tac");

        }

        string executeScript(string fileName)
        {
            //1. create process info
            var psi = new ProcessStartInfo();
            psi.FileName = @"usr/bin/python3";

            //var a = "cat";
           
            //2. provide script and arguments
            var script = fileName;
            psi.Arguments = $"\"{script}\"";


            //3. process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            //4. Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                results = process.StandardOutput.ReadToEnd();
                    errors = process.StandardError.ReadToEnd();
            }
            if (errors != "")
            {
                List<string> errorParts = errors.Split(',').ToList();
                errorParts.RemoveAt(0);
                string errorView = String.Join(',', errorParts);
                return errorView;
            }
            return results;

        }


    }
}
