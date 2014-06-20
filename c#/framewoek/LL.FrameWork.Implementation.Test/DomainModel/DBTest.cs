using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
using NHibernate.Mapping.ByCode;

using LL.FrameWork.Implementation.UOW;
using LL.FrameWork.Core.Domain;
using LL.FrameWork.Implementation.Test.DomainModel;

namespace LL.Core.Test.DomainModel
{
    [TestClass]
    public class DBTest
    {
        [TestInitialize]
        public void DBInit()
        {
            var _configuration = new Configuration();
            UnitOfWork.Configuration = _configuration;

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
                db.ConnectionString = "Data Source=myTest.db";
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

            Mapping();
            initData();
        }

        /// <summary>
        /// 类型配置
        /// </summary>
        static void Mapping()
        {
            ModelMapper mapper = new ModelMapper();
            mapper.Class<Class>(m =>
            {
                m.Cache(ch => ch.Usage(CacheUsage.ReadWrite));
                m.Id<UInt32>(c => c.Id, im =>
                {
                    im.Generator(Generators.Native);
                    im.Column("Id");
                });
                m.Property<string>(c => c.Name, im => im.Column("Name"));
                m.Property<string>(c => c.Slogan);
                m.Table("t_Class");

                m.SchemaAction(SchemaAction.Export | SchemaAction.Update);

                m.Bag<Student>(c => c.Students, bm =>
                {
                    bm.BatchSize(45);
                    bm.Lazy(CollectionLazy.Lazy);
                    //bm.Fetch(CollectionFetchMode.Select);
                    bm.Inverse(true);
                    bm.Key(km =>
                    {
                        km.Column("ClassId");
                        //km.PropertyRef<UInt32>(pg => pg.Id);
                    });
                    //bm.Cascade(Cascade.Persist | Cascade.Remove | Cascade.DeleteOrphans);
                    bm.Cascade(NHibernate.Mapping.ByCode.Cascade.Persist | NHibernate.Mapping.ByCode.Cascade.Remove);
                }, er => er.OneToMany());
            });

            mapper.Class<Student>(m =>
            {
                m.Cache(ch => ch.Usage(CacheUsage.ReadWrite));
                m.Table("t_Student");
                m.Id<UInt32>(c => c.Id, im => { im.Generator(Generators.Native); im.Column("Id"); });
                m.Property<string>(c => c.Name);
                m.Property<Int32>(c => c.Age);
                //m.Property<Int32>(c => c.ClassId);
                m.SchemaAction(SchemaAction.Export | SchemaAction.Update);
                m.ManyToOne<Class>(p => p.Class, m2m =>
                {
                    m2m.Column("ClassId");
                    m2m.Lazy(LazyRelation.Proxy);
                    m2m.Class(typeof(Class));
                    //m2m.Cascade(Cascade.None);
                    //m2m.PropertyRef("Id");
                });
            });

            var hbmmap = mapper.CompileMappingForAllExplicitlyAddedEntities();

            Console.WriteLine(hbmmap.AsString());

            UnitOfWork.Configuration.AddMapping(hbmmap);

            //使用二次缓存
            //config.EntityCache<Class>(ch =>
            //{
            //    ch.Strategy = EntityCacheUsage.ReadWrite;
            //    ch.RegionName = "asd";
            //    ch.Collection(c => c.Students, chc => { chc.Strategy = EntityCacheUsage.ReadWrite; chc.RegionName = "asdCollection"; });
            //});
        }

        static void initData()
        {
            using (UnitOfWork.Start())
            {
                UnitOfWork.Current.BeginTransaction();

                var cls = new Class();
                cls.Name = "中一班";
                cls.Slogan = "聚沙成塔，水滴石穿";

                cls.AddStudent(new Student() { Name = "张三1", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三2", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三3", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三4", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三5", Age = 15 });

                UnitOfWork.CurrentSession.Persist(cls);
                UnitOfWork.Current.Commit();
            }
        }

        [TestMethod]
        public void query_ont_to_manay()
        {
            using (UnitOfWork.Start())
            {
                var cls = UnitOfWork.CurrentSession.Get<Class>(1u);

                Assert.IsNotNull(cls);
                Assert.AreEqual(5, cls.Students.Count);
            }
        }

        [TestMethod]
        public void out_of_session()
        {
            Class cls1;
            using (UnitOfWork.Start())
            {
                cls1 = UnitOfWork.CurrentSession.Load<Class>(1u);
                //NHibernateUtil.Initialize(cls1.Students);
            }

            Assert.IsFalse(NHibernateUtil.IsPropertyInitialized(cls1, "Students"));
        }

        [TestMethod]
        public void auto_modify()
        {
            string clsName = "中三班----------22";
            using (UnitOfWork.Start())
            {
                Class cls1 = UnitOfWork.CurrentSession.Get<Class>(1u);
                cls1.Name = clsName;
                UnitOfWork.CurrentSession.Flush();
            }

            using (UnitOfWork.Start())
            {
                Assert.AreEqual(clsName, UnitOfWork.CurrentSession.Get<Class>(1u).Name);
            }
        }

        [TestMethod]
        public void statetest()
        {
            //查看状态
            Class cls1;
            using (UnitOfWork.Start())
            {
                cls1 = UnitOfWork.CurrentSession.Get<Class>(1u);
            }
            using (UnitOfWork.Start())
            {
                Console.WriteLine("第二次查询》》》》》》》》》》》》》");
                Console.WriteLine(UnitOfWork.CurrentSession.Get<Class>(1u).Name);
            }
            cls1.Name = "中三班111";

            using (UnitOfWork.Start())
            {
                //将托管状态的对象变为 持久状态
                //session.Merge(cls1);  //存在标识相同的时候 与 SaveOrUpdate 有区别
                UnitOfWork.CurrentSession.SaveOrUpdate(cls1);

                var cls2 = new Class() { Id = 2u, Name = "as", Slogan = "aaa" };
                UnitOfWork.CurrentSession.Merge(cls2); // Merge 会触发 一次load

                Assert.IsTrue(UnitOfWork.CurrentSession.Contains(cls1));
                Assert.IsFalse(UnitOfWork.CurrentSession.Contains(cls2));

                //session.Lock(cls1, LockMode.None); //将当前的状态记住，后面的修改将刷新到数据库
                //cls1.Name = "中三班111";
                UnitOfWork.CurrentSession.Flush();
            }
            using (UnitOfWork.Start())
            {
                Assert.AreEqual("中三班111", UnitOfWork.CurrentSession.Get<Class>(1u).Name);
            }
        }

        [TestMethod]
        public void Cascade_delete_collection()
        {
            using (UnitOfWork.Start())
            {
                var t = UnitOfWork.Current.BeginTransaction();

                var cls = UnitOfWork.CurrentSession.Load<Class>(1u);
                cls.Students.RemoveAt(0); //删除没有作用\Cascade.DeleteOrphans 添加这个级联操作之后有效
                UnitOfWork.CurrentSession.Persist(cls);

                t.Commit();

                cls = UnitOfWork.CurrentSession.Get<Class>(1u);

                Assert.AreEqual(4, cls.Students.Count);
            }
        }
    }
}
