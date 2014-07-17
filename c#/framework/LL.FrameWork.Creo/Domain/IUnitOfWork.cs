namespace LL.Framework.Core.Domain
{
    using System;

    /// <summary>
    /// Contract for ‘UnitOfWork pattern’. For more
    /// related info see http://martinfowler.com/eaaCatalog/unitOfWork.html or
    /// http://msdn.microsoft.com/en-us/magazine/dd882510.aspx
    /// In this solution, the Unit Of Work is implemented using the out-of-box 
    /// Entity Framework Context (EF 4.1 DbContext) persistence engine. But in order to
    /// comply the PI (Persistence Ignorant) principle in our Domain, we implement this interface/contract. 
    /// This interface/contract should be complied by any UoW implementation to be used with this Domain.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        IGenericTransaction BeginTransaction();
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        IGenericTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel);

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown
        ///</remarks>
        void Commit();

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        /// <param name="isolationLevel"></param>
        void Commit(System.Data.IsolationLevel isolationLevel);

        /// <summary>
        /// Rollback tracked changes. See references of UnitOfWork pattern
        /// </summary>
        void Rollback();
    }
}
