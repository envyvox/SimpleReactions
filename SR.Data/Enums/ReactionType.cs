using System;

namespace SR.Data.Enums
{
    public enum ReactionType : byte
    {
        Shoot = 1,
        Bite = 2,
        Hug = 3,
        Kiss = 4,
        Sleep = 5,
        Knock = 6,
        Pat = 7,
        Hello = 8,
        Poke = 9,
        Drink = 10,
        Love = 11,
        Play = 12,
        Angry = 13,
        Blush = 14,
        Laugh = 15,
        Sad = 16,
        Cry = 17,
        Shock = 18,
        Happy = 19,
        Facepalm = 20
    }

    public static class InteractiveReactionHelper
    {
        public static string Name(this ReactionType reaction) => reaction switch
        {
            ReactionType.Shoot => "выстрелить",
            ReactionType.Bite => "укусить",
            ReactionType.Hug => "обнять",
            ReactionType.Kiss => "поцеловать",
            ReactionType.Sleep => "спатки",
            ReactionType.Knock => "стукнуть",
            ReactionType.Pat => "погладить",
            ReactionType.Hello => "приветствовать",
            ReactionType.Poke => "тыкнуть",
            ReactionType.Drink => "выпить",
            ReactionType.Love => "люблю",
            ReactionType.Play => "играть",
            ReactionType.Angry => "злюсь",
            ReactionType.Blush => "краснею",
            ReactionType.Laugh => "смеюсь",
            ReactionType.Sad => "грущу",
            ReactionType.Cry => "плачу",
            ReactionType.Shock => "шок",
            ReactionType.Happy => "радуюсь",
            ReactionType.Facepalm => "фейспалм",
            _ => throw new ArgumentOutOfRangeException(nameof(reaction), reaction, null)
        };

        public static string Message(this ReactionType reaction, LanguageType languageType, params object[] replacements)
        {
            var message = reaction switch {
                ReactionType.Shoot => languageType switch
                {
                    LanguageType.English => "{0} shoots {1}",
                    LanguageType.Russian => "{0} стреляет {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Bite => languageType switch
                {
                    LanguageType.English => "{0} bites {1}",
                    LanguageType.Russian => "{0} кусает {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Hug => languageType switch
                {
                    LanguageType.English => "{0} hugs {1}",
                    LanguageType.Russian => "{0} обнимает {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Kiss => languageType switch
                {
                    LanguageType.English => "{0} kisses {1}",
                    LanguageType.Russian => "{0} целует {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Sleep => languageType switch
                {
                    LanguageType.English => "{0} wants to sleep {1}",
                    LanguageType.Russian => "{0} хочет спатки {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Knock => languageType switch
                {
                    LanguageType.English => "{0} knocks {1}",
                    LanguageType.Russian => "{0} стукает {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Pat => languageType switch
                {
                    LanguageType.English => "{0} pat {1}",
                    LanguageType.Russian => "{0} гладит {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Hello => languageType switch
                {
                    LanguageType.English => "{0} welcomes {1}",
                    LanguageType.Russian => "{0} приветствует {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Poke => languageType switch
                {
                    LanguageType.English => "{0} pokes {1}",
                    LanguageType.Russian => "{0} тыкает {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Drink => languageType switch
                {
                    LanguageType.English => "{0} offers to drink {1}",
                    LanguageType.Russian => "{0} предлагает выпить {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Love => languageType switch
                {
                    LanguageType.English => "{0} confesses his love {1}",
                    LanguageType.Russian => "{0} признается в любви {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Play => languageType switch
                {
                    LanguageType.English => "{0} calling to play {1}",
                    LanguageType.Russian => "{0} зовет поиграть {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Angry => languageType switch
                {
                    LanguageType.English => "{0} angry {1}",
                    LanguageType.Russian => "{0} злится {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Blush => languageType switch
                {
                    LanguageType.English => "{0} blushes {1}",
                    LanguageType.Russian => "{0} краснеет {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Laugh => languageType switch
                {
                    LanguageType.English => "{0} laughs {1}",
                    LanguageType.Russian => "{0} смеется {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Sad => languageType switch
                {
                    LanguageType.English => "{0} sad {1}",
                    LanguageType.Russian => "{0} грустит {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Cry => languageType switch
                {
                    LanguageType.English => "{0} {1}",
                    LanguageType.Russian => "{0} плачет {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Shock => languageType switch
                {
                    LanguageType.English => "{0} crying {1}",
                    LanguageType.Russian => "{0} в шоке {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Happy => languageType switch
                {
                    LanguageType.English => "{0} rejoices {1}",
                    LanguageType.Russian => "{0} радуется {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                ReactionType.Facepalm => languageType switch
                {
                    LanguageType.English => "{0} makes facepalm {1}",
                    LanguageType.Russian => "{0} делает фейспалм {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(languageType), languageType, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(reaction), reaction, null)
            };
            return string.Format(message, replacements);
        }
    }
}
