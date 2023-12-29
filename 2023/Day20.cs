using Shared;

namespace _2023;

public static class Day20
{
    public static readonly Dictionary<string, Module> Modules = new();
    public static readonly Queue<Pulse> Pulses = new();

    private static int Round = 1;
    public static readonly long[] PulseCount = new long[] { 0, 0 };

    private const bool IsPartOne = false;
    // Part 2 information gathered by examining the input... these are the first to last conjunction modules in the input
    public static readonly string[] ImportantModulNames = new[] { "kb", "qt", "ck", "hc" };

    public static string Solve()
    {
        foreach (var line in File.ReadLines(@".\Input\Day20.txt"))
        {
            var lineSplit = line.Split("->", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            string name = lineSplit[0];
            string[] targets = lineSplit[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            Module module = name.First() switch
            {
                '%' => new FlipFlipModule(name[1..], targets.ToList()),
                '&' => new ConjunctionModule(name[1..], targets.ToList()),
                _ => new BroadcastModule(name, targets.ToList()),
            };
            Modules.Add(module.Name, module);
        }

        // Initialize conjunction inputs to low
        foreach (ConjunctionModule conjunctionModule in Modules.Values.OfType<ConjunctionModule>())
        {
            var senders = Modules.Where(e => e.Value.Targets.Contains(conjunctionModule.Name)).Select(e => e.Key).ToList();
            foreach (var sender in senders)
                conjunctionModule.IsOn[sender] = false;
        }

        // Simulate button presses
        while (Modules.Where(m => ImportantModulNames.Contains(m.Key)).Any(m => !((ConjunctionModule)m.Value).LowSignalRound.Any()))
        {
            Pulses.Enqueue(new Pulse("broadcaster", "-", false));
            PulseCount[0]++;
            while (Pulses.TryDequeue(out var pulse))
            {
                if (Modules.ContainsKey(pulse.Target))
                    Modules[pulse.Target].ReceiveSignal(pulse.IsHigh, pulse.Source);
            }

            // Part 1 exit point
            if (IsPartOne && Round == 1000)
                return (PulseCount[0] * PulseCount[1]).ToString();

            Round++;
        }

        // Part 2 prediction for RX
        var ImportantRounds = Modules.Where(m => ImportantModulNames.Contains(m.Key)).Select(m => ((ConjunctionModule)m.Value).LowSignalRound).ToList();
        return MathHelpers.LeastCommonMultiple(ImportantRounds.Select(r => r.First()).ToArray()).ToString();
    }

    public interface IModule
    {
        public void ReceiveSignal(bool IsHigh, string source);
    }

    public abstract class Module : IModule
    {
        public string Name { get; init; }
        public List<string> Targets { get; init; }

        protected Module(string name, List<string> targets)
        {
            Name = name;
            Targets = targets;
        }

        public abstract void ReceiveSignal(bool IsHigh, string source);
    }

    public class FlipFlipModule : Module
    {
        public bool IsOn { get; set; } = false;

        public FlipFlipModule(string name, List<string> targets) : base(name, targets)
        {
        }

        public override void ReceiveSignal(bool IsHigh, string source)
        {
            if (!IsHigh)
            {
                IsOn = !IsOn;
                foreach (string target in Targets)
                {
                    Pulses.Enqueue(new Pulse(target, Name, IsOn));
                    PulseCount[IsOn ? 1 : 0]++;
                }
            }
        }
    }

    public class ConjunctionModule : Module
    {
        public HashSet<int> LowSignalRound { get; init; } = new HashSet<int>();

        public Dictionary<string, bool> IsOn { get; init; } = new Dictionary<string, bool>();

        public ConjunctionModule(string name, List<string> targets) : base(name, targets)
        {
        }

        public override void ReceiveSignal(bool IsHigh, string source)
        {
            IsOn[source] = IsHigh;
            if (IsOn.Values.All(e => e))
            {
                foreach (string target in Targets)
                {
                    Pulses.Enqueue(new Pulse(target, Name, false));
                    LowSignalRound.Add(Round);
                    PulseCount[0]++;
                }
            }
            else
            {
                foreach (string target in Targets)
                {
                    Pulses.Enqueue(new Pulse(target, Name, true));
                    PulseCount[1]++;
                }
            }
        }
    }

    public class BroadcastModule : Module
    {
        public BroadcastModule(string name, List<string> targets) : base(name, targets)
        {
        }

        public override void ReceiveSignal(bool IsHigh, string source)
        {
            foreach (string target in Targets)
            {
                Pulses.Enqueue(new Pulse(target, Name, IsHigh));
                PulseCount[IsHigh ? 1 : 0]++;
            }
        }
    }

    public record Pulse(string Target, string Source, bool IsHigh);
}