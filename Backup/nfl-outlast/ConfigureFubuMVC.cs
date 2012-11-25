using System.Web;
using FubuMVC.Core;
using FubuMVC.Razor;
using nfl_outlast.Endpoints.Home;

namespace nfl_outlast
{
    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            // This line turns on the basic diagnostics and request tracing
            IncludeDiagnostics(true);

            // All public methods from concrete classes ending in "Controller"
            // in this assembly are assumed to be action methods
            Actions.IncludeTypesNamed(x => x.EndsWith("Endpoint"));            
            
            // Policies
            Routes
                .IgnoreControllerNamespaceEntirely()      
                .IgnoreClassSuffix("Endpoint")
                .ConstrainToHttpMethod(x => x.InputType().Name.EndsWith("Request"), "GET")
                .ConstrainToHttpMethod(x => x.InputType().Name.EndsWith("Command"), "POST")                
                .IgnoreMethodSuffix("Html")
                .HomeIs<HomeRequest>()
                .RootAtAssemblyNamespace();            

            Assets.CombineAllUniqueAssetRequests();

            // Match views to action methods by matching
            // on model type, view name, and namespace
            Import<RazorEngineRegistry>();
            Views.TryToAttachWithDefaultConventions();
        }
    }     
}