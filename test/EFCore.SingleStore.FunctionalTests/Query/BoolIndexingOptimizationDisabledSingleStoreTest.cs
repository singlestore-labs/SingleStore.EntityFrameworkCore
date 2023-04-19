﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestModels.GearsOfWarModel;
using System.Data.Common;
using EntityFrameworkCore.SingleStore.Tests;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable NegativeEqualityExpression
// ReSharper disable RedundantBoolCompare

namespace EntityFrameworkCore.SingleStore.FunctionalTests.Query
{
    public class BoolIndexingOptimizationDisabledSingleStoreTest : QueryTestBase<BoolIndexingOptimizationDisabledSingleStoreTest.BoolIndexingOptimizationDisabledSingleStoreFixture>
    {
        public BoolIndexingOptimizationDisabledSingleStoreTest(BoolIndexingOptimizationDisabledSingleStoreFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
            //Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization(bool isAsync)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                isAsync,
                ss => from w in ss.Set<Weapon>()
                    where w.IsAutomatic
                    select w.Name);

            // Will not use IX_Weapons_IsAutomatic with bool indexing optimization disabled.
            AssertSql(@"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic`");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_not(bool isAsync)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                isAsync,
                ss => from w in ss.Set<Weapon>()
                    where !w.IsAutomatic
                    select w.Name);

            // Might not use IX_Weapons_IsAutomatic with bool indexing optimization disabled.
            AssertSql(
                @"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE NOT (`w`.`IsAutomatic`)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_equals_true(bool isAsync)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                isAsync,
                ss => from w in ss.Set<Weapon>()
                    where w.IsAutomatic == true
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(
                AssertSql(
                    @"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic`"),
                AppConfig.ServerVersion.Supports.ImplicitBoolCheckUsesIndex
                    ? keys
                    : null);
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_equals_false(bool isAsync)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                isAsync,
                ss => from w in ss.Set<Weapon>()
                    where w.IsAutomatic == false
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(
                AssertSql(
                    @"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE NOT (`w`.`IsAutomatic`)"),
                AppConfig.ServerVersion.Supports.ImplicitBoolCheckUsesIndex
                    ? keys
                    : null);
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_not_equals_true(bool isAsync)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                isAsync,
                ss => from w in ss.Set<Weapon>()
                    where w.IsAutomatic != true
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(
                AssertSql(
                    @"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE NOT (`w`.`IsAutomatic`)"),
                AppConfig.ServerVersion.Supports.ImplicitBoolCheckUsesIndex
                    ? keys
                    : null);
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_not_equals_false(bool isAsync)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                isAsync,
                ss => from w in ss.Set<Weapon>()
                    where w.IsAutomatic != false
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(
                AssertSql(
                    @"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic`"),
                AppConfig.ServerVersion.Supports.ImplicitBoolCheckUsesIndex
                    ? keys
                    : null);
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_not_parenthesis_equals_true(bool isAsync)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                isAsync,
                ss => from w in ss.Set<Weapon>()
                    where !(w.IsAutomatic == true)
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(
                AssertSql(
                    @"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE NOT (`w`.`IsAutomatic`)"),
                AppConfig.ServerVersion.Supports.ImplicitBoolCheckUsesIndex
                    ? keys
                    : null);
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_not_parenthesis_equals_false(bool isAsync)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                isAsync,
                ss => from w in ss.Set<Weapon>()
                    where !(w.IsAutomatic == false)
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(
                AssertSql(
                    @"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic`"),
                AppConfig.ServerVersion.Supports.ImplicitBoolCheckUsesIndex
                    ? keys
                    : null);
        }

        private string AssertSql(string expected)
        {
            Fixture.TestSqlLoggerFactory.AssertBaseline(new [] {expected});
            return expected;
        }

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);

        private void AssertKeyUsage(string sql, params string[] keys)
        {
            if (keys == null ||
                keys.Length <= 0)
            {
                return;
            }

            var keysUsed = new HashSet<string>();

            using var context = CreateContext();
            var connection = context.Database.GetDbConnection();

            using var command = connection.CreateCommand();
            command.CommandText = "EXPLAIN " + Regex.Replace(
                sql,
                @"\r?\nFROM (?:`.*?`\.)?`.*?`(?: AS `.*?`)?(?=$|\r?\n)",
                $@"$0{Environment.NewLine}FORCE INDEX ({string.Join(", ", keys.Select(s => $"`{s}`"))})",
                RegexOptions.IgnoreCase);

            using var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                var key = dataReader.GetValueOrDefault<string>("key");

                if (!string.IsNullOrEmpty(key))
                {
                    keysUsed.Add(key);
                }
            }

            Assert.Empty(keys.Except(keysUsed, StringComparer.OrdinalIgnoreCase));
        }

        protected GearsOfWarContext CreateContext() => Fixture.CreateContext();

        public class BoolIndexingOptimizationDisabledSingleStoreFixture : GearsOfWarQuerySingleStoreFixture
        {
            public override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
            {
                new SingleStoreDbContextOptionsBuilder(base.AddOptions(builder))
                    .EnableIndexOptimizedBooleanColumns(false);

                return builder;
            }
        }
    }
}
