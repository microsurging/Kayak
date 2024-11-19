using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.ServiceManage;
using Kayak.IModuleServices.Aggregation.ServiceManage.Models;
using Surging.Core.CPlatform.Engines.Implementation;
using Surging.Core.CPlatform.Engines;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kayak.Core.Common.Extensions;
using Surging.Core.CPlatform;
using Kayak.IModuleServices.ServiceManage;
using Kayak.IModuleServices.ServiceManage.Model;

namespace Kayak.Modules.Aggregation.ServiceManage.Domains
{
    internal class ModuleAggregationService : ProxyServiceBase, IModuleAggregationService, ISingleInstance
    {
        private readonly VirtualPathProviderServiceEngine _serviceEngine;
        public ModuleAggregationService(IServiceEngine serviceEngine)
        {
            _serviceEngine = serviceEngine as VirtualPathProviderServiceEngine;
        }
        public async Task<ApiResult<bool>> Add(ModuleAggModel model)
        {
            if (!model.FileId.IsNullOrEmpty())
            {
                if (_serviceEngine != null &&
                    _serviceEngine.ComponentServiceLocationFormats != null)
                {
                    var filePath = Path.Combine(AppContext.BaseDirectory, model.FileId, model.FileAddress);
                    if (File.Exists(filePath))
                    {
                        var componentPath = Path.Combine(AppConfig.ServerOptions.RootPath, _serviceEngine.ModuleServiceLocationFormats[0], model.ModuleCode);
                        ExtractZipFile(filePath, componentPath);
                        Directory.Delete(Path.Combine(AppContext.BaseDirectory, model.FileId), true);
                    }
                }
            }
            return await GetService<IModuleService>().Add(new ModuleModel
            {
                ModuleCode = model.ModuleCode,
                CreateDate = DateTime.Now,
                FileAddress = model.FileAddress,
                ModuleMode = model.ModuleMode,
                ModuleName = model.ModuleName,
                ModuleType = model.ModuleType,
                Remark = model.Remark,
                UpdateDate = DateTime.Now,
            });
        }

        public async Task<ApiResult<bool>> Modify(ModuleAggModel model)
        {
            if (!model.FileId.IsNullOrEmpty())
            {
                if (_serviceEngine != null &&
                    _serviceEngine.ComponentServiceLocationFormats != null)
                {
                    var filePath = Path.Combine(AppContext.BaseDirectory, model.FileId, model.FileAddress);
                    if (File.Exists(filePath))
                    {
                        var componentPath = Path.Combine(AppConfig.ServerOptions.RootPath, _serviceEngine.ModuleServiceLocationFormats[0], model.ModuleCode);
                        ExtractZipFile(filePath, componentPath);
                        Directory.Delete(Path.Combine(AppContext.BaseDirectory, model.FileId), true);
                    }
                }
            }
            return await GetService<IModuleService>().Modify(new ModuleModel
            {
                Id = model.Id,
                ModuleCode = model.ModuleCode,
                CreateDate = model.CreateDate,
                FileAddress = model.FileAddress,
                ModuleMode = model.ModuleMode,
                ModuleName = model.ModuleName,
                ModuleType = model.ModuleType,
                Remark = model.Remark,
                UpdateDate = DateTime.Now
            });
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
    }
}
