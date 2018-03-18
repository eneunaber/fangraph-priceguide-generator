using System;
using System.IO;
using System.Collections.Generic;
using AutoMapper;
using fangraph_priceguide_generator.Strategy;
using McMaster.Extensions.CommandLineUtils;
using fangraph_priceguide_generator.Commands;

namespace fangraph_priceguide_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoMapperMappings.Setup();

            var app = new CommandLineApplication();
            RootCommand.Configure(app);
            app.Execute(args);
        }
    }
}
