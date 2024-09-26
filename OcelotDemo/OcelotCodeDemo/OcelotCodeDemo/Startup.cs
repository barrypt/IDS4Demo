using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Cache.CacheManager;
using CacheManager.Core;
using Ocelot.Cache;
using Ocelot.Provider.Polly;
using IdentityServer4.AccessTokenValidation;
using Microsoft.IdentityModel.Tokens;

namespace OcelotCodeDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string authenticationProviderKey = "TestGatewaykey";
            services.AddAuthentication("Bearer")
                   .AddJwtBearer(authenticationProviderKey, options =>
                   {
                       // ָ��Ҫ�������Ȩ��������ַ
                       options.Authority = "http://localhost:6100";
                       // ����֤tokenʱ������֤Audience
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateAudience = false
                       };
                       // ������Https
                       options.RequireHttpsMetadata = false;
                   });

            services.AddOcelot()
                .AddConsul()
                .AddPolly();
            // ע����ط���
            //.AddCacheManager(x=> {
            //    // ����Redis�����Ϣ
            //    //x.WithRedisConfiguration("redis", config =>
            //    //{
            //    //    config.WithAllowAdmin() // ���й���Ա��ز���
            //    //    .WithPassword("redispwd") // ���Redis��Ҫ�������������
            //    //    //�����ݱ������ĸ����ݿ��У�RedisĬ����16��������ָ������λ13�����ݿ�
            //    //    .WithDatabase(13)
            //    //    .WithEndpoint("192.168.30.250",6379);//ָ��Redis�������Ͷ˿�
            //    //}).WithRedisCacheHandle("redis",true); // ָ������
            //    //x.WithJsonSerializer();//ָ���������л���ʽ
            //    x.WithDictionaryHandle();//ʹ��Dictonary��ʽ����
            //});
            //services.AddSingleton<IOcelotCache<CachedResponse>, MyCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOcelot();
        }
    }
}
