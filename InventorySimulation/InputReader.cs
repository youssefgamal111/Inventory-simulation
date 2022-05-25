using System.IO;
using InventoryModels;

namespace InventorySimulation
{
    static class InputReader
    {
        private const string FileName = "TestCase1.txt";

        public static SimulationSystem Load()
        {
            var result = new SimulationSystem();
            using (var sr = new StreamReader(FileName))
            {
                string currentLine = sr.ReadLine();
                while (currentLine != null)
                {
                    switch (currentLine)
                    {
                        case "OrderUpTo":
                            result.OrderUpTo = int.Parse(sr.ReadLine());
                            break;
                        case "ReviewPeriod":
                            result.ReviewPeriod = int.Parse(sr.ReadLine());
                            break;
                        case "StartInventoryQuantity":
                            result.StartInventoryQuantity = int.Parse(sr.ReadLine());
                            break;
                        case "StartLeadDays":
                            result.StartLeadDays = int.Parse(sr.ReadLine());
                            break;
                        case "StartOrderQuantity":
                            result.StartOrderQuantity = int.Parse(sr.ReadLine());
                            break;
                        case "NumberOfDays":
                            result.NumberOfDays = int.Parse(sr.ReadLine());
                            break;
                        case "DemandDistribution":
                            var curLinee = sr.ReadLine();
                            string[] arr;
                            Distribution db;
                            while (!string.IsNullOrEmpty(curLinee))
                            {
                                db = new Distribution();
                                arr = curLinee.Split(new[] { ", " },System.StringSplitOptions.None);
                                db.Value = int.Parse(arr[0]);
                                db.Probability = decimal.Parse(arr[1]);
                                result.DemandDistribution.Add(db);
                                curLinee = sr.ReadLine();
                            }
                            break;
                        case "LeadDaysDistribution":
                            curLinee = sr.ReadLine();
                            while (!string.IsNullOrEmpty(curLinee))
                            {
                                db = new Distribution();
                                arr = curLinee.Split(new[] { ", " }, System.StringSplitOptions.None);
                                db.Value = int.Parse(arr[0]);
                                db.Probability = decimal.Parse(arr[1]);
                                result.LeadDaysDistribution.Add(db); curLinee = sr.ReadLine();
                            }
                            break;
                    }
                    currentLine = sr.ReadLine();

                }


            }

            return result;
        }
    }
}
