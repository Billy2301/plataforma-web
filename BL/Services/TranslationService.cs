using BL.IServices;
using Google.Cloud.Translation.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class TranslationService : ITranslationService
    {
        public string TranslateText(string text)
        {
            var client = TranslationClient.Create();
            TranslationResult result = client.TranslateText(text, LanguageCodes.Spanish);
            return result.TranslatedText;
        }
    }
}
