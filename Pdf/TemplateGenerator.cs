using System.IO;
using System.Threading.Tasks;
using RazorLight;
using SkiTickets.Models;

namespace SkiTickets.Pdf
{
    public class TemplateGenerator
    {
        public async Task<string> GetHtmlString(Ticket ticket)
        {
            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(Ticket)) 
                .UseMemoryCachingProvider().Build();
            
                
            var pdf = new Pages.Pdf(ticket);
            var template = await File.ReadAllTextAsync("Pages/Pdf.cshtml");
            var result = await engine.CompileRenderStringAsync("pdf", template, pdf);

            return result;
        }
    }
}