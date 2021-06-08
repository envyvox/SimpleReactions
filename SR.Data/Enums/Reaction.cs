using System;

namespace SR.Data.Enums
{
    public enum Reaction : byte
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
        public static string Name(this Reaction reaction) => reaction switch
        {
            Reaction.Shoot => "выстрелить",
            Reaction.Bite => "укусить",
            Reaction.Hug => "обнять",
            Reaction.Kiss => "поцеловать",
            Reaction.Sleep => "спатки",
            Reaction.Knock => "стукнуть",
            Reaction.Pat => "погладить",
            Reaction.Hello => "приветствовать",
            Reaction.Poke => "тыкнуть",
            Reaction.Drink => "выпить",
            Reaction.Love => "люблю",
            Reaction.Play => "играть",
            Reaction.Angry => "злюсь",
            Reaction.Blush => "краснею",
            Reaction.Laugh => "смеюсь",
            Reaction.Sad => "грущу",
            Reaction.Cry => "плачу",
            Reaction.Shock => "шок",
            Reaction.Happy => "радуюсь",
            Reaction.Facepalm => "фейспалм",
            _ => throw new ArgumentOutOfRangeException(nameof(reaction), reaction, null)
        };

        public static string Message(this Reaction reaction, Language language, params object[] replacements)
        {
            var message = reaction switch {
                Reaction.Shoot => language switch
                {
                    Language.English => "{0} shoots {1}",
                    Language.Russian => "{0} стреляет {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Bite => language switch
                {
                    Language.English => "{0} bites {1}",
                    Language.Russian => "{0} кусает {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Hug => language switch
                {
                    Language.English => "{0} hugs {1}",
                    Language.Russian => "{0} обнимает {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Kiss => language switch
                {
                    Language.English => "{0} kisses {1}",
                    Language.Russian => "{0} целует {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Sleep => language switch
                {
                    Language.English => "{0} wants to sleep {1}",
                    Language.Russian => "{0} хочет спатки {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Knock => language switch
                {
                    Language.English => "{0} knocks {1}",
                    Language.Russian => "{0} стукает {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Pat => language switch
                {
                    Language.English => "{0} pat {1}",
                    Language.Russian => "{0} гладит {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Hello => language switch
                {
                    Language.English => "{0} welcomes {1}",
                    Language.Russian => "{0} приветствует {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Poke => language switch
                {
                    Language.English => "{0} pokes {1}",
                    Language.Russian => "{0} тыкает {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Drink => language switch
                {
                    Language.English => "{0} offers to drink {1}",
                    Language.Russian => "{0} предлагает выпить {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Love => language switch
                {
                    Language.English => "{0} confesses his love {1}",
                    Language.Russian => "{0} признается в любви {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Play => language switch
                {
                    Language.English => "{0} calling to play {1}",
                    Language.Russian => "{0} зовет поиграть {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Angry => language switch
                {
                    Language.English => "{0} angry {1}",
                    Language.Russian => "{0} злится {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Blush => language switch
                {
                    Language.English => "{0} blushes {1}",
                    Language.Russian => "{0} краснеет {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Laugh => language switch
                {
                    Language.English => "{0} laughs {1}",
                    Language.Russian => "{0} смеется {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Sad => language switch
                {
                    Language.English => "{0} sad {1}",
                    Language.Russian => "{0} грустит {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Cry => language switch
                {
                    Language.English => "{0} {1}",
                    Language.Russian => "{0} плачет {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Shock => language switch
                {
                    Language.English => "{0} crying {1}",
                    Language.Russian => "{0} в шоке {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Happy => language switch
                {
                    Language.English => "{0} rejoices {1}",
                    Language.Russian => "{0} радуется {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Reaction.Facepalm => language switch
                {
                    Language.English => "{0} makes facepalm {1}",
                    Language.Russian => "{0} делает фейспалм {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(reaction), reaction, null)
            };
            return string.Format(message, replacements);
        }
    }
}
