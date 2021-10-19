using System;

namespace SR.Data.Enums
{
    public enum ReplyMessage : byte
    {
        SettingsPrefixFieldName,
        SettingsEmbedColorFieldName,
        SettingsLanguageFieldName,
        SettingsPrefixFieldDesc,
        SettingsEmbedColorFieldDesc,
        SettingsLanguageFieldDesc,
        SetPrefixSuccess,
        SetColorSuccess,
        SetLanguageSuccess,
        HelpHowToEditBotSettingsFieldName,
        HelpHowToEditBotSettingsFieldDesc,
        HelpHowToSendReactionsFieldName,
        HelpHowToSendReactionsFieldDesc,
        HelpHowToInviteBotFieldName,
        HelpHowToInviteBotFieldDesc,
        AboutDesc,
        AboutInviteFieldName,
        AboutContactFieldName,
        AboutSupportFieldName,
        AboutInviteFieldDesc,
        AboutContactFieldDesc,
        AboutSupportFieldDesc,
        HelpFooter
    }

    public static class ReplyMessageHelper
    {
        public static string Parse(this ReplyMessage message, Language language) => message.Localize(language);

        public static string Parse(this ReplyMessage message, Language language, params object[] replacements)
        {
            try
            {
                return string.Format(Parse(message, language), replacements);
            }
            catch (FormatException)
            {
                return language switch
                {
                    Language.English => "`An output error has occurred. Please show this to <@550493599629049858>.`",
                    Language.Russian => "`Возникла ошибка вывода ответа. Пожалуйста, покажите это <@550493599629049858>.`",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                };
            }
        }

        public static string Localize(this ReplyMessage message, Language language) => message switch
        {
            ReplyMessage.SettingsPrefixFieldName => language switch
            {
                Language.English => "{0} Prefix",
                Language.Russian => "{0} Префикс",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SettingsEmbedColorFieldName => language switch
            {
                Language.English => "{0} Embed color",
                Language.Russian => "{0} Цвет эмбеда",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SettingsLanguageFieldName => language switch
            {
                Language.English => "{0} Language",
                Language.Russian => "{0} Язык",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SetPrefixSuccess => language switch
            {
                Language.English => "{0}, You have successfully updated the prefix to **{1}**.",
                Language.Russian => "{0}, Вы успешно обновили префикс на **{1}**.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SetColorSuccess => language switch
            {
                Language.English => "{0}, You have successfully updated embed color to **{1}**.",
                Language.Russian => "{0}, Вы успешно обновили цвет эмбеда на **{1}**.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SetLanguageSuccess => language switch
            {
                Language.English => "{0}, You have successfully updated language to **{1}**.",
                Language.Russian => "{0}, Вы успешно обновили язык на **{1}**.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SettingsPrefixFieldDesc => language switch
            {
                Language.English => "Current prefix: **{0}**\nUse `{0}settings prefix [new prefix]` to change",
                Language.Russian =>
                    "Текущий префикс: **{0}**\nНапишите `{0}настройки префикс [новый префикс]` чтобы изменить",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SettingsEmbedColorFieldDesc => language switch
            {
                Language.English =>
                    "Current color: **{0}**\nUse `{1}settings color [new hex-color]` to change\n\n> You can choose the color [by clicking here](https://www.google.com/search?q=color+picker)",
                Language.Russian =>
                    "Текущий цвет: **{0}**\nНапишите `{1}настройки цвет [новый hex-цвет]` чтобы изменить\n\n> Подобрать цвет можно [нажав сюда](https://www.google.com/search?q=color+picker)",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SettingsLanguageFieldDesc => language switch
            {
                Language.English =>
                    "Current language: **{0}**\nUse `{1}settings language [new language number]` to change\nAvailable languages:\n{2}",
                Language.Russian =>
                    "Текущий язык: **{0}**\nНапишите `{1}настройки язык [номер нового языка]` чтобы изменить\n\n> Доступные языки:\n{2}",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpHowToEditBotSettingsFieldName => language switch
            {
                Language.English => "How to edit bot settings {0}",
                Language.Russian => "Как изменить настройки бота {0}",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpHowToEditBotSettingsFieldDesc => language switch
            {
                Language.English =>
                    "{0} Use `{1}{2}` command in any channel (to which the bot has access) on your server.",
                Language.Russian =>
                    "{0} Напишите `{1}{2}` в любом канале (к которому у бота есть доступ) на вашем сервере.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpHowToSendReactionsFieldName => language switch
            {
                Language.English => "How to send reactions {0}",
                Language.Russian => "Как отправлять реакции {0}",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpHowToSendReactionsFieldDesc => language switch
            {
                Language.English =>
                    "{0} Use `{1}{2}` command, available reactions are shown below:\n\n> {3}\n\n> You can also add any text or mention after the command to add it to the reaction.\n> Example command:\n> `{1}kiss @SomeUserMention with some text`",
                Language.Russian =>
                    "{0} Напишите `{1}{2}`, доступные реакции отображены ниже:\n\n> {3}\n\n> Вы так же можете добавить любой текст или упоминание после команды чтобы добавить его в реакцию.\n> Пример команды:\n> `{1}поцеловать @ЛюбоеУпоминание с любым текстом`",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpHowToInviteBotFieldName => language switch
            {
                Language.English => "How to invite a bot to your server {0}",
                Language.Russian => "Как пригласить бота на ваш сервер {0}",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpHowToInviteBotFieldDesc => language switch
            {
                Language.English =>
                    "{0} Click [here to open the invite link](https://discord.com/oauth2/authorize?client_id=851029845982707722&scope=bot&permissions=289792).",
                Language.Russian =>
                    "{0} Нажмите [сюда чтобы открыть ссылку с приглашением](https://discord.com/oauth2/authorize?client_id=851029845982707722&scope=bot&permissions=289792).",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutDesc => language switch
            {
                Language.English => "{0} is a simple bot to help you send gif-reactions on your server.",
                Language.Russian => "{0} это простой бот, помогающий использовать гиф-реакции на вашем сервере.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutInviteFieldName => language switch
            {
                Language.English => "Invite a bot to your server",
                Language.Russian => "Пригласите бота на ваш сервер",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutContactFieldName => language switch
            {
                Language.English => "Contacts",
                Language.Russian => "Контакты",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutSupportFieldName => language switch
            {
                Language.English => "Support",
                Language.Russian => "Поддержка",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutInviteFieldDesc => language switch
            {
                Language.English => "{0} Click [here to open the invite link](https://discord.com/oauth2/authorize?client_id=851029845982707722&scope=bot&permissions=289792).",
                Language.Russian => "{0} Нажмите [сюда чтобы открыть ссылку с приглашением](https://discord.com/oauth2/authorize?client_id=851029845982707722&scope=bot&permissions=289792).",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutContactFieldDesc => language switch
            {
                Language.English => "{0} Discord: <@550493599629049858>\n{1} Twitter: [@evkkxo](https://twitter.com/evkkxo)",
                Language.Russian => "{0} Discord: <@550493599629049858>\n{1} Twitter: [@evkkxo](https://twitter.com/evkkxo)",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutSupportFieldDesc => language switch
            {
                Language.English => "You can support this small but useful bot by sending any amount you like. I will be grateful to you for every penny {0}\n{1} `5375 4141 0460 6651` EUGENE GARBUZOV",
                Language.Russian => "Вы можете поддержать этого маленького, но полезного бота отправив любую сумму. Я буду благодарен вам за каждую копейку {0}\n{1} `5375 4141 0460 6651` EUGENE GARBUZOV",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpFooter => language switch
            {
                Language.English => "Because you used the command in DM - I cannot find out about your server settings. The message language and prefix are displayed by default and may differ from what is used on your server. Write this command on the server to get the exact information.",
                Language.Russian => "Because you used the command in DM - I cannot find out about your server settings. The message language and prefix are displayed by default and may differ from what is used on your server. Write this command on the server to get the exact information.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(message), message, null)
        };
    }
}
