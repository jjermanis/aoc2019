using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    class Compound
    {
        public Compound() { }
        public Compound(string desc)
        {
            var vals = desc.Trim().Split(" ");
            Chemical = vals[1];
            Units = long.Parse(vals[0]);
        }
        public string Chemical { get; set; }
        public long Units { get; set; }
        public override string ToString()
            => $"{Units} {Chemical}";
    }

    class Reaction
    {
        public Reaction(string desc)
        {
            var inOut = desc.Split("=>");
            var inputs = inOut[0].Split(',');
            Inputs = new List<Compound>();
            foreach (var input in inputs)
                Inputs.Add(new Compound(input));
            Output = new Compound(inOut[1]);
        }
        public Compound Output { get; set; }
        public List<Compound> Inputs { get; set; }
    }

    public class Day14 : DayBase, IDay
    {
        private readonly IEnumerable<Reaction> _reactions;

        public Day14(string filename)
        {
            _reactions = LoadReactions(filename);
        }
        public Day14() : this("Day14.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"ORE needed for 1 FUEL: {OreNeeded()}");
            Console.WriteLine($"Max FUEL from 1 trillion ORE: {MaxFuel()}");
        }

        public long OreNeeded(long fuelNeeded = 1)
        {
            var demands = new Queue<Compound>();
            demands.Enqueue(new Compound() { Chemical = "FUEL", Units = fuelNeeded });
            var bank = new Dictionary<string, long>();
            var result = 0L;
            while (demands.Count > 0)
            {
                var curr = demands.Dequeue();
                if (curr.Chemical.Equals("ORE"))
                {
                    result += curr.Units;
                }
                else
                {
                    // Check the bank - we might already haev everything that's needed
                    var quantityNeeded = curr.Units - WithdrawlFromBank(bank, curr.Chemical, curr.Units);

                    if (quantityNeeded > 0)
                    {
                        // Find the reaction that produces the needed chemical
                        var reaction = _reactions.Where(r => r.Output.Chemical.Equals(curr.Chemical)).First();

                        long reactionsNeeded = (quantityNeeded - 1) / reaction.Output.Units + 1;
                        long unitsProduced = reactionsNeeded * reaction.Output.Units;
                        if (unitsProduced > quantityNeeded)
                            AddToBank(bank, curr.Chemical, unitsProduced - quantityNeeded);

                        // Add demands for all input chemicals in the reaction
                        foreach (var input in reaction.Inputs)
                        {
                            var newDemand = new Compound() { Chemical = input.Chemical, Units = reactionsNeeded * input.Units };
                            demands.Enqueue(newDemand);
                        }
                    }
                }
            }
            return result;
        }

        public long MaxFuel(long availableOre = 1_000_000_000_000L)
        {
            // Do a binary search for the answer.  Look for the OreNeeded(n) that is less than
            // or equal to availableOre
            long min = 1;
            long max = availableOre;

            while (max - min > 1)
            {
                var curr = (min + max) / 2;
                var currOre = OreNeeded(curr);
                if (currOre > availableOre)
                    max = curr - 1;
                else
                    min = curr;
            }
            return (OreNeeded(max) <= availableOre) ? max : min;
        }

        private void AddToBank(
            Dictionary<string, long> bank,
            string chemical,
            long count)
        {
            if (bank.ContainsKey(chemical))
                bank[chemical] += count;
            else
                bank[chemical] = count;
        }

        private long WithdrawlFromBank(
            Dictionary<string, long> bank,
            string chemical,
            long amount)
        {
            if (bank.ContainsKey(chemical))
            {
                var result = Math.Min(amount, bank[chemical]);
                bank[chemical] -= result;
                return result;
            }
            else
                return 0;
        }

        private List<Reaction> LoadReactions(string filename)
        {
            var result = new List<Reaction>();
            foreach (var line in TextFileLines(filename))
                result.Add(new Reaction(line));
            return result;
        }
    }
}
