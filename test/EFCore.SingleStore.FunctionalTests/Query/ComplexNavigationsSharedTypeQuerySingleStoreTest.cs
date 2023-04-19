﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestModels.ComplexNavigationsModel;
using Microsoft.EntityFrameworkCore.TestUtilities;
using EntityFrameworkCore.SingleStore.FunctionalTests.TestUtilities;
using EntityFrameworkCore.SingleStore.Infrastructure;
using EntityFrameworkCore.SingleStore.Tests.TestUtilities.Attributes;
using Xunit;
using Xunit.Abstractions;

namespace EntityFrameworkCore.SingleStore.FunctionalTests.Query
{
    public class ComplexNavigationsSharedTypeQuerySingleStoreTest : ComplexNavigationsSharedTypeQueryRelationalTestBase<
        ComplexNavigationsSharedTypeQuerySingleStoreTest.ComplexNavigationsSharedTypeQuerySingleStoreFixture>
    {
        // ReSharper disable once UnusedParameter.Local
        public ComplexNavigationsSharedTypeQuerySingleStoreTest(
            ComplexNavigationsSharedTypeQuerySingleStoreFixture fixture,
            ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
            // Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        [SupportedServerVersionCondition(nameof(ServerVersionSupport.OuterReferenceInMultiLevelSubquery))]
        public override Task Contains_with_subquery_optional_navigation_and_constant_item(bool async)
        {
            return base.Contains_with_subquery_optional_navigation_and_constant_item(async);
        }

        public override Task Distinct_take_without_orderby(bool async)
        {
            return AssertQuery(
                async,
                ss => from l1 in ss.Set<Level1>()
                    where l1.Id < 3
                    select (from l3 in ss.Set<Level3>()
                        select l3).Distinct().OrderBy(e => e.Id).Take(1).FirstOrDefault().Name); // Apply OrderBy before Skip
        }

        public override Task Distinct_skip_without_orderby(bool async)
        {
            return AssertQuery(
                async,
                ss => from l1 in ss.Set<Level1>()
                    where l1.Id < 3
                    select (from l3 in ss.Set<Level3>()
                        select l3).Distinct().OrderBy(e => e.Id).Skip(1).FirstOrDefault().Name); // Apply OrderBy before Skip
        }

        [ConditionalTheory(Skip = "Feature 'Correlated subselect that can not be transformed and does not match on shard keys' is not supported by SingleStore")]
        public override Task Collection_FirstOrDefault_property_accesses_in_projection(bool async)
        {
            return base.Collection_FirstOrDefault_property_accesses_in_projection(async);
        }

        [ConditionalTheory(Skip = "Feature 'Correlated subselect that can not be transformed and does not match on shard keys' is not supported by SingleStore")]
        public override Task Contains_over_optional_navigation_with_null_column(bool async)
        {
            return base.Contains_over_optional_navigation_with_null_column(async);
        }

        [ConditionalTheory(Skip = "Feature 'Correlated subselect that can not be transformed and does not match on shard keys' is not supported by SingleStore")]
        public override Task Contains_over_optional_navigation_with_null_entity_reference(bool async)
        {
            return base.Contains_over_optional_navigation_with_null_entity_reference(async);
        }

        [ConditionalTheory(Skip = "SingleStore does not support this type of query: scalar subselect references field belonging to outer select that is more than one level up")]
        public override Task Member_pushdown_with_multiple_collections(bool async)
        {
            return base.Member_pushdown_with_multiple_collections(async);
        }

        [ConditionalTheory(Skip = "SingleStore does not support this type of query: scalar subselect references field belonging to outer select that is more than one level up")]
        public override Task Multiple_collection_FirstOrDefault_followed_by_member_access_in_projection(bool async)
        {
            return base.Multiple_collection_FirstOrDefault_followed_by_member_access_in_projection(async);
        }

        [ConditionalTheory(Skip = "SingleStore does not support this type of query: correlated subselect in ORDER BY")]
        public override Task OrderBy_collection_count_ThenBy_reference_navigation(bool async)
        {
            return base.OrderBy_collection_count_ThenBy_reference_navigation(async);
        }

        [ConditionalTheory(Skip = "Feature 'Correlated subselect that can not be transformed and does not match on shard keys' is not supported by SingleStore")]
        public override Task Project_collection_navigation_count(bool async)
        {
            return base.Project_collection_navigation_count(async);
        }

        [ConditionalTheory(Skip = "SingleStore has no implicit ordering of results by primary key")]
        public override Task Subquery_with_Distinct_Skip_FirstOrDefault_without_OrderBy(bool async)
        {
            return base.Subquery_with_Distinct_Skip_FirstOrDefault_without_OrderBy(async);
        }

        [ConditionalTheory(Skip = "Feature 'Correlated subselect that can not be transformed and does not match on shard keys' is not supported by SingleStore")]
        public override Task Where_navigation_property_to_collection(bool async)
        {
            return base.Where_navigation_property_to_collection(async);
        }

        [ConditionalTheory(Skip = "Feature 'Correlated subselect that can not be transformed and does not match on shard keys' is not supported by SingleStore")]
        public override Task Where_navigation_property_to_collection2(bool async)
        {
            return base.Where_navigation_property_to_collection2(async);
        }

        [ConditionalTheory(Skip = "Feature 'Correlated subselect that can not be transformed and does not match on shard keys' is not supported by SingleStore")]
        public override Task Where_navigation_property_to_collection_of_original_entity_type(bool async)
        {
            return base.Where_navigation_property_to_collection_of_original_entity_type(async);
        }

        [SupportedServerVersionCondition("8.1.0-singlestore", Skip = "The issue failing this test is fixed in 8.1")]
        public override Task SelectMany_subquery_with_custom_projection(bool async)
        {
            return base.SelectMany_subquery_with_custom_projection(async);
        }

        [SupportedServerVersionCondition("8.1.0-singlestore", Skip = "The issue failing this test is fixed in 8.1")]
        public override Task Sum_with_filter_with_include_selector_cast_using_as(bool async)
        {
            return base.Sum_with_filter_with_include_selector_cast_using_as(async);
        }

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);

        public class ComplexNavigationsSharedTypeQuerySingleStoreFixture : ComplexNavigationsSharedTypeQueryRelationalFixtureBase
        {
            protected override ITestStoreFactory TestStoreFactory
                => SingleStoreTestStoreFactory.Instance;
        }
    }
}
