using System;
using System.Collections.Generic;
using System.Linq;

using EmitMapper;
using EmitMapper.MappingConfiguration.MappingOperations;
using EmitMapper.Utils;
using EmitMapper.MappingConfiguration;
using System.Collections.Specialized;

namespace LL.FrameWork.Impl.Infrastructure.Adapter.EmitMapperImpl
{
    /// <summary>
    /// 将From里面的数据转换为值
    /// </summary>
    public class FormCollectionMapConfig : IMappingConfigurator
    {
        public IMappingOperation[] GetMappingOperations(Type from, Type to)
        {
            var members = ReflectionUtils.GetPublicFieldsAndProperties(to);
            return members
                .Select(
                    m =>
                    new DestWriteOperation()
                    {
                        Destination = new MemberDescriptor(m),
                        Getter =
                            (ValueGetter<object>)
                            (
                                (form, valueProviderObj) =>
                                {
                                    var valueProvider = valueProviderObj as Func<string, object>;
                                    if (valueProvider == null)
                                    {
                                        valueProvider = (k) => ((NameValueCollection)form)[k];
                                    }
                                    var val = valueProvider(m.Name);
                                    if (val != null)
                                    {
                                        return ValueToWrite<object>.ReturnValue(val);
                                    }
                                    else
                                    {
                                        return ValueToWrite<object>.Skip();
                                    }
                                }
                            )
                    }
                )
                .ToArray();
        }

        public string GetConfigurationName()
        {
            return null;
        }

        public IRootMappingOperation GetRootMappingOperation(Type from, Type to)
        {
            return null;
        }

        public StaticConvertersManager GetStaticConvertersManager()
        {
            return null;
        }
    }
}
