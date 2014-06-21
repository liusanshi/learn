using System;
using System.Collections.Generic;
using System.Linq;

using EmitMapper;
using EmitMapper.Mappers;
using EmitMapper.MappingConfiguration;
using LL.FrameWork.Core.Infrastructure.Adapter;

namespace LL.FrameWork.Implementation.Infrastructure.Adapter
{
    /// <summary>
    /// EmitMapper 辅助类
    /// </summary>
    public static class EmitMapHelper
    {
        /// <summary>
        /// 对象转换的缓存
        /// </summary>
        private static Dictionary<TypeMapIdentity, IMappingConfigurator> TypeMapping = new Dictionary<TypeMapIdentity, IMappingConfigurator>(50);

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            var type = typeof(IMappingSetting);
            var profiles = AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .SelectMany(a => a.GetTypes())
                                    .Where(t => t.IsClass && type.IsAssignableFrom(t));

            foreach (var item in profiles)
            {
                var mapconfig = (IMappingSetting)Activator.CreateInstance(item);
                TypeMapping[mapconfig.GetIdentity()] = mapconfig.GetObjectsMapper();
            }
        }

        /// <summary>
        /// 获取 ObjectsMapper 对象
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <returns></returns>
        public static ObjectsMapper<TFrom, TTo> GetObjectsMapper<TFrom, TTo>()
        {
            return new ObjectsMapper<TFrom, TTo>(GetObjectsMapperImpl<TFrom, TTo>());
        }

        /// <summary>
        /// 获取 ObjectsMapperBaseImpl
        /// </summary>
        /// <param name="TFrom"></param>
        /// <param name="TTo"></param>
        /// <returns></returns>
        public static ObjectsMapperBaseImpl GetObjectsMapperImpl(Type TFrom, Type TTo)
        {
            var identity = new TypeMapIdentity(TFrom, TTo);
            IMappingConfigurator MapConfig;
            if (!TypeMapping.TryGetValue(identity, out MapConfig))
            {
                MapConfig = DefaultMapConfig.Instance;
            }
            return ObjectMapperManager.DefaultInstance.GetMapperImpl(TFrom, TTo, MapConfig);
        }
        /// <summary>
        /// 获取 ObjectsMapperBaseImpl
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <returns></returns>
        public static ObjectsMapperBaseImpl GetObjectsMapperImpl<TFrom, TTo>()
        {
            return GetObjectsMapperImpl(typeof(TFrom), typeof(TTo));
        }
        /// <summary>
        /// 获取 ObjectsMapperBaseImpl
        /// </summary>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="TFrom"></param>
        /// <returns></returns>
        public static ObjectsMapperBaseImpl GetObjectsMapperImpl<TTo>(Type TFrom)
        {
            return GetObjectsMapperImpl(TFrom, typeof(TTo));
        }
    }
}
