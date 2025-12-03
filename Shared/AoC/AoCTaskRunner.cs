using System.Reflection;

namespace Shared.AoC;

public static class AoCTaskRunner
{
    public static string RunLatest(int overrideDay = -1)
    {
        var latestAoCTask = Assembly.GetCallingAssembly()
            .GetTypes()
            .Where(t => typeof(IAoCTask).IsAssignableFrom(t))
            .OrderByDescending(t =>
            {
                var name = t.Name;
                var digits = new string(name.SkipWhile(c => !char.IsDigit(c)).TakeWhile(char.IsDigit).ToArray());
                var day = int.Parse(digits);
                return day == overrideDay ? int.MaxValue : day;
            })
            .FirstOrDefault();

        if (latestAoCTask == null)
        {
            return "No AoC task found.";
        }

        var instance = Activator.CreateInstance(latestAoCTask) as IAoCTask;
        return instance?.Solve() ?? "No instance of AoC task can be created.";
    }
}
