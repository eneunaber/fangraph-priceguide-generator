using McMaster.Extensions.CommandLineUtils;
using System;

namespace fangraph_priceguide_generator.Commands
{
    public class RootCommand : ICommand
    {
        public static void Configure(CommandLineApplication app) {
            app.Name = "priceguide-generator";
            app.HelpOption("-?|-h|--help");

            // Register commands
            app.Command("conversion-record", ConversionRecordCommand.Configure);
            app.Command("convert-fangraph", ConfigureFangraphCommand.Configure);

            app.OnExecute(() => {
                try {
                    (new RootCommand(app)).Run();
                } catch(Exception ex) {
                    Console.WriteLine("Exception caught:" + ex.Message);
                    return 1;
                }
                return 0;
            });
        }

        private readonly CommandLineApplication _app;

        public RootCommand(CommandLineApplication app) {
            _app = app;
        }

        public void Run() {
            _app.ShowHelp();
        }
    }
}