using System;
using System.Data;

using NHibernate;
using NHibernate.Engine;
using NHibernate.Context;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Connection;
using NHibernate.Driver;
using NHibernate.AdoNet;
using NHibernate.Exceptions;
using NHibernate.Cache;
using NHibernate.Bytecode;
using NHibernate.Type;
using NHibernate.Hql.Ast.ANTLR;

namespace LL.FrameWork.Core.UOW
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private ISession _currentSession;
        private IStatelessSession _currentStatelessSession;
        private ISessionFactory _sessionFactory;
        Configuration _configuration = null;
        /// <summary>
        /// 数据库字符串连接
        /// </summary>
        public static string connectionString = "Data Source=myTest.db";

        internal UnitOfWorkFactory()
        { }

        /// <summary>
        /// 使用 NHibernate 自带的上下文来创建 Session
        /// </summary>
        /// <returns></returns>
        private ISession CreateSession()
        {
            /*
             * 需要Cfg.Configuration 配置Environment.CurrentSessionContextClass 如果没有配置 则返回null
                case "call": return new CallSessionContext(this);
				case "thread_static": return new ThreadStaticSessionContext(this);
				case "web": return new WebSessionContext(this);
				case "wcf_operation": return new WcfOperationSessionContext(this);
            */
            //var SessionContext = ((ISessionFactoryImplementor)_sessionFactory).CurrentSessionContext;
            //if (!CurrentSessionContext.HasBind(_sessionFactory))
            //{
            //    CurrentSessionContext.Bind(_sessionFactory.OpenSession());
            //}
            //return SessionContext.CurrentSession();

            return SessionFactory.OpenSession();
        }

        /// <summary>
        /// 创建没有状态的会话，用于批量操作
        /// </summary>
        /// <returns></returns>
        private IStatelessSession CreateStatelessSession()
        {
            return SessionFactory.OpenStatelessSession();
        }

        /// <summary>
        /// 配置
        /// </summary>
        public Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new Configuration();
                    _configuration.DataBaseIntegration(db =>
                    {
                        db.Dialect<SQLiteDialect>();
                        db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                        db.LogSqlInConsole = true;
                        db.LogFormattedSql = true;
                        db.ConnectionProvider<DriverConnectionProvider>();
                        db.Driver<SQLite20Driver>();
                        db.IsolationLevel = IsolationLevel.ReadCommitted;
                        db.ConnectionReleaseMode = ConnectionReleaseMode.AfterTransaction;
                        db.ConnectionString = connectionString;
                        db.Batcher<NonBatchingBatcherFactory>();
                        db.BatchSize = 15;
                        db.PrepareCommands = true;
                        db.Timeout = 10;
                        db.ExceptionConverter<SQLStateConverter>();
                        db.AutoCommentSql = true;
                        db.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
                        db.MaximumDepthOfOuterJoinFetching = 11;
                        db.SchemaAction = SchemaAutoAction.Update;
                    });
                    _configuration.Cache(c =>
                    {
                        c.RegionsPrefix = "xyz";
                        c.UseMinimalPuts = true;
                        c.UseQueryCache = true;
                        c.DefaultExpiration = 15;
                        c.Provider<HashtableCacheProvider>();
                        //c.QueryCache<StandardQueryCache>();
                        //config.SetProperty(NHibernate.Cfg.Environment.QueryCacheFactory, typeof(StandardQueryCacheFactory).AssemblyQualifiedName);
                    });
                    _configuration.Proxy(p =>
                    {
                        p.Validation = false;
                        p.ProxyFactoryFactory<DefaultProxyFactoryFactory>();
                    });
                    _configuration.Mappings(m =>
                    {
                        m.DefaultCatalog = "";
                        m.DefaultSchema = "";
                    });
                    _configuration.CollectionTypeFactory<DefaultCollectionTypeFactory>();

                    _configuration.HqlQueryTranslator<ASTQueryTranslatorFactory>();

                    _configuration.SetProperty(NHibernate.Cfg.Environment.GenerateStatistics, "true");

                    //_configuration.SessionFactory().Named("SomeName")
                    //    .Caching
                    //        .Through<HashtableCacheProvider>()
                    //        .PrefixingRegionsWith("xyz")
                    //        .Queries
                    //            .Through<StandardQueryCache>()
                    //        .UsingMinimalPuts()
                    //        .WithDefaultExpiration(15)
                    //    .GeneratingCollections
                    //        .Through<DefaultCollectionTypeFactory>()
                    //    .Proxy
                    //        .DisableValidation()
                    //        .Through<DefaultProxyFactoryFactory>()
                    //    .ParsingHqlThrough<ASTQueryTranslatorFactory>()
                    //    .Mapping
                    //        .UsingDefaultCatalog("MyCatalog")
                    //        .UsingDefaultSchema("MySche")
                    //    .Integrate
                    //        .Using<SQLiteDialect>()//sqlite
                    //        .AutoQuoteKeywords()
                    //        .EnableLogFormattedSql()
                    //        .BatchingQueries
                    //            .Through<NonBatchingBatcherFactory>()
                    //            .Each(15)
                    //        .Connected
                    //            .Through<DriverConnectionProvider>()
                    //            .By<SQLite20Driver>()//SqlClientDriver
                    //            .Releasing(ConnectionReleaseMode.AfterTransaction)
                    //            .With(IsolationLevel.ReadCommitted)
                    //            .Using(connectionString)
                    //        .CreateCommands
                    //            .AutoCommentingSql()
                    //            .ConvertingExceptionsThrough<SQLStateConverter>()
                    //            .Preparing()
                    //            .WithTimeout(10)
                    //            .WithMaximumDepthOfOuterJoinFetching(11)
                    //            .WithHqlToSqlSubstitutions("true 1, false 0, yes 'Y', no 'N'")
                    //        .Schema
                    //            .Validating();

                    //var export = new SchemaExport(config);
                    //export.Create(str => Console.WriteLine(str), false);
                    //export.Execute(true, true, false);
                }
                return _configuration;
            }
            set { _configuration = null; }
        }

        public ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    _sessionFactory = Configuration.BuildSessionFactory();
                return _sessionFactory;
            }
        }

        public ISession CurrentSession
        {
            get
            {
                if (_currentSession == null)
                    throw new InvalidOperationException("You are not in a unit of work.");
                return _currentSession;
            }
            set
            {
                _currentSession = value;
            }
        }

        public IStatelessSession CurrentStatelessSession
        {
            get
            {
                if (_currentStatelessSession == null)
                    throw new InvalidOperationException("You are not in a unit of work.");
                return _currentStatelessSession;
            }
            set
            {
                _currentStatelessSession = value;
            }
        }

        public IUnitOfWork Create(bool hasState)
        {
            if (hasState)
            {
                ISession session = CreateSession();
                session.FlushMode = FlushMode.Commit;
                _currentSession = session;
                return new UnitOfWorkImplementor(this, session);
            }
            else
            {
                IStatelessSession session = CreateStatelessSession();
                _currentStatelessSession = session;
                return new StatelessUnitOfWorkImplementor(this, session);
            }
        }

        public IUnitOfWork Create()
        {
            return Create(true);
        }

        public void DisposeUnitOfWork(IUnitOfWorkImplementor adapter)
        {
            CurrentSession = null;
            UnitOfWork.DisposeUnitOfWork(adapter);
        }

        #region IUnitOfWorkFactory 接口实现

        Configuration IUnitOfWorkFactory.Configuration
        {
            get { return this.Configuration; }
            set { this.Configuration = value; }
        }

        ISessionFactory IUnitOfWorkFactory.SessionFactory
        {
            get
            {
                return this.SessionFactory;
            }
        }

        ISession IUnitOfWorkFactory.CurrentSession
        {
            get
            {
                return this.CurrentSession;
            }
            set
            {
                this.CurrentSession = value;
            }
        }

        IStatelessSession IUnitOfWorkFactory.CurrentStatelessSession
        {
            get
            {
                return this.CurrentStatelessSession;
            }
            set
            {
                this.CurrentStatelessSession = value;
            }
        }

        IUnitOfWork IUnitOfWorkFactory.Create()
        {
            return this.Create();
        }

        void IUnitOfWorkFactory.DisposeUnitOfWork(IUnitOfWorkImplementor adapter)
        {
            this.DisposeUnitOfWork(adapter);
        }
        #endregion
    }
}
