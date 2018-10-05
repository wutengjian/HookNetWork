﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Downloader
{
    public class StartUp
    {
        ContainerBuilder builder;
        IContainer container;
        public StartUp()
        {
        }
        /// <summary>
        /// 配置依赖注入
        /// </summary>
        public void ConfigureServices()
        {
        }
        /// <summary>
        /// 中间件配置
        /// </summary>
        public void Configure()
        {
            Run();
        }
        private void Run()
        {
            new GlobalTimes().Run();
        }
    }
}
