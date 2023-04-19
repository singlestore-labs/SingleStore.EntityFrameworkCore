using EntityFrameworkCore.SingleStore.FunctionalTests.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.TestUtilities;

namespace EntityFrameworkCore.SingleStore.FunctionalTests
{
    public class LoadSingleStoreTest : LoadTestBase<LoadSingleStoreTest.LoadSingleStoreFixture>
    {
        public LoadSingleStoreTest(LoadSingleStoreFixture fixture)
            : base(fixture)
        {
        }

        public class LoadSingleStoreFixture : LoadFixtureBase
        {
            protected override ITestStoreFactory TestStoreFactory => SingleStoreTestStoreFactory.Instance;

            public override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
                => base.AddOptions(builder);
        }
    }
}
