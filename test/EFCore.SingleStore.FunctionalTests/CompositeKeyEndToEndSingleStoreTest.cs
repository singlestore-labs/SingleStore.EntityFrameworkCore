using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.TestUtilities;
using EntityFrameworkCore.SingleStore.FunctionalTests.TestUtilities;

namespace EntityFrameworkCore.SingleStore.FunctionalTests
{
    public class CompositeKeyEndToEndSingleStoreTest : CompositeKeyEndToEndTestBase<CompositeKeyEndToEndSingleStoreTest.CompositeKeyEndToEndSingleStoreFixture>
    {
        public CompositeKeyEndToEndSingleStoreTest(CompositeKeyEndToEndSingleStoreFixture fixture)
            : base(fixture)
        {
        }

        public class CompositeKeyEndToEndSingleStoreFixture : CompositeKeyEndToEndFixtureBase
        {
            protected override ITestStoreFactory TestStoreFactory => SingleStoreTestStoreFactory.Instance;
        }
    }
}
