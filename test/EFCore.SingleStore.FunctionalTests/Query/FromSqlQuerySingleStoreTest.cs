using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestUtilities;
using SingleStoreConnector;
using Xunit;

namespace EntityFrameworkCore.SingleStore.FunctionalTests.Query
{
    public class FromSqlQuerySingleStoreTest : FromSqlQueryTestBase<NorthwindQuerySingleStoreFixture<NoopModelCustomizer>>
    {
        public FromSqlQuerySingleStoreTest(NorthwindQuerySingleStoreFixture<NoopModelCustomizer> fixture)
            : base(fixture)
        {
        }

        protected override DbParameter CreateDbParameter(string name, object value)
            => new SingleStoreParameter
            {
                ParameterName = name,
                Value = value
            };
    }
}
