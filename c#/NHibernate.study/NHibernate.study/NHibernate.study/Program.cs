using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NHibernate.Mapping.ByCode;
using NHibernate;
using NHibernate.Study.DomainModel;
using NHibernate.Study.UnitOfWork;

namespace NHibernate.Study
{
    class Program
    {
        static void Main(string[] args)
        {
            //log4net.Config.DOMConfigurator.Configure();

            log4net.Config.XmlConfigurator.Configure();

            Mapping();

            //var export = new SchemaExport(cfg);
            //var sb = new StringBuilder();
            //TextWriter output = new StringWriter(sb);
            //var sqlconn = new SQLiteConnection(connectionString);
            //sqlconn.Open();
            //export.Execute(false, true, false, sqlconn, output);
            //sqlconn.Close();
            //new SchemaExport(cfg).Execute(false, true, false);

            InitData();

            ReadData();

            ModifyData();

            StateTest();

            OtherTest();

            LoggerProvider.LoggerFor(typeof(Program)).Info("nihao");

            Console.Read();
        }

        /// <summary>
        /// 其他测试
        /// </summary>
        /// <param name="sessionFac"></param>
        private static void OtherTest()
        {
            Console.WriteLine("OtherTest");
            Class cls1;
            using (UnitOfWork.UnitOfWork.Start())
            {
                //cls1 = session.QueryOver<Class>().Take(1).Future().FirstOrDefault();
                //NHibernateUtil.Initialize(cls1.Students);

                cls1 = UnitOfWork.UnitOfWork.CurrentSession.Load<Class>(1u);
                //NHibernateUtil.Initialize(cls1.Students);

                //Console.WriteLine(cls1.Name);
                //Console.WriteLine(cls1.Students.Count());
            }

            if (NHibernateUtil.IsPropertyInitialized(cls1, "Students"))
            {
                foreach (var item in cls1.Students)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }

        /// <summary>
        /// 状态测试
        /// </summary>
        /// <param name="sessionFac"></param>
        private static void StateTest()
        {
            Console.WriteLine("+++++++++++++++++++++++查看状态+++++++++++++++++++++++");
            //查看状态
            Class cls1;
            using (UnitOfWork.UnitOfWork.Start())
            {
                //cls1 = session.QueryOver<Class>().Future().FirstOrDefault();
                cls1 = UnitOfWork.UnitOfWork.CurrentSession.Get<Class>(1u);
                cls1.Name = "中三班----------22";
                UnitOfWork.UnitOfWork.CurrentSession.Flush();
            }
            cls1.Name = "中三班111";

            using (UnitOfWork.UnitOfWork.Start())
            {
                //cls1 = session.QueryOver<Class>().Future().FirstOrDefault();
                Console.WriteLine("第二次查询》》》》》》》》》》》》》");
                Console.WriteLine(UnitOfWork.UnitOfWork.CurrentSession.Get<Class>(1u).Name);
            }

            using (UnitOfWork.UnitOfWork.Start())
            {
                //将托管状态的对象变为 持久状态
                //session.Merge(cls1);  //存在标识相同的时候 与 SaveOrUpdate 有区别
                UnitOfWork.UnitOfWork.CurrentSession.SaveOrUpdate(cls1);

                var cls2 = new Class() { Id = 2u, Name = "as", Slogan = "aaa" };
                UnitOfWork.UnitOfWork.CurrentSession.Merge(cls2); // Merge 会触发 一次load

                Console.WriteLine(UnitOfWork.UnitOfWork.CurrentSession.Contains(cls2));

                //session.Lock(cls1, LockMode.None); //将当前的状态记住，后面的修改将刷新到数据库
                //cls1.Name = "中三班111";
                UnitOfWork.UnitOfWork.CurrentSession.Flush();
                
            }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="sessionFac"></param>
        private static void ModifyData()
        {
            Console.WriteLine("修改数据");
            using (UnitOfWork.UnitOfWork.Start())
            {
                var t = UnitOfWork.UnitOfWork.Current.BeginTransaction();

                //Class cls = new Class() { Id = 1, Name = "中三班" };
                //session.Update(cls);

                var cls = UnitOfWork.UnitOfWork.CurrentSession.Load<Class>(1u);
                cls.Name = "中三班";
                cls.Students.RemoveAt(0); //删除没有作用\Cascade.DeleteOrphans 添加这个级联操作之后有效
                UnitOfWork.UnitOfWork.CurrentSession.Persist(cls);

                t.Commit();

                cls = UnitOfWork.UnitOfWork.CurrentSession.Get<Class>(1u);
                Console.WriteLine(cls.Name);
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sessionFac"></param>
        private static void ReadData()
        {
            Console.WriteLine("读取数据");
            using (UnitOfWork.UnitOfWork.Start())
            {
                //var cls = session.QueryOver<Class>().Future().FirstOrDefault();
                var cls1 = UnitOfWork.UnitOfWork.CurrentSession.Load<Class>(1u);
                Console.WriteLine(cls1.Name);

                //Console.WriteLine(cls.Name);
                //foreach (var item in cls.Students)
                //{
                //    Console.WriteLine(item.Name);
                //}

                //var stu = session.QueryOver<Student>().Future().FirstOrDefault();
                var stu = UnitOfWork.UnitOfWork.CurrentSession.Load<Student>(1u);
                Console.WriteLine(stu.Class.Id);
                Console.WriteLine(stu.Class.Name);
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private static void InitData()
        {
            using (UnitOfWork.UnitOfWork.Start())
            {
                //var t = UnitOfWork.UnitOfWork.Current.BeginTransaction();
                UnitOfWork.UnitOfWork.Current.BeginTransaction();
                //UnitOfWork.UnitOfWork.Current.BeginTransaction();

                var cls = new Class();
                cls.Name = "中一班";
                cls.Slogan = "聚沙成塔，水滴石穿";

                cls.AddStudent(new Student() { Name = "张三1", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三2", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三3", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三4", Age = 15 });
                cls.AddStudent(new Student() { Name = "张三5", Age = 15 });

                UnitOfWork.UnitOfWork.CurrentSession.Persist(cls);

                //UnitOfWork.UnitOfWork.Current.TransactionalFlush();
                UnitOfWork.UnitOfWork.Current.TransactionalFlush();

                //t.Commit();
            }
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
                    bm.Cascade(Cascade.Persist | Cascade.Remove);
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

            UnitOfWork.UnitOfWork.Configuration.AddMapping(hbmmap);

            //使用二次缓存
            //config.EntityCache<Class>(ch =>
            //{
            //    ch.Strategy = EntityCacheUsage.ReadWrite;
            //    ch.RegionName = "asd";
            //    ch.Collection(c => c.Students, chc => { chc.Strategy = EntityCacheUsage.ReadWrite; chc.RegionName = "asdCollection"; });
            //});
        }
    }
}
