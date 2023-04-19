using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.TestUtilities;
using System.Threading.Tasks;
using EntityFrameworkCore.SingleStore.FunctionalTests.TestUtilities;
using Xunit;

namespace EntityFrameworkCore.SingleStore.FunctionalTests
{
    public class MusicStoreSingleStoreTest : MusicStoreTestBase<MusicStoreSingleStoreTest.MusicStoreSingleStoreFixture>
    {
        public MusicStoreSingleStoreTest(MusicStoreSingleStoreFixture fixture)
            : base(fixture)
        {
        }

        [ConditionalFact(Skip = "SingleStore doesn't support correlated subselect in ORDER BY")]
        public override async Task Index_GetsSixTopAlbums()
        {
            await base.Index_GetsSixTopAlbums();
        }
        public class MusicStoreSingleStoreFixture : MusicStoreFixtureBase
        {
            protected override ITestStoreFactory TestStoreFactory => SingleStoreTestStoreFactory.Instance;

            protected override void OnModelCreating(ModelBuilder modelBuilder, DbContext context)
            {
                base.OnModelCreating(modelBuilder, context);

                SingleStoreTestHelpers.Instance.EnsureSufficientKeySpace(modelBuilder.Model, TestStore);
            }
        }
    }
}
