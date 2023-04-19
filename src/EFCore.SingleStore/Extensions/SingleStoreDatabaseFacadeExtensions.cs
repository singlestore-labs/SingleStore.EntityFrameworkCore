// Copyright (c) Pomelo Foundation. All rights reserved.
// Copyright (c) SingleStore Inc. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System;
using System.Reflection;
using EntityFrameworkCore.SingleStore.Infrastructure.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

//ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    ///     MySQL specific extension methods for <see cref="DbContext.Database" />.
    /// </summary>
    public static class SingleStoreDatabaseFacadeExtensions
    {
        /// <summary>
        ///     <para>
        ///         Returns true if the database provider currently in use is the MySQL provider.
        ///     </para>
        ///     <para>
        ///         This method can only be used after the <see cref="DbContext" /> has been configured because
        ///         it is only then that the provider is known. This means that this method cannot be used
        ///         in <see cref="DbContext.OnConfiguring" /> because this is where application code sets the
        ///         provider to use as part of configuring the context.
        ///     </para>
        /// </summary>
        /// <param name="database"> The facade from <see cref="DbContext.Database" />. </param>
        /// <returns> True if MySQL is being used; false otherwise. </returns>
        public static bool IsSingleStore([NotNull] this DatabaseFacade database)
            => database.ProviderName.Equals(
                typeof(SingleStoreOptionsExtension).GetTypeInfo().Assembly.GetName().Name,
                StringComparison.Ordinal);
    }
}
