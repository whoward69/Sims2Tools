/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.DBPF.InventoryTokens
{
    public class Personal
    {
        // University tokens
        public static readonly TypeGUID TOKEN_UNI_SECRET_SOCIETY = (TypeGUID)0x8EAE367E;
        public static readonly TypeGUID TOKEN_UNI_GPA = (TypeGUID)0xAE82B295;

        // Skill tokens
        public static readonly TypeGUID TOKEN_MISC_SKILL = (TypeGUID)0x4D8B0CC3;
        public static readonly TypeGUID TOKEN_DANCE_SKILL = (TypeGUID)0x0DA265F4;
        public static readonly TypeGUID TOKEN_DANCE_EXP = (TypeGUID)0x6FE7E453;
        public static readonly TypeGUID TOKEN_PSYCHIC_PARENT = (TypeGUID)0xB3F2D735;

        // Aspiration tokens
        public static readonly TypeGUID TOKEN_ASP_SECONDARY = (TypeGUID)0x53D08989;
        public static readonly TypeGUID TOKEN_ASP_SUPERPOWERS = (TypeGUID)0x33E355C0;
        public static readonly TypeGUID TOKEN_ASP_MOTIVE_DECAY = (TypeGUID)0xB3F19C26;
        public static readonly TypeGUID TOKEN_ASP_FAMILY = (TypeGUID)0x4C92F505;
        public static readonly TypeGUID TOKEN_ASP_FORTUNE = (TypeGUID)0x4C92F480;
        public static readonly TypeGUID TOKEN_ASP_KNOWLEDGE = (TypeGUID)0x6C92F4CF;
        public static readonly TypeGUID TOKEN_ASP_POPULARITY = (TypeGUID)0x8C92F4BC;
        public static readonly TypeGUID TOKEN_ASP_ROMANCE = (TypeGUID)0x6C92F4F3;
    }
}
