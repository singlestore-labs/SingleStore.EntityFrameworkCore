﻿// Copyright (c) Pomelo Foundation. All rights reserved.
// Copyright (c) SingleStore Inc. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    ///     Sets the collation of a type (table), property or field (column) for MySQL.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class SingleStoreCollationAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SingleStoreCollationAttribute" /> class.
        ///     Implicitly uses <see cref="Microsoft.EntityFrameworkCore.DelegationModes.ApplyToAll"/>.
        /// </summary>
        /// <param name="collation"> The name of the collation to use. </param>
        public SingleStoreCollationAttribute(string collation)
            : this(collation, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SingleStoreCollationAttribute" /> class.
        /// </summary>
        /// <param name="collation"> The name of the collation to use. </param>
        /// <param name="delegationModes">
        /// Finely controls where to recursively apply the collation and where not.
        /// Ignored when <see cref="SingleStoreCollationAttribute"/> is applied to properties/columns.
        /// </param>
        public SingleStoreCollationAttribute(string collation, DelegationModes delegationModes)
            : this(collation, (DelegationModes?)delegationModes)
        {
        }

        protected SingleStoreCollationAttribute(string collation, DelegationModes? delegationModes)
        {
            CollationName = collation;
            DelegationModes = delegationModes;
        }

        /// <summary>
        ///     The name of the collation to use.
        /// </summary>
        public virtual string CollationName { get; }

        /// <summary>
        /// Finely controls where to recursively apply the collation and where not.
        /// Implicitly uses <see cref="Microsoft.EntityFrameworkCore.DelegationModes.ApplyToAll"/> if set to <see langword="null"/>.
        /// Ignored when <see cref="SingleStoreCollationAttribute"/> is applied to properties/columns.
        /// </summary>
        public virtual DelegationModes? DelegationModes { get; }
    }
}
