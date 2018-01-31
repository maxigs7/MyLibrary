namespace Framework.Common.Entities.Helpers
{
    public class SortHelper
    {
        public static string SortExpression(string sortBy, string sortDirection)
        {
            return string.Format("{0} {1}", sortBy.ToUpper(), sortDirection.ToUpper());
        }
    }
}
