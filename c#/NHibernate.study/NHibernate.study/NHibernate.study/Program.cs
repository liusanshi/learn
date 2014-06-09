using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Cache;
using NHibernate.Type;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Dialect;
using NHibernate.AdoNet;
using NHibernate.Driver;
using NHibernate.Exceptions;
using NHibernate.Connection;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Mapping.ByCode;
using NHibernate.Study.DomainModel;
using NHibernate.Util;
using System.Data.SQLite;
using System.IO;

namespace NHibernate.Study
{
    class Program
    {

        static string connectionString = "Data Source=myTest.db";

        static void Main(string[] args)
        {
            Configuration cfg = Config();
            Mapping(cfg);

            var export = new SchemaExport(cfg);
            var sb = new StringBuilder();
            TextWriter output = new StringWriter(sb);
            var sqlconn = new SQLiteConnection(connectionString);
            sqlconn.Open();
            export.Execute(true, true, false, sqlconn, output);
            sqlconn.Close();

            //Console.WriteLine(sb.ToString());

            var sessionFac = cfg.BuildSessionFactory();
            using (var session = sessionFac.OpenSession())
            {
                var t = session.BeginTransaction();

                var cls = new Class();
                cls.Name = "中一班";

                cls.AddStudent(new Student() { Name = "张三1", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三2", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三3", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三4", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三5", Age = 15 });

                session.Persist(cls);

                t.Commit();
            }

            Console.WriteLine("读取数据");
            using (var session = sessionFac.OpenSession())
            {
                //var cls = session.QueryOver<Class>().Future().FirstOrDefault();
                //Console.WriteLine(cls.Name);
                //foreach (var item in cls.Students)
                //{
                //    Console.WriteLine(item.Name);
                //}

                var stu = session.QueryOver<Student>().Future().FirstOrDefault();
                Console.WriteLine(stu.Class.Id);
                Console.WriteLine(stu.Class.Name);
            }

            Console.WriteLine("修改数据");
            using (var session = sessionFac.OpenSession())
            {
                var t = session.BeginTransaction();

                //Class cls = new Class() { Id = 1, Name = "中三班" };
                //session.Update(cls);

                var cls = session.QueryOver<Class>().Future().FirstOrDefault();
                cls.Name = "中三班";
                cls.Students.RemoveAt(0); //删除没有作用\Cascade.DeleteOrphans 添加这个级联操作之后有效
                session.Persist(cls);

                t.Commit();

                cls = session.QueryOver<Class>().Future().FirstOrDefault();
                Console.WriteLine(cls.Name);
            }

            
            Console.Read();
        }

        /// <summary>
        /// 配置Nhibernate
        /// </summary>
        static Configuration Config()
        {
            Configuration config = new Configuration();

            config.DataBaseIntegration(db =>
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
                db.SchemaAction = SchemaAutoAction.Validate;
            });
            config.Cache(c =>
            {
                c.RegionsPrefix = "xyz";
                c.UseMinimalPuts = true;
                c.UseQueryCache = true;
                c.DefaultExpiration = 15;
                c.Provider<HashtableCacheProvider>();
                //c.QueryCache<StandardQueryCache>();
                //config.SetProperty(NHibernate.Cfg.Environment.QueryCacheFactory, typeof(StandardQueryCacheFactory).AssemblyQualifiedName);
            });
            config.Proxy(p =>
            {
                p.Validation = false;
                p.ProxyFactoryFactory<DefaultProxyFactoryFactory>();
            });
            config.Mappings(m =>
            {
                m.DefaultCatalog = "";
                m.DefaultSchema = "";
            });
            config.CollectionTypeFactory<DefaultCollectionTypeFactory>();

            config.HqlQueryTranslator<ASTQueryTranslatorFactory>();

            //config.SessionFactory().Named("SomeName")
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

            return config;
        }

        /// <summary>
        /// 类型配置
        /// </summary>
        /// <param name="config"></param>
        static void Mapping(Configuration config)
        {
            ModelMapper mapper = new ModelMapper();
            mapper.Class<Class>(m =>
            {
                m.Id<UInt32>(c => c.Id, im =>
                {
                    im.Generator(Generators.Native);
                    im.Column("Id");
                });
                m.Property<string>(c => c.Name, im => im.Column("Name"));
                m.Table("t_Class");

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
                    bm.Cascade(Cascade.Persist | Cascade.Remove | Cascade.DeleteOrphans);
                }, er => er.OneToMany());
            });

            mapper.Class<Student>(m =>
            {
                m.Table("t_Student");
                m.Id<UInt32>(c => c.Id, im => { im.Generator(Generators.Native); im.Column("Id"); });
                m.Property<string>(c => c.Name);
                m.Property<Int32>(c => c.Age);
                //m.Property<Int32>(c => c.ClassId);
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

            config.AddMapping(hbmmap);
        }
    }
}
