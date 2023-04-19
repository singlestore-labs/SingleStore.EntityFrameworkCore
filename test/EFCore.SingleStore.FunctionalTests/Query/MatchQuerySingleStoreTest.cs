﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestUtilities;
using EntityFrameworkCore.SingleStore.FunctionalTests.TestUtilities;
using EntityFrameworkCore.SingleStore.Tests;
using Xunit;

namespace EntityFrameworkCore.SingleStore.FunctionalTests.Query
{
    public class MatchQuerySingleStoreTest : MatchQuerySingleStoreTestBase<MatchQuerySingleStoreTest.MatchQuerySingleStoreFixture>
    {
        public MatchQuerySingleStoreTest(MatchQuerySingleStoreFixture fixture)
            : base(fixture)
        {
        }

        [ConditionalFact]
        public virtual void Match()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(herb.Name, "First"));

            Assert.Equal(3, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`) AGAINST ('First')");
        }

        [ConditionalFact]
        public virtual void Match_multiple_columns()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(new []{herb.Name, herb.Garden}, "First"));

            Assert.Equal(5, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`, `h`.`Garden`) AGAINST ('First')");
        }

        [ConditionalFact]
        public virtual void Match_keywords_separated()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(herb.Name, "First, Second"));

            Assert.Equal(6, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`) AGAINST ('First, Second')");
        }

        [ConditionalFact]
        public virtual void Match_keywords_separated_multiple_columns()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(new []{herb.Name, herb.Garden}, "First, Second"));

            Assert.Equal(8, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`, `h`.`Garden`) AGAINST ('First, Second')");
        }

        [ConditionalFact]
        public virtual void Match_multiple_keywords()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(herb.Name, "First Herb"));

            Assert.Equal(9, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`) AGAINST ('First Herb')");
        }

        [ConditionalFact]
        public virtual void Match_multiple_keywords_multiple_columns()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(new []{herb.Name, herb.Garden}, "First Herb"));

            Assert.Equal(9, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`, `h`.`Garden`) AGAINST ('First Herb')");
        }

        [ConditionalFact]
        public virtual void Match_multiple_keywords_separated()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(herb.Name, "First, Second"));

            Assert.Equal(6, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`) AGAINST ('First, Second')");
        }

        [ConditionalFact]
        public virtual void Match_multiple_keywords_separated_multiple_columns()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(new []{herb.Name, herb.Garden}, "First, Second"));

            Assert.Equal(8, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`, `h`.`Garden`) AGAINST ('First, Second')");
        }

        [ConditionalFact]
        public virtual void Match_with_wildcard()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(herb.Name, "First*"));

            Assert.Equal(3, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`) AGAINST ('First*')");
        }

        [ConditionalFact]
        public virtual void Match_in_boolean_mode_keywords()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(herb.Name, "First* Herb*"));

            Assert.Equal(9, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`) AGAINST ('First* Herb*')");
        }

        [ConditionalFact]
        public virtual void Match_keywords_multiple_columns()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(new []{herb.Name, herb.Garden}, "First* Herb*"));

            Assert.Equal(9, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`, `h`.`Garden`) AGAINST ('First* Herb*')");
        }

        [ConditionalFact]
        public virtual void Match_keyword_excluded()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(herb.Name, "Herb* -Second"));

            Assert.Equal(6, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`) AGAINST ('Herb* -Second')");
        }

        [ConditionalFact]
        public virtual void Match_keyword_excluded_multiple_columns()
        {
            using var context = CreateContext();
            var count = context.Set<Herb>().Count(herb => EF.Functions.Match(new []{herb.Name, herb.Garden}, "Herb* -Second"));

            Assert.Equal(4, count);

            AssertSql(@"SELECT COUNT(*)
FROM `Herb` AS `h`
WHERE MATCH (`h`.`Name`, `h`.`Garden`) AGAINST ('Herb* -Second')");
        }

        private void AssertSql(params string[] expected) => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);

        public class MatchQuerySingleStoreFixture : MatchQuerySingleStoreFixtureBase
        {
            protected override ITestStoreFactory TestStoreFactory => SingleStoreTestStoreFactory.Instance;
        }
    }

    public abstract class MatchQuerySingleStoreTestBase<TFixture> : QueryTestBase<TFixture>
        where TFixture : MatchQuerySingleStoreTestBase<TFixture>.MatchQuerySingleStoreFixtureBase, new()
    {
        protected MatchQuerySingleStoreTestBase(TFixture fixture)
            : base(fixture)
        {
            fixture.ListLoggerFactory.Clear();
        }

        protected virtual DbContext CreateContext() => Fixture.CreateContext();

        public abstract class MatchQuerySingleStoreFixtureBase : SharedStoreFixtureBase<PoolableDbContext>, IQueryFixtureBase
        {
            protected override string StoreName { get; } = "MatchQueryTest";
            public TestSqlLoggerFactory TestSqlLoggerFactory => (TestSqlLoggerFactory)ListLoggerFactory;

            protected override void OnModelCreating(ModelBuilder modelBuilder, DbContext context)
            {
                modelBuilder.Entity<Herb>(
                    herb =>
                    {
                        herb.HasData(MatchQueryData.CreateHerbs());
                        herb.HasKey(h => h.Id);
                        herb.HasIndex(h => new {h.Name, h.Garden}).IsFullText();

                        // We force a case-insensitive collation here, because there exists a bug, where MySQL and MariaDB will handle
                        // FULLTEXT searches for `..._bin` collations incorrectly.
                        herb.Property(h => h.Name).UseCollation(AppConfig.ServerVersion.DefaultUtf8CiCollation);
                        herb.Property(h => h.Garden).UseCollation(AppConfig.ServerVersion.DefaultUtf8CiCollation);
                    });
            }

            public override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
            {
                return base.AddOptions(builder).ConfigureWarnings(wcb => wcb.Throw());
            }

            public override PoolableDbContext CreateContext()
            {
                var context = base.CreateContext();
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                return context;
            }

            public Func<DbContext> GetContextCreator()
                => CreateContext;

            public ISetSource GetExpectedData()
                => new MatchQueryData();

            public IReadOnlyDictionary<Type, object> GetEntitySorters()
                => new Dictionary<Type, Func<object, object>>
                {
                    { typeof(Herb), e => ((Herb)e)?.Id },
                }.ToDictionary(e => e.Key, e => (object)e.Value);

            public IReadOnlyDictionary<Type, object> GetEntityAsserters()
                => new Dictionary<Type, Action<object, object>>
                {
                    {
                        typeof(Herb), (e, a) =>
                        {
                            Assert.Equal(e == null, a == null);

                            if (a != null)
                            {
                                var ee = (Herb)e;
                                var aa = (Herb)a;

                                Assert.Equal(ee.Id, aa.Id);
                                Assert.Equal(ee.Name, aa.Name);
                            }
                        }
                    },
                }.ToDictionary(e => e.Key, e => (object)e.Value);
        }

        public class MatchQueryData : ISetSource
        {
            private readonly IReadOnlyList<Herb> _herbs;

            public MatchQueryData()
            {
                _herbs = CreateHerbs();
            }

            public IQueryable<TEntity> Set<TEntity>() where TEntity : class
            {
                if (typeof(TEntity) == typeof(Herb))
                {
                    return (IQueryable<TEntity>)_herbs.AsQueryable();
                }

                throw new InvalidOperationException("Invalid entity type: " + typeof(TEntity));
            }

            public static IReadOnlyList<Herb> CreateHerbs()
            {
                return new List<Herb>
                {
                    new Herb {Id = 1, Name = "First Herb Name 1", Garden = "Garden spot"},
                    new Herb {Id = 2, Name = "First Herb Name 2", Garden = "First Farmer's Market"},
                    new Herb {Id = 3, Name = "First Herb Name 3", Garden = "Family's Second Farm"},
                    new Herb {Id = 4, Name = "Second Herb Name 1", Garden = "Garden spot"},
                    new Herb {Id = 5, Name = "Second Herb Name 2", Garden = "First Farmer's Market"},
                    new Herb {Id = 6, Name = "Second Herb Name 3", Garden = "Family's Second Farm"},
                    new Herb {Id = 7, Name = "Third Herb Name 1", Garden = "Garden spot"},
                    new Herb {Id = 8, Name = "Third Herb Name 2", Garden = "First Farmer's Market"},
                    new Herb {Id = 9, Name = "Third Herb Name 3", Garden = "Family's Second Farm"}
                };
            }
        }

        public class Herb
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Garden { get; set; }
        }
    }
}
