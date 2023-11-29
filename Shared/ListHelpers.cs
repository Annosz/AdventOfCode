namespace Shared
{
    public static class ListHelpers
    {
        public static T Shift<T>(ref List<T> list)
        {
            var e = list.First();

            list = list.Skip(1).ToList();

            return e;
        }

        public static T ShiftToEnd<T>(ref List<T> list)
        {
            var e = list.First();

            list = list.Skip(1).Append(e).ToList();

            return e;
        }
    }
}
