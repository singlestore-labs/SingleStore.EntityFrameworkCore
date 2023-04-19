﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.TestModels.GearsOfWarModel;
using Xunit;

// ReSharper disable RedundantBoolCompare
// ReSharper disable NegativeEqualityExpression

namespace EntityFrameworkCore.SingleStore.FunctionalTests.Query
{
    public partial class GearsOfWarQuerySingleStoreTest
    {
        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);

        private string AssertSql(string expected)
        {
            Fixture.TestSqlLoggerFactory.AssertBaseline(new[] {expected});
            return expected;
        }

        private void AssertKeyUsage(string sql, params string[] keys)
        {
            if (keys.Length <= 0)
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

        [ConditionalTheory(Skip = "SingleStore optimizes queries automatically.")]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization(bool async)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                async,
                ss => from w in ss.Set<Weapon>()
                    where w.IsAutomatic
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(AssertSql(@"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic` = TRUE"), keys);
        }

        [ConditionalTheory(Skip = "SingleStore optimizes queries automatically.")]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_not(bool async)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                async,
                ss => from w in ss.Set<Weapon>()
                    where !w.IsAutomatic
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(AssertSql(@"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic` = FALSE"), keys);
        }

        [ConditionalTheory(Skip = "SingleStore optimizes queries automatically.")]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_equals_true(bool async)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                async,
                ss => from w in ss.Set<Weapon>()
                    where w.IsAutomatic == true
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(AssertSql(@"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic` = TRUE"), keys);
        }

        [ConditionalTheory(Skip = "SingleStore optimizes queries automatically.")]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_equals_false(bool async)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                async,
                ss => from w in ss.Set<Weapon>()
                    where w.IsAutomatic == false
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(AssertSql(@"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic` = FALSE"), keys);
        }

        [ConditionalTheory(Skip = "SingleStore optimizes queries automatically.")]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_not_equals_true(bool async)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                async,
                ss => from w in ss.Set<Weapon>()
                    where w.IsAutomatic != true
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(AssertSql(@"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic` = FALSE"), keys); // Breaking change in 5.0 due to bool expression optimization in `SqlNullabilityProcessor`.
                                          // Was "`w`.`IsAutomatic` <> TRUE" before.
        }

        [ConditionalTheory(Skip = "SingleStore optimizes queries automatically.")]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_not_equals_false(bool async)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                async,
                ss => from w in ss.Set<Weapon>()
                    where w.IsAutomatic != false
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(AssertSql(@"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic` = TRUE"), keys); // Breaking change in 5.0 due to bool expression optimization in `SqlNullabilityProcessor`.
                                         // Was "`w`.`IsAutomatic` <> FALSE" before.
        }

        [ConditionalTheory(Skip = "SingleStore optimizes queries automatically.")]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_not_parenthesis_equals_true(bool async)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                async,
                ss => from w in ss.Set<Weapon>()
                    where !(w.IsAutomatic == true)
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(AssertSql(@"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic` = FALSE"), keys); // Breaking change in 5.0 due to bool expression optimization in `SqlNullabilityProcessor`.
                                          // Was "`w`.`IsAutomatic` <> TRUE" before.
        }

        [ConditionalTheory(Skip = "SingleStore optimizes queries automatically.")]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_bool_optimization_not_parenthesis_equals_false(bool async)
        {
            // Relates to SingleStoreBoolOptimizingExpressionVisitor.
            await AssertQuery(
                async,
                ss => from w in ss.Set<Weapon>()
                    where !(w.IsAutomatic == false)
                    select w.Name);

            string[] keys = {"IX_Weapons_IsAutomatic"};
            AssertKeyUsage(AssertSql(@"SELECT `w`.`Name`
FROM `Weapons` AS `w`
WHERE `w`.`IsAutomatic` = TRUE"), keys); // Breaking change in 5.0 due to bool expression optimization in `SqlNullabilityProcessor`.
                                         // Was "`w`.`IsAutomatic` <> FALSE" before.
        }
    }
}
