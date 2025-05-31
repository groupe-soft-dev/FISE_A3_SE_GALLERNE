using EasySaveV3.AppServer.Model.Interfaces;
using EasySaveV3.AppServer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySaveV3.AppServer.Commands
{
    public class LanguageCommandHandler : ISocketCommandHandler
    {
        private readonly ILocalizer _localizer;

        public LanguageCommandHandler(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        public bool CanHandle(string command)
        {
            return command is "GET_CURRENT_LANGUAGE" or "GET_AVAILABLE_LANGUAGES" or "TRANSLATE" or "SET_LANGUAGE";
        }

        public string Handle(string command, string? payload)
        {
            return command switch
            {
                "GET_CURRENT_LANGUAGE" => JsonSerializer.Serialize(_localizer.GetCurrentLanguage()),
                "GET_AVAILABLE_LANGUAGES" => JsonSerializer.Serialize(_localizer.GetAvailableLanguages()),
                "GET_TRANSLATIONS" => HandleAllTranslations(),
                "CHANGE_LANGUAGE" => SetLanguage(payload),
                _ => JsonSerializer.Serialize("Commande non reconnue")
            };
        }

        private string HandleTranslate(string? payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
                return JsonSerializer.Serialize("Aucune clé fournie");

            return JsonSerializer.Serialize(_localizer[payload]);
        }

        private string HandleAllTranslations()
        {
            Dictionary<string, string> allTranslations = _localizer.GetAllTranslations();
            return JsonSerializer.Serialize(allTranslations);
        }

        private string SetLanguage(string? payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
                return JsonSerializer.Serialize("Aucune langue fournie");

            try
            {
                var lang = JsonSerializer.Deserialize<string>(payload);
                _localizer.ChangeLanguage(lang);
                return JsonSerializer.Serialize("OK");
            }
            catch
            {
                return JsonSerializer.Serialize("Erreur lors du parsing du payload");
            }
        }
    }

}
