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


namespace LL.Framework.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using LL.Framework.Core.Domain;
    using LL.Framework.Core.Domain.Specification;
    using LL.Framework.Core.Domain.Viewpoints;

    /// <summary>
    /// Base interface for implement a "Repository Pattern", for
    /// more information about this pattern see http://martinfowler.com/eaaCatalog/repository.html
    /// or http://blogs.msdn.com/adonet/archive/2009/06/16/using-repository-and-unit-of-work-patterns-with-entity-framework-4-0.aspx
    /// </summary>
    /// <remarks>
    /// Indeed, one might think that IDbSet already a generic repository and therefore
    /// would not need this item. Using this interface allows us to ensure PI principle
    /// within our domain model
    /// </remarks>
    /// <typeparam name="TEntity">Type of entity for this repository </typeparam>
    public interface IRepository<TEntity, TID> : IDisposable
        where TEntity : EntityBase<TID>
    {
        /// <summary>
        /// Get the unit of work in this repository
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add item into repository
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        void Add(TEntity item);

        /// <summary>
        /// Delete item 
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Remove(TEntity item);

        /// <summary>
        /// Set item as modified
        /// </summary>
        /// <param name="item">Item to modify</param>
        void Modify(TEntity item);

        /// <summary>
        ///Track entity into this repository, really in UnitOfWork. 
        ///In EF this can be done with Attach and with Update in NH
        /// </summary>
        /// <param name="item">Item to attach</param>
        void TrackItem(TEntity item);

        /// <summary>
        /// Sets modified entity into the repository. 
        /// When calling Commit() method in UnitOfWork 
        /// these changes will be saved into the storage
        /// </summary>
        /// <param name="persisted">The persisted item</param>
        /// <param name="current">The current item</param>
        void Merge(TEntity persisted, TEntity current);
        
        /// <summary>
        /// 判断是否存在满足条件的数据
        /// </summary>
        /// <param name="specification">条件说明</param>
        /// <returns>判断是否存在满足条件的数据 存在返回 true、否则 false。</returns>
        bool Exists(Specification<TEntity> specification);

        /// <summary>
        /// 获取满足条件数据的数量
        /// </summary>
        /// <param name="specification">条件说明</param>
        /// <returns>返回满足条件数据的数量</returns>
        int GetCount(Specification<TEntity> specification);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id">对象的唯一标识</param>
        /// <returns>返回指定的实体对象</returns>
        TEntity FindById(TID id);

        /// <summary>
        /// 查询满足条件的对象
        /// </summary>
        /// <param name="specification">条件说明</param>
        /// <returns>返回满足条件的对象</returns>
        /// <exception cref="System.ArgumentNullException">specification 为 null </exception>
        TEntity Find(Specification<TEntity> specification);

        /// <summary>
        /// 查询满足条件的对象列表
        /// </summary>
        /// <param name="specification">条件说明</param>
        /// <param name="order">排序对象 默认为null</param>
        /// <param name="skipCount">要跳过的条数</param>
        /// <param name="takeCount">返回数据的数量</param>
        /// <returns>返回满足条件的对象列表</returns>
        IEnumerable<TEntity> FindList(Specification<TEntity> specification, Order<TEntity> order = null, int? skipCount = null, int? takeCount = null);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="order">排序对象 默认为null</param>
        /// <param name="skipCount">要跳过的条数</param>
        /// <param name="takeCount">返回数据的数量</param>
        /// <returns>返回所有对象的列表</returns>
        IEnumerable<TEntity> FindAll(Order<TEntity> order = null, int? skipCount = null, int? takeCount = null);
    }
}
