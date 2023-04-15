/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.DBPF.Neighbourhood
{
    public enum GameVersions : uint
    {
        None = 0x00,
        University = 0x01,
        Nightlife = 0x02,
        Business = 0x03,
        FamilyFun = 0x04,
        GlamourLife = 0x05,
        Pets = 0x06,
        Seasons = 0x07,
        Celebration = 0x08,
        Fashion = 0x09,
        BonVoyage = 0x0A,
        TeenStyle = 0x0B,
        StoreEdition_Old = 0x0C,
        FreeTime = 0x0D,
        KitchenBath = 0x0E,
        IkeaHome = 0x0F,
        ApartmentLife = 0x10,
        MansionGarden = 0x11,
        StoreEdition_New = 0x1F
    }

    public enum NeighborhoodType : uint
    {
        Unknown = 0x00,
        Normal = 0x01,
        University = 0x02,
        Downtown = 0x03,
        Suburb = 0x04,
        Village = 0x05,
        Lakes = 0x06,
        Island = 0x07
    }

    public enum NeighborhoodVersion : uint
    {
        Unknown = 0x00,
        Sims2 = 0x03,
        Sims2_University = 0x05,
        Sims2_Nightlife = 0x07,
        Sims2_Business = 0x08,
        Sims2_Pets = 0x09,
        Sims2_Seasons = 0x0A
    }

    public enum LotOrientation : byte
    {
        Below = 0,
        Left = 1,
        Above = 2,
        Right = 3,
    }

    public enum SDescVersions : int
    {
        Unknown = 0,
        BaseGame = 0x20,
        University = 0x22,
        Nightlife = 0x29,
        Business = 0x2a,
        Pets = 0x2c,
        Castaway = 0x2d,
        Voyage = 0x2e,
        VoyageB = 0x2f,
        Freetime = 0x33,
        Apartment = 0x36,
    }

    public enum FamiVersions : int
    {
        Original = 0x4e,
        University = 0x4f,
        Business = 0x51,
        Voyage = 0x55,
        Castaway = 0x56
    }

    public enum Gender : ushort
    {
        Male = 0x00,
        Female = 0x01
    }

    public enum LifeSections : ushort
    {
        Unknown = 0x00,
        Baby = 0x01,
        Toddler = 0x02,
        Child = 0x03,
        Teen = 0x10,
        Adult = 0x13,
        Elder = 0x33,
        YoungAdult = 0x40
    }

    public enum LifeStateFlags : uint
    {
        Zombie = 0x0001,
        Vampire = 0x0004,
        NPC = 0x0010,
        Werewolf = 0x0020,
        Plantsim = 0x0100,
        Witch = 0x0400

        //Bigfoot = 0x0A
        //Servo
        //Alien
    }

    public enum ZodiacSigns : ushort
    {
        Aries = 0x01,
        Taurus = 0x02,
        Gemini = 0x03,
        Cancer = 0x04,
        Leo = 0x05,
        Virgo = 0x06,
        Libra = 0x07,
        Scorpio = 0x08,
        Sagittarius = 0x09,
        Capricorn = 0x0A,
        Aquarius = 0x0B,
        Pisces = 0x0C
    }

    public enum AspirationTypes : ushort
    {
        Nothing = 0x00,
        Romance = 0x01,
        Family = 0x02,
        Fortune = 0x04,
        Reputation = 0x10,
        Knowledge = 0x20,
        Growup = 0x40,
        Fun = 0x80,
        Cheese = 0x100
    }

    public enum SchoolTypes : uint
    {
        NoSchool = 0x00000000,
        PublicSchool = 0xD06788B5,
        PrivateSchool = 0xCC8F4C11
    }

    public enum Grades : ushort
    {
        Unknown = 0x00,
        F = 0x01,
        DMinus = 0x02,
        D = 0x03,
        DPlus = 0x04,
        CMinus = 0x05,
        C = 0x06,
        CPlus = 0x07,
        BMinus = 0x08,
        B = 0x09,
        BPlus = 0x0A,
        AMinus = 0x0B,
        A = 0x0C,
        APlus = 0x0D
    }

    public enum FamilyTieTypes : uint
    {
        MyMotherIs = 0x00,
        MyFatherIs = 0x01,
        ImMarriedTo = 0x02,
        MySiblingIs = 0x03,
        MyChildIs = 0x04
    }

    public enum Majors : uint
    {
        Unset = 0,
        Unknown = 0xffffffff,
        Art = 0x2e9cf007,
        Biology = 0x4e9cf02b,
        Drama = 0x4e9cf04d,
        Economics = 0xEe9cf044,
        History = 0x2e9cf074,
        Literature = 0xce9cf085,
        Mathematics = 0xee9cf08d,
        Philosophy = 0x2e9cf057,
        Physics = 0xae9cf063,
        PoliticalScience = 0x4e9cf06d,
        Psychology = 0xCE9CF07C,
        Undeclared = 0x8e97bf1d
    }

    public enum Careers : uint
    {
        Unknown = 0xFFFFFFFF,
        Unemployed = 0x00000000,

        Adventurer = 0x3240CBA5,
        Athletic = 0x2C89E95F,
        Artist = 0x4E6FFFBC,
        Construction = 0xF3E1C301,
        Criminal = 0x6C9EBD0E,
        Culinary = 0xEC9EBD5F,
        Dance = 0xD3E09422,
        Economy = 0x45196555,
        Education = 0x72428B30,
        Entertainment = 0xB3E09417,
        Gamer = 0xF240C306,
        Intelligence = 0x33E0940E,
        Journalism = 0x7240D944,
        Law = 0x12428B19,
        LawEnforcement = 0xAC9EBCE3,
        Medical = 0x0C7761FD,
        Military = 0x6C9EBD32,
        Music = 0xB2428B0C,
        NaturalScientist = 0xEE70001C,
        Ocenography = 0x73E09404,
        Paranormal = 0x2E6FFF87,
        Politics = 0x2C945B14,
        Science = 0x0C9EBD47,
        ShowBiz = 0xAE6FFFB0,
        Slacker = 0xEC77620B,

        TeenElderAdventurer = 0xF240D235,
        TeenElderAthletic = 0xAC89E947,
        TeenElderBusiness = 0x4C1E0577,
        TeenElderConstruction = 0x53E1C30F,
        TeenElderCriminal = 0xACA07ACD,
        TeenElderCulinary = 0x4CA07B0C,
        TeenElderDance = 0xD3E094A5,
        TeenElderEducation = 0xD243BBEC,
        TeenElderEntertainment = 0x53E09494,
        TeenElderGamer = 0x1240C962,
        TeenElderIntelligence = 0x93E094C0,
        TeenElderJournalism = 0x5240E212,
        TeenElderLaw = 0x1243BBDE,
        TeenElderLawEnforcement = 0x6CA07B39,
        TeenElderMedical = 0xAC89E918,
        TeenElderMilitary = 0xCCA07B66,
        TeenElderMusic = 0xB243BBD2,
        TeenElderOcenography = 0x13E09443,
        TeenElderPolitics = 0xCCA07B8D,
        TeenElderScience = 0xECA07BB0,
        TeenElderSlacker = 0x6CA07BDC,

        PetSecurity = 0xD188A400,
        PetService = 0xB188A4C1,
        PetShowBiz = 0xD175CC2D
    }

    public enum JobAssignment : ushort
    {
        Nothing = 0x00,
        Chef = 0x01,
        Host = 0x02,
        Server = 0x03,
        Cashier = 0x04,
        Bartender = 0x05,
        Barista = 0x06,
        DJ = 0x07,
        SellLemonade = 0x08,
        Stylist = 0x09,
        Tidy = 0x0A,
        Restock = 0x0B,
        Sales = 0x0C,
        MakeToys = 0x0D,
        ArrangeFlowers = 0x0E,
        BuildRobots = 0x0F
    }

    public enum Hobbies : ushort
    {
        Cuisine = 0xCC,
        Arts = 0xCD,
        Film = 0xCE,
        Sport = 0xCF,
        Games = 0xD0,
        Nature = 0xD1,
        Tinkering = 0xD2,
        Fitness = 0xD3,
        Science = 0xD4,
        Music = 0xD5,
        Secret = 0xD6
    }

    public enum SpeciesType : ushort
    {
        Human = 0,
        LargeDog = 1,
        SmallDog = 2,
        Cat = 3
    }

    public enum Seasons : ushort
    {
        Spring = 0,
        Summer = 1,
        Autumn = 2,
        Winter = 3
    }

    public enum RelationshipTypes : uint
    {
        Unset_Unknown = 0x00,
        Parent = 0x01,
        Child = 0x02,
        Sibling = 0x03,
        Gradparent = 0x04,
        Grandchild = 0x05,
        Nice_Nephew = 0x07,
        Aunt = 0x06,
        Cousin = 0x08,
        Spouses = 0x09
    }

    public enum RelationshipStateBits : byte
    {
        Crush = 0x00,
        Love = 0x01,
        Engaged = 0x02,
        Married = 0x03,
        Friends = 0x04,
        Buddies = 0x05,
        Steady = 0x06,
        Enemy = 0x07,
        Family = 0x0E,
        Known = 0x0F,
    }

    public enum UIFlags2Names : byte
    {
        BestFriendForever = 0x00,
    };

}
