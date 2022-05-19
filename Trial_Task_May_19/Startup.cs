using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trial_Task_May_19
{
    public class Startup
    {
        //Task - 1
        //Brand Adinda Modeliniz Olsun
        //Id ve Name Memberleri Olsun
        //
        //Car Adinda Modeliniz Olsun
        //Id , Name , Year , EngineType ve BrandId Memberleri Olsun

        //Car Modelinizde Hansi Brand-e Aid Oldugnu Bilmek Ucun Car Modelinde Olan BrandId Memberinde Brandin Id si Olmalidi

        //Edeceyniz Task Bu Modelleri Hazirladiqdan Sonra Brand Modelinin Oz Controlleri Olsun Brand Controllerin Brandlarin Siyahisi
        //Her hansi Bir Brand-e click Edikde Car Controllerne Gedec Ilk acilan sehifede Yeni Index Actionunda Car-larin Siyahisi
        //Gorunecek Bundan Her Hansi Bir car-a Click etdikde GEdecek Car-in Detail Action-u Olacaq Click Olunan Carin
        //In-ni Name-ni Year-ni EngineType-ni Deyerlerni Gosderecek

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //her shey controllerden bashlayir, ona gore deyirik ki get kontrollere ve onun icinnen view da olacaq
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Brand}/{action=Index}/{id?}"
                        //obicno mi pishem Home, no nam xocetsa shto bi page acilanda srazu Brandlar olsun, index html olmasin tipa
                        //burda vse, getdik controllere, Brand ve Car yaratmaq lazimdi
                    );
            });
        }
    }
}
