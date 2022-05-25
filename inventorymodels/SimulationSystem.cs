using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            DemandDistribution = new List<Distribution>();
            LeadDaysDistribution = new List<Distribution>();
            SimulationCases = new List<SimulationCase>();
            PerformanceMeasures = new PerformanceMeasures();
        }
        public void generateDist()
        {
            decimal cum = 0;
            foreach (var item in DemandDistribution)
            {
                item.MinRange = (int)(cum * 100 +1) ;
                cum = cum + item.Probability;
                item.CummProbability = cum;
                item.MaxRange = (int)(cum * 100);
            }
            cum = 0;
            foreach (var item in LeadDaysDistribution)
            {
                item.MinRange = (int)(cum * 100+1) ;
                cum = cum + item.Probability;
                item.CummProbability = cum;
                item.MaxRange = (int)(cum * 100);
            }
        }

        ///////////// INPUTS /////////////

        public int OrderUpTo { get; set; }
        public int ReviewPeriod { get; set; }
        public int NumberOfDays { get; set; }
        public int StartInventoryQuantity { get; set; }
        public int StartLeadDays { get; set; }
        public int StartOrderQuantity { get; set; }
        public List<Distribution> DemandDistribution { get; set; }
        public List<Distribution> LeadDaysDistribution { get; set; }

        ///////////// OUTPUTS /////////////

        public List<SimulationCase> SimulationCases { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }
        public void generateCases()
        {
            decimal avgEnding = 0;
            decimal avgShortage = 0;

            generateDist();

            int dayOfNewOrder=-1;
            int quantityOfNewOrder=-1;
            int shortages = 0;
            Random random = new Random();
            for (int i = 1; i <= NumberOfDays; i++)
            {
                SimulationCase c = new SimulationCase();
                c.Day = i;

                if (i == 1)
                {
                    dayOfNewOrder = StartLeadDays + i ;
                    quantityOfNewOrder = StartOrderQuantity;
                    c.BeginningInventory = StartInventoryQuantity;
                    
                    c.LeadDays = 0;
                     c.OrderQuantity = StartOrderQuantity;
                    c.RandomLeadDays = 0;
                }
                else
                {
                    if (i == dayOfNewOrder)
                    {
                       c.BeginningInventory = SimulationCases.Last().EndingInventory + quantityOfNewOrder ;
                        
                       c.ShortageQuantity = 0;
                    }

                   else c.BeginningInventory = SimulationCases[i - 2].EndingInventory;
                }


               

            
                c.RandomDemand = random.Next(1, 100);
                c.Demand = getDemands(c.RandomDemand, DemandDistribution);
              
  
                    int res = c.BeginningInventory - c.Demand;
                 if (res <= 0) {
                        c.ShortageQuantity = Math.Abs(res)+shortages;
                        shortages = c.ShortageQuantity;
                        c.EndingInventory = 0; }
                    else
                    { c.EndingInventory = res-shortages;
                      c.ShortageQuantity = 0;
                    shortages = 0; } 
                if (i % ReviewPeriod == 0)
                {
                    c.Cycle = (int)(i / ReviewPeriod);
                    c.DayWithinCycle = ReviewPeriod;
                    
                    c.OrderQuantity = OrderUpTo - c.EndingInventory +shortages;
                    c.RandomLeadDays = random.Next(1, 100);
                    if (i != 1)
                    {
                        c.LeadDays = getLeadDays(c.RandomLeadDays, LeadDaysDistribution) ;
                        dayOfNewOrder = c.LeadDays + i + 1;

                    }
                    quantityOfNewOrder = c.OrderQuantity;
                }
                else
                {
                    c.DayWithinCycle = i % ReviewPeriod;
                    c.Cycle = (int)(i / ReviewPeriod +1);
                    c.OrderQuantity = 0;
                    c.RandomLeadDays = 0;
                   if(i!=1) c.LeadDays = 0;
                }
                SimulationCases.Add(c);

                avgEnding += c.EndingInventory;
                avgShortage += c.ShortageQuantity;
                if (dayOfNewOrder - i < 0)
                    c.DaysUntillArrive = 0;
                else c.DaysUntillArrive = dayOfNewOrder - i;
            }
            PerformanceMeasures.EndingInventoryAverage = avgEnding / SimulationCases.Count();
            PerformanceMeasures.ShortageQuantityAverage = avgShortage / SimulationCases.Count();

        }
        public int getLeadDays(int rand, List<Distribution> LeadDaysDistribution)
        {

            foreach (var L in LeadDaysDistribution)
            {
                if (rand >= L.MinRange && rand <= L.MaxRange)
                {


                    return L.Value;


                }
            }
            return 0;
        }
        public int getDemands(int rand, List<Distribution> DemandDistribution)
        {
            foreach (var D in DemandDistribution)
            {
                if (rand >= D.MinRange && rand <= D.MaxRange)
                {
                    return D.Value;


                }
            }
            return 0;

        }
    }
}
