//using Patagames.Ocr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OCRTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var api = OcrApi.Create())
            //{
            //    api.Init(Patagames.Ocr.Enums.Languages.English);
            //    var text = api.GetTextFromImage("Image.png");
            //    Console.WriteLine(text);
            //}

            var fileName = "file.csv";
            var allMembers = new List<CommitteeMembers>();
            File.ReadAllLines(fileName).ToList().ForEach(x =>
            {
                var data = x.Split(",");
                allMembers.Add(new CommitteeMembers
                {
                    Name=data[0],
                    Code=data[1],
                    Number=Convert.ToInt32(data[2])
                });
            });

            Console.Write("Please enter code: ");
            var code=Console.ReadLine();
            if (string.IsNullOrWhiteSpace(code))
            {
                Console.WriteLine("Please enter the code.");
            }
            else if(!allMembers.Any(x=>x.Code==code))
            {
                Console.WriteLine("Invalid code entered.");
            }
            else
            {
                var codeMember = allMembers.FirstOrDefault(x => x.Code == code);
                if (codeMember.Number != 0)
                {
                    foreach (var item in allMembers.Where(x => x.Number != 0))
                    {
                        Console.WriteLine($"{item.Name}\t{item.Number}");
                    }
                    return;
                }

                var random = new Random(100000);
                var nextNumber = random.Next(1, 10);
                while (allMembers.Any(x => x.Number == nextNumber))
                {
                    nextNumber = random.Next(1, 10);
                }

                codeMember.Number = nextNumber;

                foreach (var item in allMembers)
                {
                    if (item.Code == code)
                    {
                        item.Number = nextNumber;
                    }
                }

                File.WriteAllLines(fileName, allMembers.Select(x => $"{x.Name},{x.Code},{x.Number}"));
                foreach (var item in allMembers.Where(x => x.Number != 0))
                {
                    Console.WriteLine($"{item.Name}\t{item.Number}");
                }
            }
        }
    }

    class CommitteeMembers
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Number { get; set; }
    }
}
