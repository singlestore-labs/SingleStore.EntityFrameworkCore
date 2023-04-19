using EntityFrameworkCore.SingleStore.FunctionalTests.TestUtilities;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.SingleStore.FunctionalTests.Query
{
    public class IncludeOneToOneSingleStoreTest : IncludeOneToOneTestBase<IncludeOneToOneSingleStoreTest.OneToOneQuerySingleStoreFixture>
    {
        public IncludeOneToOneSingleStoreTest(OneToOneQuerySingleStoreFixture fixture)
            : base(fixture)
        {
        }

        public class OneToOneQuerySingleStoreFixture : OneToOneQueryFixtureBase
        {
            protected override ITestStoreFactory TestStoreFactory => SingleStoreTestStoreFactory.Instance;

            public TestSqlLoggerFactory TestSqlLoggerFactory =>
                (TestSqlLoggerFactory)ServiceProvider.GetRequiredService<ILoggerFactory>();
        }
    }
}
