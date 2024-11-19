using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceAccess;
using Kayak.IModuleServices.Aggregation.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess;
using Kayak.IModuleServices.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess.Queries;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Engines;
using Surging.Core.CPlatform.Engines.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Protocol;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Core.Implementation;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.DeviceAccess.Domains
{
    public class ProtocolManageAggService : ProxyServiceBase, IProtocolManageAggService, ISingleInstance
    {
        private readonly VirtualPathProviderServiceEngine _serviceEngine;
        private readonly IProtocolSupports _protocolSupports;
        public ProtocolManageAggService(IServiceEngine serviceEngine,IProtocolSupports protocolSupports) {
            _serviceEngine = serviceEngine as VirtualPathProviderServiceEngine;
            _protocolSupports = protocolSupports;
        }

        public async Task<ApiResult<bool>> Add(ProtocolAggModel model)
        {
            if(!model.FileId.IsNullOrEmpty())
            { 
                if (_serviceEngine != null && 
                    _serviceEngine.ComponentServiceLocationFormats != null)
                {
                    var filePath = Path.Combine(AppContext.BaseDirectory, model.FileId, model.FileAddress);
                    if (File.Exists(filePath))
                    {
                        var componentPath = Path.Combine(AppConfig.ServerOptions.RootPath, _serviceEngine.ComponentServiceLocationFormats[0], model.ProtocolCode);
                        ExtractZipFile(filePath, componentPath);
                        Directory.Delete(Path.Combine(AppContext.BaseDirectory, model.FileId), true);
                    }
                }
            }
           return await GetService<IProtocolManageService>().Add(new ProtocolModel
           {
               ClassName = model.ClassName,
               ConnProtocol = model.ConnProtocol,
               FileAddress = model.FileAddress,
               ProtocolCode = model.ProtocolCode,
               ProtocolName = model.ProtocolName,
               ProtocolType = model.ProtocolType,
               Remark = model.Remark,
               Script = model.Script
           });
        }
         
        public async Task<ApiResult<bool>> Republish(List<int> ids)
        {
            foreach (int id in ids)
            {
               var protocolResult=await GetService<IProtocolManageService>().GetProtocol(id);
                var protocol = protocolResult.Result;
                var result = GetService<IProtocolManageService>().Republish(new List<int> { id});
                if (protocol.ProtocolType == "Script")
                {
                    new CommonProtocolSupportProvider().Create(
                        new ProtocolSupportProperties
                        {
                            Description = protocol.Remark,
                            Id = protocol.ProtocolCode,
                            Name = protocol.ProtocolName,
                            Script = protocol.Script,
                            Transport = Enum.Parse<MessageTransport>(protocol.ConnProtocol, true)

                        }).Subscribe(protocolSupport =>
                        {
                            _protocolSupports.Register(protocolSupport);
                        });
                }
            }
            return ApiResult<bool>.Succeed(true);
        }

        public async Task<ApiResult<bool>> Modify(ProtocolAggModel model)
        {
            if (!model.FileId.IsNullOrEmpty())
            {
                if (_serviceEngine != null &&
                    _serviceEngine.ComponentServiceLocationFormats != null)
                {
                    var filePath = Path.Combine(AppContext.BaseDirectory, model.FileId, model.FileAddress);
                    if (File.Exists(filePath))
                    {
                        var componentPath = Path.Combine(AppConfig.ServerOptions.RootPath, _serviceEngine.ComponentServiceLocationFormats[0], model.ProtocolCode);
                        ExtractZipFile(filePath, componentPath);
                        Directory.Delete(Path.Combine(AppContext.BaseDirectory, model.FileId), true);
                    }
                }
            }
            return await GetService<IProtocolManageService>().Modify(new ProtocolModel
            {
                 Id = model.Id,
                ClassName = model.ClassName,
                ConnProtocol = model.ConnProtocol,
                FileAddress = model.FileAddress,
                ProtocolCode = model.ProtocolCode,
                ProtocolName = model.ProtocolName,
                ProtocolType = model.ProtocolType,
                Remark = model.Remark,
                Script = model.Script
            });
        }

        public async Task<ApiResult<Page<ProtocolModel>>> GetPageListAsync(ProtocolQuery query)
        {
            var result = await GetService<IProtocolManageService>().GetPageAsync(query);
            return result;
        }

        private void ExtractZipFile(string zipFilePath, string extractPath)
        {
            if (!Directory.Exists(extractPath))
            {
                // 确保提取路径存在
                Directory.CreateDirectory(extractPath);

                // 打开ZIP文件来读取
                using (var archive = ZipFile.OpenRead(zipFilePath))
                {
                    // 遍历ZIP包中的所有文件
                    foreach (var entry in archive.Entries)
                    {
                        // 如果条目是一个文件夹，则创建它
                        if (entry.FullName.EndsWith("/"))
                        {
                            Directory.CreateDirectory(Path.Combine(extractPath, entry.FullName));
                            continue;
                        }

                        // 提取文件到指定路径
                        entry.ExtractToFile(Path.Combine(extractPath, entry.FullName), overwrite: true);
                    }
                }
            }
        }

        public async Task<ApiResult<bool>> CancelPublish(List<int> ids)
        {
            foreach (int id in ids)
            {
                var protocolResult = await GetService<IProtocolManageService>().GetProtocol(id);
                var protocol = protocolResult.Result;
                var result = GetService<IProtocolManageService>().CancelPublish(new List<int> { id });
                if (protocol.ProtocolType == "Script")
                {
                    _protocolSupports.UnRegister(protocol.ProtocolCode);
                }
            }
            return ApiResult<bool>.Succeed(true);
        }
    }
}
