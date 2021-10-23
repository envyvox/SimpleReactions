using System;

namespace SR.Data.Enums
{
    public enum ReplyMessage : byte
    {
        SettingsLanguageFieldName,
        SettingsLanguageFieldDesc,
        SettingsEmbedColorFieldName,
        SettingsEmbedColorFieldDesc,

        SetLanguageSuccess,
        SetColorSuccess,
        SyncCommandsSuccess,

        AboutDesc,
        AboutJoinSupportServerFieldName,
        AboutJoinSupportServerFieldDesc,
        AboutInviteBotFieldName,
        AboutInviteBotFieldDesc,
        AboutContactsFieldName,
        AboutContactsFieldDesc,
        AboutVoteFieldName,
        AboutVoteFieldDesc,
        AboutSupportFieldName,
        AboutSupportFieldDesc,

        HelpHowToSetupBotFieldName,
        HelpHowToSetupBotFieldDesc,
        HelpHowToUseReactionsFieldName,
        HelpHowToUseReactionsFieldDesc
    }

    public static class ReplyMessageHelper
    {
        public static string Parse(this ReplyMessage message, LanguageType languageType) =>
            message.Localize(languageType);

        public static string Parse(this ReplyMessage message, LanguageType languageType, params object[] replacements)
        {
            try
            {
                return string.Format(Parse(message, languageType), replacements);
            }
            catch (FormatException)
            {
                return languageType switch
                {
                    LanguageType.English =>
                        "`An output error has occurred. Please show this to <@550493599629049858>.`",
                    LanguageType.Russian =>
                        "`Возникла ошибка вывода ответа. Пожалуйста, покажите это <@550493599629049858>.`",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                };
            }
        }

        private static string Localize(this ReplyMessage message, LanguageType language) => message switch
        {
            ReplyMessage.SettingsLanguageFieldName => language switch
            {
                LanguageType.English => "{0} Language",
                LanguageType.Russian => "{0} Язык",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SettingsLanguageFieldDesc => language switch
            {
                LanguageType.English =>
                    "Current language: **{0}**.\nUse `/settings update-language` to change.",
                LanguageType.Russian =>
                    "Текущий язык: **{0}**.\nНапишите `/settings update-language` чтобы изменить.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SettingsEmbedColorFieldName => language switch
            {
                LanguageType.English => "{0} Embed color",
                LanguageType.Russian => "{0} Цвет эмбеда",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SettingsEmbedColorFieldDesc => language switch
            {
                LanguageType.English =>
                    "Current color: **{0}**.\nUse `/settings update-color` to change.\n\n> You can choose the color [by clicking here](https://www.google.com/search?q=color+picker).",
                LanguageType.Russian =>
                    "Текущий цвет: **{0}**.\nНапишите `/settings update-color` чтобы изменить.\n\n> Подобрать цвет можно [нажав сюда](https://www.google.com/search?q=color+picker).",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },

            ReplyMessage.SetLanguageSuccess => language switch
            {
                LanguageType.English => "{0}, you have successfully updated language to **{1}**.",
                LanguageType.Russian => "{0}, вы успешно обновили язык на **{1}**.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SetColorSuccess => language switch
            {
                LanguageType.English => "{0}, you have successfully updated embed color to **{1}**.",
                LanguageType.Russian => "{0}, вы успешно обновили цвет эмбеда на **{1}**.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.SyncCommandsSuccess => language switch
            {
                LanguageType.English => "{0}, reaction commands created.",
                LanguageType.Russian => "{0}, команды реакций созданы.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },

            ReplyMessage.AboutDesc => language switch
            {
                LanguageType.English => "{0} is a simple bot to help you send gif-reactions on your server.",
                LanguageType.Russian => "{0} это простой бот для отправки гиф-реакций на вашем сервере.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutJoinSupportServerFieldName => language switch
            {
                LanguageType.English => "Join support server",
                LanguageType.Russian => "Присоединитесь на сервер поддержки",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutJoinSupportServerFieldDesc => language switch
            {
                LanguageType.English => "{0} [Click here to open the invite link]({1}).",
                LanguageType.Russian => "{0} [Нажмите сюда чтобы открыть приглашение]({1}).",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutInviteBotFieldName => language switch
            {
                LanguageType.English => "Add a bot to your server",
                LanguageType.Russian => "Добавьте бота на свой сервер",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutInviteBotFieldDesc => language switch
            {
                LanguageType.English => "{0} [Click here to open the invite link]({1}).",
                LanguageType.Russian => "{0} [Нажмите сюда чтобы открыть приглашение]({1}).",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutContactsFieldName => language switch
            {
                LanguageType.English => "Contacts",
                LanguageType.Russian => "Контакты",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutContactsFieldDesc => language switch
            {
                LanguageType.English =>
                    "{0} Discord: <@550493599629049858>\n{1} Twitter: [@envyvox](https://twitter.com/envyvox)",
                LanguageType.Russian =>
                    "{0} Discord: <@550493599629049858>\n{1} Twitter: [@envyvox](https://twitter.com/envyvox)",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutSupportFieldName => language switch
            {
                LanguageType.English => "Support",
                LanguageType.Russian => "Поддержка",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutSupportFieldDesc => language switch
            {
                LanguageType.English =>
                    "You can support this small but useful bot by sending any amount you like. I will be grateful to you for every penny {0}\n{1} `5375 4141 0460 6651` EUGENE GARBUZOV",
                LanguageType.Russian =>
                    "Вы можете поддержать этого маленького, но полезного бота отправив любую сумму. Я буду благодарен за каждую копейку {0}\n{1} `5375 4141 0460 6651` EUGENE GARBUZOV",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutVoteFieldName => language switch
            {
                LanguageType.English => "Vote for bot on top.gg",
                LanguageType.Russian => "Проголосуйте за бота на top.gg",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.AboutVoteFieldDesc => language switch
            {
                LanguageType.English => "{0} [Click here to vote]({1}).",
                LanguageType.Russian => "{0} [Нажмите сюда чтобы проголосовать]({1}).",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpHowToSetupBotFieldName => language switch
            {
                LanguageType.English => "How to set up a bot {0}",
                LanguageType.Russian => "Как настроить бота {0}",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpHowToSetupBotFieldDesc => language switch
            {
                LanguageType.English =>
                    "{0} After adding the bot to your server, you need to follow a few steps to set it up:" +
                    "\n\n1. Set the language you need. To do this, use `/settings update-language`." +
                    "\n\n> The default language is **{1}**. If it suits you, you can skip this step." +
                    "\n\n2. Use `/settings sync-commands` to add reaction commands with your chosen language." +
                    "\n\n3. The bot is ready to use. Optionally, you can change the color of the embed messages by using `/settings update-color`.",
                LanguageType.Russian =>
                    "{0} После добавление бота на ваш сервер, необходимо выполнить несколько шагов для настройки:" +
                    "\n\n1. Выставить необходимый вам язык. Для этого напишите `/settings update-language`." +
                    "\n\n> Язык по-умолчанию - **{1}**. Если вам он подходит - вы можете пропустить этот шаг." +
                    "\n\n2. Написать `/settings sync-commands` чтобы добавить команды реакций на выбранном вами языке." +
                    "\n\n3. Бот готов к использованию. По желанию, вы можете изменить цвет эмбед сообщений написав `/settings update-color`.",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpHowToUseReactionsFieldName => language switch
            {
                LanguageType.English => "How to use reactions {0}",
                LanguageType.Russian => "Как использовать реакции {0}",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            ReplyMessage.HelpHowToUseReactionsFieldDesc => language switch
            {
                LanguageType.English => "{0} After the commands were added, all available reactions appeared in the list of commands:\n\n{1}",
                LanguageType.Russian => "{0} После того как команды были добавлены, в списке команд появились все доступные реакции:\n\n{1}",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(message), message, null)
        };
    }
}
