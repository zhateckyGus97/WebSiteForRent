using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

public class TestHostEnvironment : IWebHostEnvironment
{
    public string EnvironmentName { get; set; } = Environments.Development;
    public string ApplicationName { get; set; } = "TestApplication";

    public string WebRootPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    public IFileProvider WebRootFileProvider { get; set; }

    public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
    public IFileProvider ContentRootFileProvider { get; set; }

    public TestHostEnvironment()
    {
        Directory.CreateDirectory(WebRootPath);
        Directory.CreateDirectory(ContentRootPath);

        WebRootFileProvider = new PhysicalFileProvider(WebRootPath);
        ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
    }
}