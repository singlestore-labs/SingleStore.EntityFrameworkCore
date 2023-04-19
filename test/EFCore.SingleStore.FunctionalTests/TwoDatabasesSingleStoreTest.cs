using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.SingleStore.FunctionalTests.TestUtilities;
using EntityFrameworkCore.SingleStore.Tests;
using Xunit;

namespace EntityFrameworkCore.SingleStore.FunctionalTests
{
    public class TwoDatabasesSingleStoreTest : TwoDatabasesTestBase, IClassFixture<SingleStoreFixture>
    {
        public TwoDatabasesSingleStoreTest(SingleStoreFixture fixture)
            : base(fixture)
        {
        }

        protected new SingleStoreFixture Fixture
            => (SingleStoreFixture)base.Fixture;

        protected override DbContextOptionsBuilder CreateTestOptions(
            DbContextOptionsBuilder optionsBuilder, bool withConnectionString = false)
            => withConnectionString
                ? optionsBuilder.UseSingleStore(DummyConnectionString)
                : optionsBuilder.UseSingleStore();

        protected override TwoDatabasesWithDataContext CreateBackingContext(string databaseName)
            => new TwoDatabasesWithDataContext(Fixture.CreateOptions(SingleStoreTestStore.Create(databaseName)));

        protected override string DummyConnectionString { get; } = "Server=localhost;Database=DoesNotExist;Allow User Variables=True;Use Affected Rows=False";
    }
}
