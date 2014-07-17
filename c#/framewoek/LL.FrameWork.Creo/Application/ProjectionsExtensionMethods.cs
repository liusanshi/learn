//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================


namespace LL.Framework.Core.Application
{
    using System.Collections.Generic;
    using LL.Framework.Core.Domain;
    using LL.Framework.Core.Infrastructure.Adapter;

    public static class ProjectionsExtensionMethods
    {
        /// <summary>
        /// Project a type using a DTO
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <typeparam name="TID"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static TProjection ProjectedAs<TProjection, TID>(this EntityBase<TID> item)
            where TProjection : class, new()
        {
            var adapter = TypeAdapterFactory.CreateAdapter();
            return adapter.Adapt<TProjection>(item);
        }

        /// <summary>
        /// projected a enumerable collection of items
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <typeparam name="TID"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<TProjection> ProjectedAsCollection<TProjection, TID>(this IEnumerable<EntityBase<TID>> items)
            where TProjection : class, new()
        {
            var adapter = TypeAdapterFactory.CreateAdapter();
            return adapter.Adapt<List<TProjection>>(items);
        }
    }
}
