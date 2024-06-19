using QuickTie.Data.Models;
using System.Diagnostics.CodeAnalysis;

namespace QuickTie.Portal.Models.EqualityComparer
{
    public class DistinctProductComparer : IEqualityComparer<QuickTieCable> 
    {
        public bool Equals(QuickTieCable x, QuickTieCable y)
        {
            return x.ProductType == y.ProductType && x.CableType == y.CableType;
        }

        public int GetHashCode([DisallowNull] QuickTieCable obj)
        {
            return obj.GetHashCode();
        }
    }
}
