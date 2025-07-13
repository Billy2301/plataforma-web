namespace PortalClienteV2.Utilities.Helpers
{
    public class EmailTemplate
    {
        private readonly IWebHostEnvironment _env;
        public EmailTemplate(IWebHostEnvironment env)
        {
            _env = env;
        }


        public string GetEmailTemplate(string templateName)
        {
            var path = Path.Combine(_env.WebRootPath, "Templates", templateName);
            var templateContent = File.ReadAllText(path);
            return templateContent;
        }
    }
}
